using LunaR.ConsoleUtils;
using LunaR.Extensions;
using LunaR.Modules;
using LunaR.Wrappers;
using Photon.Realtime;
using System.Collections.Generic;

namespace LunaR.Patching
{
    internal class BotDetection
    {
        public static List<int> DetectedSerializer = new();
        public static List<int> DetectedBots = new();

        public static List<int> MissingSync1 = new();
        public static List<int> MissingSync3 = new();

        public static List<int> MissingModeration = new();

        public static void ClearDetections()
        {
            MissingSync1.Clear();
            MissingSync3.Clear();

            MissingModeration.Clear();

            DetectedSerializer.Clear();
            DetectedBots.Clear();
        }

        private static void Validate(int Actor)
        {
            Player PhotonPlayer = PhotonExtensions.GetPhotonPlayer(Actor);
            if (PhotonPlayer != null)
            {
                if (MissingSync1.Contains(Actor) || MissingSync3.Contains(Actor) || MissingModeration.Contains(Actor))
                {
                    if (!PlayerExtensions.IsBlocked(PhotonPlayer.GetPlayer().UserID())) PhotonModule.Block(PhotonPlayer.GetPlayer().UserID(), true);
                    DetectedBots.Add(Actor);

                    if (MissingSync1.Contains(Actor)) MissingSync1.Remove(Actor);
                    if (MissingSync3.Contains(Actor)) MissingSync3.Remove(Actor);
                    if (MissingModeration.Contains(Actor)) MissingModeration.Remove(Actor);

                    VRConsole.Log(VRConsole.LogsType.Protection, $"{PhotonPlayer.GetPlayer().DisplayName()} --> Invalid Instantiation");
                    Logger.Log($"Prevented {PhotonPlayer.GetPlayer().DisplayName()} from using Invalid Instantiation", Logger.LogsType.Protection);
                }

                else if (PhotonPlayer.GetPlayer().GetIsBot())
                {
                    if (!PlayerExtensions.IsBlocked(PhotonPlayer.GetPlayer().UserID())) PhotonModule.Block(PhotonPlayer.GetPlayer().UserID(), true);
                    DetectedSerializer.Add(Actor);

                    VRConsole.Log(VRConsole.LogsType.Protection, $"{PhotonPlayer.GetPlayer().DisplayName()} --> No Serialization");
                    Logger.Log($"Prevented {PhotonPlayer.GetPlayer().DisplayName()} from using No Serialization", Logger.LogsType.Protection);
                }
            }
        }

        public static void InstantiateCheck(int Actor)
        {
            Player PhotonPlayer = PhotonExtensions.GetPhotonPlayer(Actor);
            if (PhotonPlayer != null)
            {
                if (PhotonPlayer.GetPlayer() == null)
                {
                    string UserID = PhotonPlayer.UserID();
                    string DisplayName = PhotonPlayer.GetDisplayName();

                    if (!PlayerExtensions.IsBlocked(UserID)) PhotonModule.Block(UserID, true);
                    DetectedBots.Add(Actor);

                    VRConsole.Log(VRConsole.LogsType.Protection, $"{DisplayName} --> No Instantiation");
                    Logger.Log($"Prevented {DisplayName} from using No Instantiation", Logger.LogsType.Protection);
                }
                else Validate(Actor);
            }
        }
    }
}