using CoreRuntime.Manager;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Hexed.Wrappers
{
    internal static class GeneralUtils
    {
        public static int IndexOfByteArray(byte[] source, byte[] pattern)
        {
            if (source == null || pattern == null || source.Length == 0 || pattern.Length == 0 || source.Length < pattern.Length)
                return -1;

            for (int i = 0; i <= source.Length - pattern.Length; i++)
            {
                if (Matches(source, i, pattern))
                    return i;
            }

            return -1;
        }

        public static int[] IndexesOfByteArray(byte[] source, byte[] pattern)
        {
            List<int> Offsets = new();

            if (source == null || pattern == null || source.Length == 0 || pattern.Length == 0 || source.Length < pattern.Length) return Offsets.ToArray();

            for (int i = 0; i <= source.Length - pattern.Length; i++)
            {
                if (Matches(source, i, pattern)) Offsets.Add(i);
            }

            return Offsets.ToArray();
        }

        private static bool Matches(byte[] source, int startIndex, byte[] pattern)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (source[startIndex + i] != pattern[i])
                    return false;
            }
            return true;
        }

        public static int lastIndexOfByteArray(byte[] source, byte[] pattern)
        {
            byte[] reversedSource = reverseArray(source);
            byte[] reversedPattern = reverseArray(pattern);
            int index = IndexOfByteArray(reversedSource, reversedPattern);
            if (index == -1)
            {
                return -1;
            }
            return source.Length - index - pattern.Length;
        }

        public static byte[] reverseArray(byte[] array)
        {
            byte[] reversedArray = new byte[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                reversedArray[i] = array[array.Length - 1 - i];
            }
            return reversedArray;
        }

        public static Process GetProcessByName(string Name)
        {
            Process[] AllProcesses = Process.GetProcessesByName(Name);
            if (AllProcesses != null && AllProcesses.Length > 0) return AllProcesses[0];

            return null;
        }

        public static IEnumerator DelayAction(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action();
        }

        public static void Start(this IEnumerator e)
        {
            CoroutineManager.RunCoroutine(e);
        }

        public static void CopyToClipboard(string Text)
        {
            if (Text == null) return;

            if (!NativeMethods.OpenClipboard(IntPtr.Zero)) return;

            try
            {
                NativeMethods.EmptyClipboard();

                uint flags = 0x0002 /*GMEM_MOVEABLE*/;
                UIntPtr size = (UIntPtr)((Text.Length + 1) * 2);
                IntPtr hGlobalMem = NativeMethods.GlobalAlloc(flags, size);

                if (hGlobalMem == IntPtr.Zero) return;

                IntPtr lpGlobalMem = NativeMethods.GlobalLock(hGlobalMem);

                Marshal.Copy(Text.ToCharArray(), 0, lpGlobalMem, Text.Length);

                Marshal.WriteInt16(lpGlobalMem + Text.Length * 2, 0);

                NativeMethods.GlobalUnlock(hGlobalMem);

                if (NativeMethods.SetClipboardData(13 /*CF_UNICODETEXT*/, hGlobalMem) == IntPtr.Zero) return;

                Logger.Log($"Copied: {Text}", Logger.LogsType.Info);
            }
            finally
            {
                NativeMethods.CloseClipboard();
            }
        }

        public static long GetUnixTimeInMilliseconds()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}
