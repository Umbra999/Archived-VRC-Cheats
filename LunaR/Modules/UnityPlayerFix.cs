using HarmonyLib;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using UnhollowerBaseLib;
using UnityEngine.SceneManagement;

namespace LunaR.Modules
{
    internal class UnityPlayerFix
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr AudioMixerDelegate(IntPtr thisPtr, IntPtr readerPtr);

        private static AudioMixerDelegate MixerReadDelegate;

        [UnmanagedFunctionPointer(CallingConvention.FastCall)]
        private delegate void FindAndLoadUnityPlugin(IntPtr name, out IntPtr loadedModule, byte bEnableSomeDebug);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate long CountNodesDeepDelegate(NodeContainer* thisPtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DebugAssertDelegate(IntPtr data);

        private static DebugAssertDelegate DebugAssert;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ReaderOobDelegate(IntPtr thisPtr, long a, long b);

        private static ReaderOobDelegate OriginalReaderOob;

        [ThreadStatic]
        private static int ReaderOobDepth;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr StringReallocateDelegate(NativeString* str, long newSize);

        private static StringReallocateDelegate ourOriginalRealloc;

        [ThreadStatic] private static unsafe NativeString* ourLastReallocatedString;
        [ThreadStatic] private static int ourLastReallocationCount;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void FloatReadDelegate(IntPtr readerPtr, float* result, byte* fieldName);

        private static FloatReadDelegate ourFloatReadDelegate;

        private static void DoPatch<T>(IntPtr BaseAddress, int offset, T patchDelegate, out T delegateField) where T : MulticastDelegate
        {
            delegateField = null;
            if (offset == 0) return;
            var targetPtr = BaseAddress + offset;
            NativePatch(targetPtr, out delegateField, patchDelegate);
        }

        internal static void NativePatch<T>(MethodInfo original, out T callOriginal, MethodInfo patch) where T : MulticastDelegate
        {
            var patchDelegate = (T)Delegate.CreateDelegate(typeof(T), patch);
            NativePatch(original, out callOriginal, patchDelegate);
        }

        internal static void NativePatch<T>(IntPtr originalPointer, out T callOriginal, MethodInfo patch, string? context = null) where T : MulticastDelegate
        {
            var patchDelegate = (T)Delegate.CreateDelegate(typeof(T), patch);
            NativePatch(originalPointer, out callOriginal, patchDelegate, context);
        }

        internal static unsafe void NativePatch<T>(MethodInfo original, out T callOriginal, T patchDelegate) where T : MulticastDelegate
        {
            if (original == null) throw new ArgumentNullException(nameof(original));

            var originalPointer = *(IntPtr*)(IntPtr)UnhollowerUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod(original).GetValue(null);
            NativePatch(originalPointer, out callOriginal, patchDelegate, original.FullDescription());
        }

        internal static unsafe void NativePatch<T>(IntPtr originalPointer, out T callOriginal, T patchDelegate, string? context = null) where T : MulticastDelegate
        {
            var patchPointer = Marshal.GetFunctionPointerForDelegate(patchDelegate);
            MelonUtils.NativeHookAttach((IntPtr)(&originalPointer), patchPointer);
            callOriginal = Marshal.GetDelegateForFunctionPointer<T>(originalPointer);
        }

        public static void ApplyPatches()
        {
            SceneManager.add_sceneLoaded(new Action<Scene, LoadSceneMode>((s, _) =>
            {
                if (s.buildIndex == -1)
                {
                    BlockMixers = true;
                    ShaderFilterApi.SetFilteringState(true, true, true);
                }
            }));

            SceneManager.add_sceneUnloaded(new Action<Scene>(s =>
            {
                if (s.buildIndex == -1)
                {
                    BlockMixers = false;
                    ShaderFilterApi.SetFilteringState(false, false, false);
                }
            }));

            try
            {
                using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LunaR.Resources.DxbcShaderFilter.dll");
                using FileStream fileStream = File.Open($"VRChat_Data\\Plugins\\x86_64\\DxbcShaderFilter.dll", FileMode.Create, FileAccess.Write);
                stream.CopyTo(fileStream);
            }
            catch
            {
                Extensions.Logger.LogError("Failed to Load Shader Filter");
            }

            IntPtr unityPlayer = Process.GetCurrentProcess().Modules.Cast<ProcessModule>().FirstOrDefault(module => module.ModuleName == "UnityPlayer.dll")!.BaseAddress;
            unsafe
            {
                DoPatch(unityPlayer, 0xA86270, AudioMixerReadPatch, out MixerReadDelegate);
                DoPatch(unityPlayer, 0xC8230, FloatTransferPatch, out ourFloatReadDelegate);
                DoPatch<CountNodesDeepDelegate>(unityPlayer, 0xDF29F0, CountNodesDeepThunk, out _);
                DoPatch(unityPlayer, 0xDDBDC0, DebugAssertPatch, out DebugAssert);
                DoPatch(unityPlayer, 0x7B9EB0, ReaderOobPatch, out OriginalReaderOob);
                DoPatch(unityPlayer, 0xC69F0, ReallocateStringPatch, out ourOriginalRealloc);

                FindAndLoadUnityPlugin dg = Marshal.GetDelegateForFunctionPointer<FindAndLoadUnityPlugin>(unityPlayer + 0x79F070);
                IntPtr strPtr = Marshal.StringToHGlobalAnsi("DxbcShaderFilter");
                dg(strPtr, out var loaded, 1);
                ShaderFilterApi.Init(loaded);
                Marshal.FreeHGlobal(strPtr);

                ShaderFilterApi.SetMaxTesselationPower(5);
                ShaderFilterApi.SetLoopLimit(128);
                ShaderFilterApi.SetGeometryLimit(60);
            }
        }

        private static bool BlockMixers = false;

        private static IntPtr AudioMixerReadPatch(IntPtr thisPtr, IntPtr readerPtr)
        {
            if (BlockMixers) return IntPtr.Zero;
            while (MixerReadDelegate == null) Thread.Sleep(15);
            return MixerReadDelegate(thisPtr, readerPtr);
        }

        private static unsafe long CountNodesDeepThunk(NodeContainer* thisPtr)
        {
            try
            {
                return CountNodesDeepImpl(thisPtr, new HashSet<IntPtr>());
            }
            catch
            {
                return 1;
            }
        }

        private static unsafe long CountNodesDeepImpl(NodeContainer* thisPtr, HashSet<IntPtr> parents)
        {
            if (thisPtr == null) return 1;
            var directSubsCount = thisPtr->DirectSubCount;

            long totalNodes = 1;
            if (directSubsCount <= 0) return totalNodes;

            parents.Add((IntPtr)thisPtr);

            var subsBase = thisPtr->Subs;
            if (subsBase == null)
            {
                thisPtr->DirectSubCount = 0;
                return totalNodes;
            }

            for (var i = 0; i < directSubsCount; ++i)
            {
                var subNode = subsBase[i];

                if (subNode == null)
                {
                    thisPtr->DirectSubCount = 0;
                    return totalNodes;
                }

                if (parents.Contains((IntPtr)subNode))
                {
                    subNode->DirectSubCount = thisPtr->DirectSubCount = 0;
                    return totalNodes;
                }
                totalNodes += CountNodesDeepImpl(subNode, parents);
            }
            return totalNodes;
        }

        [StructLayout(LayoutKind.Explicit, Size = 0x88)]
        private unsafe struct NodeContainer
        {
            [FieldOffset(0x70)]
            public NodeContainer** Subs;

            [FieldOffset(0x80)]
            public long DirectSubCount;
        }

        private static unsafe void DebugAssertPatch(IntPtr data)
        {
            if (ReaderOobDepth > 0) *(byte*)(data + 0x30) &= 0xef;
            DebugAssert(data);
        }

        private static void ReaderOobPatch(IntPtr thisPtr, long a, long b)
        {
            ReaderOobDepth++;
            try
            {
                OriginalReaderOob(thisPtr, a, b);
            }
            finally
            {
                ReaderOobDepth--;
            }
        }

        private static unsafe IntPtr ReallocateStringPatch(NativeString* str, long newSize)
        {
            if (str != null && newSize > 128 && str->Data != IntPtr.Zero)
            {
                if (ourLastReallocatedString != str)
                {
                    ourLastReallocatedString = str;
                    ourLastReallocationCount = 0;
                }
                else
                {
                    ourLastReallocationCount++;
                    if (ourLastReallocationCount >= 8 && newSize <= str->Capacity + 16 && str->Capacity > 16) newSize = str->Capacity * 2;
                }
            }

            while (ourOriginalRealloc == null) Thread.Sleep(15);
            return ourOriginalRealloc(str, newSize);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NativeString
        {
            public IntPtr Data;
            public long Capacity;
            public long Unknown;
            public long Length;
        }

        private static unsafe void FloatTransferPatch(IntPtr reader, float* result, byte* fieldName)
        {
            ourFloatReadDelegate(reader, result, fieldName);

            if (!BlockMixers || *result > -3.402823E+7f && *result < 3.402823E+7f) return;

            if (float.IsNaN(*result)) goto clamp;

            string[] AllowedFields = { "m_BreakForce", "m_BreakTorque", "collisionSphereDistance", "maxDistance", "inSlope", "outSlope" };
            if (fieldName != null)
            {
                foreach (string allowedField in AllowedFields)
                {
                    for (int j = 0; j < allowedField.Length; j++)
                    {
                        if (fieldName[j] == 0 || fieldName[j] != allowedField[j]) goto next;
                    }
                    return;
                next:;
                }
            }

        clamp:
            *result = 0;
        }
    }
}