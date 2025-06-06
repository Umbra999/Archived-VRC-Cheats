using BestHTTP;
using ExitGames.Client.Photon;
using Hexed.Modules.Standalone;
using Photon.Realtime;
using System.Text;
using System.Text.Json;
using VRC.SDKBase;

namespace Hexed.Wrappers
{
    public static class Logger
    {
        private static readonly object consoleLock = new();
        public enum LogsType
        {
            Info,
            Protection,
            Room,
            Friends,
            Chat,
            Group,
            Bot
        }

        public static void Log(object obj, LogsType Type)
        {
            lock (consoleLock)
            {
                if (obj == null) obj = "NULL";
                string log = obj.ToString().Replace("\a", "a").Replace("\u001B[", "u001B[");

                ExternalConsole.writeMessage($"[{DateTime.Now.ToShortTimeString()}] [Hexed] [{Type}] {log}");
            }
        }

        public static void LogError(object obj)
        {
            lock (consoleLock)
            {
                if (obj == null) obj = "NULL";
                string log = obj.ToString().Replace("\a", "a").Replace("\u001B[", "u001B[");

                ExternalConsole.writeMessage($"[{DateTime.Now.ToShortTimeString()}] [Hexed] [ERROR] {log}");
            }
        }

        public static void LogWarning(object obj)
        {
            lock (consoleLock)
            {
                if (obj == null) obj = "NULL";
                string log = obj.ToString().Replace("\a", "a").Replace("\u001B[", "u001B[");

                ExternalConsole.writeMessage($"[{DateTime.Now.ToShortTimeString()}] [Hexed] [WARNING] {log}");
            }
        }

        public static void LogDebug(object obj)
        {
            lock (consoleLock)
            {
                if (obj == null) obj = "NULL";
                string log = obj.ToString().Replace("\a", "a").Replace("\u001B[", "u001B[");

                ExternalConsole.writeMessage($"[{DateTime.Now.ToShortTimeString()}] [Hexed] [DEBUG] {log}");
            }
        }

        private static readonly byte[] OnEventLogIgnore = new byte[] { 1, 8, 35, 66 };
        private static readonly byte[] OpRaiseLogIgnore = new byte[] { 8, 66 };
        private static readonly byte[] OperationLogIgnore = new byte[] { 253 };

        public static void LogOpRaise(byte Code, Il2CppSystem.Object Object, RaiseEventOptions RaiseOptions, SendOptions SendOptions)
        {
            try
            {
                if (OpRaiseLogIgnore.Contains(Code)) return;

                object CustomData = Object == null ? "NULL" : CPP2IL.TypeSerializer.ILToManaged(Object);
                string TargetActors = RaiseOptions?.field_Public_Il2CppStructArray_1_Int32_0 != null ? string.Join(", ", RaiseOptions.field_Public_Il2CppStructArray_1_Int32_0) : "NULL";

                string IfBytes = null;
                if (CustomData.GetType() == typeof(byte[]))
                {
                    IfBytes = "  |  ";
                    byte[] data = (byte[])CustomData;
                    IfBytes += string.Join(", ", data);
                    IfBytes += $" [L: {data.Length}]";
                }

                else if (CustomData.GetType() == typeof(ArraySegment<byte>))
                {
                    IfBytes = "  |  ";
                    ArraySegment<byte> segment = (ArraySegment<byte>)CustomData;

                    IfBytes += string.Join(", ", segment.Array);
                    IfBytes += $" [L: {segment.Count}]";
                    IfBytes += $" [O: {segment.Offset}]";
                }

                Log(string.Concat(new object[]
                {
                   Environment.NewLine,
                    $"======= OPRAISEEVENT {Code} =======", Environment.NewLine,
                    $"TYPE: {CustomData}", Environment.NewLine,
                   $"DATA SERIALIZED: {(IfBytes == null ? JsonSerializer.Serialize(CustomData, new JsonSerializerOptions() { WriteIndented = true }) : IfBytes)}", Environment.NewLine,
                    $"TARGET ACTORS: {TargetActors}", Environment.NewLine,
                    $"EVENT CACHING: {RaiseOptions.field_Public_EventCaching_0}", Environment.NewLine,
                    $"INTEREST GROUP: {RaiseOptions.field_Public_Byte_0}", Environment.NewLine,
                    $"RECEIVER GROUP: {RaiseOptions.field_Public_ReceiverGroup_0}", Environment.NewLine,
                    $"CHANNEL: {SendOptions.Channel}",Environment.NewLine,
                    $"DELIVERY MODE: {SendOptions.DeliveryMode}",Environment.NewLine,
                    $"ENCRYPT: {SendOptions.Encrypt}",Environment.NewLine,
                    $"RELIABLE: {SendOptions.Reliability}",Environment.NewLine,
                    "======= END =======",
                }), LogsType.Info);
            }
            catch (Exception e)
            {
                LogError($"Failed to Log Event with code {Code} with Exception: {e}");
            }
        }

