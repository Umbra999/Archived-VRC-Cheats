using AmplitudeSDKWrapper;
using CoreRuntime.Manager;
using Hexed.Interfaces;
using System.Reflection;

namespace Hexed.Hooking
{
    internal class AmplitudeWrapper_ALL : IHook
    {
        private delegate void _ReturnAllDelegate();

        public void Initialize()
        {
            foreach (MethodInfo method in typeof(AmplitudeWrapper).GetMethods().Where(x => x.Name.Contains("Start")|| x.Name.Contains("UpdateServer") || x.Name.Contains("PostEvents") || x.Name.Contains("LogEvent") || x.Name.Contains("SaveEvent") || x.Name.Contains("SaveAndUpload")))
            {
                HookManager.Detour<_ReturnAllDelegate>(method, Patch);
            }
        }

        private static void Patch() // idk if thats an issue to redirect them all to this empty method? maybe returnparams causing trouble idk
        {

        }
    }
}
