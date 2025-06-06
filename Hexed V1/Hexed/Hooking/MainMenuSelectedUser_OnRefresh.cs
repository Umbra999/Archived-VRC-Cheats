//using CoreRuntime.Manager;
//using Hexed.Interfaces;
//using Hexed.Wrappers;
//using System.Globalization;
//using UnityEngine;
//using VRC.Core;
//using VRC.UI;
//using VRC.UI.Elements.Controls;
//using VRC.UI.Elements.Menus;

//namespace Hexed.Hooking
//{
//    internal class MainMenuSelectedUser_OnRefresh : IHook
//    {
//        private delegate void _OnRefresDelegate(nint instance, nint __0);
//        private static _OnRefresDelegate originalMethod;

//        public void Initialize()
//        {
//            originalMethod = HookManager.Detour<_OnRefresDelegate>(typeof(MainMenuSelectedUser).GetMethod(nameof(MainMenuSelectedUser.Method_Public_Void_IUser_0)), Patch);
//        }

//        private static void Patch(nint instance, nint __0)
//        {
//            originalMethod(instance, __0);

//            MainMenuSelectedUser menu = instance == nint.Zero ? null : new MainMenuSelectedUser(instance);

//            if (menu == null) return;

//            IUser user = __0 == nint.Zero ? menu.field_Private_IUser_0 : new IUser(__0);

//            if (user == null) return;

//            Object1PublicOb1ApStBo1StLoBoSiUnique IUser = user.TryCast<Object1PublicOb1ApStBo1StLoBoSiUnique>();
//            if (IUser == null || IUser.prop_TYPE_0 == null) return;

//            PlayerUtils.TrustRanks Rank = IUser.prop_TYPE_0.GetRank();
//            Color RankColor = Rank.GetRankColor();

//            TextMeshProUGUIEx UsernameTitle = menu.transform.Find("Header_MM_UserName/LeftItemContainer/Text_Title").GetComponent<TextMeshProUGUIEx>();
//            UsernameTitle.color = RankColor;
//            UsernameTitle.richText = true;
//            UsernameTitle.text = $"<color=#{ColorUtility.ToHtmlStringRGB(RankColor)}>{IUser.prop_TYPE_0.DisplayName()}</color>";

//            TextMeshProUGUIEx TrustText = menu.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup/Row1/Profile/DetailsArea/Field_Trust/Text_FieldContent").GetComponent<TextMeshProUGUIEx>();
//            TrustText.color = RankColor;
//            TrustText.richText = true;
//            TrustText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(RankColor)}>{Rank.GetRankString()} {(IUser.prop_TYPE_0.IsFriend() ? "(Friend)" : "")}</color>";

//            RawImageEx TrustIcon = menu.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup/Row1/Profile/DetailsArea/Field_Trust/Icon").GetComponent<RawImageEx>();
//            TrustIcon.color = RankColor;

//            if (IUser.prop_TYPE_0.date_joined == null)
//            {
//                APIUser.FetchUser(IUser.prop_TYPE_0.UserID(), new Action<APIUser>((user) =>
//                {
//                    OverridePlayerProfile(user, menu);
//                }), new Action<string>((error) =>
//                {
//                    OverridePlayerProfile(IUser.prop_TYPE_0, menu);
//                }));
//            }
//            else
//            {
//                OverridePlayerProfile(IUser.prop_TYPE_0, menu);
//            }
//        }

//        private static void OverridePlayerProfile(APIUser user, MainMenuSelectedUser menu)
//        {
//            string LastOnlineString = "";
//            if (user.last_login != null && user.last_login != "")
//            {
//                DateTime lastOnlineDateTime = DateTime.Parse(user.last_login, null, DateTimeStyles.RoundtripKind);
//                TimeSpan timeSinceLastOnline = DateTime.UtcNow - lastOnlineDateTime;

//                int Days = (int)timeSinceLastOnline.TotalDays;
//                int Hours = (int)timeSinceLastOnline.TotalHours % 24;
//                int Minutes = (int)timeSinceLastOnline.TotalMinutes % 60;

//                if (Days > 0) LastOnlineString += $"{Days} Days";
//                if (Hours > 0)
//                {
//                    if (LastOnlineString != "") LastOnlineString += ", ";
//                    LastOnlineString += $"{Hours} Hours";
//                }
//                if (Minutes > 0)
//                {
//                    if (LastOnlineString != "") LastOnlineString += ", ";
//                    LastOnlineString += $"{Minutes} Minutes";
//                }

//                if (LastOnlineString == "") LastOnlineString = "Now";
//                else LastOnlineString += " ago";
//            }
//            else LastOnlineString = "Unknown";

//            TextMeshProUGUIEx BioHeader = menu.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup/Row1/Bio&Notes/Panel_MM_ScrollRect/Viewport/VerticalLayoutGroup/Header_Bio/LeftItemContainer/Text_MM_H4").GetComponent<TextMeshProUGUIEx>();
//            BioHeader.text = $"Creation: {user.date_joined}\nPlatform: {user.GetAPIDevice()}\nLogin: {LastOnlineString}";
//            BioHeader.enabled = true;
//        }
//    }
//}
