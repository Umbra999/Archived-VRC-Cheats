using VRC.SDKBase;
using UnityEngine;
using VRC.Core;
using VRC;
using Hexed.Wrappers;
using Photon.Pun;

namespace Hexed.Extensions
{
    internal class ItemHelper
    {
        public static void TakeOwnershipIfNecessary(GameObject gameObject)
        {
            if (GetObjectOwner(gameObject)?.UserID() != APIUser.CurrentUser.UserID()) Networking.SetOwner(GameHelper.CurrentPlayer.GetVRCPlayerApi(), gameObject); // use raw event instead
        }

        public static Player GetObjectOwner(GameObject gameObject)
        {
            foreach (Player player in GameHelper.PlayerManager.GetAllPlayers())
            {
                if (player.GetVRCPlayerApi().IsOwner(gameObject)) return player;
            }

            return null;
        }

        public static void ItemsToPlayer(Player Player)
        {
            ItemsToPosition(Player.transform.position);
        }

        public static void ItemsToPosition(Vector3 Position)
        {
            foreach (VRC_Pickup vrc_Pickup in GetAllPickups())
            {
                TakeOwnershipIfNecessary(vrc_Pickup.gameObject);
                vrc_Pickup.transform.position = Position;
                // add delay to not cause unusual client behaviour
            }
        }

        public static VRC_Pickup[] GetAllPickups()
        {
            return Resources.FindObjectsOfTypeAll<VRC_Pickup>().Where(x => x.name != "PhotoCamera" && x.name != "AvatarDebugConsole" && x.name != "OscDebugConsole" && x.name != "DebugConsole" && x.name != "ViewFinder" && x.name != "MirrorPickup")?.ToArray();
        }

        public static bool IsViewIdPickupable(int viewId)
        {
            //PhotonView view = PhotonView.Method_Public_Static_PhotonView_Int32_0(viewId);

            //if (view == null) return false;

            //return view.gameObject.GetComponent<VRC_Pickup>() != null;

            return false;
        }

        public static void ToggleHeadlight(bool state)
        {
            if (state)
            {
                Light light = Camera.main.gameObject.AddComponent<Light>();
                light.type = LightType.Spot;
                light.range = 40;
                light.spotAngle = 80;
                light.color = Color.white;
                light.intensity = 1;
                light.shadows = LightShadows.None;
                light.boundingSphereOverride = new Vector4(0, 0, 0, 4);
                light.renderMode = LightRenderMode.ForcePixel;
                light.useBoundingSphereOverride = true;
            }
            else
            {
                Light light = Camera.main.gameObject.GetComponent<Light>();
                if (light != null) UnityEngine.Object.Destroy(light);
            }
        }
    }
}
