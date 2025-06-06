using MelonLoader;
using System;
using System.Collections;
using UnityEngine;
using VRC;
using WengaPort.Wrappers;

namespace WengaPort.Modules
{
    class AvatarHider : MonoBehaviour
    {
        public float LoadingDelay = 0f;
        public void Update()
        {
            LoadingDelay += Time.deltaTime;
            if (LoadingDelay > 13f)
            {
                AssetBundleDownloadManager.prop_AssetBundleDownloadManager_0.field_Private_Boolean_0 = false;
                LoadingDelay = 0f;
            }
            HideDelay += Time.deltaTime;
            if (HideDelay > 2f)
            {
                try
                {
                    foreach (Player p in Utils.PlayerManager.GetAllPlayers())
                    {
                        GameObject avtrObject = p.GetAvatarObject();
                        if (p != Utils.CurrentUser && p != null && avtrObject != null && !PlayerList.BlockList.Contains(p.UserID()))
                        {
                            float dist = Vector3.Distance(Utils.CurrentUser.transform.position, avtrObject.transform.position);
                            bool isActive = p.GetAvatarObject().active;

                            if (isActive && dist > 32)
                            {
                                avtrObject.SetActive(false);
                            }
                            else if (!isActive && dist < 30)
                            {
                                avtrObject.SetActive(true);
                            }
                        }
                    }
                }
                catch { }
                HideDelay = 0f;
            }
        }
        public float HideDelay = 0f;

        public static void AvatarSpoofInit()
        {
            GameObject.Find("UserInterface/MenuContent/Screens/Avatar/Change Button").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(new Action(() =>
            {
                if (!string.IsNullOrEmpty(AviSpoofID) && AvatarSpoofToggle) MelonCoroutines.Start(AvatarSpoof());
            }));
        }

        public static string AviSpoofID = "avtr_2a945204-f146-4053-97f0-96e98a39d568";
        public static bool AvatarSpoofToggle = false;

        private static IEnumerator AvatarSpoof()
        {
            do
            {
                yield return new WaitForSeconds(5f);
            } while (Utils.CurrentUser.GetAPIAvatar().id == AviSpoofID);
            VRC.Core.API.SendPutRequest($"avatars/{AviSpoofID}/select");
        }
        public AvatarHider(IntPtr ptr) : base(ptr) { }
    }
}
