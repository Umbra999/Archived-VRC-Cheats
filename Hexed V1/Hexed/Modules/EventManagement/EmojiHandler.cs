using Hexed.Core;
using Hexed.Modules.Standalone;
using Hexed.Wrappers;
using Photon.Realtime;
using System.Text;
using System.Text.Json;
using VRC.Core;

namespace Hexed.Modules.EventManagement
{
    internal class EmojiHandler
    {
        public static bool ReceivedEmojiEvent(Dictionary<byte, object> Data, Player PhotonPlayer, byte EventCode)
        {
            switch ((EnumPublicSealedvaLeCu3vUnique)Data[0])
            {
                case EnumPublicSealedvaLeCu3vUnique.Legacy:
                    {
                        int Emoji = Convert.ToInt32(Data[2]);
                        if (Emoji < 0 || Emoji > 64)
                        {
                            EventSanitizer.LimitActor(PhotonPlayer.ActorID(), EventCode, "Invalid legacy Emoji");
                            return false;
                        }

                        string json = JsonSerializer.Serialize(Data);
                        string Base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
                        Base64 += PhotonPlayer.ActorID(); // Limit only same Actor repeating
                        if (EventSanitizer.BlacklistedBytes.Contains(Base64))
                        {
                            EventSanitizer.LimitActor(PhotonPlayer.ActorID(), EventCode, "Blacklisted");
                            return false;
                        }
                        EventSanitizer.BlacklistedBytes.Add(Base64);

                        string DisplayName = PhotonPlayer.DisplayName() ?? "NO NAME";

                        Wrappers.Logger.Log($"{DisplayName} used a legacy Emoji [{Emoji}]", Wrappers.Logger.LogsType.Room);
                        VRConsole.Log($"{DisplayName} --> {Emoji}", VRConsole.LogsType.Emoji);
                    }
                    break;

                case EnumPublicSealedvaLeCu3vUnique.Custom:
                    {
                        string file = Data[1].ToString();
                        if (!EventSanitizer.CheckFileID(file))
                        {
                            EventSanitizer.LimitActor(PhotonPlayer.ActorID(), EventCode, "Invalid custom Emoji");
                            return false;
                        }

                        string json = JsonSerializer.Serialize(Data);
                        string Base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
                        if (EventSanitizer.BlacklistedBytes.Contains(Base64))
                        {
                            EventSanitizer.LimitActor(PhotonPlayer.ActorID(), EventCode, "Blacklisted");
                            return false;
                        }
                        EventSanitizer.BlacklistedBytes.Add(Base64);

                        APIEmoji.Fetch(file, new Action<APIEmoji>((Emoji) =>
                        {
                            string UserID = PhotonPlayer.UserID();

                            if (Emoji.ownerId != UserID || Emoji.animationStyle == null)
                            {
                                EventSanitizer.LimitActor(PhotonPlayer.ActorID(), EventCode, "Invalid custom Emoji");
                                // cant return because async so just limiting down
                            }

                            string DisplayName = PhotonPlayer.DisplayName() ?? "NO NAME";
                            Wrappers.Logger.Log($"{DisplayName} used a custom Emoji {file} [{Emoji.animationStyle}]", Wrappers.Logger.LogsType.Room);
                            VRConsole.Log($"{DisplayName} --> {file}", VRConsole.LogsType.Emoji);
                        }), new Action<string>((error) =>
                        {
                            EventSanitizer.LimitActor(PhotonPlayer.ActorID(), EventCode, "Invalid custom Emoji");
                        }));     
                    }
                    break;
            }

            return !InternalSettings.NoEmojiSpawn;
        }

        public static bool RaisedEmojiEvent(Dictionary<byte, object> Data)
        {
            switch ((EnumPublicSealedvaLeCu3vUnique)Data[0])
            {
                case EnumPublicSealedvaLeCu3vUnique.Legacy:
                    break;

                case EnumPublicSealedvaLeCu3vUnique.Custom:
                    break;
            }

            return true;
        }
    }
}
