using MelonLoader;
using System;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.UI;

namespace WengaPort.Modules.Reupload
{
    class NameChanger
    {
        public static void ChangeAvatarDescription()
        {
            VRCUiManager vRCUiManager = VRCUiManager.prop_VRCUiManager_0;
            if (!vRCUiManager)
            {
                return;
            }
            GameObject gameObject = vRCUiManager.prop_VRCUiPopupManager_0.transform.Find("Screens/Avatar").gameObject;
            if (!gameObject)
            {
                return;
            }
            PageAvatar component = gameObject.GetComponent<PageAvatar>();
            if (!component)
            {
                return;
            }
            SimpleAvatarPedestal avatar = component.field_Public_SimpleAvatarPedestal_0;
            if (!avatar)
            {
                return;
            }
            ApiAvatar field_Internal_ApiAvatar_ = avatar.field_Internal_ApiAvatar_0;
            if (field_Internal_ApiAvatar_ != null && !(field_Internal_ApiAvatar_.authorId != APIUser.CurrentUser.id))
            {
                Extensions.Logger.WengaLogger("Enter new description below:");
                string description = Console.ReadLine();
                field_Internal_ApiAvatar_.description = description;
                field_Internal_ApiAvatar_.Save((Action<ApiContainer>)delegate
                {
                    Extensions.Logger.WengaLogger("Avatar Description changed");
                }, (Action<ApiContainer>)delegate
                {
                    Extensions.Logger.WengaLogger("Failed to change Avatar Description");
                });
            }
        }

        public static void ChangeAvatarImage()
        {
            VRCUiManager vRCUiManager = VRCUiManager.prop_VRCUiManager_0;
            if (!vRCUiManager)
            {
                return;
            }
            GameObject gameObject = vRCUiManager.prop_VRCUiPopupManager_0.transform.Find("Screens/Avatar").gameObject;
            if (!gameObject)
            {
                return;
            }
            PageAvatar component = gameObject.GetComponent<PageAvatar>();
            if (!component)
            {
                return;
            }
            SimpleAvatarPedestal avatar = component.field_Public_SimpleAvatarPedestal_0;
            if (!avatar)
            {
                return;
            }
            ApiAvatar apiAvatar_0 = avatar.field_Internal_ApiAvatar_0;
            if (apiAvatar_0 == null || apiAvatar_0.authorId != APIUser.CurrentUser.id)
            {
                return;
            }
            CursorLockMode lockState = Cursor.lockState;
            bool visible = Cursor.visible;
            Extensions.Logger.WengaLogger("Select new Image:");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            string text = OpenFileWindows.OpenfileDialog();
            if (!string.IsNullOrEmpty(text))
            {
                Cursor.lockState = lockState;
                Cursor.visible = visible;
                MelonLogger.Log(text);
                string name = apiAvatar_0.name;
                string text2 = apiAvatar_0.unityVersion.ToLower();
                string text3 = apiAvatar_0.platform.ToLower();
                string text4 = ApiWorld.VERSION.ApiVersion.ToString().ToLower();
                if (string.IsNullOrEmpty(text4))
                {
                    text4 = "4";
                }
                string text5 = "Avatar - " + name + " - Image - " + text2 + "_" + text4 + "_" + text3 + "_Release";
                string value = ApiFile.ParseFileIdFromFileAPIUrl(apiAvatar_0.imageUrl);
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                ApiFileHelper.upload(text, value, text5, delegate (ApiFile apiFile_0, string string_0)
                {
                    apiAvatar_0.imageUrl = apiFile_0.GetFileURL();
                    apiAvatar_0.Save((Action<ApiContainer>)delegate
                    {
                        Extensions.Logger.WengaLogger("Successfully changed Image");
                    }, (Action<ApiContainer>)delegate
                    {
                        Extensions.Logger.WengaLogger("Couldn't change avatar image (POST)");
                    });
                }, delegate (ApiFile apiFile_0, string string_0)
                {
                    Extensions.Logger.WengaLogger("Couldn't change avatar image (UPLOAD), " + string_0);
                }, delegate
                {
                }, (ApiFile apiFile_0) => false);
            }
            else
            {
                Extensions.Logger.WengaLogger("Couldn't open filedialog or path was null");
            }
        }

        public static void ChangeAvatarName()
        {
            VRCUiManager vRCUiManager = VRCUiManager.prop_VRCUiManager_0;
            if (!vRCUiManager)
            {
                return;
            }
            GameObject gameObject = vRCUiManager.prop_VRCUiPopupManager_0.transform.Find("Screens/Avatar").gameObject;
            if (!gameObject)
            {
                return;
            }
            PageAvatar component = gameObject.GetComponent<PageAvatar>();
            if (!component)
            {
                return;
            }
            SimpleAvatarPedestal avatar = component.field_Public_SimpleAvatarPedestal_0;
            if (!avatar)
            {
                return;
            }
            ApiAvatar field_Internal_ApiAvatar_ = avatar.field_Internal_ApiAvatar_0;
            if (field_Internal_ApiAvatar_ != null && !(field_Internal_ApiAvatar_.authorId != APIUser.CurrentUser.id))
            {
                Extensions.Logger.WengaLogger("Enter new name below:");
                string name = Console.ReadLine();
                field_Internal_ApiAvatar_.name = name;
                field_Internal_ApiAvatar_.Save((Action<ApiContainer>)delegate
                {
                    Extensions.Logger.WengaLogger("Avatar name changed");
                }, (Action<ApiContainer>)delegate
                {
                    Extensions.Logger.WengaLogger("Failed to change Avatar name");
                });
            }
        }

        public static void DeleteAvatar()
        {
            VRCUiManager vRCUiManager = VRCUiManager.prop_VRCUiManager_0;
            if (!vRCUiManager)
            {
                return;
            }
            GameObject gameObject = vRCUiManager.prop_VRCUiPopupManager_0.transform.Find("Screens/Avatar").gameObject;
            if (!gameObject)
            {
                return;
            }
            PageAvatar component = gameObject.GetComponent<PageAvatar>();
            if (!component)
            {
                return;
            }
            SimpleAvatarPedestal avatar = component.field_Public_SimpleAvatarPedestal_0;
            if (!avatar)
            {
                return;
            }
            ApiAvatar field_Internal_ApiAvatar_ = avatar.field_Internal_ApiAvatar_0;
            if (field_Internal_ApiAvatar_ == null || field_Internal_ApiAvatar_.authorId != APIUser.CurrentUser.id)
            {
                return;
            }
            field_Internal_ApiAvatar_.Delete((Action<ApiContainer>)delegate
            {
                Extensions.Logger.WengaLogger("Avatar deleted");
            }, (Action<ApiContainer>)delegate
            {
                Extensions.Logger.WengaLogger("Failed to delete Avatar");
            });
        }
    }
}
