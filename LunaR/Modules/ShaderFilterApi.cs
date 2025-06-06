using System;
using System.Runtime.InteropServices;

namespace LunaR.Modules
{
    public static class ShaderFilterApi
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void TriBool(bool limitLoops, bool limitGeometry, bool limitTesselation);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void OneFloat(float value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void OneInt(int value);

        private static TriBool SetFilterState;
        private static OneFloat SetTess;
        private static OneInt SetLoops;
        private static OneInt SetGeom;

        public static void Init(IntPtr hmodule)
        {
            SetFilterState = Marshal.GetDelegateForFunctionPointer<TriBool>(GetProcAddress(hmodule, nameof(SetFilteringState)));
            SetTess = Marshal.GetDelegateForFunctionPointer<OneFloat>(GetProcAddress(hmodule, nameof(SetMaxTesselationPower)));
            SetLoops = Marshal.GetDelegateForFunctionPointer<OneInt>(GetProcAddress(hmodule, nameof(SetLoopLimit)));
            SetGeom = Marshal.GetDelegateForFunctionPointer<OneInt>(GetProcAddress(hmodule, nameof(SetGeometryLimit)));
        }

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        public static void SetFilteringState(bool limitLoops, bool limitGeometry, bool limitTesselation) => SetFilterState(limitLoops, limitGeometry, limitTesselation);

        public static void SetMaxTesselationPower(float maxTesselation) => SetTess(maxTesselation);

        public static void SetLoopLimit(int limit) => SetLoops(limit);

        public static void SetGeometryLimit(int limit) => SetGeom(limit);
    }
}