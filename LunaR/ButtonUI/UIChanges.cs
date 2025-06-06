using LunaR.Modules;
using LunaR.Wrappers;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using VRC.Core;
using VRC.UI;

namespace LunaR.Buttons
{
    internal class UIChanges
    {
        public static void ChangeTrustColors()
        {
            VRCPlayer.field_Internal_Static_Color_0 = new Color(0.72f, 0.13f, 0.11f); // Nuisance
            VRCPlayer.field_Internal_Static_Color_1 = Color.yellow; // Friends
            VRCPlayer.field_Internal_Static_Color_2 = new Color(0.3f, 0.3f, 0.3f); // Visitor
            VRCPlayer.field_Internal_Static_Color_3 = new Color(0f, 1f, 1f); // New User
            VRCPlayer.field_Internal_Static_Color_4 = new Color(0f, 1f, 0f); // User
            VRCPlayer.field_Internal_Static_Color_5 = new Color(1f, 0.39f, 0f); // Known
            VRCPlayer.field_Internal_Static_Color_6 = new Color(0.87f, 0, 0.51f); // Trusted
            VRCPlayer.field_Internal_Static_Color_8 = Color.red; // MOD
        }

        public static void ColorScreens()
        {
            foreach (CanvasRenderer btn in GameObject.Find("UserInterface/MenuContent").GetComponentsInChildren<CanvasRenderer>(true))
            {
                if (btn.GetComponent<Image>() && (!btn.name.ToLower().Contains("icon") || btn.name == "UploadUserIcon" || btn.name == "ToggleIcon"))
                {
                    switch(ColorUtility.ToHtmlStringRGB(btn.GetComponent<Image>().color))
                    {
                        case "FFFFFF":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#1b1c21");
                            break;

                        case "002527":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#1a1b24");
                            break;

                        case "008389":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#2a3380");
                            break;

                        case "40FFEF":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#3d1ca3");
                            break;

                        case "0EA6AD":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#271978");
                            break;

                        case "004245":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#1b1c21") / 1.11f;
                            break;

                        case "103737":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#1b1c21");
                            break;

                        case "0B181C":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#15151a");
                            break;

                        case "072F30":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#1b1b24");
                            break;

                        case "1B788B":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#271978");
                            break;

                        case "6AE3F9":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#271978");
                            break;

                        case "31DDFF":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#271978");
                            break;

                        case "001516":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#1b1c21");
                            break;

                        case "10A5A5":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#271978");
                            break;

                        case "2DFFF6":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#271978");
                            break;

                        case "4D807D":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#3b3b47");
                            break;

                        case "00969F":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#2a2c3b");
                            break;

                        case "016C6D":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#3b3b47");
                            break;

                        case "00737A":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#1b1c21");
                            break;

                        case "4A9384":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#1b1c21");
                            break;

                        case "164960":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#3b3b47");
                            break;

                        case "086064":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#3b3b47");
                            break;

                        case "005F63":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#3b3b47");
                            break;

                        case "00FFFF":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#271978");
                            break;

                        case "00B7FF":
                            btn.GetComponent<Image>().color = GeneralWrappers.GetColor("#271978");
                            break;

                        default:
                            //Extensions.Logger.LogError(ColorUtility.ToHtmlStringRGB(btn.GetComponent<Image>().color));
                            break;
                    }
                }
                if (btn.GetComponent<Text>()) btn.GetComponent<Text>().color = GeneralWrappers.GetColor("#919191");
                if (btn.GetComponent<Outline>()) btn.GetComponent<Outline>().enabled = false;
                if (btn.GetComponent<Button>())
                {
                    Color Background = GeneralWrappers.GetColor("#4f4f4f");
                    btn.GetComponent<Button>().colors = new ColorBlock()
                    {
                        colorMultiplier = 5,
                        disabledColor = Color.black,
                        highlightedColor = GeneralWrappers.GetColor("#2c0073") * 4,
                        normalColor = Background,
                        pressedColor = Color.black,
                        fadeDuration = 0.35f,
                        selectedColor = Background
                    };
                }
                if (btn.name == "Image_NEW") btn.transform.localScale = Vector3.zero;
            }
        }

