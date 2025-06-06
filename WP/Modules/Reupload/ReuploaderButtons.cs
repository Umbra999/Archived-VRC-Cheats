using MelonLoader;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;
using VRC.Core;
using VRC.UI;
using WengaPort.ConsoleUtils;

namespace WengaPort.Modules.Reupload
{
    public class ReuploaderButtons : MonoBehaviour
    {
        public static string string_8;

        public static Delegate ReferencedDelegate;

        public static IntPtr MethodInfo;

        public static Il2CppSystem.Collections.Generic.List<MonoBehaviour> AntiGcList;

        public static volatile bool ActionsBool;

        public static System.Collections.Generic.List<Action> Actions = new System.Collections.Generic.List<Action>(10);

        public static System.Collections.Generic.List<Action> Actions2 = new System.Collections.Generic.List<Action>(10);

        private static string NewAvatarID = string.Empty;

        private static ApiAvatar SelectedAvatar;

        private static ApiAvatar apiAvatar_1;

        private static ApiWorld ReuploadedWorld;

        private static string NewWorldID = string.Empty;

        private static ApiWorld SelectedWorld;

        private static ApiFile WorldAssetBundle;

        private static ApiFile AvatarAssetBundle;

        private static readonly string AssetBundlePath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "AssetBundles");

        private static readonly string VrcaStorePath = Path.Combine(AssetBundlePath, "VrcaStore");

        private static Assembly assembly_0;

        private static Type GetUnpackerType;

        private static MethodInfo GetUnpackerMethod;

        public ReuploaderButtons(IntPtr intptr_1) : base(intptr_1)
        {
            AntiGcList = new Il2CppSystem.Collections.Generic.List<MonoBehaviour>(1);
            AntiGcList.Add(this);
        }

        public ReuploaderButtons(Delegate delegate_1, IntPtr intptr_1) : base(ClassInjector.DerivedConstructorPointer<ReuploaderButtons>())
        {
            ClassInjector.DerivedConstructorBody(this);
            ReferencedDelegate = delegate_1;
            MethodInfo = intptr_1;
        }

        ~ReuploaderButtons()
        {
            Marshal.FreeHGlobal(MethodInfo);
            MethodInfo = IntPtr.Zero;
            ReferencedDelegate = null;
            AntiGcList.Remove(this);
            AntiGcList = null;
        }

