using CoreRuntime.Manager;
using Hexed.Interfaces;
using UnityEngine;

namespace Hexed.Hooking
{
    internal class VRCUiPageLoading_OnEnable : IHook
    {
        private delegate void _OnEnableDelegate(nint instance);
        private static _OnEnableDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_OnEnableDelegate>(typeof(VRCUiPageLoading).GetMethod(nameof(VRCUiPageLoading.OnEnable)), Patch);
        }

        private static void Patch(nint instance)
        {
            originalMethod(instance);

            VRCUiPageLoading popup = instance == nint.Zero ? null : new VRCUiPageLoading(instance);
            if (popup == null) return;

            popup.transform.Find("3DElements/LoadingBackground_TealGradient/SkyCube_Baked").transform.localScale = Vector3.zero;
            popup.transform.Find("3DElements/LoadingInfoPanel").transform.localScale = Vector3.zero;
            popup.transform.Find("ProgressPanel/Parent_Loading_Progress/Panel_Backdrop").transform.localScale = Vector3.zero;
            popup.transform.Find("ProgressPanel/Parent_Loading_Progress/Decoration_Left").transform.localScale = Vector3.zero;
            popup.transform.Find("ProgressPanel/Parent_Loading_Progress/Decoration_Right").transform.localScale = Vector3.zero;

            ParticleSystem FarParticles = popup.transform.Find("3DElements/LoadingBackground_TealGradient/_FX_ParticleBubbles/FX_snow").gameObject.GetComponent<ParticleSystem>();
            FarParticles.startColor = Color.white;
            FarParticles.gameObject.SetActive(false);
            FarParticles.gameObject.SetActive(true);

            ParticleSystem NearParticles = popup.transform.Find("3DElements/LoadingBackground_TealGradient/_FX_ParticleBubbles/FX_CloseParticles").gameObject.GetComponent<ParticleSystem>();
            NearParticles.startColor = Color.white;
            NearParticles.gameObject.SetActive(false);
            NearParticles.gameObject.SetActive(true);
        }
    }
}
