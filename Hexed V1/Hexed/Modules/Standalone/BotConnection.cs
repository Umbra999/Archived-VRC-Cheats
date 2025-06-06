using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Hexed.Modules.Standalone
{
    internal class BotConnection
    {
        private static Socket Server;
        private static Socket Client;

        public static string LatestResponse;

        public static void StartBot() // REWORK; ITS GARBAGE
        {
            new Thread(() =>
            {
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint localEndPoint = new(ipAddress, 9999);

                try
                {
                    Server = new(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    Server.Bind(localEndPoint);
                    Server.Listen(10);

                    Client = Server.Accept();
                }
                catch
                {
                    StopBot();
                }

                while (Client != null && Client.Connected)
                {
                    try
                    {
                        byte[] bytes = new byte[5000];
                        int bytesRec = Client.Receive(bytes);
                        string Message = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                        string[] command = Message.Contains('/') ? Message.Split('/') : new string[] { Message };

                        switch (command[0])
                        {
                            case "SelfBot":
                                switch (command[1])
                                {
                                    case "Expose":
                                        LatestResponse = command[2];
                                        break;
                                }
                                break;
                        }
                    }
                    catch
                    {
                        StopBot();
                    }
                }
            })
            { IsBackground = true }.Start();
        }

        public static void StopBot()
        {
            SendMessage("Shutdown");
            Server?.Close();
            Client?.Close();
            Server = null;
            Client = null;
        }

        private static void SendMessage(string Message)
        {
            if (Client != null && Client.Connected) Client.Send(Encoding.ASCII.GetBytes(Message));
        }

        public static void JoinRoom(string WorldID)
        {
            SendMessage($"JoinRoom/{WorldID}");
        }

        public static void LeaveRoom()
        {
            SendMessage($"LeaveRoom");
        }

        public static void PlayAudioFile(string PathB64)
        {
            SendMessage($"HandleAudio/PlayAudio/{PathB64}");
        }

        public static void PlayTextToVoice(string Text)
        {
            SendMessage($"HandleAudio/TextToVoice/{Text}");
        }

        public static void PlayYoutube(string URL)
        {
            SendMessage($"HandleAudio/PlayYoutube/{URL}");
        }

        public static void StopAudioReplay()
        {
            SendMessage($"HandleAudio/StopAudio");
        }

        public static void ChangeAudioVolume(bool Increase)
        {
            SendMessage($"HandleAudio/ChangeVolume/{Increase}");
        }

        public static void ResetAudioVolume()
        {
            SendMessage($"HandleAudio/ResetVolume");
        }

        public static void MimicMovement(int Actor)
        {
            SendMessage($"HandleMimic/Movement/{Actor}");
        }

        public static void MimicAvatarSync(int Actor)
        {
            SendMessage($"HandleMimic/AvatarSync/{Actor}");
        }

        public static void MimicVoice(int Actor)
        {
            SendMessage($"HandleMimic/Voice/{Actor}");
        }

        public static void MimicRPC(int Actor)
        {
            SendMessage($"HandleMimic/RPC/{Actor}");
        }

        public static void MimicChat(int Actor)
        {
            SendMessage($"HandleMimic/Chat/{Actor}");
        }

        public static void ChangePositionOffset(string coordinate, bool Increase)
        {
            SendMessage($"HandleMovementOffset/Pos{coordinate}/{Increase}");
        }

        public static void ChangeRotationOffset(string coordinate, bool Increase)
        {
            SendMessage($"HandleMovementOffset/Rot{coordinate}/{Increase}");
        }

        public static void ResetPositionOffset(string coordinate)
        {
            SendMessage($"HandleMovementOffset/Reset{coordinate}Pos");
        }

        public static void ResetRotationOffset(string coordinate)
        {
            SendMessage($"HandleMovementOffset/Reset{coordinate}Rot");
        }

        public static void ChangeOrbitRange(bool Increase)
        {
            SendMessage($"HandleOrbit/Range/{Increase}");
        }

        public static void ChangeOrbitSpeed(bool Increase)
        {
            SendMessage($"HandleOrbit/Speed/{Increase}");
        }

        public static void ResetOrbitRange()
        {
            SendMessage($"HandleOrbit/ResetRange");
        }

        public static void ResetOrbitSpeed()
        {
            SendMessage($"HandleOrbit/ResetSpeed");
        }

        public static void FreezePosition(bool State)
        {
            SendMessage($"FreezePosition/{State}");
        }

        public static void FreezeRotation(bool State)
        {
            SendMessage($"FreezeRotation/{State}");
        }

        public static void ChangeAvatar(string ID)
        {
            SendMessage($"ChangeAvatar/{ID}");
        }

        public static void ChangeFallbackAvatar(string ID)
        {
            SendMessage($"ChangeFallbackAvatar/{ID}");
        }

        public static void ChangeStatus(string Text)
        {
            SendMessage($"ChangeStatus/{Text}");
        }

        public static void ChangeBio(string Text)
        {
            SendMessage($"ChangeBio/{Text}");
        }

        public static void ChatMessage(string message)
        {
            SendMessage($"SendChat/{message}");
        }

        public static void AvatarHeight(int Height)
        {
            SendMessage($"AvatarHeight/{Height}");
        }

        public static void Block(string UserID, bool State)
        {
            SendMessage($"HandleBlock/{State}/{UserID}");
        }

        public static void Mute(string UserID, bool State)
        {
            SendMessage($"HandleMute/{State}/{UserID}");
        }

        public static void CacheCrash()
        {
            SendMessage($"HandleExploit/CacheCrash");
        }

        public static void VoiceCrash()
        {
            SendMessage($"HandleExploit/VoiceCrash");
        }

        public static void Forcekick(string UserID)
        {
            SendMessage($"HandleExploit/Forcekick/{UserID}");
        }

        public static void AFKMode(bool State)
        {
            SendMessage($"HandleAFK/{State}");
        }

        public static void CameraFollowMode(bool State)
        {
            SendMessage($"HandleCameraFollow/{State}");
        }

        public static void LoopAnimationRecord(bool State)
        {
            SendMessage($"HandleMovementRecord/Loop/{State}");
        }

        public static void PlayMotionFile(string Name)
        {
            SendMessage($"HandleMovementRecord/Play/{Name}");
        }

        public static void StopMotionReplay()
        {
            SendMessage($"HandleMovementRecord/Stop");
        }

        public static void StartRecordMotion(int Actor, string Filename)
        {
            SendMessage($"HandleMovementRecord/StartRecord/{Actor}/{Filename}");
        }

        public static void StopRecordMotion()
        {
            SendMessage($"HandleMovementRecord/StopRecord");
        }

        public static void AddFriend(string UserID)
        {
            SendMessage($"HandleFriend/Add/{UserID}");
        }

        public static void SelfbotExpose()
        {
            LatestResponse = null;
            SendMessage($"SelfBot/Expose");
        }

        public static void SelfbotVoice(string Base64)
        {
            SendMessage($"SelfBot/Voice/{Base64}");
        }

        public static void SelfbotMovement(string Base64)
        {
            SendMessage($"SelfBot/Movement/{Base64}");
        }

        public static void SelfbotAvatarSync(string Base64)
        {
            SendMessage($"SelfBot/AvatarSync/{Base64}");
        }

        public static void SelfbotChat(string Message)
        {
            SendMessage($"SelfBot/Chat/{Message}");
        }

        public static void ChatCommands(bool State)
        {
            SendMessage($"HandleChatCommands/{State}");
        }
    }
}
