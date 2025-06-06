using UnityEngine;
using WengaPort.Modules;
using VRC.Core;
using WengaPort.Extensions;
using System;
using WengaPort.Utility;
using MenuType = WengaPort.Utility.MenuType;
using WengaPort.Wrappers;
using System.Diagnostics;
using VRC.UI;
using UnityEngine.UI;
using WengaPort.FoldersManager;
using VRC.SDKBase;
using VRC;
using System.Collections;
using Newtonsoft.Json;
using MelonLoader;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace WengaPort.Buttons
{
    class SocialMenu
    {
        public static void Initialize()
        {
            new MenuButton(MenuType.UserInfo, MenuButtonType.PlaylistButton, "Avatar Author", 585f, -280f, new Action(() =>
            {
                GetAvatarFromSocial();
            }));

            new MenuButton(MenuType.UserInfo, MenuButtonType.PlaylistButton, "Drop Portal", -100f, -280f, new Action(() =>
            {
                APIUser user = PatchManager.currentUserSocial;
                if (user.location.Contains("wrld_"))
                {
                    PortalHandler.DropPortal(user.location);
                }
            }));

            new MenuButton(MenuType.UserInfo, MenuButtonType.PlaylistButton, "Check Instance", 355f, -280f, new Action(() =>
            {
                if (PhotonMenu.Ini.GetBool("Toggles", "Bot", true))
                {
                    APIUser user = PatchManager.currentUserSocial;
                    if (user.location.Contains("wrld_"))
                    {
                        Extensions.Logger.WengaLogger("Checking " + user.location);
                        PhotonMenu.Ini.SetString("Toggles", "SocialInstance", user.location);
                        PhotonMenu.Ini.SetBool("Toggles", "WhoInInstance", true);
                    }
                }
                else
                {
                    Extensions.Logger.WengaLogger("Start the Photonbots before using this feature");
                }
            }));

            new MenuButton(MenuType.WorldInfoMenu, MenuButtonType.ReportButton, "Check Instance", 65f, -145f, new Action(() =>
            {
                if (PhotonMenu.Ini.GetBool("Toggles", "Bot", true))
                {
                    var WorldScreen = GameObject.Find("Screens").transform.Find("WorldInfo");
                    var Infos = WorldScreen.transform.GetComponentInChildren<PageWorldInfo>();

                    var wid = Infos.field_Private_ApiWorld_0.id + ":" + Infos.field_Public_ApiWorldInstance_0.idWithTags;  
                    if (wid.Contains("wrld_"))
                    {
                        Extensions.Logger.WengaLogger("Checking " + wid);
                        PhotonMenu.Ini.SetString("Toggles", "SocialInstance", wid);
                        PhotonMenu.Ini.SetBool("Toggles", "WhoInInstance", true);
                    }
                }
                else
                {
                    Extensions.Logger.WengaLogger("Start the Photonbots before using this feature");
                }
            }));

            new MenuButton(MenuType.WorldInfoMenu, MenuButtonType.ReportButton, "Reupload World", 65f, -255f, new Action(() =>
            {
                Modules.Reupload.ReuploaderButtons.ReuploadWorldAction();
            }));

            new MenuButton(MenuType.UserInfo, MenuButtonType.PlaylistButton, "Open VRChat", 125f, -280f, new Action(() =>
            {
                Process.Start("https://vrchat.com/home/user/" + PatchManager.currentUserSocial.UserID());
            }));

            Button button2 = GameObject.Find("/UserInterface/MenuContent/Screens/UserInfo/User Panel/OnlineFriend/JoinButton").GetComponent<Button>();

            if (PatchManager.QuestSpoof || Create.Ini.GetBool("Toggles", "QuestSpoof", true))
            {
                button2.onClick.RemoveAllListeners();
                button2.onClick.AddListener(new Action(() =>
                {
                    if (PatchManager.currentUserSocial != null)
                    {
                        APIUser user = PatchManager.currentUserSocial;
                        if (user.location.Contains("wrld_"))
                        {
                            Networking.GoToRoom(user.location);
                        }
                    }
                }));
            }
        }
        public static void GetAvatarFromSocial()
        {
            void OnSuccess(APIUser user)
            {
                GameObject gameObject = GameObject.Find("UserInterface/MenuContent/Screens/UserInfo");
                VRCUiPage vrcUiPage = gameObject.GetComponent<VRCUiPage>();
                VRCUiManager.prop_VRCUiManager_0.Method_Public_VRCUiPage_VRCUiPage_0(vrcUiPage);
                vrcUiPage.Cast<PageUserInfo>().Method_Public_Void_APIUser_PDM_0(user);
            }
            if (PatchManager.avatarLink == null) return;
            else if (!PatchManager.canGet)
            {
                Extensions.Logger.WengaLogger("You have to wait a bit before making a API call again");
                return;
            }
            MelonCoroutines.Start(APIDelay());

            WebRequest request = WebRequest.Create(PatchManager.avatarLink);
            WebResponse response = request.GetResponse();
            if (((HttpWebResponse)response).StatusCode != HttpStatusCode.OK) return;
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                JObject jsonData = (JObject)JsonSerializer.CreateDefault().Deserialize(streamReader, typeof(JObject));

                JsonData requestedData = jsonData.ToObject<JsonData>();
                //Extensions.Logger.WengaLogger(JsonConvert.SerializeObject(jsonData, Formatting.Indented));
                APIUser.FetchUser(requestedData.ownerId, new Action<APIUser>(OnSuccess), new Action<string>((thing) => { }));
            }

            response.Close();
        }
        public static IEnumerator APIDelay()
        {
            PatchManager.canGet = false;
            yield return new WaitForSeconds(35);
            PatchManager.canGet = true;
            yield break;
        }
    }
    public class JsonData
    {
        [JsonProperty("ownerId")]
        public string ownerId;
    }
}
