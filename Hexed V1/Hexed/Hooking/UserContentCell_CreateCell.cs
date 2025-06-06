using CoreRuntime.Manager;
using Hexed.Interfaces;
using Hexed.Wrappers;
using UnityEngine;
using VRC.UI.Elements.Controls;

namespace Hexed.Hooking
{
    internal class UserContentCell_CreateCell : IHook
    {
        private delegate void _CreateCellDelegate(nint instance, nint __0);
        private static _CreateCellDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_CreateCellDelegate>(typeof(UserContentCell).GetMethod(nameof(UserContentCell.Method_Private_Void_IUser_0)), Patch);
        }

        private static void Patch(nint instance, nint __0)
        {
            originalMethod(instance, __0);

            UserContentCell MMUserCell = instance == nint.Zero ? null : new UserContentCell(instance);

            if (MMUserCell == null) return;

            IUser user = __0 == nint.Zero ? MMUserCell.field_Private_IUser_0 : new IUser(__0);

            IUser activeUser = user ?? MMUserCell.field_Private_IUser_0;
            if (activeUser == null) return;

            Object1PublicOb1ApStBo1StLoBoSiUnique IUser = activeUser.TryCast<Object1PublicOb1ApStBo1StLoBoSiUnique>();
            if (IUser == null || IUser.prop_TYPE_0 == null) return;

            Color RankColor = IUser.prop_TYPE_0.GetRank().GetRankColor();
            string richColor = ColorUtility.ToHtmlStringRGB(RankColor);

            if (MMUserCell.field_Public_TextMeshProUGUIEx_0 == null || MMUserCell.field_Public_TextMeshProUGUIEx_1 == null) return;

            MMUserCell.field_Public_TextMeshProUGUIEx_0.richText = true;
            MMUserCell.field_Public_TextMeshProUGUIEx_1.richText = true;
            MMUserCell.field_Public_TextMeshProUGUIEx_0.text = $"<color=#{richColor}>{MMUserCell.field_Public_TextMeshProUGUIEx_0.text}</color>";
            MMUserCell.field_Public_TextMeshProUGUIEx_1.text = $"<color=#{richColor}>{MMUserCell.field_Public_TextMeshProUGUIEx_0.text}</color>";
            MMUserCell.field_Public_TextMeshProUGUIEx_0.color = RankColor;
            MMUserCell.field_Public_TextMeshProUGUIEx_1.color = RankColor;
        }
    }
}
