using Hexed.Interfaces;
using Hexed.UIApi;
using Hexed.Wrappers;

namespace Hexed.Modules
{
    internal class UserDetailList : IGlobalModule
    {
        public void Initialize()
        {
            
        }

        public void OnUpdate()
        {
            if (!GameUtils.IsInWorld()) return;

            VRC.Player Selected = PlayerSimplifier.GetSelectedPlayer();

            if (CustomUI.SelectedUserMenu.MainMenu.InfoText == null || !MenuHelper.QuickMenu.isActiveAndEnabled || Selected == null) return;

            CustomUI.SelectedUserMenu.MainMenu.InfoText.SetText(GetPlayerString(Selected.GetPhotonPlayer()));
        }

        private static string GetPlayerString(Photon.Realtime.Player PhotonPlayer)
        {
            VRC.Player player = PhotonPlayer.GetPlayer();

            if (player == null || player.GetAPIUser() == null) return "";

            string[] Details =
            [
                $"Name: {player.DisplayName()}",
                $"Status: {player.GetAPIUser().status}",
                $"Actor: {PhotonPlayer.ActorID()}",
                $"FPS: {player.GetFrames()}",
                $"Ping: {player.GetPing()}",
                $"Quality: {player.GetQualityPercentage()}%",
                $"Avatar Name: {player.GetAvatar().name}",
                $"Avatar Author: {player.GetAvatar().authorName}",
                $"Avatar Version: {player.GetAvatar().unityVersion}",
                $"Avatar Updated: {player.GetAvatar().updated_at}",
                $"Avatar Status: {player.GetAvatar().releaseStatus}",
                $"Fallback Name: {player.GetFallbackAvatar().name}",
                $"Fallback Author: {player.GetFallbackAvatar().authorName}",
                $"Fallback Version: {player.GetFallbackAvatar().unityVersion}",
                $"Fallback Updated: {player.GetFallbackAvatar().updated_at}",
                $"Fallback Status: {player.GetFallbackAvatar().releaseStatus}",
            ];

            return string.Join("\n", Details);
        }
    }
}
