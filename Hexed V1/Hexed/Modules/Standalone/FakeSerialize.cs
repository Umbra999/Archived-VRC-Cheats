using Hexed.CustomUI.QM;
using Hexed.Wrappers;
using UnityEngine;

namespace Hexed.Modules.Standalone
{
    internal class FakeSerialize
    {
        private static GameObject SerializeCapsule;
        public static bool NoSerialize = false;
        public static byte[] CachedMovement;

        public static bool BotSerialize = false;
        public static int BotActor = -2;
        public static VRC.Player ActiveBot;

        public static void CustomSerialize(bool Toggle, bool TeleportBack = false)
        {
            if (Toggle)
            {
                CreateCapsule();
                NoSerialize = true;
                UtilsMenu.FakeSerializeToggle.SetToggleState(true);
                BotMenu.BotSerializeToggle.SetToggleState(true);
            }
            else
            {
                if (BotSerialize)
                {
                    BotSerialize = false;
                    CustomSerialize(false, true);
                    if (ActiveBot != null)
                    {
                        ActiveBot.GetVRCAvatarManager().prop_GameObject_0.SetActive(true);
                        ActiveBot = null;
                    }
                    return;
                }

                if (SerializeCapsule != null)
                {
                    if (TeleportBack) GameHelper.CurrentPlayer.transform.position = SerializeCapsule.transform.position;
                    UnityEngine.Object.Destroy(SerializeCapsule);
                }

                NoSerialize = false;
                BotSerialize = false;
                CachedMovement = null;

                UtilsMenu.FakeSerializeToggle.SetToggleState(false);
                BotMenu.BotSerializeToggle.SetToggleState(false);
            }
        }

        private static void CreateCapsule()
        {
            SerializeCapsule = UnityEngine.Object.Instantiate(GameHelper.CurrentPlayer.GetVRCAvatarManager().prop_GameObject_0, null);
            SerializeCapsule.name = "Clone Capsule";
            SerializeCapsule.transform.position = GameHelper.CurrentPlayer.transform.position;
            SerializeCapsule.transform.rotation = GameHelper.CurrentPlayer.transform.rotation;

            Animator component = SerializeCapsule.GetComponent<Animator>();
            if (component != null)
            {
                if (component.isHuman)
                {
                    Transform boneTransform = component.GetBoneTransform(HumanBodyBones.Head);
                    if (boneTransform != null) boneTransform.localScale = Vector3.one;
                }
                component.enabled = false;
            }
        }

        public static void AddBotSerialize(VRC.Player Bot = null)
        {
            ActiveBot = Bot;
            BotSerialize = true;

            GameHelper.CurrentPlayer.transform.position = ActiveBot.transform.position;
            ActiveBot.GetVRCAvatarManager().prop_GameObject_0.SetActive(false);
        }
    }
}
