using Hexed.Core;
using Hexed.Extensions;
using Hexed.Interfaces;
using Hexed.Wrappers;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Hexed.Modules
{
    internal class WorldESP : IGlobalModule, IDelayModule
    {
        public void Initialize()
        {
            
        }

        public void OnUpdate()
        {
            if (!GameUtils.IsInWorld())
            {
                cachedTriggers.Clear();
                cachedPickups.Clear();
                return;
            }

            if (InternalSettings.ItemESP)
            {
                if (cachedPickups.Count == 0)
                {
                    foreach (VRC_Pickup pickup in ItemHelper.GetAllPickups())
                    {
                        cachedPickups.Add(pickup);
                    }
                }

                foreach (VRC_Pickup pickup in cachedPickups)
                {
                    TogglePickupESP(pickup, true);
                }
            }
            else
            {
                foreach (VRC_Pickup pickup in cachedPickups)
                {
                    TogglePickupESP(pickup, false);
                }
                cachedPickups.Clear();
            }

            if (InternalSettings.TriggerESP)
            {
                if (cachedTriggers.Count == 0)
                {
                    foreach (UdonBehaviour trigger in Resources.FindObjectsOfTypeAll<UdonBehaviour>())
                    {
                        cachedTriggers.Add(trigger);
                    }
                }

                foreach (UdonBehaviour trigger in cachedTriggers)
                {
                    ToggleTriggerESP(trigger, true);
                }
            }
            else
            {
                foreach (UdonBehaviour trigger in cachedTriggers)
                {
                    ToggleTriggerESP(trigger, false);
                }
                cachedTriggers.Clear();
            }
        }

        public void TogglePickupESP(VRC_Pickup pickup, bool State)
        {
            string HighlightName = "PickupRenderer-" + pickup.name;

            if (State)
            {
                MeshFilter filter = pickup.GetComponent<MeshFilter>();
                if (filter == null) return;

                HighlightsFXStandalone highlightFx = GetOrAddHighlight(HighlightName);
                highlightFx.highlightColor = Color.gray;

                HighlightHelper.ToggleHighlightFx(highlightFx, filter, State);
            }
            else
            {
                DestroyHighlightFx(HighlightName);
            }
        }

        public void ToggleTriggerESP(UdonBehaviour trigger, bool State)
        {
            string HighlightName = "TriggerRenderer-" + trigger.name;

            if (State)
            {
                if (!trigger._hasInteractiveEvents) return;

                MeshFilter filter = trigger.GetComponent<MeshFilter>();
                if (filter == null) return;

                HighlightsFXStandalone highlightFx = GetOrAddHighlight(HighlightName);
                highlightFx.highlightColor = Color.gray;

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
        private List<UdonBehaviour> cachedTriggers = new();
        private List<VRC_Pickup> cachedPickups = new();
    }
}
