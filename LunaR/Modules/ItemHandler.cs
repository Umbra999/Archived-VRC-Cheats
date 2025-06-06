using LunaR.Extensions;
using LunaR.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.SDKBase;
using static VRC.SDKBase.VRC_EventHandler;

namespace LunaR.Modules
{
    internal class ItemHandler
    {
        public static bool NoPickup = false;

        public static void TakeOwnershipIfNecessary(GameObject gameObject)
        {
            if (GetObjectOwner(gameObject).UserID() != APIUser.CurrentUser.UserID()) Networking.SetOwner(Utils.CurrentUser.GetVRCPlayerApi(), gameObject);
        }

        public static Player GetObjectOwner(GameObject gameObject)
        {
            foreach (Player player in Utils.PlayerManager.GetAllPlayers())
            {
                if (player.GetVRCPlayerApi().IsOwner(gameObject)) return player;
            }
            return null;
        }

        public static bool PickupSpamToggle = false;

        public static IEnumerator PickupSpam(VRCPlayer P)
        {
            PickupSpamToggle = true;
            while (PickupSpamToggle && GeneralWrappers.IsInWorld())
            {
                ItemsToPlayerHead(P);
                yield return new WaitForSeconds(0.2f);
            }
            PickupSpamToggle = false;
        }

