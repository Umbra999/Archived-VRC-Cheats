using LunaR.Extensions;
using LunaR.QMButtons;
using LunaR.Wrappers;
using System.Collections;
using UnityEngine;
using VRC.Core;

namespace LunaR.Modules
{
    internal class FakeSerialize
    {
        public static GameObject SerializeCapsule;
        public static bool SerializeToggle = false;
        public static byte[] MovementToCopy;

        public static void CustomSerialize(bool Toggle, bool TeleportBack = false)
        {
            if (Toggle)
            {
                CreateCapsule();
                SerializeToggle = true;

                UtilsMenu.SerializeToggle.SetToggleState(true, false);
                BotMenu.SelfbotToggle.SetToggleState(true, false);
            }
            else
            {
                if (SelfBotToggle)
                {
                    FakeBotSerialize(false).Start();
                    return;
                }

                if (SerializeCapsule != null)
                {
                    if (TeleportBack) Utils.CurrentUser.transform.position = SerializeCapsule.transform.position;
                    Object.Destroy(SerializeCapsule);
                }
                SerializeToggle = false;
                UtilsMenu.SerializeToggle.SetToggleState(false, false);
                BotMenu.SelfbotToggle.SetToggleState(false, false);
                MovementToCopy = null;
            }
        }

        public static bool SelfBotToggle = false;
        public static Il2CppSystem.Object AvatarDictCache;
        public static string BotID;

        public static IEnumerator FakeBotSerialize(bool Toggle)
        {
            if (BotID == null) yield break;
            VRC.Player Target = Utils.PlayerManager.GetPlayer(BotID);

            if (Toggle)
            {
                if (Target != null)
                {
                    CustomSerialize(true);
                    AvatarDictCache = Target.prop_Player_1.GetHashtable()["avatarDict"];
                    SelfBotToggle = true;
                    while (MovementToCopy == null) yield return new WaitForEndOfFrame();
                    Utils.CurrentUser.transform.position = Target.transform.position;
                    APIUser.CurrentUser.ReloadAvatar();
                }
            }
            else
            {
                SelfBotToggle = false;
                CustomSerialize(false, true);
                AvatarDictCache = null;
                APIUser.CurrentUser.ReloadAvatar();
            }
        }

        public static void CreateCapsule()
        {
            SerializeCapsule = Object.Instantiate(Utils.CurrentUser.GetAvatarObject(), null, true);
            Animator component = SerializeCapsule.GetComponent<Animator>();
            if (component != null)
            {
                if (component.isHuman)
                {
                    Transform boneTransform = component.GetBoneTransform((HumanBodyBones)10);
                    if (boneTransform != null) boneTransform.localScale = Vector3.one;
                }
                component.enabled = false;
            }
            SerializeCapsule.name = "Serialize Capsule";
            SerializeCapsule.transform.position = Utils.CurrentUser.transform.position;
            SerializeCapsule.transform.rotation = Utils.CurrentUser.transform.rotation;
        }
    }
}