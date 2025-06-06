using CoreRuntime.Manager;
using Hexed.Interfaces;
using Hexed.Wrappers;
using UnityEngine;
using VRC.UI.Elements.Controls;

namespace Hexed.Hooking
{
    internal class UserCellListViewQM_CreateCell : IHook
    {
        private delegate void _CreateCellDelegate(nint instance, nint __0);
        private static _CreateCellDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_CreateCellDelegate>(typeof(UserCellListViewQM).GetMethod(nameof(UserCellListViewQM.Method_Private_Void_IUser_0)), Patch);
        }

        private static void Patch(nint instance, nint __0)
        {
            originalMethod(instance, __0);

            UserCellListViewQM MMUserCell = instance == nint.Zero ? null : new UserCellListViewQM(instance);
            IUser user = __0 == nint.Zero ? null : new IUser(__0);

            if (MMUserCell == null) return;

            IUser activeUser = user ?? MMUserCell.field_Private_IUser_0;
            if (activeUser == null) return;

            Object1PublicOb1ApStBo1StLoBoSiUnique IUser = activeUser.TryCast<Object1PublicOb1ApStBo1StLoBoSiUnique>();
            if (IUser == null || IUser.prop_TYPE_0 == null) return;

            Color RankColor = IUser.prop_TYPE_0.IsFriend() ? VRCPlayer.field_Internal_Static_Color_1 : IUser.prop_TYPE_0.GetRank().GetRankColor();
            string richColor = ColorUtility.ToHtmlStringRGB(RankColor);

            if (MMUserCell.field_Public_TextMeshProUGUI_0 == null || MMUserCell.field_Public_TextMeshProUGUI_1 == null) return;

            MMUserCell.field_Public_TextMeshProUGUI_0.richText = true;
            MMUserCell.field_Public_TextMeshProUGUI_1.richText = true;
            MMUserCell.field_Public_TextMeshProUGUI_0.text = $"<color=#{richColor}>{MMUserCell.field_Public_TextMeshProUGUI_0.text}</color>";
            MMUserCell.field_Public_TextMeshProUGUI_1.text = $"<color=#{richColor}>{MMUserCell.field_Public_TextMeshProUGUI_0.text}</color>";
            MMUserCell.field_Public_TextMeshProUGUI_0.color = RankColor;
            MMUserCell.field_Public_TextMeshProUGUI_1.color = RankColor;
        }
    }
}