        public static void ItemsToPlayer(VRCPlayer Player)
        {
            foreach (VRC_Pickup vrc_ObjectSync in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
            {
                TakeOwnershipIfNecessary(vrc_ObjectSync.gameObject);
                vrc_ObjectSync.transform.rotation = new Quaternion(-0.7f, 0f, 0f, 0.7f);
                vrc_ObjectSync.transform.position = Player.transform.position;
            }
        }

        public static void ItemsToPosition(Vector3 Position)
        {
            foreach (VRC_Pickup vrc_Pickup in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
            {
                TakeOwnershipIfNecessary(vrc_Pickup.gameObject);
                vrc_Pickup.transform.position = Position;
            }
        }

        public static void ItemsToPlayerHead(VRCPlayer Player)
        {
            foreach (var pickup in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
            {
                TakeOwnershipIfNecessary(pickup.gameObject);
                pickup.transform.position = Player.transform.Find("ForwardDirection/Avatar").GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head).FindChild("HmdPivot").transform.position;
            }
        }

        public static IEnumerator DrawCircle(Player Player)
        {
            foreach (VRC_Pickup Pickup in UnityEngine.Object.FindObjectsOfType<VRC_Pickup>())
            {
                if (!Pickup.transform.parent.name.ToLower().Contains("eraser") && (Pickup.name.ToLower().Contains("pen") || Pickup.name.ToLower().Contains("marker") || Pickup.name.ToLower().Contains("grip")))
                {
                    TakeOwnershipIfNecessary(Pickup.gameObject);
                    Vector3 OriginalPosition = Pickup.transform.position;
                    Quaternion OriginalRotation = Pickup.transform.rotation;
                    float CircleSpeed = 15f;
                    float alpha = 0f;
                    float a = 1f;
                    float b = 1f;
                    Pickup.Drop();
                    Pickup.gameObject.GetComponent<VRC_Trigger>().OnDrop();
                    Pickup.gameObject.GetComponent<VRC_Trigger>().OnPickup();
                    Pickup.transform.position = new Vector3(Player.transform.position.x + a * (float)Math.Cos(alpha), Player.transform.position.y + 0.3f, Player.transform.position.z + b * (float)Math.Sin(alpha));
                    Pickup.transform.rotation = new Quaternion(-0.7f, 0f, 0f, 0.7f);
                    yield return new WaitForSeconds(0.2f);
                    Pickup.gameObject.GetComponent<VRC_Trigger>().OnPickupUseDown();
                    yield return new WaitForSeconds(0.01f);
                    for (int i = 0; i < 95; i++)
                    {
                        alpha += Time.deltaTime * CircleSpeed;
                        Pickup.transform.position = new Vector3(Player.transform.position.x + a * (float)Math.Cos(alpha), Player.transform.position.y + 0.3f, Player.transform.position.z + b * (float)Math.Sin(alpha));
                        yield return new WaitForSeconds(0.01f);
                    }
                    Pickup.gameObject.GetComponent<VRC_Trigger>().OnPickupUseUp();
                    yield return new WaitForSeconds(0.01f);
                    Pickup.transform.position = OriginalPosition;
                    Pickup.transform.rotation = OriginalRotation;
                    Pickup.gameObject.GetComponent<VRC_Trigger>().OnDrop();
                }
            }
            yield break;
        }

        public static List<GameObject> SpawnedPrefabs = new();
        public static bool LagPrefab = false;

        public static IEnumerator PrefabLag()
        {
            LagPrefab = true;
            while (LagPrefab && GeneralWrappers.IsInWorld())
            {
                foreach (GameObject SelectedPrefab in GeneralWrappers.GetWorldPrefabs())
                {
                    GameObject Item = Networking.Instantiate(VrcBroadcastType.Always, SelectedPrefab.name, PortalHandler.Position.INFBypass, PortalHandler.Position.ROTBypass);
                    if (Item != null)
                    {
                        SpawnedPrefabs.Add(Item);
                        Item.SetActive(false);
                        yield return new WaitForSeconds(0.24f);
                    }
                }
            }
            LagPrefab = false;
        }

        public static void Headlight(bool state)
        {
            Transform head = Utils.CurrentUser.field_Internal_Animator_0.GetBoneTransform(HumanBodyBones.Head);
            Light light = head.GetComponent<Light>();
            if (state)
            {
                light = head.gameObject.AddComponent<Light>();
                light.color = Color.white;
                light.type = LightType.Spot;
                light.shadows = LightShadows.Soft;
                light.intensity = 0.85f;
            }
            else UnityEngine.Object.Destroy(light);
        }


        public static bool InfinitRangeToggle = false;
        public static void InfinitRange(bool state)
        {
            InfinitRangeToggle = state;
            if (state)
            {
                foreach (VRC_Pickup Pickup in UnityEngine.Object.FindObjectsOfType<VRC_Pickup>())
                {
                    Pickup.proximity = float.MaxValue;
                }
            }
            else
            {
                foreach (VRC_Pickup Pickup in UnityEngine.Object.FindObjectsOfType<VRC_Pickup>())
                {
                    Pickup.proximity = 1;
                }
            }
        }

        public static IEnumerator ItemRotate(Player player)
        {
            ItemOrbitEnabled = true;
            List<VRC_Pickup> Items = Resources.FindObjectsOfTypeAll<VRC_Pickup>().ToList();
            while (ItemOrbitEnabled && GeneralWrappers.IsInWorld())
            {
                GameObject gameObject = new();
                Transform transform = gameObject.transform;
                transform.position = player.transform.position + new Vector3(0f, 0.2f, 0f);
                gameObject.transform.Rotate(new Vector3(0f, 360f * Time.time, 0f));
                try
                {
                    foreach (VRC_Pickup obj in Items)
                    {
                        TakeOwnershipIfNecessary(obj.gameObject);
                        obj.transform.position = gameObject.transform.position + gameObject.transform.forward;
                        obj.transform.LookAt(player.transform);
                        gameObject.transform.Rotate(new Vector3(0f, 360 / Items.Count, 0f));
                    }
                }
                catch { }
                UnityEngine.Object.Destroy(gameObject);
                yield return new WaitForSeconds(0.035f);
            }
            ItemOrbitEnabled = false;
        }

        public static bool ItemOrbitEnabled = false;
        public static bool PenVirus = false;

        public static IEnumerator SpreadVirus()
        {
            PenVirus = true;
            foreach (VRC_Pickup Pickup in UnityEngine.Object.FindObjectsOfType<VRC_Pickup>())
            {
                if (!Pickup.transform.parent.name.ToLower().Contains("eraser") && Pickup != null && (Pickup.name.ToLower().Contains("pen") || Pickup.name.ToLower().Contains("marker") || Pickup.name.ToLower().Contains("grip")))
                {
                    TakeOwnershipIfNecessary(Pickup.gameObject);
                    Pickup.Drop();
                    Pickup.transform.position = Utils.CurrentUser.transform.position;
                    Pickup.gameObject.GetComponent<VRC_Trigger>().OnDrop();
                    yield return new WaitForSeconds(0.2f);
                    Pickup.gameObject.GetComponent<VRC_Trigger>().OnPickup();
                    yield return new WaitForSeconds(0.2f);
                    Pickup.gameObject.GetComponent<VRC_Trigger>().OnPickupUseDown();
                }
            }
            float a = 0f;
            float b = 0f;
            float playerx = Utils.CurrentUser.transform.position.x;
            float playery = Utils.CurrentUser.transform.position.y;
            float playerz = Utils.CurrentUser.transform.position.z;
            while (PenVirus && GeneralWrappers.IsInWorld())
            {
                foreach (VRC_Pickup Pickup in UnityEngine.Object.FindObjectsOfType<VRC_Pickup>())
                {
                    if (!Pickup.transform.parent.name.ToLower().Contains("eraser") && Pickup != null && (Pickup.name.ToLower().Contains("pen") || Pickup.name.ToLower().Contains("marker") || Pickup.name.ToLower().Contains("grip")))
                    {
                        yield return new WaitForSeconds(0.01f);
                        float CircleSpeed = 9999;
                        float alpha = 0f;
                        Pickup.transform.rotation = new Quaternion(-0.7f, 0f, 0f, 0.7f);
                        for (int x = 0; x < 4000; x++)
                        {
                            alpha += Time.deltaTime * CircleSpeed;
                            Pickup.transform.position = new Vector3(playerx + a * (float)Math.Cos(alpha), playery + 0.3f, playerz + b * (float)Math.Sin(alpha));
                        }
                        a += 0.003f;
                        b += 0.003f;
                    }
                }
            }
            PenVirus = false;
        }
    }
}