using DotZLib;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Tar;
using librsync.net;
using MelonLoader;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using VRC;
using VRC.Core;

namespace WengaPort.Modules.Reupload
{
    public class ApiFileHelper : MonoBehaviour
    {
        public delegate void GDelegate0(ApiFile apiFile, string message);

        public delegate void GDelegate1(ApiFile apiFile, string error);

        public delegate void GDelegate2(ApiFile apiFile, string status, string subStatus, float pct);

        public delegate bool GDelegate3(ApiFile apiFile);

        public enum GEnum0
        {
            Success,
            Unchanged
        }

        public System.Delegate ReferencedDelegate;

        public System.IntPtr MethodInfo;

        public Il2CppSystem.Collections.Generic.List<MonoBehaviour> AntiGcList;

        private readonly int int_0 = 10485760;

        private readonly int int_1 = 52428800;

        private readonly float float_0 = 120f;

        private readonly float float_1 = 600f;

        private readonly float float_2 = 2f;

        private readonly float float_3 = 10f;

        private static bool bool_0;

        private readonly Regex[] regex_0 = new Regex[4]
        {
            new Regex("/LightingData\\.asset$"),
            new Regex("/Lightmap-.*(\\.png|\\.exr)$"),
            new Regex("/ReflectionProbe-.*(\\.exr|\\.png)$"),
            new Regex("/Editor/Data/UnityExtensions/")
        };

        private static ApiFileHelper helper;
        public static RemoteConfig remoteConfig_0;

        public static ApiFileHelper apifilehelper
        {
            get
            {
                smethod_9();
                return helper;
            }
        }

        public ApiFileHelper(System.IntPtr intptr_1) : base(intptr_1)
        {
            AntiGcList = new Il2CppSystem.Collections.Generic.List<MonoBehaviour>(1);
            AntiGcList.Add(this);
        }

        public ApiFileHelper(System.Delegate delegate_1, System.IntPtr intptr_1) : base(ClassInjector.DerivedConstructorPointer<ApiFileHelper>())
        {
            ClassInjector.DerivedConstructorBody(this);
            ReferencedDelegate = delegate_1;
            MethodInfo = intptr_1;
        }

        ~ApiFileHelper()
        {
            Marshal.FreeHGlobal(MethodInfo);
            MethodInfo = Il2CppSystem.IntPtr.Zero;
            ReferencedDelegate = null;
            AntiGcList.Remove(this);
            AntiGcList = null;
        }

        public static void upload(string FilePath, string record, string AssetBundle, GDelegate0 success, GDelegate1 failure, GDelegate2 filecheck, GDelegate3 cancelled)
        {
            try
            {
                string extension = Path.GetExtension(FilePath);
                Extensions.Logger.WengaLogger("Extension: " + extension);
            }
            catch
            { }
            MelonCoroutines.Start(apifilehelper.Upload(FilePath, record, AssetBundle, success, failure, filecheck, cancelled));
        }

        public static string smethod_1(string string_0)
        {
            switch (string_0)
            {
                case ".vrcw":
                    return "application/x-world";

                case ".dll":
                    return "application/x-msdownload";

                case ".unitypackage":
                    return "application/gzip";

                case ".jpg":
                    return "image/jpg";

                default:
                    return "application/octet-stream";

                case ".delta":
                    return "application/x-rsync-delta";

                case ".sig":
                    return "application/x-rsync-signature";

                case ".png":
                    return "image/png";

                case ".gz":
                    return "application/gzip";

                case ".vrca":
                    return "application/x-avatar";
            }
        }

        public static bool smethod_2(string string_0)
        {
            return smethod_1(Path.GetExtension(string_0)) == "application/gzip";
        }

