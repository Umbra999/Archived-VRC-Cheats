using CoreRuntime.Manager;
using Hexed.Interfaces;
using Hexed.Wrappers;
using System.Globalization;
using VRC.Core;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements.Groups;

namespace Hexed.Hooking
{
    internal class GroupInfoPanel_OnRefresh : IHook
    {
        private delegate void _OnRefresDelegate(nint instance, nint __0);
        private static _OnRefresDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_OnRefresDelegate>(typeof(GroupInfoPanel).GetMethod(nameof(GroupInfoPanel.Method_Private_Void_IGroup_PDM_0)), Patch);
        }

        private static void Patch(nint instance, nint __0)
        {
            originalMethod(instance, __0);

            GroupInfoPanel menu = instance == nint.Zero ? null : new GroupInfoPanel(instance);

            if (menu == null) return;

            IGroup group = __0 == nint.Zero ? menu.field_Private_IGroup_0 : new IGroup(__0);

            if (group == null) return;

            Object1PublicIEquatable1ObfOb1ObTeBoLiInOb1LiUnique IGroup = group.TryCast<Object1PublicIEquatable1ObfOb1ObTeBoLiInOb1LiUnique>();
            if (IGroup == null || IGroup.prop_TYPE_0 == null) return;

            TextMeshProUGUIEx AboutNonMember = menu.transform.Find("ScrollRect_MM/Viewport/Content/BodyContainer_NonMember/About Group/Description/Panel_MM_ScrollRect/Viewport/VerticalLayoutGroup/Body").GetComponent<TextMeshProUGUIEx>();
            TextMeshProUGUIEx AboutMember = menu.transform.Find("ScrollRect_MM/Viewport/Content/BodyContainer_Member/Panel_MM_GroupInfo/About Group/Description/Panel_MM_ScrollRect/Viewport/VerticalLayoutGroup/Body_Description_Member").GetComponent<TextMeshProUGUIEx>();

            string LastSyncedString = "";
            if (IGroup.prop_TYPE_0.memberCountSyncedAt != null && IGroup.prop_TYPE_0.memberCountSyncedAt != "")
            {
                DateTime lastOnlineDateTime = DateTime.Parse(IGroup.prop_TYPE_0.memberCountSyncedAt, null, DateTimeStyles.RoundtripKind);
                TimeSpan timeSinceLastOnline = DateTime.UtcNow - lastOnlineDateTime;

                int Days = (int)timeSinceLastOnline.TotalDays;
                int Hours = (int)timeSinceLastOnline.TotalHours % 24;
                int Minutes = (int)timeSinceLastOnline.TotalMinutes % 60;

                if (Days > 0) LastSyncedString += $"{Days} Days";
                if (Hours > 0)
                {
                    if (LastSyncedString != "") LastSyncedString += ", ";
                    LastSyncedString += $"{Hours} Hours";
                }
                if (Minutes > 0)
                {
                    if (LastSyncedString != "") LastSyncedString += ", ";
                    LastSyncedString += $"{Minutes} Minutes";
                }

                if (LastSyncedString == "") LastSyncedString = "Now";
                else LastSyncedString += " ago";
            }
            else LastSyncedString = "Unknown";

            APIUser.FetchUser(IGroup.prop_TYPE_0.ownerId, new Action<APIUser>((user) =>
            {
                string Details = $"Owner: {user.DisplayName()} \nCreation: {IGroup.prop_TYPE_0.createdAt} \nSynced: {LastSyncedString}";
                AboutNonMember.text = Details + "\n\n" + AboutNonMember.text;
                AboutMember.text = Details + "\n\n" + AboutMember.text;
            }), new Action<string>((error) =>
            {
                string Details = $"Creation: {IGroup.prop_TYPE_0.createdAt} \nSynced: {LastSyncedString}";
                AboutNonMember.text = Details + "\n\n" + AboutNonMember.text;
                AboutMember.text = Details + "\n\n" + AboutMember.text;
            }));
        }
    }
}
