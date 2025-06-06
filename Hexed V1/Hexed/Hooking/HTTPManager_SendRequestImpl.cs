using BestHTTP;
using CoreRuntime.Manager;
using Hexed.Core;
using Hexed.Interfaces;

namespace Hexed.Hooking
{
    internal class HTTPManager_SendRequestImpl : IHook
    {
        private delegate void _SendRequestImplDelegate(nint __0);
        private static _SendRequestImplDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_SendRequestImplDelegate>(typeof(HTTPManager).GetMethod(nameof(HTTPManager.SendRequestImpl)), Patch); // replace with API_SendRequestInternal
        }

        private static void Patch(nint __0)
        {
            HTTPRequest request = __0 == nint.Zero ? null : new(__0);

            if (request != null)
            {
                if (InternalSettings.APILog) Wrappers.Logger.LogApi(request);

                if (request.Uri != null && request.Uri.ToString().Contains("amplitude.com")) return;
            }

            originalMethod(__0);
        }
    }
}