        public static void LogEventData(EventData EventData)
        {
            try
            {
                if (OnEventLogIgnore.Contains(EventData.Code)) return;

                object CustomData = EventData.CustomData == null ? "NULL" : CPP2IL.TypeSerializer.ILToManaged(EventData.CustomData);

                Player PhotonPlayer = null;
                string SenderNAME = "NULL";

                if (EventData.Sender > 0) PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer(EventData.Sender);
                if (PhotonPlayer != null) SenderNAME = PhotonPlayer.DisplayName() ?? "NO NAME";

                string IfBytes = null;
                if (CustomData.GetType() == typeof(byte[]))
                {
                    IfBytes = "  |  ";
                    byte[] arr = (byte[])CustomData;
                    IfBytes += string.Join(", ", arr);
                    IfBytes += $" [L: {arr.Length}]";
                }

                else if (CustomData.GetType() == typeof(ArraySegment<byte>))
                {
                    IfBytes = "  |  ";
                    ArraySegment<byte> segment = (ArraySegment<byte>)CustomData;

                    IfBytes += string.Join(", ", segment.Array);
                    IfBytes += $" [L: {segment.Count}]";
                    IfBytes += $" [O: {segment.Offset}]";
                }

                object ParameterData = "NULL";
                if (EventData.Parameters != null) ParameterData = CPP2IL.TypeSerializer.ILToManaged(EventData.Parameters);

                Log(string.Concat(new object[]
                {
                   Environment.NewLine,
                    $"======= ONEVENT {EventData.Code} =======", Environment.NewLine,
                    $"ACTOR NUMBER: {EventData.Sender}", Environment.NewLine,
                    $"SENDER NAME: {SenderNAME}", Environment.NewLine,
                    $"TYPE: {CustomData}", Environment.NewLine,
                    $"DATA SERIALIZED: {(IfBytes == null ? JsonSerializer.Serialize(CustomData, new JsonSerializerOptions() { WriteIndented = true }) : IfBytes)}", Environment.NewLine,
                    $"PARAMETER SERIALIZED: {JsonSerializer.Serialize(ParameterData, new JsonSerializerOptions() { WriteIndented = true })}", Environment.NewLine,
                    $"MEMORY POINTER: {EventData.Pointer}", Environment.NewLine,
                    "======= END =======",
                }), LogsType.Info);
            }
            catch (Exception e)
            {
                LogError($"Failed to Log Event with code {EventData.Code} with Exception: {e}");
            }
        }

        public static void LogOperation(byte Code, Il2CppSystem.Collections.Generic.Dictionary<byte, Il2CppSystem.Object> Params, SendOptions SendOptions)
        {
            try
            {
                if (OperationLogIgnore.Contains(Code)) return;

                object Data = CPP2IL.TypeSerializer.ILToManaged(Params);

                Log(string.Concat(new object[]
                {
                   Environment.NewLine,
                    $"======= OPERATION {Code} =======", Environment.NewLine,
                    $"DATA SERIALIZED: {JsonSerializer.Serialize(Data, new JsonSerializerOptions() { WriteIndented = true })}", Environment.NewLine,
                    $"CHANNEL: {SendOptions.Channel}",Environment.NewLine,
                    $"DELIVERY MODE: {SendOptions.DeliveryMode}",Environment.NewLine,
                    $"ENCRYPT: {SendOptions.Encrypt}",Environment.NewLine,
                    $"RELIABLE: {SendOptions.Reliability}",Environment.NewLine,
                    "======= END =======",
                }), LogsType.Info);
            }
            catch (Exception e)
            {
                LogError($"Failed to Log Operation with code {Code} and Exception: {e}");
            }
        }

