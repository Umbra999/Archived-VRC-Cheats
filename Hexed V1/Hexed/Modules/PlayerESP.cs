using Hexed.Core;
using Hexed.Extensions;
using Hexed.Interfaces;
using Hexed.Wrappers;
using UnityEngine;
using VRC.Core;

namespace Hexed.Modules
{
    internal class PlayerESP : IPlayerModule, IDelayModule
    {
        private VRCPlayer targetPlayer;

        public void Initialize(VRCPlayer player)
        {
            targetPlayer = player;
        }

        public void OnUpdate()
        {
            if (targetPlayer == null) return;

            if (InternalSettings.PlayerESP) ToggleCapsuleHighlight(true);
            else ToggleCapsuleHighlight(false);
        }

        public void ToggleCapsuleHighlight(bool State)
        {
            string HighlightName = "PlayerCapsule-" + targetPlayer.UserID();

            if (State)
            {
                HighlightsFXStandalone highlightFx = GetOrAddHighlight(HighlightName);

                if (targetPlayer.prop_PlayerSelector_0 == null) return;

                MeshFilter filter = targetPlayer.prop_PlayerSelector_0.GetComponent<MeshFilter>();
                if (filter == null) return;

                APIUser ApiUser = targetPlayer.GetAPIUser();
                if (ApiUser == null) return;

                highlightFx.highlightColor = ApiUser.GetRankColor();

                HighlightHelper.ToggleHighlightFx(highlightFx, filter, State);
            }
            else
            {
                DestroyHighlightFx(HighlightName);
            }
        }

        private HighlightsFXStandalone GetOrAddHighlight(string name)
        {
            if (!presentHighlights.TryGetValue(name, out HighlightsFXStandalone highlightFx))
            {
                highlightFx = HighlightsFX.prop_HighlightsFX_0.gameObject.AddComponent<HighlightsFXStandalone>();
                highlightFx.blurSize = highlightFx.blurSize / 2;
                presentHighlights[name] = highlightFx;
            }

            return highlightFx;
        }

        private HighlightsFXStandalone GetHighlight(string name)
        {
            if (presentHighlights.TryGetValue(name, out HighlightsFXStandalone highlightFx)) return highlightFx;

            return null;
        }

        private void DestroyHighlightFx(string name)
        {
            HighlightsFXStandalone highlights = GetHighlight(name);
            if (highlights != null)
            {
                highlights.field_Protected_HashSet_1_MeshFilter_0.Clear();
            }
        }

        private readonly Dictionary<string, HighlightsFXStandalone> presentHighlights = new();
    }
}
