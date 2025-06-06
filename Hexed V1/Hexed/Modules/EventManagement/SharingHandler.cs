using Hexed.Core;
using Hexed.Modules.EventManagement;
using Hexed.Modules.Standalone;
using Hexed.Wrappers;
using UnityEngine;
using VRC.Core;

namespace Hexed.Modules
{
    internal class SharingHandler
    {
        public static bool RecievedContentShare(Dictionary<byte, object> Data, byte EventCode)
        {
            //switch ((EnumPublicSealedvaPoShSt4vUnique)Data[0])
            //{
            //    case EnumPublicSealedvaPoShSt4vUnique.Portal:
            //        {
            //            switch ((EnumPublicSealedvaCrUpDePoReCr7vUnique)Data[1])
            //            {
            //                case EnumPublicSealedvaCrUpDePoReCr7vUnique.Create:
            //                    {
            //                        VRC.Player player = GameHelper.PlayerManager.GetPlayer(Data[2].ToString());
            //                        if (player == null) return true;

            //                        switch (Convert.ToByte(Data[1]))
            //                        {
            //                            case 0:
            //                                {
            //                                    byte[] RawPosition = Data[4] as byte[];
            //                                    float x = BitConverter.ToSingle(RawPosition, 0);
            //                                    float y = BitConverter.ToSingle(RawPosition, 4);
            //                                    float z = BitConverter.ToSingle(RawPosition, 8);
            //                                    Vector3 Position = new(x, y, z);

            //                                    if (UnityUtils.IsBadPosition(Position) || Vector3.Distance(player.transform.position, Position) > 50 || UnityUtils.IsBadDistance(GameHelper.CurrentPlayer.transform.position, Position))
            //                                    {
            //                                        EventSanitizer.LimitActor(player.GetPhotonPlayer().ActorID(), EventCode, "Invalid Position");
            //                                        return false;
            //                                    }

            //                                    if (Data.ContainsKey(133))
            //                                    {
            //                                        Dictionary<string, object> worldInfos = Data[133] as Dictionary<string, object>;

            //                                        Wrappers.Logger.Log($"{player.DisplayName()} spawned a dynamic Portal [{Data[1]}] [{worldInfos["name"]}] [{Data[132]}] [{Data[130]}/{worldInfos["capacity"]} Player]", Wrappers.Logger.LogsType.Room);
            //                                        VRConsole.Log($"{player.DisplayName()} --> {worldInfos["name"]} [Dynamic]", VRConsole.LogsType.Portal);
            //                                    }

            //                                    if (player.UserID() != APIUser.CurrentUser.UserID() && (InternalSettings.AntiPortal == InternalSettings.AntiPortalMode.All || InternalSettings.AntiPortal == InternalSettings.AntiPortalMode.Friends && !PlayerUtils.IsFriend(player.UserID()))) return false;
            //                                }
            //                                break;

            //                            case 1:
            //                                {
            //                                    Wrappers.Logger.Log($"{player.DisplayName()} spawned a static Portal [{Data[128]}] [{Data[130]} Player]", Wrappers.Logger.LogsType.Room);
            //                                    VRConsole.Log($"{player.DisplayName()} --> {Data[3]} [Static]", VRConsole.LogsType.Portal);
            //                                }
            //                                break;
            //                        }
            //                    }
            //                    break;

            //                case EnumPublicSealedvaCrUpDePoReCr7vUnique.Destroy:
            //                    {
            //                        if (InternalSettings.NoObjectDestroy) return false;

            //                        Wrappers.Logger.Log($"Server destroyed a Portal [{Data[128]}]", Wrappers.Logger.LogsType.Room);
            //                        VRConsole.Log($"Server --> Destroy [{Data[128]}]", VRConsole.LogsType.Portal);
            //                    }
            //                    break;

            //                case EnumPublicSealedvaCrUpDePoReCr7vUnique.Update:
            //                    {
            //                        //Wrappers.Logger.Log($"Server updated a Portal [{Data[1]}] [{Data[3]} Player | {Data[4]} Time]", Wrappers.Logger.LogsType.Room);
            //                        //VRConsole.Log($"Server --> Update [{Data[1]}]", VRConsole.LogsType.Portal);
            //                    }
            //                    break;

            //                case EnumPublicSealedvaCrUpDePoReCr7vUnique.PortalEntered:
            //                    break;

            //                case EnumPublicSealedvaCrUpDePoReCr7vUnique.CreateStaticPortal:
            //                    break;

            //                case EnumPublicSealedvaCrUpDePoReCr7vUnique.RequestFailure:
            //                    {
            //                        Wrappers.Logger.Log($"Portal request failed", Wrappers.Logger.LogsType.Room);
            //                        VRConsole.Log($"Server --> Portal failed", VRConsole.LogsType.Portal);
            //                    }
            //                    break;
            //            }
            //        }
            //        break;

            //    case EnumPublicSealedvaPoShSt4vUnique.Share:
            //        {

            //        }
            //        break;

            //    case EnumPublicSealedvaPoShSt4vUnique.Sticker:
            //        {
            //            switch ((EnumPublicSealedvaCrDeUpUn5vUnique)Data[1])
            //            {
            //                case EnumPublicSealedvaCrDeUpUn5vUnique.Create:
            //                    {
            //                        if (Data.ContainsKey(3)) // Weird stuff it does a pre req and then does the main req with data???
            //                        {
            //                            VRC.Player player = GameHelper.PlayerManager.GetPlayer(Data[3].ToString());
            //                            if (player == null) return false;

            //                            byte[] RawPosition = Data[4] as byte[];
            //                            float x = BitConverter.ToSingle(RawPosition, 0);
            //                            float y = BitConverter.ToSingle(RawPosition, 4);
            //                            float z = BitConverter.ToSingle(RawPosition, 8);
            //                            Vector3 Position = new(x, y, z);

            //                            if (UnityUtils.IsBadPosition(Position) || Vector3.Distance(player.transform.position, Position) > 50 || UnityUtils.IsBadDistance(GameHelper.CurrentPlayer.transform.position, Position))
            //                            {
            //                                EventSanitizer.LimitActor(player.GetPhotonPlayer().ActorID(), EventCode, "Invalid Position");
            //                                return false;
            //                            }

            //                            Wrappers.Logger.Log($"{player.DisplayName()} spawned a Sticker [{Data[129]}]", Wrappers.Logger.LogsType.Room);
            //                            VRConsole.Log($"{player.DisplayName()} --> {Data[129]}", VRConsole.LogsType.Sticker);
            //                        }
            //                    }
            //                    break;

            //                case EnumPublicSealedvaCrDeUpUn5vUnique.Destroy:
            //                    {
            //                        if (InternalSettings.NoObjectDestroy) return false;

            //                        Wrappers.Logger.Log($"Server destroyed a Sticker [{Data[2]}]", Wrappers.Logger.LogsType.Room);
            //                        VRConsole.Log($"Server --> Destroy [{Data[2]}]", VRConsole.LogsType.Sticker);
            //                    }
            //                    break;

            //                case EnumPublicSealedvaCrDeUpUn5vUnique.Update:
            //                    break;

            //                case EnumPublicSealedvaCrDeUpUn5vUnique.Unique:
            //                    break;
            //            }
            //        }
            //        break;
            //}

            return true;
        }

