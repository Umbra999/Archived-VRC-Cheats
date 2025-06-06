using LunaR.Extensions;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

namespace LunaR.Modules
{
    internal class AssetChanger
    {
        private static IEnumerator LoadCustomAsset(string url)
        {
            UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);
            www.SendWebRequest();
            while (!www.isDone) yield return new WaitForEndOfFrame();

            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            string prefab_path = bundle.GetAllAssetNames().Where(x => x.EndsWith(".prefab")).FirstOrDefault();

            if (prefab_path == null)
            {
                Extensions.Logger.LogError("Failed to find prefab in Assetbundle");
                yield break;
            }

            GameObject NewHideObject = bundle.LoadAsset<GameObject>(prefab_path);
            NewHideObject.hideFlags = HideFlags.DontUnloadUnusedAsset;

            VRCAvatarManager manager = null;
            while (manager == null)
            {
                manager = Resources.FindObjectsOfTypeAll<VRCAvatarManager>().FirstOrDefault();
                yield return new WaitForSeconds(1f);
            }

            string[] TargetFieldValueNames =
            {
                "Avatar_Utility_Base_ERROR",
                "Avatar_Utility_Base_SAFETY",
                "Avatar_Utility_Base_BLOCKED_PERFORMANCE"
            };

            foreach (PropertyInfo prop in typeof(VRCAvatarManager).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(prop => prop.Name.StartsWith("field_Public_GameObject_")))
            {
                string name = ((GameObject)prop.GetValue(manager))?.name;
                if (name != null && TargetFieldValueNames.Contains(name)) prop.SetValue(manager, NewHideObject);
            }
        }

        public static void Init()
        {
            LoadCustomAsset(ConfigManager.Ini.GetString("Toggles", "HideRobot")).Start();
        }
    }
}