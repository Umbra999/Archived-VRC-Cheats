using Hexed.Core;
using Hexed.Modules.Standalone;
using Hexed.Wrappers;
using Il2CppInterop.Runtime;

namespace Hexed.Modules.EventManagement
{
    internal class UserPropHandler
    {
        public static bool ReceivedPropEvent(Il2CppSystem.Collections.Hashtable Data, Photon.Realtime.Player PhotonPlayer)
        {
            string oldAvatarID = PhotonPlayer.AvatarID();
            string oldGroupID = PhotonPlayer.RepresentedGroupID();
            bool oldVRMode = PhotonPlayer.IsVR();
            bool oldShowGroup = PhotonPlayer.ShowGroupToOthers();
            bool oldShowRank = PhotonPlayer.ShowSocialRankToOthers();
            bool oldUseImpostor = PhotonPlayer.UseImpostorAsFallback();
            int oldAvatarHeight = PhotonPlayer.AvatarHeight();

            PhotonUtils.networkedProperties[PhotonPlayer.ActorID()] = Data;

            string newAvatarID = PhotonPlayer.AvatarID();
            string newGroupID = PhotonPlayer.RepresentedGroupID();
            bool newVRMode = PhotonPlayer.IsVR();
            bool newShowGroup = PhotonPlayer.ShowGroupToOthers();
            bool newShowRank = PhotonPlayer.ShowSocialRankToOthers();
            bool newUseImpostor = PhotonPlayer.UseImpostorAsFallback();
            int newAvatarHeight = PhotonPlayer.AvatarHeight();

            string DisplayName = PhotonPlayer.DisplayName() ?? "NO NAME";

            if (oldAvatarID != newAvatarID)
            {
                string AvatarName = PhotonPlayer.AvatarName() ?? "NO NAME";
                string releaseStatus = PhotonPlayer.AvatarReleaseStatus() ?? "NO RELEASE";

                Logger.Log($"{DisplayName} changed Avatar from {oldAvatarID} to {newAvatarID}", Logger.LogsType.Room);
                VRConsole.Log($"{DisplayName} --> {AvatarName} [{releaseStatus}]", VRConsole.LogsType.Avatar);
            }

            if (oldGroupID != newGroupID)
            {
                Logger.Log($"{DisplayName} changed represented Group from {oldGroupID} to {newGroupID}", Logger.LogsType.Group);
                VRConsole.Log($"{DisplayName} --> Group update", VRConsole.LogsType.Group);
            }

            if (oldVRMode != newVRMode)
            {
                Logger.Log($"{DisplayName} changed VR State from {oldVRMode} to {newVRMode}", Logger.LogsType.Protection);
                VRConsole.Log($"{DisplayName} --> {(newVRMode ? "VR Switch" : "Desktop Switch")}", VRConsole.LogsType.Protection);
            }

            if (oldShowGroup != newShowGroup)
            {
                Logger.Log($"{DisplayName} changed Group visibility from {oldShowGroup} to {newShowGroup}", Logger.LogsType.Group);
                VRConsole.Log($"{DisplayName} --> {(newShowGroup ? "Show Group" : "Hide Group")}", VRConsole.LogsType.Group);
            }

            if (oldShowRank != newShowRank)
            {
                Logger.Log($"{DisplayName} changed Rank visibility from {oldShowRank} to {newShowRank}", Logger.LogsType.Room);
                VRConsole.Log($"{DisplayName} --> {(newShowRank ? "Show Rank" : "Hide Rank")}", VRConsole.LogsType.Room);
            }

            if (oldUseImpostor != newUseImpostor)
            {
                Logger.Log($"{DisplayName} changed Impostor fallback from {oldUseImpostor} to {newUseImpostor}", Logger.LogsType.Room);
                VRConsole.Log($"{DisplayName} --> {(newUseImpostor ? "Impostor Fallback" : "Default Fallback")}", VRConsole.LogsType.Avatar);
            }

            if (oldAvatarHeight != newAvatarHeight)
            {
                Logger.Log($"{DisplayName} changed Avatar height from {oldAvatarHeight} to {newAvatarHeight}", Logger.LogsType.Room);
                VRConsole.Log($"{DisplayName} --> Avatar Height [{newAvatarHeight}]", VRConsole.LogsType.Avatar);
            }

            return true;
        }

        public static bool RaisedPropEvent(Il2CppSystem.Collections.Hashtable Data)
        {
            if (Data.ContainsKey("avatarEyeHeight"))
            {
                if (InternalSettings.BigAvatar)
                {
                    Data["avatarEyeHeight"] = new Il2CppSystem.Int32()
                    {
                        m_value = int.MaxValue
                    }.BoxIl2CppObject();
                }
            }

            if (Data.ContainsKey("groupOnNameplate") && InternalSettings.GroupSpoof) Data["groupOnNameplate"] = new(IL2CPP.ManagedStringToIl2Cpp(InternalSettings.FakeGroupValue));

            return true;
        }
    }
}