        public static void HudElements()
        {
            Color Override = GeneralWrappers.GetColor("#2e58ff");
            foreach (PedalGraphic pedalGraphic in Resources.FindObjectsOfTypeAll<PedalGraphic>())
            {
                pedalGraphic.color = Override;
            }
            foreach (HudVoiceIndicator VoiceIcon in UnityEngine.Object.FindObjectsOfType<HudVoiceIndicator>())
            {   
                VoiceIcon.field_Private_Image_0.color = Override;
                VoiceIcon.field_Private_Image_1.color = Override;
            }

            GameObject.Find("UserInterface/UnscaledUI/HudContent_Old/Hud/VoiceDotParent/VoiceDotDisabled").GetComponent<FadeCycleEffect>().enabled = false;
            //GameObject.Find("UserInterface/UnscaledUI/HudContent/HUD_UI 2/VR Canvas/Container/Left/Icons/Mic/Icon").GetComponent<Image>().color = Color.blue;
        }

        public static void ChangePages()
        {
            GameObject.Find("UserInterface/PlayerDisplay/BlackFade/inverted_sphere").SetActive(false);
            GameObject.Find("/UserInterface/MenuContent/Screens/UserInfo/Buttons/RightSideButtons/RightUpperButtonColumn/PlaylistsButton").transform.localScale = Vector3.zero;
            GameObject.Find("/UserInterface/MenuContent/Screens/UserInfo/Buttons/RightSideButtons/RightUpperButtonColumn/EditStatusButton").transform.localScale = Vector3.zero;
            GameObject.Find("/UserInterface/MenuContent/Screens/UserInfo/ViewUserOnVRChatWebsiteButton").transform.localScale = Vector3.zero;
            GameObject.Find("/UserInterface/MenuContent/Backdrop/Backdrop/Background").GetComponent<Image>().color /= 1.135f;
            Texture2D texture = new(2, 2);
            ImageConversion.LoadImage(texture, File.ReadAllBytes("LunaR\\Background.jpg"));
            var rect = new Rect(0.0f, 0.0f, texture.width, texture.height);
            var pivot = new Vector2(0.5f, 0.5f);
            var border = Vector4.zero;
            Sprite NewSprite = Sprite.CreateSprite_Injected(texture, ref rect, ref pivot, 100.0f, 0, SpriteMeshType.Tight, ref border, false);
            GameObject.Find("/UserInterface/MenuContent/Backdrop/Backdrop/Background").GetComponent<Image>().sprite = NewSprite;
            GameObject.Find("/UserInterface/MenuContent/Backdrop/Backdrop/EarlyAccessText").SetActive(true);
            GameObject.Find("/UserInterface/MenuContent/Backdrop/Backdrop/EarlyAccessText").GetComponent<I2.Loc.Localize>().enabled = false;
            GameObject.Find("/UserInterface/MenuContent/Backdrop/Backdrop/EarlyAccessText").GetComponent<Text>().text = "LunaR";
            GameObject.Find("/UserInterface/MenuContent/Backdrop/Backdrop/Image").SetActive(false);

            GameObject AvatarWorldList = GameObject.Find("UserInterface/MenuContent/Screens/Avatar/Vertical Scroll View/Viewport/Content/Avatar Worlds (Random)");
            GameObject AvatarHotList = GameObject.Find("UserInterface/MenuContent/Screens/Avatar/Vertical Scroll View/Viewport/Content/Avatar Worlds (What's Hot)");
            GameObject LegacyAvatarList = GameObject.Find("UserInterface/MenuContent/Screens/Avatar/Vertical Scroll View/Viewport/Content/Legacy Avatar List");
            GameObject PublicAvatarList = GameObject.Find("UserInterface/MenuContent/Screens/Avatar/Vertical Scroll View/Viewport/Content/Public Avatar List");

            AvatarWorldList.GetComponent<UiWorldList>().enabled = false;
            AvatarHotList.GetComponent<UiWorldList>().enabled = false;
            LegacyAvatarList.GetComponent<UiAvatarList>().enabled = false;
            PublicAvatarList.GetComponent<UiAvatarList>().enabled = false;

            AvatarWorldList.transform.localScale = Vector3.zero;
            AvatarHotList.transform.localScale = Vector3.zero;
            LegacyAvatarList.transform.localScale = Vector3.zero;
            PublicAvatarList.transform.localScale = Vector3.zero;
        }

