using Harmony;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using VRC;
using WengaPort.Buttons;
using WengaPort.Extensions;

namespace WengaPort.Modules
{
    class AntiMenuOverrender : MonoBehaviour
    {
        private static int _originalCullingMask;
        private static int _newCullingMask;

        private static GameObject _menuCameraClone;
        private static Camera _menuCameraUI;
        private static Camera _originalCamera;

        public static int _uiLayer;
        public static int _uiMenuLayer;
        private static int _playerLocalLayer;
        private static int _playerLayer;

        public static int _uiPlayerNameplateLayer = 30;
        public static bool AntiOverrenderToggle;

        public static void AntiOverrender (bool toggle)
        {
            if (toggle)
            {
                AntiOverrenderToggle = true;
                _menuCameraClone.SetActive(true);
                _originalCamera.cullingMask = _newCullingMask;
                SafetyMenu.AntiMenuToggle.setToggleState(true);
            }
            else
            {
                AntiOverrenderToggle = false;
                _menuCameraClone.SetActive(false);
                _originalCamera.cullingMask = _originalCullingMask;
                SafetyMenu.AntiMenuToggle.setToggleState(false);
            }
        }

        public static void UIInit()
        {
            VRCVrCamera vrCamera = VRCVrCamera.field_Private_Static_VRCVrCamera_0;
            if (!vrCamera)
                return;
            Camera screenCamera = vrCamera.field_Public_Camera_0;
            if (!screenCamera)
                return;

            _originalCamera = screenCamera;

            _originalCullingMask = screenCamera.cullingMask;

            screenCamera.cullingMask = screenCamera.cullingMask
                & ~(1 << LayerMask.NameToLayer("UiMenu"))
                & ~(1 << LayerMask.NameToLayer("UI"));
            screenCamera.cullingMask = screenCamera.cullingMask | (1 << _uiPlayerNameplateLayer);

            _newCullingMask = screenCamera.cullingMask;

            _menuCameraClone = new GameObject();
            _menuCameraClone.transform.parent = screenCamera.transform.parent;

            _menuCameraUI = _menuCameraClone.AddComponent<Camera>();
            _menuCameraUI.cullingMask = (1 << LayerMask.NameToLayer("UiMenu")) | (1 << LayerMask.NameToLayer("UI"));
            _menuCameraUI.clearFlags = CameraClearFlags.Depth;

            _uiLayer = LayerMask.NameToLayer("UI");
            _uiMenuLayer = LayerMask.NameToLayer("UiMenu");
            _playerLocalLayer = LayerMask.NameToLayer("PlayerLocal");
            _playerLayer = LayerMask.NameToLayer("Player");

            GameObject loadingScreenOverlayPanel = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel");
            SetLayerRecursively(loadingScreenOverlayPanel.transform, _uiMenuLayer, -1);

            GameObject userCamera = GameObject.Find("/_Application/TrackingVolume/PlayerObjects/UserCamera");
            SetLayerRecursively(userCamera.transform, _playerLocalLayer, _uiLayer);
            SetLayerRecursively(userCamera.transform, _playerLocalLayer, _uiMenuLayer);

            typeof(VRCUiBackgroundFade).GetMethods(BindingFlags.Public | BindingFlags.Instance)
               .Where(m => m.Name.Contains("Method_Public_Void_Single_Action") && !m.Name.Contains("PDM"))
               .ToList().ForEach(m => PatchManager.Instance.Patch(m, postfix: new HarmonyMethod(typeof(AntiMenuOverrender).GetMethod("OnFade", BindingFlags.Static | BindingFlags.NonPublic))));



            typeof(SimpleAvatarPedestal).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.Name.Contains("Method_Private_Void_GameObject"))
                .ToList().ForEach(m => PatchManager.Instance.Patch(m, postfix: new HarmonyMethod(typeof(AntiMenuOverrender).GetMethod("OnAvatarScale", BindingFlags.Static | BindingFlags.NonPublic))));

            AntiOverrenderToggle = false;
            _menuCameraClone.SetActive(false);
            _originalCamera.cullingMask = _originalCullingMask;
        }

        public void Update()
        {
            if (_menuCameraClone != null && _menuCameraClone.activeSelf)
            {
                _menuCameraClone.transform.localPosition = _originalCamera.transform.localPosition;

                if (_menuCameraUI != null)
                {
                    _menuCameraUI.nearClipPlane = _originalCamera.nearClipPlane;
                    _menuCameraUI.farClipPlane = _originalCamera.farClipPlane;
                }
            }
        }

        private static void OnAvatarScale(ref SimpleAvatarPedestal __instance, GameObject __0)
        {
            if (AntiOverrenderToggle && __instance != null && __0 != null)
            {
                if (__0.transform.parent.gameObject.name.Equals("AvatarModel"))
                {
                    SetLayerRecursively(__0.transform.parent, _uiMenuLayer, _playerLocalLayer);
                }
                else
                {
                    SetLayerRecursively(__0.transform, _uiMenuLayer, _playerLocalLayer);
                }
            }
        }

        private static void OnFade()
        {
            if (_menuCameraClone != null)
            {
                _menuCameraUI.clearFlags = CameraClearFlags.Depth;

                foreach (PostProcessLayer postProcessLayer in _menuCameraClone.GetComponents<PostProcessLayer>())
                    Destroy(postProcessLayer);
            }
        }

        public static void SetLayerRecursively(Transform obj, int newLayer, int match)
        {
            if (obj.gameObject.name.Equals("SelectRegion"))
            {
                return;
            }

            if (obj.gameObject.layer == match || match == -1)
            {
                obj.gameObject.layer = newLayer;
            }

            foreach (var o in obj)
            {
                var otherTransform = o.Cast<Transform>();
                SetLayerRecursively(otherTransform, newLayer, match);
            }
        }
        public AntiMenuOverrender(IntPtr ptr) : base(ptr) { }
    }
}