        public IEnumerator Upload(string Path, string record, string assetbundle, GDelegate0 success, GDelegate1 Failure, GDelegate2 filecheck, GDelegate3 Cancelled)
        {
            VRC.Core.Logger.Log("UploadFile: filename: " + Path + ", file id: " + ((!string.IsNullOrEmpty(record)) ? record : "<new>") + ", name: " + assetbundle, DebugLevel.All);
            if (!remoteConfig_0.IsInitialized())
            {
                bool bool_ = false;
                remoteConfig_0.Init((System.Action)delegate
                {
                    bool_ = true;
                }, (System.Action)delegate
                {
                    bool_ = true;
                });
                while (!bool_)
                {
                    yield return null;
                }
                if (!remoteConfig_0.IsInitialized())
                {
                    smethod_5(Failure, null, "Failed to fetch configuration.");
                    yield break;
                }
            }
            bool_0 = remoteConfig_0.GetBool("sdkEnableDeltaCompression");
            CheckFile(filecheck, null, "Checking file...");
            string whyNot;
            if (string.IsNullOrEmpty(Path))
            {
                smethod_5(Failure, null, "Upload filename is empty!");
            }
            else if (!System.IO.Path.HasExtension(Path))
            {
                smethod_5(Failure, null, "Upload filename must have an extension: " + Path);
            }
            else if (Tools.FileCanRead(Path, out whyNot))
            {
                CheckFile(filecheck, null, string.IsNullOrEmpty(record) ? "Creating file record..." : "Getting file record...");
                bool bool_0 = true;
                bool bool_3 = false;
                bool bool_2 = false;
                string string_5 = "";
                if (string.IsNullOrEmpty(assetbundle))
                {
                    assetbundle = Path;
                }
                string extension = System.IO.Path.GetExtension(Path);
                string mimeType = smethod_1(extension);
                ApiFile apiFile_2 = null;
                Action<ApiContainer> action = delegate (ApiContainer apiContainer_0)
                {
                    apiFile_2 = apiContainer_0.Model.Cast<ApiFile>();
                    bool_0 = false;
                };
                Action<ApiContainer> action2 = delegate (ApiContainer apiContainer_0)
                {
                    string_5 = apiContainer_0.Error;
                    bool_0 = false;
                    if (apiContainer_0.Code == 400)
                    {
                        bool_2 = true;
                    }
                };
                while (true)
                {
                    apiFile_2 = null;
                    bool_0 = true;
                    bool_2 = false;
                    string_5 = "";
                    if (string.IsNullOrEmpty(record))
                    {
                        ApiFile.Create(assetbundle, mimeType, extension, action, action2);
                    }
                    else
                    {
                        API.Fetch<ApiFile>(record, action, action2);
                    }
                    while (bool_0)
                    {
                        if (apiFile_2 != null && ApiFileHelper.Cancelled(Cancelled, Failure, apiFile_2))
                        {
                            yield break;
                        }
                        yield return null;
                    }
                    if (!string.IsNullOrEmpty(string_5))
                    {
                        if (string_5.Contains("File not found"))
                        {
                            Extensions.Logger.WengaLogger("Couldn't find file record: " + record + ", creating new file record");
                            record = "";
                            continue;
                        }
                        string string_8 = (string.IsNullOrEmpty(record) ? "Failed to create file record." : "Failed to get file record.");
                        smethod_5(Failure, null, string_8, string_5);
                        if (!bool_2)
                        {
                            yield break;
                        }
                    }
                    if (bool_2)
                    {
                        yield return new WaitForSecondsRealtime(0.75f);
                        continue;
                    }
                    if (apiFile_2 == null)
                    {
                        yield break;
                    }
                    smethod_3(apiFile_2, bool_1: false, bool_2: true);
                    break;
                }
                string string_6;
                string string_7;
                while (true)
                {
                    if (apiFile_2.HasQueuedOperation(ApiFileHelper.bool_0))
                    {
                        bool_0 = true;
                        apiFile_2.DeleteLatestVersion((Action<ApiContainer>)delegate
                        {
                            bool_0 = false;
                        }, (Action<ApiContainer>)delegate
                        {
                            bool_0 = false;
                        });
                        while (bool_0)
                        {
                            if (apiFile_2 != null && ApiFileHelper.Cancelled(Cancelled, Failure, apiFile_2))
                            {
                                yield break;
                            }
                            yield return null;
                        }
                        continue;
                    }
                    yield return new WaitForSecondsRealtime(0.75f);
                    smethod_3(apiFile_2, bool_1: false);
                    if (apiFile_2.IsInErrorState())
                    {
                        Extensions.Logger.WengaLogger("ApiFile: " + apiFile_2.id + ": server failed to process last uploaded, deleting failed version");
                        while (true)
                        {
                            CheckFile(filecheck, apiFile_2, "Preparing file for upload...", "Cleaning up previous version");
                            bool_0 = true;
                            string_5 = "";
                            bool_2 = false;
                            apiFile_2.DeleteLatestVersion(action, action2);
                            while (bool_0)
                            {
                                if (!ApiFileHelper.Cancelled(Cancelled, Failure, null))
                                {
                                    yield return null;
                                    continue;
                                }
                                yield break;
                            }
                            if (!string.IsNullOrEmpty(string_5))
                            {
                                smethod_5(Failure, apiFile_2, "Failed to delete previous failed version!", string_5);
                                if (!bool_2)
                                {
                                    smethod_8(apiFile_2.id);
                                    yield break;
                                }
                            }
                            if (!bool_2)
                            {
                                break;
                            }
                            yield return new WaitForSecondsRealtime(0.75f);
                        }
                    }
                    yield return new WaitForSecondsRealtime(0.75f);
                    smethod_3(apiFile_2, bool_1: false);
                    if (!apiFile_2.HasQueuedOperation(ApiFileHelper.bool_0))
                    {
                        CheckFile(filecheck, apiFile_2, "Preparing file for upload...", "Optimizing file");
                        string_6 = Tools.GetTempFileName(System.IO.Path.GetExtension(Path), out string_5, apiFile_2.id);
                        if (!string.IsNullOrEmpty(string_6))
                        {
                            bool_3 = false;
                            yield return MelonCoroutines.Start(method_1(Path, string_6, delegate (GEnum0 genum0_0)
                            {
                                if (genum0_0 == GEnum0.Unchanged)
                                {
                                    string_6 = Path;
                                }
                            }, delegate (string string_4)
                            {
                                smethod_5(Failure, apiFile_2, "Failed to optimize file for upload.", string_4);
                                smethod_8(apiFile_2.id);
                                bool_3 = true;
                            }));
                            if (bool_3)
                            {
                                break;
                            }
                            smethod_3(apiFile_2, bool_1: false);
                            CheckFile(filecheck, apiFile_2, "Preparing file for upload...", "Generating file hash");
                            bool_0 = true;
                            string_5 = "";
                            string text = Convert.ToBase64String(MD5.Create().ComputeHash(File.ReadAllBytes(string_6)));
                            bool_0 = false;
                            while (bool_0)
                            {
                                if (!ApiFileHelper.Cancelled(Cancelled, Failure, apiFile_2))
                                {
                                    yield return null;
                                    continue;
                                }
                                smethod_8(apiFile_2.id);
                                yield break;
                            }
                            if (!string.IsNullOrEmpty(string_5))
                            {
                                smethod_5(Failure, apiFile_2, "Failed to generate MD5 hash for upload file.", string_5);
                                smethod_8(apiFile_2.id);
                                break;
                            }
                            smethod_3(apiFile_2, bool_1: false);
                            CheckFile(filecheck, apiFile_2, "Preparing file for upload...", "Checking for changes");
                            bool flag = false;
                            if (apiFile_2.HasExistingOrPendingVersion())
                            {
                                if (string.Compare(text, apiFile_2.GetFileMD5(apiFile_2.GetLatestVersionNumber())) == 0)
                                {
                                    if (!apiFile_2.IsWaitingForUpload())
                                    {
                                        smethod_4(success, apiFile_2, "The file to upload is unchanged.");
                                        smethod_8(apiFile_2.id);
                                        break;
                                    }
                                    flag = true;
                                    Extensions.Logger.WengaLogger("Retrying previous upload");
                                }
                                else if (apiFile_2.IsWaitingForUpload())
                                {
                                    do
                                    {
                                        CheckFile(filecheck, apiFile_2, "Preparing file for upload...", "Cleaning up previous version");
                                        bool_0 = true;
                                        bool_2 = false;
                                        string_5 = "";
                                        apiFile_2.DeleteLatestVersion(action, action2);
                                        while (bool_0)
                                        {
                                            if (!ApiFileHelper.Cancelled(Cancelled, Failure, apiFile_2))
                                            {
                                                yield return null;
                                                continue;
                                            }
                                            yield break;
                                        }
                                        if (!string.IsNullOrEmpty(string_5))
                                        {
                                            smethod_5(Failure, apiFile_2, "Failed to delete previous incomplete version!", string_5);
                                            if (!bool_2)
                                            {
                                                smethod_8(apiFile_2.id);
                                                yield break;
                                            }
                                        }
                                        yield return new WaitForSecondsRealtime(0.75f);
                                    }
                                    while (bool_2);
                                }
                            }
                            smethod_3(apiFile_2, bool_1: false);
                            CheckFile(filecheck, apiFile_2, "Preparing file for upload...", "Generating signature");
                            string tempFileName = Tools.GetTempFileName(".sig", out string_5, apiFile_2.id);
                            if (!string.IsNullOrEmpty(tempFileName))
                            {
                                bool_3 = false;
                                yield return MelonCoroutines.Start(method_2(string_6, tempFileName, delegate
                                {
                                }, delegate (string string_4)
                                {
                                    smethod_5(Failure, apiFile_2, "Failed to generate file signature!", string_4);
                                    smethod_8(apiFile_2.id);
                                    bool_3 = true;
                                }));
                                if (bool_3)
                                {
                                    break;
                                }
                                smethod_3(apiFile_2, bool_1: false);
                                CheckFile(filecheck, apiFile_2, "Preparing file for upload...", "Generating signature hash");
                                bool_0 = true;
                                string_5 = "";
                                string text2 = Convert.ToBase64String(MD5.Create().ComputeHash(File.ReadAllBytes(tempFileName)));
                                bool_0 = false;
                                while (bool_0)
                                {
                                    if (ApiFileHelper.Cancelled(Cancelled, Failure, apiFile_2))
                                    {
                                        smethod_8(apiFile_2.id);
                                        yield break;
                                    }
                                    yield return null;
                                }
                                if (string.IsNullOrEmpty(string_5))
                                {
                                    long size = 0L;
                                    if (Tools.GetFileSize(tempFileName, out size, out string_5))
                                    {
                                        smethod_3(apiFile_2, bool_1: false);
                                        string_7 = null;
                                        if (ApiFileHelper.bool_0 && apiFile_2.HasExistingVersion())
                                        {
                                            CheckFile(filecheck, apiFile_2, "Preparing file for upload...", "Downloading previous version signature");
                                            bool_0 = true;
                                            string_5 = "";
                                            apiFile_2.DownloadSignature((Action<Il2CppStructArray<byte>>)delegate (Il2CppStructArray<byte> il2CppStructArray_0)
                                            {
                                                string_7 = Tools.GetTempFileName(".sig", out string_5, apiFile_2.id);
                                                if (!string.IsNullOrEmpty(string_7))
                                                {
                                                    try
                                                    {
                                                        File.WriteAllBytes(string_7, il2CppStructArray_0);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        string_7 = null;
                                                        string_5 = "Failed to write signature temp file:\n" + ex.Message;
                                                    }
                                                    bool_0 = false;
                                                }
                                                else
                                                {
                                                    string_5 = "Failed to create temp file: \n" + string_5;
                                                    bool_0 = false;
                                                }
                                            }, (Action<string>)delegate (string string_4)
                                            {
                                                string_5 = string_4;
                                                bool_0 = false;
                                            }, (Action<long, long>)delegate (long long_0, long long_1)
                                            {
                                                CheckFile(filecheck, apiFile_2, "Preparing file for upload...", "Downloading previous version signature", Tools.DivideSafe(long_0, long_1));
                                            });
                                            while (bool_0)
                                            {
                                                if (!ApiFileHelper.Cancelled(Cancelled, Failure, apiFile_2))
                                                {
                                                    yield return null;
                                                    continue;
                                                }
                                                smethod_8(apiFile_2.id);
                                                yield break;
                                            }
                                            if (!string.IsNullOrEmpty(string_5))
                                            {
                                                smethod_5(Failure, apiFile_2, "Failed to download previous file version signature.", string_5);
                                                smethod_8(apiFile_2.id);
                                                break;
                                            }
                                        }
                                        smethod_3(apiFile_2, bool_1: false);
                                        string text3 = null;
                                        if (ApiFileHelper.bool_0 && !string.IsNullOrEmpty(string_7))
                                        {
                                            CheckFile(filecheck, apiFile_2, "Preparing file for upload...", "Creating file delta");
                                            text3 = Tools.GetTempFileName(".delta", out string_5, apiFile_2.id);
                                            if (string.IsNullOrEmpty(text3))
                                            {
                                                smethod_5(Failure, apiFile_2, "Failed to create file delta for upload.", "Failed to create temp file: \n" + string_5);
                                                smethod_8(apiFile_2.id);
                                                break;
                                            }
                                            bool_3 = false;
                                            yield return MelonCoroutines.Start(method_3(string_6, string_7, text3, delegate
                                            {
                                            }, delegate (string string_4)
                                            {
                                                smethod_5(Failure, apiFile_2, "Failed to create file delta for upload.", string_4);
                                                smethod_8(apiFile_2.id);
                                                bool_3 = true;
                                            }));
                                            if (bool_3)
                                            {
                                                break;
                                            }
                                        }
                                        long size2 = 0L;
                                        long size3 = 0L;
                                        if (Tools.GetFileSize(string_6, out size2, out string_5) && (string.IsNullOrEmpty(text3) || Tools.GetFileSize(text3, out size3, out string_5)))
                                        {
                                            bool flag2 = ApiFileHelper.bool_0 && size3 > 0L && size3 < size2;
                                            if (ApiFileHelper.bool_0)
                                            {
                                                VRC.Core.Logger.Log("Delta size " + size3 + " (" + (float)size3 / (float)size2 + " %), full file size " + size2 + ", uploading " + (flag2 ? " DELTA" : " FULL FILE"), DebugLevel.All);
                                            }
                                            else
                                            {
                                                VRC.Core.Logger.Log("Delta compression disabled, uploading FULL FILE, size " + size2, DebugLevel.All);
                                            }
                                            smethod_3(apiFile_2, flag2);
                                            string text4 = "";
                                            if (flag2)
                                            {
                                                CheckFile(filecheck, apiFile_2, "Preparing file for upload...", "Generating file delta hash");
                                                bool_0 = true;
                                                string_5 = "";
                                                text4 = Convert.ToBase64String(MD5.Create().ComputeHash(File.ReadAllBytes(text3)));
                                                bool_0 = false;
                                                while (bool_0)
                                                {
                                                    if (!ApiFileHelper.Cancelled(Cancelled, Failure, apiFile_2))
                                                    {
                                                        yield return null;
                                                        continue;
                                                    }
                                                    smethod_8(apiFile_2.id);
                                                    yield break;
                                                }
                                                if (!string.IsNullOrEmpty(string_5))
                                                {
                                                    smethod_5(Failure, apiFile_2, "Failed to generate file delta hash.", string_5);
                                                    smethod_8(apiFile_2.id);
                                                    break;
                                                }
                                            }
                                            bool flag3 = false;
                                            smethod_3(apiFile_2, flag2);
                                            if (flag)
                                            {
                                                ApiFile.Version version = apiFile_2.GetVersion(apiFile_2.GetLatestVersionNumber());
                                                if (version == null || !(flag2 ? (size3 == version.delta.sizeInBytes && text4.CompareTo(version.delta.md5) == 0 && size == version.signature.sizeInBytes && text2.CompareTo(version.signature.md5) == 0) : (size2 == version.file.sizeInBytes && text.CompareTo(version.file.md5) == 0 && size == version.signature.sizeInBytes && text2.CompareTo(version.signature.md5) == 0)))
                                                {
                                                    CheckFile(filecheck, apiFile_2, "Preparing file for upload...", "Cleaning up previous version");
                                                    do
                                                    {
                                                        bool_0 = true;
                                                        string_5 = "";
                                                        bool_2 = false;
                                                        apiFile_2.DeleteLatestVersion(action, action2);
                                                        while (bool_0)
                                                        {
                                                            if (!ApiFileHelper.Cancelled(Cancelled, Failure, null))
                                                            {
                                                                yield return null;
                                                                continue;
                                                            }
                                                            yield break;
                                                        }
                                                        if (!string.IsNullOrEmpty(string_5))
                                                        {
                                                            smethod_5(Failure, apiFile_2, "Failed to delete previous incomplete version!", string_5);
                                                            if (!bool_2)
                                                            {
                                                                smethod_8(apiFile_2.id);
                                                                yield break;
                                                            }
                                                        }
                                                        yield return new WaitForSecondsRealtime(0.75f);
                                                    }
                                                    while (bool_2);
                                                }
                                                else
                                                {
                                                    flag3 = true;
                                                    Extensions.Logger.WengaLogger("Using existing version record");
                                                }
                                            }
                                            smethod_3(apiFile_2, flag2);
                                            if (!flag3)
                                            {
                                                do
                                                {
                                                    CheckFile(filecheck, apiFile_2, "Creating file version record...");
                                                    bool_0 = true;
                                                    string_5 = "";
                                                    bool_2 = false;
                                                    if (!flag2)
                                                    {
                                                        apiFile_2.CreateNewVersion(ApiFile.Version.FileType.Full, text, size2, text2, size, action, action2);
                                                    }
                                                    else
                                                    {
                                                        apiFile_2.CreateNewVersion(ApiFile.Version.FileType.Delta, text4, size3, text2, size, action, action2);
                                                    }
                                                    while (bool_0)
                                                    {
                                                        if (ApiFileHelper.Cancelled(Cancelled, Failure, apiFile_2))
                                                        {
                                                            smethod_8(apiFile_2.id);
                                                            yield break;
                                                        }
                                                        yield return null;
                                                    }
                                                    if (!string.IsNullOrEmpty(string_5))
                                                    {
                                                        smethod_5(Failure, apiFile_2, "Failed to create file version record.", string_5);
                                                        if (!bool_2)
                                                        {
                                                            smethod_8(apiFile_2.id);
                                                            yield break;
                                                        }
                                                    }
                                                    yield return new WaitForSecondsRealtime(0.75f);
                                                }
                                                while (bool_2);
                                            }
                                            smethod_3(apiFile_2, flag2);
                                            if (!flag2)
                                            {
                                                if (apiFile_2.GetLatestVersion().file.status == ApiFile.Status.Waiting)
                                                {
                                                    CheckFile(filecheck, apiFile_2, "Uploading file...");
                                                    bool_3 = false;
                                                    yield return MelonCoroutines.Start(method_10(apiFile_2, ApiFile.Version.FileDescriptor.Type.file, string_6, text, size2, delegate (ApiFile apiFile_1)
                                                    {
                                                        VRC.Core.Logger.Log("Successfully uploaded file.", DebugLevel.All);
                                                        apiFile_2 = apiFile_1;
                                                    }, delegate (string string_4)
                                                    {
                                                        smethod_5(Failure, apiFile_2, "Failed to upload file.", string_4);
                                                        smethod_8(apiFile_2.id);
                                                        bool_3 = true;
                                                    }, delegate (long long_0, long long_1)
                                                    {
                                                        CheckFile(filecheck, apiFile_2, "Uploading file...", "", Tools.DivideSafe(long_0, long_1));
                                                    }, Cancelled));
                                                    if (bool_3)
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                            else if (apiFile_2.GetLatestVersion().delta.status == ApiFile.Status.Waiting)
                                            {
                                                CheckFile(filecheck, apiFile_2, "Uploading file delta...");
                                                bool_3 = false;
                                                yield return MelonCoroutines.Start(method_10(apiFile_2, ApiFile.Version.FileDescriptor.Type.delta, text3, text4, size3, delegate (ApiFile apiFile_1)
                                                {
                                                    Extensions.Logger.WengaLogger("Successfully uploaded file delta.");
                                                    apiFile_2 = apiFile_1;
                                                }, delegate (string string_4)
                                                {
                                                    smethod_5(Failure, apiFile_2, "Failed to upload file delta.", string_4);
                                                    smethod_8(apiFile_2.id);
                                                    bool_3 = true;
                                                }, delegate (long long_0, long long_1)
                                                {
                                                    CheckFile(filecheck, apiFile_2, "Uploading file delta...", "", Tools.DivideSafe(long_0, long_1));
                                                }, Cancelled));
                                                if (bool_3)
                                                {
                                                    break;
                                                }
                                            }
                                            smethod_3(apiFile_2, flag2);
                                            if (apiFile_2.GetLatestVersion().signature.status == ApiFile.Status.Waiting)
                                            {
                                                CheckFile(filecheck, apiFile_2, "Uploading file signature...");
                                                bool_3 = false;
                                                yield return MelonCoroutines.Start(method_10(apiFile_2, ApiFile.Version.FileDescriptor.Type.signature, tempFileName, text2, size, delegate (ApiFile apiFile_1)
                                                {
                                                    VRC.Core.Logger.Log("Successfully uploaded file signature.", DebugLevel.All);
                                                    apiFile_2 = apiFile_1;
                                                }, delegate (string string_4)
                                                {
                                                    smethod_5(Failure, apiFile_2, "Failed to upload file signature.", string_4);
                                                    smethod_8(apiFile_2.id);
                                                    bool_3 = true;
                                                }, delegate (long long_0, long long_1)
                                                {
                                                    CheckFile(filecheck, apiFile_2, "Uploading file signature...", "", Tools.DivideSafe(long_0, long_1));
                                                }, Cancelled));
                                                if (bool_3)
                                                {
                                                    break;
                                                }
                                            }
                                            smethod_3(apiFile_2, flag2);
                                            CheckFile(filecheck, apiFile_2, "Validating upload...");
                                            if (!(flag2 ? (apiFile_2.GetFileDescriptor(apiFile_2.GetLatestVersionNumber(), ApiFile.Version.FileDescriptor.Type.delta).status == ApiFile.Status.Complete) : (apiFile_2.GetFileDescriptor(apiFile_2.GetLatestVersionNumber(), ApiFile.Version.FileDescriptor.Type.file).status == ApiFile.Status.Complete)) || apiFile_2.GetFileDescriptor(apiFile_2.GetLatestVersionNumber(), ApiFile.Version.FileDescriptor.Type.signature).status != ApiFile.Status.Complete)
                                            {
                                                smethod_5(Failure, apiFile_2, "Failed to upload file.", "Record status is not 'complete'");
                                                smethod_8(apiFile_2.id);
                                                break;
                                            }
                                            if (!(flag2 ? (apiFile_2.GetFileDescriptor(apiFile_2.GetLatestVersionNumber(), ApiFile.Version.FileDescriptor.Type.file).status != ApiFile.Status.Waiting) : (apiFile_2.GetFileDescriptor(apiFile_2.GetLatestVersionNumber(), ApiFile.Version.FileDescriptor.Type.delta).status != ApiFile.Status.Waiting)))
                                            {
                                                smethod_5(Failure, apiFile_2, "Failed to upload file.", "Record is still in 'waiting' status");
                                                smethod_8(apiFile_2.id);
                                                break;
                                            }
                                            smethod_3(apiFile_2, flag2);
                                            CheckFile(filecheck, apiFile_2, "Processing upload...");
                                            float num = float_2;
                                            float b = float_3;
                                            float num2 = method_5(apiFile_2.GetLatestVersion().file.sizeInBytes);
                                            double num3 = Time.realtimeSinceStartup;
                                            double num4 = num3;
                                            while (apiFile_2.HasQueuedOperation(flag2))
                                            {
                                                CheckFile(filecheck, apiFile_2, "Processing upload...", "Checking status in " + Mathf.CeilToInt(num) + " seconds");
                                                while ((double)Time.realtimeSinceStartup - num4 < (double)num)
                                                {
                                                    if (ApiFileHelper.Cancelled(Cancelled, Failure, apiFile_2))
                                                    {
                                                        smethod_8(apiFile_2.id);
                                                        yield break;
                                                    }
                                                    if ((double)Time.realtimeSinceStartup - num3 > (double)num2)
                                                    {
                                                        smethod_3(apiFile_2, flag2);
                                                        smethod_5(Failure, apiFile_2, "Timed out waiting for upload processing to complete.");
                                                        smethod_8(apiFile_2.id);
                                                        yield break;
                                                    }
                                                    yield return null;
                                                }
                                                do
                                                {
                                                    CheckFile(filecheck, apiFile_2, "Processing upload...", "Checking status...");
                                                    bool_0 = true;
                                                    bool_2 = false;
                                                    string_5 = "";
                                                    API.Fetch<ApiFile>(apiFile_2.id, action, action2);
                                                    while (bool_0)
                                                    {
                                                        if (ApiFileHelper.Cancelled(Cancelled, Failure, apiFile_2))
                                                        {
                                                            smethod_8(apiFile_2.id);
                                                            yield break;
                                                        }
                                                        yield return null;
                                                    }
                                                    if (!string.IsNullOrEmpty(string_5))
                                                    {
                                                        smethod_5(Failure, apiFile_2, "Checking upload status failed.", string_5);
                                                        if (!bool_2)
                                                        {
                                                            smethod_8(apiFile_2.id);
                                                            yield break;
                                                        }
                                                    }
                                                }
                                                while (bool_2);
                                                num = Mathf.Min(num * 2f, b);
                                                num4 = Time.realtimeSinceStartup;
                                            }
                                            yield return MelonCoroutines.Start(method_4(apiFile_2.id));
                                            smethod_4(success, apiFile_2, "Upload complete!");
                                        }
                                        else
                                        {
                                            smethod_5(Failure, apiFile_2, "Failed to create file delta for upload.", "Couldn't get file size: " + string_5);
                                            smethod_8(apiFile_2.id);
                                        }
                                    }
                                    else
                                    {
                                        smethod_5(Failure, apiFile_2, "Failed to generate file signature!", "Couldn't get file size:\n" + string_5);
                                        smethod_8(apiFile_2.id);
                                    }
                                }
                                else
                                {
                                    smethod_5(Failure, apiFile_2, "Failed to generate MD5 hash for signature file.", string_5);
                                    smethod_8(apiFile_2.id);
                                }
                            }
                            else
                            {
                                smethod_5(Failure, apiFile_2, "Failed to generate file signature!", "Failed to create temp file: \n" + string_5);
                                smethod_8(apiFile_2.id);
                            }
                        }
                        else
                        {
                            smethod_5(Failure, apiFile_2, "Failed to optimize file for upload.", "Failed to create temp file: \n" + string_5);
                        }
                    }
                    else
                    {
                        smethod_5(Failure, apiFile_2, "A previous upload is still being processed. Please try again later.");
                    }
                    break;
                }
            }
            else
            {
                smethod_5(Failure, null, "Could not read file to upload!", Path + "\n" + whyNot);
            }
        }

        private static void smethod_3(ApiFile apiFile_0, bool bool_1, bool bool_2 = false)
        {
            if (apiFile_0 != null && apiFile_0.IsInitialized)
            {
                if (!apiFile_0.IsInErrorState() && bool_2)
                {
                    VRC.Core.Logger.Log("< color = yellow > Processing { 3}: { 0}, { 1}, { 2}</ color > " + (apiFile_0.IsWaitingForUpload() ? "waiting for upload" : "upload complete") + (apiFile_0.HasExistingOrPendingVersion() ? "has existing or pending version" : "no previous version") + (apiFile_0.IsLatestVersionQueued(bool_1) ? "latest version queued" : "latest version not queued") + apiFile_0.name, DebugLevel.All);
                }
            }
            else
            {
                Debug.LogFormat("<color=yellow>apiFile not initialized</color>", null);
            }
            if ((apiFile_0?.IsInitialized ?? false) && bool_2)
            {
                Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object> dictionary = apiFile_0.ExtractApiFields();
                if (dictionary != null)
                {
                    VRC.Core.Logger.Log("<color=yellow>{0}</color>" + Tools.JsonEncode(dictionary), DebugLevel.All);
                }
            }
        }

        public IEnumerator method_1(string string_0, string string_1, System.Action<GEnum0> action_0, System.Action<string> action_1)
        {
            VRC.Core.Logger.Log("CreateOptimizedFile: " + string_0 + " => " + string_1, DebugLevel.All);
            if (!smethod_2(string_0))
            {
                VRC.Core.Logger.Log("CreateOptimizedFile: (not gzip compressed, done)", DebugLevel.All);
                action_0?.Invoke(GEnum0.Unchanged);
                yield break;
            }
            bool flag = string.Compare(Path.GetExtension(string_0), ".unitypackage", ignoreCase: true) == 0;
            yield return null;
            Stream stream;
            try
            {
                stream = new GZipStream(string_0, 262144);
            }
            catch (Exception ex)
            {
                action_1?.Invoke("Couldn't read file: " + string_0 + "\n" + ex.Message);
                yield break;
            }
            yield return null;
            GZipStream gZipStream;
            try
            {
                gZipStream = new GZipStream(string_1, CompressLevel.Best, rsyncable: true, 262144);
            }
            catch (System.Exception ex2)
            {
                stream?.Close();
                action_1?.Invoke("Couldn't create output file: " + string_1 + "\n" + ex2.Message);
                yield break;
            }
            yield return null;
            if (flag)
            {
                try
                {
                    System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                    byte[] array = new byte[4096];
                    TarInputStream tarInputStream = new TarInputStream(stream);
                    for (TarEntry nextEntry = tarInputStream.GetNextEntry(); nextEntry != null; nextEntry = tarInputStream.GetNextEntry())
                    {
                        if (nextEntry.Size > 0L && nextEntry.Name.EndsWith("/pathname", System.StringComparison.OrdinalIgnoreCase))
                        {
                            int num = tarInputStream.Read(array, 0, (int)nextEntry.Size);
                            if (num > 0)
                            {
                                string string_3 = Encoding.ASCII.GetString(array, 0, num);
                                if (regex_0.Any((Regex regex_0) => regex_0.IsMatch(string_3)))
                                {
                                    string item = string_3.Substring(0, string_3.IndexOf('/'));
                                    list.Add(item);
                                }
                            }
                        }
                    }
                    tarInputStream.Close();
                    stream.Close();
                    stream = new GZipStream(string_0, 262144);
                    TarOutputStream tarOutputStream = new TarOutputStream(gZipStream);
                    TarInputStream tarInputStream2 = new TarInputStream(stream);
                    for (TarEntry nextEntry2 = tarInputStream2.GetNextEntry(); nextEntry2 != null; nextEntry2 = tarInputStream2.GetNextEntry())
                    {
                        string string_2 = nextEntry2.Name.Substring(0, nextEntry2.Name.IndexOf('/'));
                        if (!list.Any((string string_1) => string.Compare(string_1, string_2) == 0))
                        {
                            tarOutputStream.PutNextEntry(nextEntry2);
                            tarInputStream2.CopyEntryContents(tarOutputStream);
                            tarOutputStream.CloseEntry();
                        }
                    }
                    tarInputStream2.Close();
                    tarOutputStream.Close();
                }
                catch (Exception ex3)
                {
                    stream?.Close();
                    gZipStream?.Close();
                    action_1?.Invoke("Failed to strip and recompress file.\n" + ex3.Message);
                    yield break;
                }
            }
            else
            {
                try
                {
                    byte[] buffer = new byte[262144];
                    StreamUtils.Copy(stream, gZipStream, buffer);
                }
                catch (Exception ex4)
                {
                    stream?.Close();
                    gZipStream?.Close();
                    action_1?.Invoke("Failed to recompress file.\n" + ex4.Message);
                    yield break;
                }
            }
            yield return null;
            stream?.Close();
            gZipStream?.Close();
            yield return null;
            action_0?.Invoke(GEnum0.Success);
        }

        public IEnumerator method_2(string string_0, string string_1, Action action_0, Action<string> action_1)
        {
            VRC.Core.Logger.Log("CreateFileSignature: " + string_0 + " => " + string_1, DebugLevel.All);
            yield return null;
            byte[] array = new byte[65536];
            Stream stream;
            try
            {
                stream = Librsync.ComputeSignature(File.OpenRead(string_0));
            }
            catch (Exception ex)
            {
                action_1?.Invoke("Couldn't open input file: " + ex.Message);
                yield break;
            }
            FileStream fileStream;
            try
            {
                fileStream = File.Open(string_1, FileMode.Create, FileAccess.Write);
            }
            catch (Exception ex2)
            {
                action_1?.Invoke("Couldn't create output file: " + ex2.Message);
                yield break;
            }
            while (true)
            {
                IAsyncResult asyncResult;
                try
                {
                    asyncResult = stream.BeginRead(array, 0, array.Length, null, null);
                }
                catch (Exception ex3)
                {
                    action_1?.Invoke("Couldn't read file: " + ex3.Message);
                    yield break;
                }
                while (!asyncResult.IsCompleted)
                {
                    yield return null;
                }
                int num;
                try
                {
                    num = stream.EndRead(asyncResult);
                }
                catch (Exception ex4)
                {
                    action_1?.Invoke("Couldn't read file: " + ex4.Message);
                    yield break;
                }
                if (num <= 0)
                {
                    break;
                }
                IAsyncResult asyncResult2;
                try
                {
                    asyncResult2 = fileStream.BeginWrite(array, 0, num, null, null);
                }
                catch (Exception ex5)
                {
                    action_1?.Invoke("Couldn't write file: " + ex5.Message);
                    yield break;
                }
                while (!asyncResult2.IsCompleted)
                {
                    yield return null;
                }
                try
                {
                    fileStream.EndWrite(asyncResult2);
                }
                catch (Exception ex6)
                {
                    action_1?.Invoke("Couldn't write file: " + ex6.Message);
                    yield break;
                }
            }
            stream.Close();
            fileStream.Close();
            yield return null;
            action_0?.Invoke();
        }

        public IEnumerator method_3(string string_0, string string_1, string string_2, System.Action action_0, System.Action<string> action_1)
        {
            MelonLogger.Log("CreateFileDelta: " + string_0 + " (delta) " + string_1 + " => " + string_2);
            yield return null;
            byte[] array = new byte[65536];
            Stream stream;
            try
            {
                stream = Librsync.ComputeDelta(File.OpenRead(string_1), File.OpenRead(string_0));
            }
            catch (Exception ex)
            {
                action_1?.Invoke("Couldn't open input file: " + ex.Message);
                yield break;
            }
            FileStream fileStream;
            try
            {
                fileStream = File.Open(string_2, FileMode.Create, FileAccess.Write);
            }
            catch (Exception ex2)
            {
                action_1?.Invoke("Couldn't create output file: " + ex2.Message);
                yield break;
            }
            while (true)
            {
                IAsyncResult asyncResult;
                try
                {
                    asyncResult = stream.BeginRead(array, 0, array.Length, null, null);
                }
                catch (Exception ex3)
                {
                    action_1?.Invoke("Couldn't read file: " + ex3.Message);
                    yield break;
                }
                while (!asyncResult.IsCompleted)
                {
                    yield return null;
                }
                int num;
                try
                {
                    num = stream.EndRead(asyncResult);
                }
                catch (Exception ex4)
                {
                    action_1?.Invoke("Couldn't read file: " + ex4.Message);
                    yield break;
                }
                if (num <= 0)
                {
                    break;
                }
                IAsyncResult asyncResult2;
                try
                {
                    asyncResult2 = fileStream.BeginWrite(array, 0, num, null, null);
                }
                catch (Exception ex5)
                {
                    action_1?.Invoke("Couldn't write file: " + ex5.Message);
                    yield break;
                }
                while (!asyncResult2.IsCompleted)
                {
                    yield return null;
                }
                try
                {
                    fileStream.EndWrite(asyncResult2);
                }
                catch (Exception ex6)
                {
                    action_1?.Invoke("Couldn't write file: " + ex6.Message);
                    yield break;
                }
            }
            stream.Close();
            fileStream.Close();
            yield return null;
            action_0?.Invoke();
        }

        protected static void smethod_4(GDelegate0 gdelegate0_0, ApiFile apiFile_0, string string_0)
        {
            if (apiFile_0 == null)
            {
                apiFile_0 = new ApiFile();
            }
            VRC.Core.Logger.Log("ApiFile " + apiFile_0.ToStringBrief() + ": Operation Succeeded!", DebugLevel.All);
            gdelegate0_0?.Invoke(apiFile_0, string_0);
        }

        protected static void smethod_5(GDelegate1 gdelegate1_0, ApiFile apiFile_0, string string_0, string string_1 = "")
        {
            if (apiFile_0 == null)
            {
                apiFile_0 = new ApiFile();
            }
            Extensions.Logger.WengaLogger("ApiFile " + apiFile_0.ToStringBrief() + ": Error: " + string_0 + "\n" + string_1);
            gdelegate1_0?.Invoke(apiFile_0, string_0);
        }

        protected static void CheckFile(GDelegate2 gdelegate2_0, ApiFile apiFile_0, string string_0, string string_1 = "", float float_5 = 0f)
        {
            if (apiFile_0 == null)
            {
                apiFile_0 = new ApiFile();
            }
            gdelegate2_0?.Invoke(apiFile_0, string_0, string_1, float_5);
        }

        protected static bool Cancelled(GDelegate3 gdelegate3_0, GDelegate1 gdelegate1_0, ApiFile apiFile_0)
        {
            if (apiFile_0 == null)
            {
                Extensions.Logger.WengaLogger("apiFile was null");
                return true;
            }
            if (gdelegate3_0 != null && gdelegate3_0(apiFile_0))
            {
                Extensions.Logger.WengaLogger("ApiFile " + apiFile_0.ToStringBrief() + ": Operation cancelled");
                gdelegate1_0?.Invoke(apiFile_0, "Cancelled by user.");
                return true;
            }
            return false;
        }

        protected static void smethod_8(string string_0)
        {
            MelonCoroutines.Start(apifilehelper.method_4(string_0));
        }

        protected IEnumerator method_4(string string_0)
        {
            if (string.IsNullOrEmpty(string_0))
            {
                yield break;
            }
            string tempFolderPath = Tools.GetTempFolderPath(string_0);
            while (Directory.Exists(tempFolderPath))
            {
                try
                {
                    if (Directory.Exists(tempFolderPath))
                    {
                        Directory.Delete(tempFolderPath, recursive: true);
                    }
                }
                catch { }
                yield return null;
            }
        }

        private static void smethod_9()
        {
            if (helper == null)
            {
                GameObject obj = new GameObject("ApiFileHelper")
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
                helper = obj.AddComponent<ApiFileHelper>();
                remoteConfig_0 = new RemoteConfig();
                DontDestroyOnLoad(obj);
            }
        }

        private float method_5(int int_2)
        {
            return Mathf.Clamp(Mathf.Ceil(int_2 / (float)int_1) * float_0, float_0, float_1);
        }

        private bool method_6(ApiFile apiFile_0, string string_0, string string_1, long long_0, ApiFile.Version.FileDescriptor fileDescriptor_0, System.Action<ApiFile> action_0, System.Action<string> action_1)
        {
            if (fileDescriptor_0.status != ApiFile.Status.Waiting)
            {
                Extensions.Logger.WengaLogger("UploadFileComponent: (file record not in waiting status, done)");
                action_0?.Invoke(apiFile_0);
                return false;
            }
            if (long_0 == fileDescriptor_0.sizeInBytes)
            {
                if (string.Compare(string_1, fileDescriptor_0.md5) == 0)
                {
                    long size = 0L;
                    string errorStr = "";
                    if (!Tools.GetFileSize(string_0, out size, out errorStr))
                    {
                        action_1?.Invoke("Couldn't get file size");
                        return false;
                    }
                    if (size != long_0)
                    {
                        action_1?.Invoke("File size does not match input size");
                        return false;
                    }
                    return true;
                }
                action_1?.Invoke("File MD5 does not match version descriptor");
                return false;
            }
            action_1?.Invoke("File size does not match version descriptor");
            return false;
        }

        private IEnumerator method_7(ApiFile apiFile_0, ApiFile.Version.FileDescriptor.Type type_0, string string_0, string string_1, long long_0, System.Action<ApiFile> action_0, System.Action<string> action_1, System.Action<long, long> action_2, GDelegate3 gdelegate3_0)
        {
            GDelegate1 gdelegate1_ = delegate (ApiFile apiFile_1, string string_1)
            {
                action_1?.Invoke(string_1);
            };
            string string_2 = "";
            while (true)
            {
                bool bool_0 = true;
                string string_3 = "";
                bool bool_3 = false;
                apiFile_0.StartSimpleUpload(type_0, (Action<ApiContainer>)delegate (ApiContainer apiContainer_0)
                {
                    string_2 = IL2CPP.Il2CppStringToManaged(apiContainer_0.Cast<ApiDictContainer>().ResponseDictionary["url"].Pointer);
                    bool_0 = false;
                }, (Action<ApiContainer>)delegate (ApiContainer apiContainer_0)
                {
                    string_3 = "Failed to start upload: " + apiContainer_0.Error;
                    bool_0 = false;
                    if (apiContainer_0.Code == 400)
                    {
                        bool_3 = true;
                    }
                });
                while (bool_0)
                {
                    if (!Cancelled(gdelegate3_0, gdelegate1_, apiFile_0))
                    {
                        yield return null;
                        continue;
                    }
                    yield break;
                }
                if (!string.IsNullOrEmpty(string_3))
                {
                    action_1?.Invoke(string_3);
                    if (!bool_3)
                    {
                        yield break;
                    }
                }
                yield return new WaitForSecondsRealtime(0.75f);
                if (!bool_3)
                {
                    break;
                }
            }
            bool bool_ = true;
            string string_4 = "";
            HttpRequest httpRequest = ApiFile.PutSimpleFileToURL(string_2, string_0, smethod_1(Path.GetExtension(string_0)), string_1, (Action)delegate
            {
                bool_ = false;
            }, (Action<string>)delegate (string string_1)
            {
                string_4 = "Failed to upload file: " + string_1;
                bool_ = false;
            }, (Action<long, long>)delegate (long long_0, long long_1)
            {
                action_2?.Invoke(long_0, long_1);
            });
            while (true)
            {
                if (!bool_)
                {
                    if (!string.IsNullOrEmpty(string_4))
                    {
                        action_1?.Invoke(string_4);
                        yield break;
                    }
                    break;
                }
                if (Cancelled(gdelegate3_0, gdelegate1_, apiFile_0))
                {
                    httpRequest?.Abort();
                    yield break;
                }
                yield return null;
            }
            while (true)
            {
                yield return new WaitForSecondsRealtime(0.75f);
                bool bool_2 = true;
                string string_5 = "";
                bool bool_4 = false;
                apiFile_0.FinishUpload(type_0, null, (Action<ApiContainer>)delegate (ApiContainer apiContainer_0)
                {
                    Extensions.Logger.WengaLogger("!!!!YOU CAN IGNORE THIS CASTING ERRORS!!!!");
                    apiFile_0 = apiContainer_0.Model.Cast<ApiFile>();
                    bool_2 = false;
                }, (Action<ApiContainer>)delegate (ApiContainer apiContainer_0)
                {
                    string_5 = "Failed to finish upload: " + apiContainer_0.Error;
                    bool_2 = false;
                    if (apiContainer_0.Code == 400)
                    {
                        bool_4 = false;
                    }
                });
                while (bool_2)
                {
                    if (Cancelled(gdelegate3_0, gdelegate1_, apiFile_0))
                    {
                        yield break;
                    }
                    yield return null;
                }
                if (!string.IsNullOrEmpty(string_5))
                {
                    action_1?.Invoke(string_5);
                    if (!bool_4)
                    {
                        break;
                    }
                }
                yield return new WaitForSecondsRealtime(0.75f);
                if (!bool_4)
                {
                    break;
                }
            }
        }

        private IEnumerator method_8(ApiFile apiFile_0, ApiFile.Version.FileDescriptor.Type type_0, string string_0, string string_1, long long_0, System.Action<ApiFile> action_0, System.Action<string> action_1, System.Action<long, long> action_2, GDelegate3 gdelegate3_0)
        {
            FileStream fileStream_0 = null;
            GDelegate1 gdelegate1_ = delegate (ApiFile apiFile_1, string string_0)
            {
                if (fileStream_0 != null)
                {
                    fileStream_0.Close();
                }
                action_1?.Invoke(string_0);
            };
            ApiFile.UploadStatus uploadStatus_0 = null;
            byte[] array;
            long long_4;
            System.Collections.Generic.List<string> list_0;
            int num;
            int i;
            while (true)
            {
                bool bool_3 = true;
                string string_6 = "";
                bool bool_5 = false;
                apiFile_0.GetUploadStatus(apiFile_0.GetLatestVersionNumber(), type_0, (Action<ApiContainer>)delegate (ApiContainer apiContainer_0)
                {
                    uploadStatus_0 = apiContainer_0.Model.Cast<ApiFile.UploadStatus>();
                    bool_3 = false;
                    VRC.Core.Logger.Log("Found existing multipart upload status (next part = " + uploadStatus_0.nextPartNumber + ")", DebugLevel.All);
                }, (Action<ApiContainer>)delegate (ApiContainer apiContainer_0)
                {
                    string_6 = "Failed to query multipart upload status: " + apiContainer_0.Error;
                    bool_3 = false;
                    if (apiContainer_0.Code == 400)
                    {
                        bool_5 = true;
                    }
                });
                while (bool_3)
                {
                    if (!Cancelled(gdelegate3_0, gdelegate1_, apiFile_0))
                    {
                        yield return null;
                        continue;
                    }
                    yield break;
                }
                if (!string.IsNullOrEmpty(string_6))
                {
                    action_1?.Invoke(string_6);
                    if (!bool_5)
                    {
                        yield break;
                    }
                }
                if (bool_5)
                {
                    continue;
                }
                try
                {
                    fileStream_0 = File.OpenRead(string_0);
                }
                catch (System.Exception ex)
                {
                    action_1?.Invoke("Couldn't open file: " + ex.Message);
                    yield break;
                }
                array = new byte[this.int_0 * 2];
                long_4 = 0L;
                list_0 = new System.Collections.Generic.List<string>();
                if (uploadStatus_0 != null)
                {
                    list_0 = uploadStatus_0.etags.ToArray().ToList();
                }
                num = Mathf.Max(1, Mathf.FloorToInt((float)fileStream_0.Length / (float)this.int_0));
                i = 1;
                break;
            }
            for (; i <= num; i++)
            {
                int num2 = (int)((i < num) ? this.int_0 : (fileStream_0.Length - fileStream_0.Position));
                int int_0 = 0;
                try
                {
                    int_0 = fileStream_0.Read(array, 0, num2);
                }
                catch (System.Exception ex2)
                {
                    fileStream_0.Close();
                    action_1?.Invoke("Couldn't read file: " + ex2.Message);
                    yield break;
                }
                if (int_0 != num2)
                {
                    fileStream_0.Close();
                    action_1?.Invoke("Couldn't read file: read incorrect number of bytes from stream");
                    yield break;
                }
                if (uploadStatus_0 == null || !(i <= uploadStatus_0.nextPartNumber))
                {
                    string string_5 = "";
                    bool flag;
                    do
                    {
                        bool bool_2 = true;
                        string string_4 = "";
                        flag = false;
                        apiFile_0.StartMultipartUpload(type_0, i, (Action<ApiContainer>)delegate (ApiContainer apiContainer_0)
                        {
                            string_5 = IL2CPP.Il2CppStringToManaged(apiContainer_0.Cast<ApiDictContainer>().ResponseDictionary["url"].Pointer);
                            bool_2 = false;
                        }, (Action<ApiContainer>)delegate (ApiContainer apiContainer_0)
                        {
                            string_4 = "Failed to start part upload: " + apiContainer_0.Error;
                            bool_2 = false;
                        });
                        while (bool_2)
                        {
                            if (!Cancelled(gdelegate3_0, gdelegate1_, apiFile_0))
                            {
                                yield return null;
                                continue;
                            }
                            yield break;
                        }
                        if (!string.IsNullOrEmpty(string_4))
                        {
                            fileStream_0.Close();
                            action_1?.Invoke(string_4);
                            if (!flag)
                            {
                                yield break;
                            }
                        }
                        yield return new WaitForSecondsRealtime(0.75f);
                    }
                    while (flag);
                    bool bool_ = true;
                    string string_3 = "";
                    HttpRequest httpRequest = ApiFile.PutMultipartDataToURL(string_5, array, int_0, smethod_1(Path.GetExtension(string_0)), (System.Action<string>)delegate (string string_1)
                    {
                        if (!string.IsNullOrEmpty(string_1))
                        {
                            list_0.Add(string_1);
                        }
                        long_4 += int_0;
                        bool_ = false;
                    }, (System.Action<string>)delegate (string string_1)
                    {
                        string_3 = "Failed to upload data: " + string_1;
                        bool_ = false;
                    }, (System.Action<long, long>)delegate (long long_2, long long_3)
                    {
                        action_2?.Invoke(long_4 + long_2, long_0);
                    });
                    while (bool_)
                    {
                        if (!Cancelled(gdelegate3_0, gdelegate1_, apiFile_0))
                        {
                            yield return null;
                            continue;
                        }
                        httpRequest?.Abort();
                        yield break;
                    }
                    if (!string.IsNullOrEmpty(string_3))
                    {
                        fileStream_0.Close();
                        action_1?.Invoke(string_3);
                        yield break;
                    }
                }
                else
                {
                    long_4 += int_0;
                }
            }
            while (true)
            {
                yield return new WaitForSecondsRealtime(0.75f);
                bool bool_0 = true;
                string string_2 = "";
                bool bool_4 = false;
                Il2CppSystem.Collections.Generic.List<string> list = new Il2CppSystem.Collections.Generic.List<string>();
                foreach (string item in list_0)
                {
                    list.Add(item);
                }
                apiFile_0.FinishUpload(type_0, list, (Action<ApiContainer>)delegate (ApiContainer apiContainer_0)
                {
                    Extensions.Logger.WengaLogger("!!!!YOU CAN IGNORE THIS CASTING ERRORS!!!!");
                    apiFile_0 = apiContainer_0.Model.Cast<ApiFile>();
                    bool_0 = false;
                }, (Action<ApiContainer>)delegate (ApiContainer apiContainer_0)
                {
                    string_2 = "Failed to finish upload: " + apiContainer_0.Error;
                    bool_0 = false;
                    if (apiContainer_0.Code == 400)
                    {
                        bool_4 = true;
                    }
                });
                while (bool_0)
                {
                    if (!Cancelled(gdelegate3_0, gdelegate1_, apiFile_0))
                    {
                        yield return null;
                        continue;
                    }
                    yield break;
                }
                if (!string.IsNullOrEmpty(string_2))
                {
                    fileStream_0.Close();
                    action_1?.Invoke(string_2);
                    if (!bool_4)
                    {
                        yield break;
                    }
                }
                yield return new WaitForSecondsRealtime(0.75f);
                if (!bool_4)
                {
                    break;
                }
            }
            fileStream_0.Close();
        }

        private IEnumerator method_9(ApiFile apiFile_0, ApiFile.Version.FileDescriptor.Type type_0, string string_0, string string_1, long long_0, ApiFile.Version.FileDescriptor fileDescriptor_0, System.Action<ApiFile> action_0, System.Action<string> action_1, System.Action<long, long> action_2, GDelegate3 gdelegate3_0)
        {
            GDelegate1 gdelegate1_ = delegate (ApiFile apiFile_0, string string_0)
            {
                action_1?.Invoke(string_0);
            };
            float realtimeSinceStartup = Time.realtimeSinceStartup;
            float num = realtimeSinceStartup;
            float num2 = method_5(fileDescriptor_0.sizeInBytes);
            float num3 = float_2;
            float b = float_3;
            while (apiFile_0 != null)
            {
                ApiFile.Version.FileDescriptor fileDescriptor = apiFile_0.GetFileDescriptor(apiFile_0.GetLatestVersionNumber(), type_0);
                if (fileDescriptor != null)
                {
                    if (fileDescriptor.status == ApiFile.Status.Waiting)
                    {
                        while (Time.realtimeSinceStartup - num < num3)
                        {
                            if (Cancelled(gdelegate3_0, gdelegate1_, apiFile_0))
                            {
                                yield break;
                            }
                            if (Time.realtimeSinceStartup - realtimeSinceStartup <= num2)
                            {
                                yield return null;
                                continue;
                            }
                            action_1?.Invoke("Couldn't verify upload status: Timed out wait for server processing");
                            yield break;
                        }
                        while (true)
                        {
                            bool bool_0 = true;
                            string string_2 = "";
                            bool bool_1 = false;
                            apiFile_0.Refresh((Action<ApiContainer>)delegate
                            {
                                bool_0 = false;
                            }, (Action<ApiContainer>)delegate (ApiContainer apiContainer_0)
                            {
                                string_2 = "Couldn't verify upload status: " + apiContainer_0.Error;
                                bool_0 = false;
                                if (apiContainer_0.Code == 400)
                                {
                                    bool_1 = true;
                                }
                            });
                            while (bool_0)
                            {
                                if (!Cancelled(gdelegate3_0, gdelegate1_, apiFile_0))
                                {
                                    yield return null;
                                    continue;
                                }
                                yield break;
                            }
                            if (!string.IsNullOrEmpty(string_2))
                            {
                                action_1?.Invoke(string_2);
                                if (!bool_1)
                                {
                                    yield break;
                                }
                            }
                            if (!bool_1)
                            {
                                break;
                            }
                        }
                        num3 = Mathf.Min(num3 * 2f, b);
                        num = Time.realtimeSinceStartup;
                        continue;
                    }
                    action_0?.Invoke(apiFile_0);
                    yield break;
                }
                action_1?.Invoke("File descriptor is null ('" + type_0.ToString() + "')");
                yield break;
            }
            action_1?.Invoke("ApiFile is null");
        }

        private IEnumerator method_10(ApiFile apiFile_0, ApiFile.Version.FileDescriptor.Type type_0, string string_0, string string_1, long long_0, System.Action<ApiFile> action_0, System.Action<string> action_1, System.Action<long, long> action_2, GDelegate3 gdelegate3_0)
        {
            VRC.Core.Logger.Log("UploadFileComponent: " + type_0.ToString() + " (" + apiFile_0.id + "): " + string_0, DebugLevel.All);
            ApiFile.Version.FileDescriptor fileDescriptor = apiFile_0.GetFileDescriptor(apiFile_0.GetLatestVersionNumber(), type_0);
            if (method_6(apiFile_0, string_0, string_1, long_0, fileDescriptor, action_0, action_1))
            {
                switch (fileDescriptor.category)
                {
                    case ApiFile.Category.Simple:
                        yield return method_7(apiFile_0, type_0, string_0, string_1, long_0, action_0, action_1, action_2, gdelegate3_0);
                        break;

                    case ApiFile.Category.Multipart:
                        yield return method_8(apiFile_0, type_0, string_0, string_1, long_0, action_0, action_1, action_2, gdelegate3_0);
                        break;

                    default:
                        action_1?.Invoke("Unknown file category type: " + fileDescriptor.category);
                        yield break;
                }
                yield return method_9(apiFile_0, type_0, string_0, string_1, long_0, fileDescriptor, action_0, action_1, action_2, gdelegate3_0);
            }
        }
    }
}