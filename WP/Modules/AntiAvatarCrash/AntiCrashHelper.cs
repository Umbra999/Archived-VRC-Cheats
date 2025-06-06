using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using UnhollowerBaseLib;
using UnhollowerBaseLib.Runtime;
using UnityEngine;
using VRC.Core;
using WengaPort.Modules;
using AMEnumA = VRCAvatarManager.ObjectNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObAcOb1GaondoAconUnique;
using AMEnumB = VRCAvatarManager.ObjectNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObAcOb1GaonPrAconUnique;
using AMEnumC = VRCAvatarManager.ObjectNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObGaobApObapBoisfrUnique;
using Object = UnityEngine.Object;

namespace WengaPort.Extensions
{
    class AntiCrashHelper
    {
        public static void Hook()
        {
            unsafe
            {
                var originalMethodPointer = *(IntPtr*)(IntPtr)UnhollowerUtils
                    .GetIl2CppMethodInfoPointerFieldForGeneratedMethod(typeof(AMEnumA).GetMethod(
                        nameof(AMEnumA.MoveNext)))
                    .GetValue(null);

                Imports.Hook((IntPtr)(&originalMethodPointer), typeof(AntiCrashHelper).GetMethod(nameof(MoveNextPatchA), BindingFlags.Static | BindingFlags.NonPublic).MethodHandle.GetFunctionPointer());

                ourMoveNextA = originalMethodPointer;
            }

            unsafe
            {
                var originalMethodPointer = *(IntPtr*)(IntPtr)UnhollowerUtils
                    .GetIl2CppMethodInfoPointerFieldForGeneratedMethod(typeof(AMEnumB).GetMethod(
                        nameof(AMEnumB.MoveNext)))
                    .GetValue(null);

                Imports.Hook((IntPtr)(&originalMethodPointer), typeof(AntiCrashHelper).GetMethod(nameof(MoveNextPatchB), BindingFlags.Static | BindingFlags.NonPublic).MethodHandle.GetFunctionPointer());

                ourMoveNextB = originalMethodPointer;
            }

            unsafe
            {
                var originalMethodPointer = *(IntPtr*)(IntPtr)UnhollowerUtils
                    .GetIl2CppMethodInfoPointerFieldForGeneratedMethod(typeof(AMEnumC).GetMethod(
                        nameof(AMEnumC.MoveNext)))
                    .GetValue(null);

                Imports.Hook((IntPtr)(&originalMethodPointer), typeof(AntiCrashHelper).GetMethod(nameof(MoveNextPatchC), BindingFlags.Static | BindingFlags.NonPublic).MethodHandle.GetFunctionPointer());

                ourMoveNextC = originalMethodPointer;
            }

            unsafe
            {
                var originalMethodInfo = (Il2CppMethodInfo*)(IntPtr)UnhollowerUtils
                    .GetIl2CppMethodInfoPointerFieldForGeneratedMethod(typeof(AMEnumB).GetMethod(
                        nameof(AMEnumB.MoveNext)))
                    .GetValue(null);

                var methodInfoCopy = (Il2CppMethodInfo*)Marshal.AllocHGlobal(Marshal.SizeOf<Il2CppMethodInfo>());
                *methodInfoCopy = *originalMethodInfo;

                ourInvokeMethodInfo = (IntPtr)methodInfoCopy;
            }
            var instance = Harmony.HarmonyInstance.Create("DaysAntiCrash");
            var matchingMethods = typeof(AssetManagement)
                .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).Where(it =>
                    it.Name.StartsWith("Method_Public_Static_Object_Object_Vector3_Quaternion_Boolean_Boolean_Boolean_") && it.GetParameters().Length == 6).ToList();
            foreach (var method in matchingMethods)
            {
                instance.Patch(method, new Harmony.HarmonyMethod(typeof(AntiCrashHelper).GetMethod("ObjectInstantiatePatch", BindingFlags.NonPublic | BindingFlags.Static)));
            }
        }
        private static bool ObjectInstantiatePatch(ref Object __0)//,ref Vector3 __1, ref Quaternion __2, ref bool __3, ref bool __4,ref bool __5)//, ref Object __result)
        {
            try
            {
                if (__0 == null || AvatarManagerCookie.CurrentManager == null || !__0.name.ToLower().Contains("avatar"))
                    return true;
                GameObject AvatarObject = __0.Cast<GameObject>();
                if (AvatarObject == null) return true;

                VRCAvatarManager avatarManager = AvatarManagerCookie.CurrentManager;
                var player = avatarManager.field_Private_VRCPlayer_0;
                if (player == null || player.UserID() == APIUser.CurrentUser.id) return true;

                var wasawake = AvatarObject.activeSelf;
                AvatarObject.SetActive(false);
                try
                {
                    AvatarProcesser.ProcessAvatar(AvatarObject, avatarManager);
                }
                catch { }
                AvatarObject.SetActive(wasawake);
            }
            catch { }
            return true;
        }
        private static bool MoveNextPatchA(IntPtr thisPtr)
        {
            try
            {
                using (new AvatarManagerCookie(new AMEnumA(thisPtr).__4__this))
                    return SafeInvokeMoveNext(ourMoveNextA, thisPtr);
            }
            catch (Il2CppException ex)
            {
                if (Imports.IsDebugMode())
                    MelonLogger.Log($"Caught top-level native exception: {ex}");
                return false;
            }
            catch (Exception ex)
            {
                MelonLogger.LogError($"Error when wrapping avatar creation: {ex}");
                return false;
            }
        }

