//using CoreRuntime.Manager;
//using Hexed.Interfaces;
//using UnityEngine;
//using VRC.UI.Elements.Controls;

//namespace Hexed.Hooking
//{
//    internal class AvatarContentCell_CreateCell : IHook
//    {
//        private delegate void _CreateCellDelegate(nint instance, nint __0);
//        private static _CreateCellDelegate originalMethod;

//        public void Initialize()
//        {
//            originalMethod = HookManager.Detour<_CreateCellDelegate>(typeof(AvatarContentCell).GetMethod(nameof(AvatarContentCell.Method_Private_Void_IAvatar_0)), Patch);
//        }

//        private static void Patch(nint instance, nint __0)
//        {
//            originalMethod(instance, __0);

//            AvatarContentCell AvatarCell = instance == nint.Zero ? null : new AvatarContentCell(instance);
//            IAvatar avatar = __0 == nint.Zero ? null : new IAvatar(__0);

//            if (AvatarCell == null) return;

//            IAvatar activeAvatar = avatar ?? AvatarCell.field_Private_IAvatar_0;
//            if (activeAvatar == null) return;

//            Object1PublicOb1BoObStBoLiStDaBoUnique FullAvatar = activeAvatar.TryCast<Object1PublicOb1BoObStBoLiStDaBoUnique>();
//            if (FullAvatar == null || FullAvatar.prop_TYPE_0 == null) return;

//            Color releaseColor = Color.gray;
//            switch (FullAvatar.prop_TYPE_0.releaseStatus)
//            {
//                case "private":
//                    releaseColor = Color.red;
//                    break;

//                case "public":
//                    releaseColor = Color.green;
//                    break;

//                case "hidden":
//                    releaseColor = Color.yellow;
//                    break;
//            }

//            string richColor = ColorUtility.ToHtmlStringRGB(releaseColor);

//            if (AvatarCell.field_Public_TextMeshProUGUI_0 == null) return;

//            AvatarCell.field_Public_TextMeshProUGUI_0.richText = true;
//            AvatarCell.field_Public_TextMeshProUGUI_0.text = $"<color=#{richColor}>{AvatarCell.field_Public_TextMeshProUGUI_0.text}</color>";
//            AvatarCell.field_Public_TextMeshProUGUI_0.color = releaseColor;
//        }
//    }
//}
