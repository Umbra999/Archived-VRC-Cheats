using ExitGames.Client.Photon;
using LunaR.Extensions;
using LunaR.Wrappers;
using Newtonsoft.Json;
using System.Linq;

namespace LunaR.Patching
{
    internal class OperationHandler
    {
        private static readonly byte[] LogIgnoreCodes = new byte[] { 253 };

        public static void LogOperation(byte Code, Il2CppSystem.Collections.Generic.Dictionary<byte, Il2CppSystem.Object> Params, SendOptions Options)
        {
            try
            {
                if (LogIgnoreCodes.Contains(Code)) return;
                object Data = IL2CPPSerializer.IL2CPPToManaged.Serialize(Params);
                Logger.Log($"[Operation] {Code}", Logger.LogsType.Clean);
                Logger.Log($"[Dictionary] \n{JsonConvert.SerializeObject(Data, Formatting.Indented)}", Logger.LogsType.Clean);
                Logger.Log($"[SendOptions] Channel: {Options.Channel} Mode: {Options.DeliveryMode} Encrypt: {Options.Encrypt}", Logger.LogsType.Clean);
            }
            catch
            {
                Logger.LogError($"Failed to Log Operation with code {Code}");
            }
        }

        public static void ChangeProperties(Il2CppSystem.Collections.Generic.Dictionary<byte, Il2CppSystem.Object> param, byte propertyIndex)
        {
            if (param.ContainsKey(propertyIndex))
            {
                Il2CppSystem.Collections.Hashtable hashtable = param[propertyIndex].TryCast<Il2CppSystem.Collections.Hashtable>();
                if (hashtable != null && hashtable.ContainsKey("inVRMode"))
                {
                    switch (PatchExtensions.VRSpoof)
                    {
                        case PatchExtensions.VRMode.VR:
                            hashtable["inVRMode"] = new Il2CppSystem.Boolean()
                            {
                                m_value = true
                            }.BoxIl2CppObject();
                            break;

                        case PatchExtensions.VRMode.Desktop:
                            hashtable["inVRMode"] = new Il2CppSystem.Boolean()
                            {
                                m_value = false
                            }.BoxIl2CppObject();
                            break;
                    }
                }
            }
        }
    }
}