        public static bool RaisedContentShare(Dictionary<byte, object> Data)
        {
            //switch ((EnumPublicSealedvaPoShSt4vUnique)Data[0])
            //{
            //    case EnumPublicSealedvaPoShSt4vUnique.Portal:
            //        {
            //            switch ((EnumPublicSealedvaCrUpDePoReCr7vUnique)Data[1])
            //            {
            //                case EnumPublicSealedvaCrUpDePoReCr7vUnique.Create:
            //                    break;

            //                case EnumPublicSealedvaCrUpDePoReCr7vUnique.Destroy:
            //                    break;

            //                case EnumPublicSealedvaCrUpDePoReCr7vUnique.Update:
            //                    break;

            //                case EnumPublicSealedvaCrUpDePoReCr7vUnique.PortalEntered:
            //                    return false; // might make issues in the future if the server checks if you really entered the portal to unlock you in a world, right now it works

            //                case EnumPublicSealedvaCrUpDePoReCr7vUnique.CreateStaticPortal:
            //                    break;

            //                case EnumPublicSealedvaCrUpDePoReCr7vUnique.RequestFailure:
            //                    break;
            //            }
            //        }
            //        break;

            //    case EnumPublicSealedvaPoShSt4vUnique.Share:
            //        {

            //        }
            //        break;

            //    case EnumPublicSealedvaPoShSt4vUnique.Sticker:
            //        {
            //            switch ((EnumPublicSealedvaCrDeUpUn5vUnique)Data[1])
            //            {
            //                case EnumPublicSealedvaCrDeUpUn5vUnique.Create:
            //                    break;

            //                case EnumPublicSealedvaCrDeUpUn5vUnique.Destroy:
            //                    break;

            //                case EnumPublicSealedvaCrDeUpUn5vUnique.Update:
            //                    break;

            //                case EnumPublicSealedvaCrDeUpUn5vUnique.Unique:
            //                    break;
            //            }
            //        }
            //        break;
            //}

            return true;
        }

        public static void DeletePortals()
        {
            foreach (PortalInternal portal in Resources.FindObjectsOfTypeAll<PortalInternal>())
            {
                if (portal.gameObject.activeInHierarchy) UnityEngine.Object.Destroy(portal.gameObject);
            }
        }
    }
}
