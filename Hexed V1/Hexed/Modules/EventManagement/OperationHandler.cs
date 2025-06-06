using Hexed.Core;
using Il2CppInterop.Runtime;

namespace Hexed.Modules.EventManagement
{
    internal class OperationHandler
    {
        public static void ChangeOperation226(Il2CppSystem.Collections.Generic.Dictionary<byte, Il2CppSystem.Object> param)
        {
            if (!param.ContainsKey(249)) return;

            Il2CppSystem.Collections.Hashtable hashtable = param[249].TryCast<Il2CppSystem.Collections.Hashtable>();

            if (hashtable == null) return;

            if (hashtable.ContainsKey("inVRMode"))
            {
                switch (InternalSettings.VRSpoof)
                {
                    case InternalSettings.VRMode.VR:
                        hashtable["inVRMode"] = new Il2CppSystem.Boolean()
                        {
                            m_value = true
                        }.BoxIl2CppObject();
                        break;

                    case InternalSettings.VRMode.Desktop:
                        hashtable["inVRMode"] = new Il2CppSystem.Boolean()
                        {
                            m_value = false
                        }.BoxIl2CppObject();
                        break;
                }
            }

            if (hashtable.ContainsKey("groupOnNameplate"))
            {
                if (InternalSettings.GroupSpoof) hashtable["groupOnNameplate"] = new(IL2CPP.ManagedStringToIl2Cpp(InternalSettings.FakeGroupValue));
            }
        }
    }
}
