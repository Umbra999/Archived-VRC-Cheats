using CoreRuntime.Manager;
using Hexed.Interfaces;
using Hexed.Wrappers;
using UnityEngine;

namespace Hexed.Hooking
{
    internal class QMUserCell_CreateCell : IHook
    {
        private delegate void _CreateCellDelegate(nint instance, nint __0);
        private static _CreateCellDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_CreateCellDelegate>(typeof(MonoBehaviourPublicObTe_cTeObObObObUnique).GetMethod(nameof(MonoBehaviourPublicObTe_cTeObObObObUnique.Method_Private_Void_IUser_0)), Patch);
        }

        private static void Patch(nint instance, nint __0)
        {
            originalMethod(instance, __0);

            MonoBehaviourPublicObTe_cTeObObObObUnique QMUserCell = instance == nint.Zero ? null : new MonoBehaviourPublicObTe_cTeObObObObUnique(instance);
            IUser user = __0 == nint.Zero ? null : new IUser(__0);

            if (QMUserCell == null) return;

            IUser activeUser = user ?? QMUserCell.field_Private_IUser_0;
            if (activeUser == null) return;

            Object1PublicOb1ApStBo1StLoBoSiUnique IUser = activeUser.TryCast<Object1PublicOb1ApStBo1StLoBoSiUnique>();
            if (IUser == null || IUser.prop_TYPE_0 == null) return;

            Color RankColor = IUser.prop_TYPE_0.IsFriend() ? VRCPlayer.field_Internal_Static_Color_1 : IUser.prop_TYPE_0.GetRank().GetRankColor();
            string richColor = ColorUtility.ToHtmlStringRGB(RankColor);

            if (QMUserCell.field_Public_TextMeshProUGUI_0 == null) return;

            QMUserCell.field_Public_TextMeshProUGUI_0.richText = true;
            QMUserCell.field_Public_TextMeshProUGUI_0.text = $"<color=#{richColor}>{QMUserCell.field_Public_TextMeshProUGUI_0.text}</color>";
            QMUserCell.field_Public_TextMeshProUGUI_0.color = RankColor;
        }
    }
}