        public void Start()
        {
            try
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WengaPort.Resources.UBPU.exe"))
                {
                    using MemoryStream memoryStream = new MemoryStream((int)stream.Length);
                    stream.CopyTo(memoryStream);
                    assembly_0 = Assembly.Load(memoryStream.ToArray());
                }
                GetUnpackerType = assembly_0.GetTypes().First((Type type_0) => type_0.Name.Equals("Program"));
                GetUnpackerMethod = GetUnpackerType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic).First((MethodInfo methodInfo_0) => methodInfo_0.Name.Equals("Main"));
            }
            catch {}
            if (Directory.Exists(AssetBundlePath))
	        {
		        Directory.EnumerateDirectories(AssetBundlePath).ToList().ForEach(delegate(string string_0)
		        {
			        try
			        {
				        if (string_0.EndsWith("_dump"))
				        {
					        Directory.Delete(string_0, recursive: true);
				        }
			        }
			        catch { }
		        });
		        Directory.EnumerateFiles(AssetBundlePath).ToList().ForEach(delegate(string string_0)
		        {
			        try
			        {
				        File.Delete(string_0);
			        }
			        catch {}
		        });
		        Directory.EnumerateFiles(VrcaStorePath).ToList().ForEach(delegate(string string_0)
		        {
			        try
			        {
				        File.Delete(string_0);
			        }
			        catch {}
		        });
	        }
	        else
	        {
		        Directory.CreateDirectory(AssetBundlePath);
		        Directory.CreateDirectory(VrcaStorePath);
	        }
        }

        public void Update()
        {
            if (ActionsBool)
            {
                lock (Actions)
                {
                    System.Collections.Generic.List<Action> list = Actions2;
                    Actions2 = Actions;
                    Actions = list;
                    ActionsBool = false;
                }
                foreach (Action item in Actions2)
                {
                    item();
                    Actions.Remove(item);
                }
                Actions2.Clear();
            }
        }

        public static void RegisterAction(Action action_0)
        {
            lock (Actions)
            {
                Actions.Add(action_0);
                ActionsBool = true;
            }
        }

        public static void ReuploadWorldAction()
        {
            VRCUiManager vRCUiManager = VRCUiManager.prop_VRCUiManager_0;
            if (!vRCUiManager)
            {
                return;
            }
            GameObject gameObject = vRCUiManager.prop_VRCUiPopupManager_0.transform.Find("Screens/WorldInfo").gameObject;
            if (!gameObject)
            {
                return;
            }
            PageWorldInfo component = gameObject.GetComponent<PageWorldInfo>();
            if ((bool)component)
            {
                ApiWorld field_Private_ApiWorld_ = component.field_Private_ApiWorld_0;
                if (field_Private_ApiWorld_ != null)
                {
                    ReuploadWorld(field_Private_ApiWorld_.id);
                }
                else
                {
                    Extensions.Logger.WengaLogger("Failed to fetch APIWorld");
                }
            }
        }

        public static void ReuploadAvatar(string avatarID)
        {
            if (string.IsNullOrEmpty(avatarID))
            {
                Extensions.Logger.WengaLogger("No AvatarID found");
                return;
            }
            API.Fetch<ApiAvatar>(avatarID, (Action<ApiContainer>)delegate (ApiContainer apiContainer_0)
            {
                SelectedAvatar = apiContainer_0.Model.Cast<ApiAvatar>();
                if (SelectedAvatar == null)
                {
                    Extensions.Logger.WengaLogger("Failed to get Avatar");
                }
                else
                {
                    NewAvatarID = GenerateAvatarID();
                    API.Fetch<ApiAvatar>(NewAvatarID, (Action<ApiContainer>)delegate
                    {
                        Extensions.Logger.WengaLogger("AvatarId " + NewAvatarID + " already in use");
                        ReuploadAvatar(avatarID);
                    }, (Action<ApiContainer>)delegate
                    {
                        Task.Run(async delegate
                        {
                            Extensions.Logger.WengaLogger("AvatarId: " + SelectedAvatar.id + " | AssetUrl: " + SelectedAvatar.assetUrl + " | Author: " + SelectedAvatar.authorName);
                            try
                            {
                                string DownloadPath = await DownloadAvatar(SelectedAvatar);
                                if (!string.IsNullOrEmpty(DownloadPath))
                                {
                                    Extensions.Logger.WengaLogger("Avatar Downloaded");
                                    string UncompressedVRCA = await UncompressBundle(DownloadPath);
                                    Extensions.Logger.WengaLogger("AssetBundle created");
                                    if (!string.IsNullOrEmpty(UncompressedVRCA))
                                    {
                                        string unityVersion = SelectedAvatar.unityVersion.ToLower();
                                        string platform = SelectedAvatar.platform.ToLower();
                                        string ApiVersion = ApiWorld.VERSION.ApiVersion.ToString().ToLower();
                                        if (string.IsNullOrEmpty(ApiVersion))
                                        {
                                            ApiVersion = "4";
                                        }
                                       var avatarimage = "Avatar - " + SelectedAvatar.name + " - Image - " + unityVersion + "_" + ApiVersion + "_" + platform + "_Release";
                                        var AvatarAssetBundle = "Avatar - " + SelectedAvatar.name + " - Asset bundle - " + unityVersion + "_" + ApiVersion + "_" + platform + "_Release";
                                        if (!(await ReplaceID(UncompressedVRCA, NewAvatarID, SelectedAvatar.id)))
                                        {
                                            Extensions.Logger.WengaLogger("Failed to set AvatarID");
                                        }
                                        string PackedBundle = await CompressAssetBundle();
                                        if (!string.IsNullOrEmpty(PackedBundle))
                                        {
                                            RegisterAction(delegate
                                            {
                                                Extensions.Logger.WengaLogger("Uploading VRCA...");
                                                ApiFileHelper.upload(PackedBundle, null, AvatarAssetBundle, OnUploadVrcaAsyncSuccess, OnUploadVrcaAsyncFailure, delegate (ApiFile imageBundle,  string string_0, string string_1, float UploadingStatus)
                                                {
                                                    Extensions.Logger.WengaLogger($"VRCA Uploading Progress: {Math.Round(UploadingStatus * 100, 2, MidpointRounding.AwayFromZero)}%");
                                                }, (ApiFile File) => false);
                                            });
                                        }
                                        else
                                        {
                                            Extensions.Logger.WengaLogger("Failed to recompress AssetBundle");
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MelonLogger.Error(ex.ToString());
                            }
                        });
                    });
                }
            }, (Action<ApiContainer>)delegate
            {
                Extensions.Logger.WengaLogger("Couldn't fetch avatar (API)");
            });
        }

        private static void OnUploadVrcaAsyncSuccess(ApiFile avatar, string string_21)
        {
            Extensions.Logger.WengaLogger("VRCA Uploaded");
            AvatarAssetBundle = avatar;
            Extensions.Logger.WengaLogger("AvatarAssetBundle : " + avatar.GetFileURL());
            Task.Run(async delegate
            {
                var image = await DownloadImage(SelectedAvatar.imageUrl);
                if (!string.IsNullOrEmpty(image))
                {
                    Extensions.Logger.WengaLogger("Avatar Image downloaded");
                    RegisterAction(delegate
                    {
                        Extensions.Logger.WengaLogger("Uploading Image...");
                        ApiFileHelper.upload(image, null, avatar.GetFileURL(), OnUploadVrcaAsynSuccess, OnUploadImageAsyncFailure, delegate (ApiFile apiFile_0, string string_0, string string_1, float Progress)
                        {
                            Extensions.Logger.WengaLogger($"Avatar Image Uploading: {Math.Round(Progress * 100, 2, MidpointRounding.AwayFromZero)}%");
                        }, (ApiFile Assets) => false);
                    });
                }
                else
                {
                    Extensions.Logger.WengaLogger("Avatar Image missing - replacing with WengaPort Image");
                    image = await DownloadImage("https://cdn.discordapp.com/attachments/797668950883565638/801867151287910430/Komp_1_00501.png");
                    RegisterAction(delegate
                    {
                        Extensions.Logger.WengaLogger("Uploading Image...");
                        ApiFileHelper.upload(image, null, avatar.GetFileURL(), OnUploadVrcaAsynSuccess, OnUploadImageAsyncFailure, delegate (ApiFile apiFile_0, string string_0, string string_1, float Progress)
                        {
                            Extensions.Logger.WengaLogger($"Avatar Image Uploading: {Math.Round(Progress * 100, 2, MidpointRounding.AwayFromZero)}%");
                        }, (ApiFile Assets) => false);
                    });
                }
            });
        }

        private static void OnUploadVrcaAsyncFailure(ApiFile ImageUrl, string status)
        {
            Extensions.Logger.WengaLogger("VRCA Upload Failed");
        }

        private static void OnUploadVrcaAsynSuccess(ApiFile ImageUrl, string status)
        {

            Extensions.Logger.WengaLogger("VRCA Upload Success");
            RegisterAction(delegate
            {
                apiAvatar_1 = new ApiAvatar
                {
                    id = NewAvatarID,
                    authorName = APIUser.CurrentUser.username,
                    authorId = APIUser.CurrentUser.id,
                    name = SelectedAvatar.name,
                    imageUrl = ImageUrl.GetFileURL(),
                    assetUrl = AvatarAssetBundle.GetFileURL(),
                    description = PhotonModule.RandomNumberString(16),
                    releaseStatus = "private"
                };
                apiAvatar_1.Post((Action<ApiContainer>)OnApiAvatarPostSuccess, (Action<ApiContainer>)OnApiAvatarPostFailure);
            });
        }

        private static void OnUploadImageAsyncFailure(ApiFile apiFile_2, string string_21)
        {
            Extensions.Logger.WengaLogger("Avatar Image upload failed");
        }

        private static void OnApiAvatarPostSuccess(ApiContainer apiContainer_0)
        {
            Extensions.Logger.WengaLogger("Avatar Reuploaded");
            VRConsole.Log(VRConsole.LogsType.Avatar, $"Avatar Reuploaded");
            ClearOldSession();
        }

        private static void OnApiAvatarPostFailure(ApiContainer apiContainer_0)
        {
            Extensions.Logger.WengaLogger("Failed to Reupload Avatar");
            ClearOldSession();
        }

        public static void ReuploadWorld(string SelectedWorldID)
        {
            if (string.IsNullOrEmpty(SelectedWorldID))
            {
                Extensions.Logger.WengaLogger("WorldID not found");
                return;
            }
            API.Fetch<ApiWorld>(SelectedWorldID, (Action<ApiContainer>)delegate (ApiContainer apiContainer_0)
            {
                SelectedWorld = apiContainer_0.Model.Cast<ApiWorld>();
                if (SelectedWorld == null)
                {
                    Extensions.Logger.WengaLogger("Failed to fetch ApiWorld");
                }
                else
                {
                    NewWorldID = GenerateWorldID();
                    API.Fetch<ApiAvatar>(NewWorldID, (Action<ApiContainer>)delegate
                    {
                        Extensions.Logger.WengaLogger("WorldID " + NewWorldID + " already in use");
                        ReuploadWorld(SelectedWorldID);
                    }, (Action<ApiContainer>)delegate
                    {
                        Task.Run(async delegate
                        {
                            Extensions.Logger.WengaLogger("WorldId: " + SelectedWorld.id + " | AssetUrl: " + SelectedWorld.assetUrl + " | Author: " + SelectedWorld.authorName);
                            try
                            {
                                string DownloadPath = await DownloadWorld(SelectedWorld);
                                if (!string.IsNullOrEmpty(DownloadPath))
                                {
                                    Extensions.Logger.WengaLogger("World downloaded");
                                    string UncompressedVRCW = await UncompressBundle(DownloadPath);
                                    Extensions.Logger.WengaLogger("AssetBundle created");
                                    if (!string.IsNullOrEmpty(UncompressedVRCW))
                                    {
                                        string unityVersion = SelectedWorld.unityVersion.ToLower();
                                        string platform = SelectedWorld.platform.ToLower();
                                        string ApiVersion = SelectedWorld.apiVersion.ToString().ToLower();
                                        if (string.IsNullOrEmpty(ApiVersion))
                                        {
                                            ApiVersion = "4";
                                        }
                                        var WorldImage = "World - " + SelectedWorld.name + " - Image - " + unityVersion + "_" + ApiVersion + "_" + platform + "_Release";
                                        var WorldAsset = "World - " + SelectedWorld.name + " - Asset bundle - " + unityVersion + "_" + ApiVersion + "_" + platform + "_Release";
                                        if (!(await ReplaceID(UncompressedVRCW, NewWorldID, SelectedWorld.id)))
                                        {
                                            Extensions.Logger.WengaLogger("Failed to set WorldID");
                                        }
                                        string PackedBundle = await CompressAssetBundle();
                                        if (!string.IsNullOrEmpty(PackedBundle))
                                        {
                                            RegisterAction(delegate
                                            {
                                                Extensions.Logger.WengaLogger("Uploading VRCW...");
                                                ApiFileHelper.upload(PackedBundle, null, WorldAsset, OnUploadVrcwAsyncSuccess, OnUploadVrcwAsyncFailure, delegate (ApiFile imageBundle,  string string_0, string string_1, float UploadingStatus)
                                                {
                                                    Extensions.Logger.WengaLogger($"VRCW Uploading Progress: {Math.Round(UploadingStatus * 100, 2, MidpointRounding.AwayFromZero)}%");
                                                }, (ApiFile File) => false);
                                            });
                                        }
                                        else
                                        {
                                            Extensions.Logger.WengaLogger("Failed to recompress AssetBundle");
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MelonLogger.Error(ex.ToString());
                            }
                        });
                    });
                }
            }, (Action<ApiContainer>)delegate
            {
              Extensions.Logger.WengaLogger("Couldn't fetch World (API)");
            });
        }

        private static void OnUploadVrcwAsyncSuccess(ApiFile World, string AssetBundle)
        {
            Extensions.Logger.WengaLogger("VRCW Uploaded");
            WorldAssetBundle = World;
            Task.Run(async delegate
            {
                var Image = await DownloadImage(SelectedWorld.imageUrl);
                if (!string.IsNullOrEmpty(Image))
                {
                    Extensions.Logger.WengaLogger("World Image downloaded");
                    RegisterAction(delegate
                    {
                        Extensions.Logger.WengaLogger("Uploading Image...");
                        ApiFileHelper.upload(Image, null, AssetBundle, OnUploadVrcwAsynSuccess, OnUploadImageAsyncFailure, delegate (ApiFile world, string ImageUrl, string AssetUrl, float progress)
                        {
                            Extensions.Logger.WengaLogger($"World Image Uploading: {Math.Round(progress * 100, 2, MidpointRounding.AwayFromZero)}%");
                        }, (ApiFile worlddone) => false);
                    });
                }
                else
                {
                    Extensions.Logger.WengaLogger("World Image missing - replacing with WengaPort Image");
                    Image = await DownloadImage("https://cdn.discordapp.com/attachments/797668950883565638/801867151287910430/Komp_1_00501.png");
                    RegisterAction(delegate
                    {
                        Extensions.Logger.WengaLogger("Uploading Image...");
                        ApiFileHelper.upload(Image, null, AssetBundle, OnUploadVrcwAsynSuccess, OnUploadImageAsyncFailure, delegate (ApiFile world, string ImageUrl, string AssetUrl, float progress)
                        {
                            Extensions.Logger.WengaLogger($"World Image Uploading: {Math.Round(progress * 100, 2, MidpointRounding.AwayFromZero)}%");
                        }, (ApiFile worlddone) => false);
                    });
                }
            });
        }

        private static void OnUploadVrcwAsyncFailure(ApiFile apiFile_2, string string_21)
        {
            Extensions.Logger.WengaLogger("Failed to upload VRCW");
        }

        private static void OnUploadVrcwAsynSuccess(ApiFile WorldImage, string notused)
        {
            Extensions.Logger.WengaLogger("VRCW Upload Success");
            RegisterAction(delegate
            {
                ReuploadedWorld = new ApiWorld
                {
                    id = NewWorldID,
                    authorName = APIUser.CurrentUser.username,
                    authorId = APIUser.CurrentUser.id,
                    name = SelectedWorld.name,
                    imageUrl = WorldImage.GetFileURL(),
                    assetUrl = WorldAssetBundle.GetFileURL(),
                    description = PhotonModule.RandomNumberString(16),
                    releaseStatus = "private"
                };
                ReuploadedWorld.Post((Action<ApiContainer>)OnApiWorldPostSuccess, (Action<ApiContainer>)OnApiWorldPostFailure);
            });
        }



        private static void OnApiWorldPostSuccess(ApiContainer apiContainer_0)
        {
            Extensions.Logger.WengaLogger("World Reuploaded");
            VRConsole.Log(VRConsole.LogsType.Avatar, $"World Reuploaded");
            ClearOldSession();
        }

        private static void OnApiWorldPostFailure(ApiContainer apiContainer_0)
        {
            Extensions.Logger.WengaLogger("Failed to Reupload World");
            ClearOldSession();
        }

        private static void ClearOldSession()
        {
            try
            {
                DirectoryInfo VrcaStore = new DirectoryInfo(VrcaStorePath);
                DirectoryInfo AssetBundles = new DirectoryInfo(VrcaStorePath);
                foreach (DirectoryInfo dir in AssetBundles.GetDirectories())
                {
                    if (!dir.Name.Contains("VrcaStore"))
                    {
                        dir.Delete(true);
                    }
                }
                foreach (FileInfo file in AssetBundles.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in VrcaStore.GetDirectories())
                {
                    dir.Delete(true);
                }
                foreach (FileInfo file in VrcaStore.GetFiles())
                {
                    file.Delete();
                }
            }
            catch { }
        }

        private static string GenerateAvatarID()
        {
            return "avtr_" + Guid.NewGuid().ToString();
        }

        private static string GenerateWorldID()
        {
            return "wrld_" + Guid.NewGuid().ToString();
        }

        private static int Dunnowtfisdis(byte[] byte_0, byte[] byte_1)
        {
            int num = 0;
            while (true)
            {
                if (num < byte_0.Length - byte_1.Length)
                {
                    bool flag = true;
                    for (int i = 0; i < byte_1.Length; i++)
                    {
                        if (byte_0[num + i] != byte_1[i])
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        break;
                    }
                    num++;
                    continue;
                }
                return -1;
            }
            return num;
        }

        private static async Task<bool> ReplaceID(string filepath, string NewID, string OldID)
        {
            await Task.Delay(100);
            try
            {
                byte[] array = File.ReadAllBytes(filepath);
                byte[] bytes = Encoding.ASCII.GetBytes(OldID);
                Encoding.ASCII.GetBytes(OldID.ToLower());
                byte[] bytes2 = Encoding.ASCII.GetBytes(NewID);
                if (!OldID.Contains("avtr_") && !OldID.Contains("wrld_"))
                {
                    Extensions.Logger.WengaLogger("Custom ID found");
                    return false;
                }
                byte[] array2 = new byte[array.Length + bytes2.Length - bytes.Length];
                byte[] array3 = array;
                int num;
                while ((num = Dunnowtfisdis(array3, bytes)) >= 0)
                {
                    Buffer.BlockCopy(array3, 0, array2, 0, num);
                    Buffer.BlockCopy(bytes2, 0, array2, num, bytes2.Length);
                    Buffer.BlockCopy(array3, num + bytes.Length, array2, num + bytes2.Length, array3.Length - num - bytes.Length);
                    array3 = array2;
                }
                File.WriteAllBytes(filepath, array2);
                Extensions.Logger.WengaLogger("AssetBundle overwritten");
                return true;
            }
            catch (Exception ex)
            {
                MelonLogger.Error(ex.ToString());
                return false;
            }
        }

        private static async Task<string> DownloadAvatar(ApiAvatar apiAvatar_2)
        {
            byte[] bytes = await new HttpClient().GetByteArrayAsync(apiAvatar_2.assetUrl);
            string text = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".vrca");
            File.WriteAllBytes(text, bytes);
            Extensions.Logger.WengaLogger("Downloading Avatar");
            return text;
        }

        private static async Task<string> DownloadWorld(ApiWorld apiWorld_2)
        {
            byte[] bytes = await new HttpClient().GetByteArrayAsync(apiWorld_2.assetUrl);
            string text = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".vrcw");
            File.WriteAllBytes(text, bytes);
            Extensions.Logger.WengaLogger("Downloading World");
            return text;
        }

        private static async Task<string> DownloadImage(string string_21)
        {
            HttpResponseMessage httpResponseMessage = await new HttpClient().GetAsync(string_21);
            byte[] array = await httpResponseMessage.Content.ReadAsByteArrayAsync();
            if (array == null || array.Length == 0)
            {
                Extensions.Logger.WengaLogger("image was null or 0");
            }
            string text = httpResponseMessage.Content.Headers.GetValues("Content-Type").First().Split('/')[1];
            string text2 = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + "." + text);
            File.WriteAllBytes(text2, array);
            Extensions.Logger.WengaLogger("DownloadImage");
            return text2;
        }

        private static string GetDumpedVRCA(string path)
        {
            string result = string.Empty;
            System.Collections.Generic.IEnumerable<string> enumerable = from string_0 in Directory.EnumerateFiles(path + "_dump")
                                                                        select new FileInfo(string_0) into fileInfo_0
                                                                        orderby fileInfo_0.CreationTime
                                                                        select fileInfo_0.FullName;
            if (enumerable.Count() != 1)
            {
                foreach (string item in enumerable)
                {
                    if (string.IsNullOrEmpty(Path.GetExtension(item)))
                    {
                        result = item;
                    }
                }
                return result;
            }
            return enumerable.ElementAt(0);
        }

        public static void RunUBPU(string[] string_21)
        {
            try
            {
                GetUnpackerMethod?.Invoke(null, new object[1]
                {
                    string_21
                });
            }
            catch (Exception ex)
            {
                MelonLogger.Error(ex.ToString());
            }
        }

        private static async Task<string> UncompressBundle(string downloadedpath)
        {

            string AssetBundlePath = string.Empty;

            if (File.Exists(downloadedpath))
            {
                string path = Path.Combine(ReuploaderButtons.AssetBundlePath, Path.GetFileName(downloadedpath));
                File.Move(downloadedpath, path);
                if (File.Exists(path))
                {
                    File.Delete(downloadedpath);
                    RunUBPU(new string[1]
                    {
                    path
                    });
                    Extensions.Logger.WengaLogger("Decompressing assetbundle..");
                    
                    await Task.Delay(1000);
                    Extensions.Logger.WengaLogger("Finished Decompressing");
                    AssetBundlePath = GetDumpedVRCA(path);
                    File.Delete(path);
                    return AssetBundlePath;
                }
                else
                {
                    return AssetBundlePath;
                }
            }
            else
            {
                return AssetBundlePath;
            }
        }


        private static async Task<string> CompressAssetBundle()
        {
            try
            {
                string text = GetXMLFile();
                if (string.IsNullOrEmpty(text))
                {
                    Extensions.Logger.WengaLogger("XML File Empty!");
                    return string.Empty;
                }
                Extensions.Logger.WengaLogger("Compressing Assetbundle..");
                string directoryName = Path.GetDirectoryName(Application.dataPath);
                Directory.SetCurrentDirectory(AssetBundlePath);
                RunUBPU(new string[2]
                {
                    text,
                    "lz4hc"
                });
                await Task.Delay(1000);
                Directory.SetCurrentDirectory(directoryName);
                Extensions.Logger.WengaLogger("Finished Compressing");
                var Compressed = GetLZ4HCFile();
                if (!string.IsNullOrEmpty(Compressed))
                {
                    int startIndex = Compressed.IndexOf(".LZ4HC");
                    string fileName = Path.GetFileName(Compressed.Remove(startIndex, 6));
                    string destFileName = Path.Combine(VrcaStorePath, fileName);
                    File.Move(Compressed, destFileName);
                    return destFileName;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                MelonLogger.Error(ex.Message);
                return string.Empty;
            }
        }

        private static string GetXMLFile()
        {
            string result = string.Empty;
            System.Collections.Generic.IEnumerable<string> enumerable = from Path in Directory.EnumerateFiles(AssetBundlePath)
                                                                        select new FileInfo(Path) into File
                                                                        orderby File.CreationTime
                                                                        select File.FullName;
            if (enumerable.Any())
            {
                foreach (string item in enumerable)
                {
                    if (item.EndsWith(".xml"))
                    {
                        result = item;
                    }
                }
                return result;
            }
            return result;
        }

        private static string GetLZ4HCFile()
        {
            string result = string.Empty;
            System.Collections.Generic.IEnumerable<string> enumerable = from Path in Directory.EnumerateFiles(AssetBundlePath)
                                                                        select new FileInfo(Path) into File
                                                                        orderby File.CreationTime
                                                                        select File.FullName;
            if (enumerable.Any())
            {
                foreach (string item in enumerable)
                {
                    if (item.EndsWith(".LZ4HC"))
                    {
                        result = item;
                    }
                }
                return result;
            }
            return result;
        }
    }
}