        private static bool MoveNextPatchB(IntPtr thisPtr)
        {
            try
            {
                using (new AvatarManagerCookie(new AMEnumB(thisPtr).__4__this))
                    return SafeInvokeMoveNext(ourMoveNextB, thisPtr);
            }
            catch (Il2CppException ex)
            {
                if (Imports.IsDebugMode())
                    MelonLogger.Log($"Caught top-level native exception: {ex}");
                return false;
            }
            catch (Exception ex)
            {
                MelonLogger.LogError($"Error when wrapping avatar creation: {ex}");
                return false;
            }
        }

        private static bool MoveNextPatchC(IntPtr thisPtr)
        {
            try
            {
                using (new AvatarManagerCookie(new AMEnumC(thisPtr).__4__this))
                    return SafeInvokeMoveNext(ourMoveNextC, thisPtr);
            }
            catch (Il2CppException ex)
            {
                if (Imports.IsDebugMode())
                    MelonLogger.Log($"Caught top-level native exception: {ex}");
                return false;
            }
            catch (Exception ex)
            {
                MelonLogger.LogError($"Error when wrapping avatar creation: {ex}");
                return false;
            }
        }
        private static IntPtr ourMoveNextA;
        private static IntPtr ourMoveNextB;
        private static IntPtr ourMoveNextC;

        private static IntPtr ourInvokeMethodInfo;

        private unsafe static bool SafeInvokeMoveNext(IntPtr methodPtr, IntPtr thisPtr)
        {
            var exc = IntPtr.Zero;
            ((Il2CppMethodInfo*)ourInvokeMethodInfo)->methodPointer = methodPtr;
            var result = IL2CPP.il2cpp_runtime_invoke(ourInvokeMethodInfo, thisPtr, (void**)IntPtr.Zero, ref exc);
            Il2CppException.RaiseExceptionIfNecessary(exc);
            return *(bool*)IL2CPP.il2cpp_object_unbox(result);
        }
       
    }
    public readonly struct AvatarManagerCookie : IDisposable
    {
        internal static VRCAvatarManager CurrentManager;
        private readonly VRCAvatarManager myLastManager;

        public AvatarManagerCookie(VRCAvatarManager avatarManager)
        {
            myLastManager = CurrentManager;
            CurrentManager = avatarManager;
        }
        public void Dispose()
        {
            CurrentManager = myLastManager;
        }
    }
    public readonly struct GameObjectWithPriorityData
    {
        public readonly GameObject GameObject;
        public readonly bool IsActive;
        public readonly int NumChildren;
        public readonly int Depth;

        public GameObjectWithPriorityData(GameObject go, int depth)
        {
            GameObject = go;
            Depth = depth;
            IsActive = go.activeSelf;
            NumChildren = go.transform.childCount;
        }

        public int Priority => Depth + NumChildren;

        private sealed class IsActiveDepthNumChildrenRelationalComparer : IComparer<GameObjectWithPriorityData>
        {
            public int Compare(GameObjectWithPriorityData x, GameObjectWithPriorityData y)
            {
                var isActiveComparison = -x.IsActive.CompareTo(y.IsActive);
                if (isActiveComparison != 0) return isActiveComparison;
                return x.Priority.CompareTo(y.Priority);
            }
        }

        public static IComparer<GameObjectWithPriorityData> IsActiveDepthNumChildrenComparer { get; } = new IsActiveDepthNumChildrenRelationalComparer();
    }
    public class PriorityQueue<T>
    {
        private readonly List<T> myBackingStorage;
        private readonly IComparer<T> myComparer;

        public PriorityQueue(IComparer<T> comparer)
        {
            myComparer = comparer;
            myBackingStorage = new List<T>();
        }

        public void Enqueue(T value)
        {
            myBackingStorage.Add(value);
            SiftUp(myBackingStorage.Count - 1);
        }

        public T Dequeue()
        {
            if (myBackingStorage.Count == 0)
                return default(T);
            Swap(0, myBackingStorage.Count - 1);
            var result = myBackingStorage[myBackingStorage.Count - 1];
            myBackingStorage.RemoveAt(myBackingStorage.Count - 1);
            SiftDown(0);
            return result;
        }

        public T Peek()
        {
            if (myBackingStorage.Count == 0)
                return default(T);
            return myBackingStorage[0];
        }

        public int Count => myBackingStorage.Count;

        private void Swap(int i1, int i2)
        {
            var value1 = myBackingStorage[i1];
            var value2 = myBackingStorage[i2];
            myBackingStorage[i1] = value2;
            myBackingStorage[i2] = value1;
        }

        private void SiftDown(int i)
        {
            var childIndex1 = i * 2 + 1;
            var childIndex2 = i * 2 + 2;
            if (childIndex1 >= myBackingStorage.Count)
                return;
            var child1 = myBackingStorage[childIndex1];
            if (childIndex2 >= myBackingStorage.Count)
            {
                var compared = myComparer.Compare(myBackingStorage[i], child1);
                if (compared > 0)
                {
                    Swap(i, childIndex1);
                }
                return;
            }
            var child2 = myBackingStorage[childIndex2];
            var compared1 = myComparer.Compare(myBackingStorage[i], child1);
            var compared2 = myComparer.Compare(myBackingStorage[i], child2);
            if (compared1 > 0 || compared2 > 0)
            {
                var compared12 = myComparer.Compare(child1, child2);
                if (compared12 > 0)
                {
                    Swap(i, childIndex2);
                    SiftDown(childIndex2);
                }
                else
                {
                    Swap(i, childIndex1);
                    SiftDown(childIndex1);
                }
            }
        }

        private void SiftUp(int i)
        {
            if (i == 0)
                return;
            var parentIndex = (i - 1) / 2;
            var compared = myComparer.Compare(myBackingStorage[i], myBackingStorage[parentIndex]);
            if (compared < 0)
            {
                Swap(i, parentIndex);
                SiftUp(parentIndex);
            }
        }
    }
}
