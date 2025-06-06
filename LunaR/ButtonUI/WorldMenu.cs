using LunaR.Buttons.Bots;
using LunaR.Extensions;
using LunaR.Modules;
using LunaR.UIApi;
using LunaR.Wrappers;
using System;
using System.Linq;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using VRC.UI;

namespace LunaR.ButtonUI
{
    internal class WorldMenu
    {
        public static void SetupWorldPage()
        {
            GameObject WorldImage = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo/WorldImage");
            GameObject PlatformIcons = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo/WorldImage/OverlayIcons");
            GameObject InstancePanel = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo/WorldImage/Panel");

            PlatformIcons.SetActive(false);
            InstancePanel.SetActive(false);

            WorldImage.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            WorldImage.transform.localPosition = new Vector3(-510, 230, -2);

            GameObject PortalButton = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo/WorldButtons/PortalButton");
            GameObject GoButton = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo/WorldButtons/GoButton");

            PortalButton.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
            PortalButton.transform.localPosition = new Vector3(-180, 150, 0);
            GoButton.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
            GoButton.transform.localPosition = new Vector3(-415, 150, 0);

            GameObject FavoriteButton = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo/FavoriteButton");
            GameObject MakeHomeButton = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo/MakeHomeButton");
            GameObject ResetHomeButton = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo/ResetHomeButton");
            GameObject ReportButton = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo/ReportButton");

            FavoriteButton.transform.localPosition = new Vector3(550, 340, 0);
            FavoriteButton.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
            MakeHomeButton.transform.localPosition = new Vector3(550, 240, 0);
            MakeHomeButton.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
            ReportButton.transform.localPosition = new Vector3(550, 140, 0);
            ReportButton.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
            ResetHomeButton.SetActive(false);

            GameObject DetailsPanel = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo/Panels/DetailsPanel");
            DetailsPanel.transform.localScale = Vector3.zero;

            GameObject PanelBackground = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo/Panels/PanelBackground");
            PanelBackground.transform.localPosition = new Vector3(275, 60, 0);
            PanelBackground.transform.localScale = new Vector3(0.7f, 1.42f, 1);

            GameObject NewPanel = UnityEngine.Object.Instantiate(PanelBackground, GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo/Panels").transform);
            NewPanel.transform.localPosition = new Vector3(-55, 60, 0);
            NewPanel.transform.localScale = new Vector3(0.7f, 1.42f, 1);

            MenuButton CheckInstance = new(GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo"), MenuButtonType.ReportButton, "Check Instance", 0, 0, new Action(() =>
            {
                Transform WorldScreen = GameObject.Find("Screens").transform.Find("WorldInfo");
                PageWorldInfo Infos = WorldScreen.transform.GetComponentInChildren<PageWorldInfo>();

                string wid = Infos.field_Private_ApiWorld_0.id + ":" + Infos.field_Public_ApiWorldInstance_0.instanceId;
                if (wid.StartsWith("wrld_")) ServerRequester.SendCheckInstance(wid);
            }));
            CheckInstance.Button.transform.localPosition = new Vector3(550, 40, 0);
            CheckInstance.Button.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);

