using System.Collections.Generic;
using UnityEngine;

namespace WengaPort.Modules
{
    internal static class Lewdify
    {
        internal static List<string> TermsToToggleOff = new List<string>
        {
            "cloth",
            "shirt",
            "pant",
            "under",
            "undi",
            "jacket",
            "top",
            "bra",
            "skirt",
            "jean",
            "trouser",
            "boxers",
            "hoodi",
            "bottom",
            "dress",
            "bandage",
            "bondage",
            "sweat",
            "cardig",
            "corset",
            "tiddy",
            "pastie",
            "suit",
            "stocking",
            "jewel",
            "frill",
            "gauze",
            "cover",
            "pubic",
            "sfw",
            "harn",
            "biki"
        };

        internal static List<string> TermsToToggleOn = new List<string>
        {
            "penis",
            "dick",
            "cock",
            "futa",
            "dildo",
            "strap",
            "shlong",
            "schlong",
            "dong",
            "vibrat",
            "lovense",
            "toy",
            "butt",
            "plug",
            "whip",
            "cum",
            "sperm",
            "facial",
            "nude",
            "naked",
            "nsfw"
        };

        internal static void LewdifyAvatar(this GameObject avatar)
        {
            if (avatar == null)
            {
                return;
            }
            foreach (SkinnedMeshRenderer componentsInChild in avatar.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true))
            {
                if (!(componentsInChild.transform.parent != null) || !(componentsInChild.transform.parent.parent != null) || !(componentsInChild.transform.parent.parent.parent != null) || !(componentsInChild.transform.parent.parent.parent.parent != null))
                {
                    continue;
                }
                foreach (string item in TermsToToggleOn)
                {
                    if ((componentsInChild.transform.parent.parent.parent.parent.name.ToLower().Contains(item) || (componentsInChild.transform.parent.parent.parent.parent.parent != null && componentsInChild.transform.parent.parent.parent.parent.parent.name.ToLower().Contains(item))) && componentsInChild.transform.parent.gameObject != null)
                    {
                        componentsInChild.gameObject.SetActive(value: true);
                        componentsInChild.transform.parent.gameObject.SetActive(value: true);
                        componentsInChild.transform.parent.parent.gameObject.SetActive(value: true);
                        componentsInChild.transform.parent.parent.parent.gameObject.SetActive(value: true);
                        componentsInChild.transform.parent.parent.parent.parent.gameObject.SetActive(value: true);
                        if (componentsInChild.transform.parent.parent.parent.parent.parent != null)
                        {
                            componentsInChild.transform.parent.parent.parent.parent.parent.gameObject.SetActive(value: true);
                        }
                        if (componentsInChild.transform.parent.parent.parent.parent.GetComponent<Animator>() != null)
                        {
                            componentsInChild.transform.parent.parent.parent.parent.GetComponent<Animator>().enabled = false;
                        }
                        else if (componentsInChild.GetComponent<Animator>() != null)
                        {
                            componentsInChild.GetComponent<Animator>().enabled = false;
                        }
                    }
                }
                foreach (string item2 in TermsToToggleOff)
                {
                    if (componentsInChild.transform.parent.parent.parent.parent.name.ToLower().Replace("nsfw", "").Contains(item2) && componentsInChild.transform.parent.gameObject != null)
                    {
                        if (componentsInChild.transform.parent.parent.parent.parent.GetComponent<Animator>() != null)
                        {
                            componentsInChild.transform.parent.parent.parent.parent.GetComponent<Animator>().enabled = false;
                        }
                        else if (componentsInChild.GetComponent<Animator>() != null)
                        {
                            componentsInChild.GetComponent<Animator>().enabled = false;
                        }
                        Object.Destroy(componentsInChild.transform.parent.gameObject);
                    }
                }
            }
            foreach (MeshRenderer componentsInChild2 in avatar.GetComponentsInChildren<MeshRenderer>(includeInactive: true))
            {
                if (!(componentsInChild2.transform.parent != null) || !(componentsInChild2.transform.parent.parent != null) || !(componentsInChild2.transform.parent.parent.parent != null) || !(componentsInChild2.transform.parent.parent.parent.parent != null))
                {
                    continue;
                }
                foreach (string item3 in TermsToToggleOn)
                {
                    if ((componentsInChild2.transform.parent.parent.parent.parent.name.ToLower().Contains(item3) || (componentsInChild2.transform.parent.parent.parent.parent.parent != null && componentsInChild2.transform.parent.parent.parent.parent.parent.name.ToLower().Contains(item3))) && componentsInChild2.transform.parent.gameObject != null)
                    {
                        componentsInChild2.gameObject.SetActive(value: true);
                        componentsInChild2.transform.parent.gameObject.SetActive(value: true);
                        componentsInChild2.transform.parent.parent.gameObject.SetActive(value: true);
                        componentsInChild2.transform.parent.parent.parent.gameObject.SetActive(value: true);
                        componentsInChild2.transform.parent.parent.parent.parent.gameObject.SetActive(value: true);
                        if (componentsInChild2.transform.parent.parent.parent.parent.parent != null)
                        {
                            componentsInChild2.transform.parent.parent.parent.parent.parent.gameObject.SetActive(value: true);
                        }
                        if (componentsInChild2.transform.parent.parent.parent.parent.GetComponent<Animator>() != null)
                        {
                            componentsInChild2.transform.parent.parent.parent.parent.GetComponent<Animator>().enabled = false;
                        }
                        else if (componentsInChild2.GetComponent<Animator>() != null)
                        {
                            componentsInChild2.GetComponent<Animator>().enabled = false; 
                        }
                    }
                }
                foreach (string item4 in TermsToToggleOff)
                {
                    if (componentsInChild2.transform.parent.parent.parent.parent.name.ToLower().Replace("nsfw", "").Contains(item4) && componentsInChild2.transform.parent.gameObject != null)
                    {
                        if (componentsInChild2.transform.parent.parent.parent.parent.GetComponent<Animator>() != null)
                        {
                            componentsInChild2.transform.parent.parent.parent.parent.GetComponent<Animator>().enabled = false;
                        }
                        else if (componentsInChild2.GetComponent<Animator>() != null)
                        {
                            componentsInChild2.GetComponent<Animator>().enabled = false;
                            Object.Destroy(componentsInChild2.transform.parent.gameObject);
                        }
                    }
                }
            }
        }
    }
}
