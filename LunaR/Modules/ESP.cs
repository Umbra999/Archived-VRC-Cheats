using LunaR.Wrappers;
using System;
using System.Collections;
using System.Linq;
using UnhollowerBaseLib;
using UnityEngine;
using VRC.SDKBase;

namespace LunaR.Modules
{
    internal class ESP : MonoBehaviour
    {
        public static void HighlightColor(Color highlightcolor)
        {
            foreach (HighlightsFXStandalone HighlightFX in Resources.FindObjectsOfTypeAll<HighlightsFXStandalone>())
            {
                HighlightFX.highlightColor = highlightcolor;
            }
        }

        public static void CoreColor(Color highlightcolor)
        {
            foreach (MeshRenderer Renderer in Resources.FindObjectsOfTypeAll<MeshRenderer>().Where(m => m.name.Contains("SelectRegion")))
            {
                Renderer.material.color = highlightcolor;
            }
        }

        public static bool ESPEnabled = false;

        public static void ESPToggle(bool State)
        {
            if (State)
            {
                ESPEnabled = true;
                foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (gameObject.transform.Find("SelectRegion")) HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(gameObject.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                }
            }
            else
            {
                ESPEnabled = false;
                foreach (GameObject gameObject2 in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (gameObject2.transform.Find("SelectRegion")) HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(gameObject2.transform.Find("SelectRegion").GetComponent<Renderer>(), false);
                }
            }
        }

        public static bool PickupESP = false;

        public static IEnumerator ItemESP()
        {
            PickupESP = true;
            Il2CppArrayBase<VRC_Pickup> Items = Resources.FindObjectsOfTypeAll<VRC_Pickup>();
            while (PickupESP && GeneralWrappers.IsInWorld())
            {
                foreach (VRC_Pickup Item in Items)
                {
                    if (Item != null && Item.isActiveAndEnabled && Item.pickupable && !Item.name.Contains("ViewFinder") && HighlightsFX.prop_HighlightsFX_0 != null) HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(Item.GetComponentInChildren<MeshRenderer>(), true);
                    else if (Item == null) Items = Resources.FindObjectsOfTypeAll<VRC_Pickup>();
                }
                yield return new WaitForEndOfFrame();
            }

            foreach (VRC_Pickup Item in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
            {
                if (Item != null && Item.isActiveAndEnabled && Item.pickupable && !Item.name.Contains("ViewFinder") && HighlightsFX.prop_HighlightsFX_0 != null) HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(Item.GetComponentInChildren<MeshRenderer>(), false);
            }

            PickupESP = false;
        }

        public static bool TriggerESPToggle = false;

        public static IEnumerator TriggerESP()
        {
            TriggerESPToggle = true;
            Il2CppArrayBase<VRC_Trigger> Triggers = Resources.FindObjectsOfTypeAll<VRC_Trigger>();
            while (TriggerESPToggle && GeneralWrappers.IsInWorld())
            {
                foreach (VRC_Trigger Trigger in Triggers)
                {
                    if (Trigger != null && HighlightsFX.prop_HighlightsFX_0 != null) HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(Trigger.GetComponentInChildren<MeshRenderer>(), true);
                    else if (Trigger == null) Triggers = Resources.FindObjectsOfTypeAll<VRC_Trigger>();
                }
                yield return new WaitForSeconds(1);
            }

            foreach (VRC_Trigger Trigger in Resources.FindObjectsOfTypeAll<VRC_Trigger>())
            {
                if (Trigger != null && HighlightsFX.prop_HighlightsFX_0 != null) HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(Trigger.GetComponentInChildren<MeshRenderer>(), false);
            }

            TriggerESPToggle = false;
        }

        public ESP(IntPtr ptr) : base(ptr)
        {
        }
    }
}