            MenuButton CopyWorld = new(GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo"), MenuButtonType.ReportButton, "Copy WorldID", 0, 0, new Action(() =>
            {
                Transform WorldScreen = GameObject.Find("Screens").transform.Find("WorldInfo");
                PageWorldInfo Infos = WorldScreen.transform.GetComponentInChildren<PageWorldInfo>();

                string WID = $"{Infos.field_Private_ApiWorld_0.id}:{Infos.field_Public_ApiWorldInstance_0.instanceId}";
                Clipboard.SetText(WID);
                Extensions.Logger.Log(WID, Extensions.Logger.LogsType.Info);
            }));
            CopyWorld.Button.transform.localPosition = new Vector3(550, -60, 0);
            CopyWorld.Button.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);

            MenuButton DownloadWorld = new(GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo"), MenuButtonType.ReportButton, "Download File", 0, 0, new Action(() =>
            {
                Transform WorldScreen = GameObject.Find("Screens").transform.Find("WorldInfo");
                PageWorldInfo Infos = WorldScreen.transform.GetComponentInChildren<PageWorldInfo>();
                RippingHandler.DownloadWorld(Infos.field_Private_ApiWorld_0).Start();
            }));
            DownloadWorld.Button.transform.localPosition = new Vector3(550, -160, 0);
            DownloadWorld.Button.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);

            MenuButton ShowAuthor = new(GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo"), MenuButtonType.ReportButton, "Author", 0, 0, new Action(() =>
            {
                Transform WorldScreen = GameObject.Find("Screens").transform.Find("WorldInfo");
                PageWorldInfo Infos = WorldScreen.transform.GetComponentInChildren<PageWorldInfo>();
                VRCUiPage vrcUiPage = GameObject.Find("UserInterface/MenuContent/Screens/UserInfo").GetComponent<VRCUiPage>();
                Utils.VRCUiManager.Method_Public_VRCUiPage_VRCUiPage_0(vrcUiPage);
                vrcUiPage.Cast<PageUserInfo>().Method_Public_Void_APIUser_PDM_0(Infos.field_Public_APIUser_0);
            }));
            ShowAuthor.Button.transform.localPosition = new Vector3(550, -260, 0);
            ShowAuthor.Button.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);

            NewPanel.AddComponent<Mask>();

            GameObject TextContainer = UnityEngine.Object.Instantiate(new GameObject(), NewPanel.transform);
            RectTransform ContainerTransform = TextContainer.AddComponent<RectTransform>();

            ContainerTransform.localPosition = new Vector3(55, 180, 0);
            ContainerTransform.localScale = new Vector3(1.35f, 0.7f, 1);

            WorldPanel1 = new UIMenuText(TextContainer, "", new Vector2(0, 0), 18);
            WorldPanel1.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 255);

            ScrollRect Rect = NewPanel.AddComponent<ScrollRect>();
            Rect.content = TextContainer.GetComponent<RectTransform>();
            Rect.viewport = NewPanel.GetComponent<RectTransform>();
            Rect.vertical = true;
            Rect.horizontal = false;
            Rect.movementType = ScrollRect.MovementType.Elastic;
            Rect.elasticity = 0.2f;

            GameObject WorldInfo = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo");
            WorldPanel2 = new UIMenuText(WorldInfo, "", new Vector2(300, 310), 18);
        }

        private static UIMenuText WorldPanel1;
        private static UIMenuText WorldPanel2;

        public static void UpdatPanel1(string Infos)
        {
            WorldPanel1.SetText(Infos);
        }

        public static void UpdatPanel2(string Infos)
        {
            WorldPanel2.SetText(Infos);
        }

        public static void SetInfos()
        {
            PageWorldInfo WorldInfo = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo").GetComponent<PageWorldInfo>();
            if (WorldInfo.prop_ApiWorld_0 == null) return;

            if (GeneralWrappers.GetWorldID() == WorldInfo.field_Public_ApiWorldInstance_0.id)
            {
                string PanelString = "";

                foreach (VRC.Player Player in Utils.PlayerManager.GetAllPlayers())
                {
                    Color RankColor = Player.GetAPIUser().GetRankColor();

                    PanelString += Player.GetPhotonPlayer().ActorID() + " | " + $"<color={GeneralWrappers.ToRGBHex(RankColor)}>" + Player.DisplayName() + "</color>" + " | " + (Player.GetVRCPlayerApi().IsUserInVR() ? "VR" : "PC") + "\n";
                }
                UpdatPanel1(PanelString);
            }
            else UpdatPanel1("");

            WorldInfo.field_Public_ApiWorldInstance_0.Fetch(new Action<ApiContainer>((container) =>
            {
                ApiWorldInstance FullInstance = container.Model.TryCast<ApiWorldInstance>();
                string Infos = string.Concat(new string[]
                {
                    WorldInfo.prop_ApiWorld_0.name + "\n",
                    "\nAuthor: " + WorldInfo.prop_ApiWorld_0.authorName,
                    "\nCreated: " + WorldInfo.prop_ApiWorld_0.created_at.ToString("dd.MM.yyyy"),
                    "\nUpdated: " + WorldInfo.prop_ApiWorld_0.updated_at.ToString("dd.MM.yyyy"),
                    "\nPublic Players: " + WorldInfo.prop_ApiWorld_0.publicOccupants,
                    "\nPrivate Players: " + WorldInfo.prop_ApiWorld_0.privateOccupants,
                    "\nPlayers: " + FullInstance.count + " / " + WorldInfo.prop_ApiWorld_0.capacity * 2,
                    "\nPC: " + "<color=blue>" + FullInstance.platforms["standalonewindows"]+ "</color>",
                    "\nQuest: " + "<color=green>" + FullInstance.platforms["android"] + "</color>",
                    "\nModerator: " + (FullInstance.tags.Contains("show_mod_tag") ? "<color=red>Yes</color>" : "<color=lime>No</color>"),
                    "\nType: " + FullInstance.type,
                    "\nRegion: " + WorldInfo.field_Public_ApiWorldInstance_0.region,
                    "\nPlatforms: " + WorldInfo.prop_ApiWorld_0.supportedPlatforms,
                    "\nLanguages: \n" + string.Join(", ", FullInstance.tags.ToArray().Where(x => x.StartsWith("language_"))).Replace("language_", ""),
                    "\n\nTags: \n" + string.Join("\n", WorldInfo.prop_ApiWorld_0.tags.ToArray())
                });
                UpdatPanel2(Infos);
            }));

            UnityEngine.UI.Button JoinWorld = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo/WorldButtons/GoButton").GetComponent<UnityEngine.UI.Button>();
            UnityEngine.UI.Button PortalWorld = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo/WorldButtons/PortalButton").GetComponent<UnityEngine.UI.Button>();
            Utils.DelayAction(0.1f, delegate
            {
                JoinWorld.interactable = true;
                JoinWorld.m_Interactable = true;
                PortalWorld.interactable = true;
                PortalWorld.m_Interactable = true;
            }).Start();
        }
    }
}