        public static void LogOperationResponse(OperationResponse reponse)
        {
            try
            {
                object ParameterData = "NULL";
                if (reponse.Parameters != null) ParameterData = CPP2IL.TypeSerializer.ILToManaged(reponse.Parameters);

                Log(string.Concat(new object[]
                {
                   Environment.NewLine,
                    $"======= OPERATION RESPONSE {reponse.OperationCode} =======", Environment.NewLine,
                    $"PARAMETER SERIALIZED: {JsonSerializer.Serialize(ParameterData, new JsonSerializerOptions() { WriteIndented = true })}", Environment.NewLine,
                    $"RETURN CODE: {reponse.ReturnCode}",Environment.NewLine,
                    $"DEBUG MESSAGE: {reponse.DebugMessage}",Environment.NewLine,
                    "======= END =======",
                }), LogsType.Info);
            }
            catch (Exception e)
            {
                LogError($"Failed to Log Operation Response with code {reponse.OperationCode} and Exception: {e}");
            }
        }

        public static void LogRPC(VRC.Player Player, VRC_EventHandler.VrcEvent Event, VRC_EventHandler.VrcBroadcastType BroadcastType, int instagatorId, float fastForward, object[] DecodedBytes)
        {
            try
            {
                Log(string.Concat(new object[]
                {
                    Environment.NewLine,
                    $"======= RPC EVENT =======", Environment.NewLine,
                    $"ACTOR NUMBER: {instagatorId}", Environment.NewLine,
                    $"SENDER NAME: {Player.DisplayName()}", Environment.NewLine,
                    $"BYTES DECODED: {(DecodedBytes.Length > 0 ? string.Join(" | ", DecodedBytes.Where(b => b != null)) : "NULL")}", Environment.NewLine,
                    $"EVENTTYPE: {Event.EventType}", Environment.NewLine,
                    $"PARAMETER STRING: {Event.ParameterString}", Environment.NewLine,
                    $"PARAMETER INT: {Event.ParameterInt}", Environment.NewLine,
                    $"PARAMETER FLOAT: {Event.ParameterFloat}", Environment.NewLine,
                    $"PARAMETER BOOL: {Event.ParameterBool}", Environment.NewLine,
                    $"PARAMETER BOOLOP: {Event.ParameterBoolOp}", Environment.NewLine,
                    $"TAKE OWNERSHIP: {Event.TakeOwnershipOfTarget}", Environment.NewLine,
                    $"BROADCAST TYPE: {BroadcastType}", Environment.NewLine,
                    $"FAST FORWARD: {fastForward}", Environment.NewLine,
                    "======= END =======",
                }), LogsType.Info);
            }
            catch (Exception e)
            {
                LogError($"Failed to Log RPC with Exception: {e}");
            }
        }

        public static void LogApi(HTTPRequest request)
        {
            byte[] il2cppData = request.GetEntityBody();

            Log(string.Concat(new object[]
            {
                Environment.NewLine,
                $"======= API REQUEST =======", Environment.NewLine,
                $"METHOD: {request.MethodType}", Environment.NewLine,
                $"ENDPOINT: {request.Uri?.ToString()}", Environment.NewLine,
                $"CONTENT: {(il2cppData == null ? "NULL" : Encoding.UTF8.GetString(il2cppData))}", Environment.NewLine,
                $"HEADER: \n{request.DumpHeaders()}", Environment.NewLine,
                "======= END =======",
            }), LogsType.Info);
        }
    }
}