        public static void CreatePasteButton()
        {
            GameObject keybardPasteButton = UnityEngine.Object.Instantiate(GameObject.Find("/UserInterface/MenuContent/Popups/InputPopup/ButtonLeft"), Utils.VRCUiPopupManager.field_Public_VRCUiPopupInput_0.transform);
            keybardPasteButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(335f, -275f);
            keybardPasteButton.GetComponentInChildren<Text>().text = "Paste";
            keybardPasteButton.name = "KeyboardPasteButton";
            keybardPasteButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            keybardPasteButton.GetComponent<Button>().m_Interactable = true;
            keybardPasteButton.GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                try
                {
                    if (GUIUtility.systemCopyBuffer.Length < 256) GameObject.Find("UserInterface/MenuContent/Popups/InputPopup/InputField").GetComponent<InputField>().text = GUIUtility.systemCopyBuffer;
                }
                catch { }
            }));
        }

        public static void CreateRestartButton()
        {
            GameObject RealSettingsExit, SettingsExit, SettingsRestart;
            RealSettingsExit = GameObject.Find("UserInterface/MenuContent/Screens/Settings/Footer/Exit").gameObject;
            RealSettingsExit.SetActive(false);

            SettingsExit = UnityEngine.Object.Instantiate(GameObject.Find("UserInterface/MenuContent/Screens/Settings/Footer/Exit"), GameObject.Find("UserInterface/MenuContent/Screens/Settings/Footer").transform);
            SettingsExit.GetComponentInChildren<Text>().text = "EXIT";
            SettingsExit.name = "ExitButton";
            SettingsExit.GetComponent<RectTransform>().localPosition = new Vector2(-90f, -456f);
            SettingsExit.GetComponent<RectTransform>().sizeDelta -= new Vector2(150.0f, 0.0f);
            SettingsExit.SetActive(true);
            SettingsExit.GetComponentInChildren<Button>().onClick.AddListener(new Action(() =>
            RealSettingsExit.GetComponent<Button>().onClick.Invoke()));

            SettingsRestart = UnityEngine.Object.Instantiate(GameObject.Find("UserInterface/MenuContent/Screens/Settings/Footer/Exit"), GameObject.Find("UserInterface/MenuContent/Screens/Settings/Footer").transform);
            SettingsRestart.transform.SetParent(GameObject.Find("UserInterface/MenuContent/Screens/Settings/Footer/").transform);
            SettingsRestart.name = "RestartButton";
            SettingsRestart.GetComponentInChildren<Text>().text = "RESTART";
            SettingsRestart.GetComponent<RectTransform>().localPosition = new Vector2(90f, -456f);
            SettingsRestart.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 109f);
            SettingsRestart.SetActive(true);
            SettingsRestart.GetComponentInChildren<Button>().onClick.AddListener(new Action(() =>
            {
                GeneralWrappers.RestartGame();
            }));
        }

        public static void FixButtons()
        {
            Button Invite = GameObject.Find("UserInterface/MenuContent/Screens/UserInfo/OnlineFriendButtons/Invite").GetComponent<Button>();
            GameObject.Find("UserInterface/MenuContent/Screens/UserInfo").AddComponent<EnableDisableListener>().OnEnableEvent += () =>
            {
                Utils.DelayAction(0.1f, delegate
                {
                    if (RoomManager.field_Internal_Static_ApiWorldInstance_0?.type != InstanceAccessType.InviteOnly)
                    {
                        Invite.interactable = true;
                        Invite.m_Interactable = true;
                    }
                }).Start();
            };
        }

        public static void ChangeLoadingPicture()
        {
            GameObject mainFrame = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel/InfoPanel_Template_ANIM/SCREEN/mainFrame");
            GameObject screen = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel/InfoPanel_Template_ANIM/SCREEN/mainScreen");
            GameObject parentScreen = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel/InfoPanel_Template_ANIM/SCREEN");
            Renderer screenRender = screen.GetComponent<Renderer>();
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Plane);
            cube.transform.SetParent(parentScreen.transform);
            cube.transform.rotation = screen.transform.rotation;
            cube.transform.localPosition = new Vector3(0, 0, -0.19f);
            cube.GetComponent<Collider>().enabled = false;
            cube.layer = LayerMask.NameToLayer("InternalUI");
            Texture2D texture = new(2, 2);
            ImageConversion.LoadImage(texture, File.ReadAllBytes("LunaR\\LoadingPicture.jpg"));
            Renderer pic = cube.GetComponent<Renderer>();
            pic.material.mainTexture = texture;
            screenRender.enabled = false;
            if (pic.material.mainTexture.height > pic.material.mainTexture.width)
            {
                cube.transform.localScale = new Vector3(0.099f, 1, 0.175f);
                mainFrame.transform.localScale = new Vector3(10.80f, 19.20f, 1);
            }
            else
            {
                cube.transform.localScale = new Vector3(0.175f, 1, 0.099f);
                mainFrame.transform.localScale = new Vector3(19.20f, 10.80f, 1);
            }
            GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel/InfoPanel_Template_ANIM/ICON").active = false;
            GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel/InfoPanel_Template_ANIM/TITLE").active = false;

            var Border = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel/InfoPanel_Template_ANIM/SCREEN/mainFrame").GetComponent<MeshRenderer>();
            var PointLight = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/_Lighting (1)/Point light").GetComponent<Light>();
            var ReflectionProbe1 = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/_Lighting (1)/Reflection Probe").GetComponent<ReflectionProbe>();
            ReflectionProbe1.mode = ReflectionProbeMode.Realtime;
            ReflectionProbe1.backgroundColor = new Color(0.4006691f, 0, 1, 0);
            Material BorderMaterial = new(Shader.Find("Standard"));
            Border.material = BorderMaterial;
            Border.material.color = Color.black;
            Border.material.SetFloat("_Metallic", 1);
            Border.material.SetFloat("_SmoothnessTextureChar", 1);
            PointLight.color = Color.white;

            var Snow = GameObject.Find("/UserInterface/LoadingBackground_TealGradient_Music/_FX_ParticleBubbles/FX_snow").GetComponent<ParticleSystem>();
            var SnowRenderer = GameObject.Find("/UserInterface/LoadingBackground_TealGradient_Music/_FX_ParticleBubbles/FX_snow").GetComponent<ParticleSystemRenderer>();

            Material TrailMaterial = new(Shader.Find("UI/Default"))
            {
                color = Color.white
            };

            SnowRenderer.trailMaterial = TrailMaterial;
            SnowRenderer.material.color = Color.white;
            Snow.gameObject.transform.position -= new Vector3(4, 3, 4);

            //Trail
            Snow.trails.enabled = true;
            Snow.trails.mode = ParticleSystemTrailMode.PerParticle;
            Snow.trails.ratio = 1;
            Snow.trails.lifetime = 0.04f;
            Snow.trails.minVertexDistance = 0;
            Snow.trails.worldSpace = false;
            Snow.trails.dieWithParticles = true;
            Snow.trails.textureMode = ParticleSystemTrailTextureMode.RepeatPerSegment;
            Snow.trails.sizeAffectsWidth = true;
            Snow.trails.sizeAffectsLifetime = false;
            Snow.trails.inheritParticleColor = false;
            Snow.trails.colorOverLifetime = Color.white;
            Snow.trails.widthOverTrail = 0.1f;
            Snow.trails.colorOverTrail = new Color(0.02987278f, 0, 0.3962264f, 0.5f);

            //MainParticle
            Snow.shape.scale = new Vector3(1, 1, 1.82f);
            Snow.main.startColor.mode = ParticleSystemGradientMode.Color;
            Snow.colorOverLifetime.enabled = false;
            Snow.main.prewarm = false;
            Snow.playOnAwake = true;
            Snow.startColor = Color.blue;
            Snow.noise.frequency = 1;
            Snow.noise.strength = 0.5f;
            Snow.maxParticles = 250;
            Snow.gameObject.SetActive(false);

            GameObject.Find("/UserInterface/LoadingBackground_TealGradient_Music/_FX_ParticleBubbles/FX_CloseParticles").GetComponent<ParticleSystem>().startColor = Color.blue;
            GameObject.Find("/UserInterface/LoadingBackground_TealGradient_Music/_FX_ParticleBubbles/FX_floor").GetComponent<ParticleSystem>().startColor = Color.blue;

            var Snow3 = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/_FX_ParticleBubbles/FX_snow").GetComponent<ParticleSystem>();
            var Snow4 = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/_FX_ParticleBubbles/FX_snow").GetComponent<ParticleSystemRenderer>();

            Snow3.gameObject.transform.position -= new Vector3(4, 3, 4);
            TrailMaterial.color = Color.white;
            Snow4.trailMaterial = TrailMaterial;
            Snow4.material.color = Color.white;

            //Trail
            Snow3.trails.enabled = true;
            Snow3.trails.mode = ParticleSystemTrailMode.PerParticle;
            Snow3.trails.ratio = 1;
            Snow3.trails.lifetime = 0.04f;
            Snow3.trails.minVertexDistance = 0;
            Snow3.trails.worldSpace = false;
            Snow3.trails.dieWithParticles = true;
            Snow3.trails.textureMode = ParticleSystemTrailTextureMode.RepeatPerSegment;
            Snow3.trails.sizeAffectsWidth = true;
            Snow3.trails.sizeAffectsLifetime = false;
            Snow3.trails.inheritParticleColor = false;
            Snow3.trails.colorOverLifetime = Color.white;
            Snow3.trails.widthOverTrail = 0.1f;
            Snow3.trails.colorOverTrail = new Color(0.02987278f, 0, 0.3962264f, 0.5f);

            //MainParticle
            Snow3.shape.scale = new Vector3(1, 1, 1.82f);
            Snow3.main.startColor.mode = ParticleSystemGradientMode.Color;
            Snow3.colorOverLifetime.enabled = false;
            Snow3.main.prewarm = false;
            Snow3.playOnAwake = true;
            Snow3.startColor = Color.blue;
            Snow3.noise.frequency = 1;
            Snow3.noise.strength = 0.5f;
            Snow3.maxParticles = 250;

            GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/_FX_ParticleBubbles/FX_CloseParticles").GetComponent<ParticleSystem>().startColor = Color.blue;
            GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/_FX_ParticleBubbles/FX_floor").GetComponent<ParticleSystem>().startColor = Color.blue;
            GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Loading Elements/LOADING_BAR_BG").GetComponent<Image>().color = Color.blue;
            GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Loading Elements/LOADING_BAR").GetComponent<Image>().color = Color.blue * 5;
            GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Panel_Backdrop").GetComponent<Image>().color = Color.blue * 5;
            GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Decoration_Right").GetComponent<Image>().color = Color.blue * 5;
            GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Decoration_Left").GetComponent<Image>().color = Color.blue * 5; ;
        }

        public static void ChangeVRLaser()
        {
            HandDotCursor LeftHand = VRCUiCursorManager.field_Private_Static_VRCUiCursorManager_0.transform.Find("DotLeftHand").GetComponent<HandDotCursor>();
            HandDotCursor RightHand = VRCUiCursorManager.field_Private_Static_VRCUiCursorManager_0.transform.Find("DotRightHand").GetComponent<HandDotCursor>();
            SpriteRenderer LeftDot = LeftHand.GetComponentInChildren<SpriteRenderer>(true);
            SpriteRenderer RightDot = RightHand.GetComponentInChildren<SpriteRenderer>(true);
            RightHand.field_Public_Color_0 = Color.blue;
            RightHand.field_Public_Color_1 = Color.blue;
            RightDot.color = Color.magenta;
            LeftHand.field_Public_Color_0 = Color.blue;
            LeftHand.field_Public_Color_1 = Color.blue;
            LeftDot.color = Color.magenta;
            GameObject Curor = GameObject.Find("_Application/CursorManager/MouseArrow/VRCUICursorIcon");
            Curor.GetComponent<SpriteRenderer>().color = Color.magenta;
        }

        public static void ChangeClipping()
        {
            Camera.main.nearClipPlane = 0.001f;
            VRCVrCamera.field_Private_Static_VRCVrCamera_0.field_Public_Camera_0.nearClipPlane = 0.001f;
        }
    }
}