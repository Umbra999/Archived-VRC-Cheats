using CoreRuntime.Manager;
using Hexed.Interfaces;
using Hexed.Wrappers;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Hexed.Hooking
{
    internal class QMBlockedCell_CreateCell : IHook
    {
        private delegate void _CreateCellDelegate(nint instance, nint __0);
        private static _CreateCellDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_CreateCellDelegate>(typeof(MonoBehaviourPublicTeObTeObObUnique).GetMethod(nameof(MonoBehaviourPublicTeObTeObObUnique.Method_Private_Void_IUser_0)), Patch);
        }

        private static void Patch(nint instance, nint __0)
        {
            originalMethod(instance, __0);

            MonoBehaviourPublicTeObTeObObUnique QMBlockedCell = instance == nint.Zero ? null : new MonoBehaviourPublicTeObTeObObUnique(instance);
            IUser user = __0 == nint.Zero ? null : new IUser(__0);

            if (QMBlockedCell == null) return;

            IUser activeUser = user ?? QMBlockedCell.field_Private_IUser_0;
            if (activeUser == null) return;

            Object1PublicOb1ApStBo1StLoBoSiUnique IUser = activeUser.TryCast<Object1PublicOb1ApStBo1StLoBoSiUnique>();
            if (IUser == null || IUser.prop_TYPE_0 == null) return;

            Color RankColor = IUser.prop_TYPE_0.GetRank().GetRankColor();
            string richColor = ColorUtility.ToHtmlStringRGB(RankColor);

            if (QMBlockedCell.field_Public_TextMeshProUGUI_0 == null) return;

            QMBlockedCell.field_Public_TextMeshProUGUI_0.richText = true;
            QMBlockedCell.field_Public_TextMeshProUGUI_0.text = $"<color=#{richColor}>{QMBlockedCell.field_Public_TextMeshProUGUI_0.text}</color>";
            QMBlockedCell.field_Public_TextMeshProUGUI_0.color = RankColor;
        }
    }
}
