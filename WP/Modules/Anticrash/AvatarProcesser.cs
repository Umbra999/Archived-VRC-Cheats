using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.Animations;
using VRC.Core;
using WengaPort.ConsoleUtils;
using WengaPort.Extensions;
using Logger = WengaPort.Extensions.Logger;

namespace WengaPort.Modules
{
    static class AvatarProcesser
    {
        private static readonly PriorityQueue<GameObjectWithPriorityData> ourBfsQueue = new PriorityQueue<GameObjectWithPriorityData>(GameObjectWithPriorityData.IsActiveDepthNumChildrenComparer);

        public static bool DisableBeforeScan = true;
        public static bool Cloths = true;
        public static bool AudioSources = true;
        public static bool DynamicBones = true;
        public static bool SpawnSound = true;
        public static bool ParticleSystems = true;
        public static bool Shaders = true;
        public static bool ScreenShaders = true;

        public static bool Enabled = true;

        public static int MaxAudio = 350;
        public static int MaxParticleSystems = 750;
        public static int MaxLights = 350;
        public static int MaxAnimators = 500;
        public static int MaxRidgitBodys = 500;
        public static int MaxColliders = 3000;
        public static int MaxConstraints = 5000;
        public static int MaxMaterialslots = 250;
        public static int MaxComponents = 5000;
        public static int MaxClothVerticies = 50000;
        public static int MaxParticles = 350000;
        public static int MaxPolys = 1500000;
        public static int MaxTransforms = 5000;

        public static void ProcessAvatar(GameObject AvatarObject,VRCAvatarManager avatarManager)
        {
            ApiAvatar avatar = avatarManager.field_Private_ApiAvatar_0;
            VRCPlayer player = avatarManager.field_Private_VRCPlayer_0;
            var start = Stopwatch.StartNew();
            var scannedObjects = 0;
            var destroyedObjects = 0;

            var seenTransforms = 0;
            var seenPolys = 0;
            var seenMaterials = 0;
            var seenAudioSources = 0;
            var seenParticles = 0;
            var seenConstraints = 0;
            var seenClothVertices = 0;
            var seenColliders = 0;
            var seenRigidbodies = 0;
            var seenAnimators = 0;
            var seenLights = 0;
            var seenComponents = 0;

            var animator = AvatarObject.GetComponent<Animator>();
            var componentList = new Il2CppSystem.Collections.Generic.List<Component>();
            void Bfs(GameObjectWithPriorityData objWithPriority)
            {
                var obj = objWithPriority.GameObject;
                obj.GetComponents(componentList);
                foreach (var compj in componentList)
                {
                    if (compj == null) continue;
                    compj.TryCast<Transform>()?.Check(ref scannedObjects, ref destroyedObjects, ref seenTransforms, animator);
                    compj.TryCast<AudioSource>()?.Check(ref scannedObjects, ref destroyedObjects, ref seenAudioSources);
                    compj.TryCast<Renderer>()?.Check(ref scannedObjects, ref destroyedObjects, ref seenPolys, ref seenMaterials);
                    compj.TryCast<Animator>()?.Check(ref scannedObjects, ref destroyedObjects, ref seenAnimators);
                    compj.TryCast<Collider>()?.Check(ref scannedObjects, ref destroyedObjects, ref seenColliders);
                    compj.TryCast<Light>()?.Check(ref scannedObjects, ref destroyedObjects, ref seenLights);
                    compj.TryCast<Cloth>()?.Check(ref scannedObjects, ref destroyedObjects, ref seenClothVertices);
                    compj.TryCast<Rigidbody>()?.Check(ref scannedObjects, ref destroyedObjects, ref seenRigidbodies);
                    compj.TryCast<ParticleSystem>()?.Check(ref scannedObjects, ref destroyedObjects, ref seenParticles);
                    compj.TryCast<IConstraint>()?.Check(ref scannedObjects, ref destroyedObjects, ref seenConstraints);
                    if (compj.TryCast<Transform>() == null)
                        compj.Check(ref scannedObjects, ref destroyedObjects, ref seenComponents);
                }
                foreach (var child in obj.transform)
                    ourBfsQueue.Enqueue(new GameObjectWithPriorityData(child.Cast<Transform>().gameObject, objWithPriority.Depth + 1));
            }
            Bfs(new GameObjectWithPriorityData(AvatarObject, 0));
            while (ourBfsQueue.Count > 0)
                Bfs(ourBfsQueue.Dequeue());

            AvatarObject.SetActive(true);
            if (destroyedObjects != 0)
            {
                Logger.WengaLogger($"[Anti Crash] {avatar.name} | {player.DisplayName()} | Destroyed: {destroyedObjects} | Scan Time: {start.ElapsedMilliseconds} MS");
                VRConsole.Log(VRConsole.LogsType.Protection, $"{player.DisplayName()} --> CrashAvatar");
            }
            start.Stop();
        }
        private static void Check(this AudioSource instance,ref int scannedObjects,ref int destroyedObjects, ref int seenAudioSources)
        {
            scannedObjects++;
            seenAudioSources++;
            if (seenAudioSources > MaxAudio)
            {
                destroyedObjects++;
                UnityEngine.Object.Destroy(instance);
            }
            if (!SpawnSound)
            {
                if(instance.enabled && instance.gameObject.activeSelf && instance.playOnAwake)
                {
                    instance.playOnAwake = false;
                    instance.Stop();
                }
            }
        }
        private static readonly Il2CppSystem.Collections.Generic.List<Material> ourMaterialsList = new Il2CppSystem.Collections.Generic.List<Material>();
        private static void Check(this Renderer instance, ref int scannedObjects, ref int destroyedObjects, ref int seenPolys, ref int seenMaterials)
        {
            scannedObjects++;
            var skinnedMeshRenderer = instance.TryCast<SkinnedMeshRenderer>();
            var meshFilter = instance.gameObject.GetComponent<MeshFilter>();

            instance.GetSharedMaterials(ourMaterialsList);
            if (ourMaterialsList.Count == 0) return;

            var mesh = skinnedMeshRenderer?.sharedMesh ?? meshFilter?.sharedMesh;
            if (mesh != null)
            {
                if (seenPolys + mesh.vertexCount >= MaxPolys)
                {
                    instance.SetMaterialArray(new Il2CppReferenceArray<Material>(0));

                    destroyedObjects++;
                    return;
                }

                seenPolys += mesh.vertexCount;
                if (!ScreenShaders && meshFilter != null && (ourMaterialsList[0]?.renderQueue ?? 0) >= 2500)
                {
                    var meshLowerName = mesh.name.ToLower();
                    if (meshLowerName.Contains("sphere") || meshLowerName.Contains("cube"))
                    {
                        destroyedObjects++;

                        instance.SetMaterialArray(new Il2CppReferenceArray<Material>(0));
                        return;
                    }
                }
            }
            var allowedMaterialCount = MaxMaterialslots - seenMaterials;
            if (allowedMaterialCount < instance.GetMaterialCount())
            {
                instance.GetSharedMaterials(ourMaterialsList);

                destroyedObjects += ourMaterialsList.Count - allowedMaterialCount;

                ourMaterialsList.RemoveRange(allowedMaterialCount, ourMaterialsList.Count - allowedMaterialCount);
                instance.materials = (Il2CppReferenceArray<Material>)ourMaterialsList.ToArray();
            }
            seenMaterials += instance.GetMaterialCount();
        }
        private static void Check(this Animator instance, ref int scannedObjects, ref int destroyedObjects,ref int seenAnimators)
        {
            scannedObjects++;
            seenAnimators++;
            if (seenAnimators > MaxAnimators)
            {
                destroyedObjects++;
                UnityEngine.Object.Destroy(instance);
            }
        }
        private static void Check(this Light instance, ref int scannedObjects, ref int destroyedObjects,ref int seenLights)
        {
            scannedObjects++;
            seenLights++;
            if (seenLights > MaxLights)
            {
                destroyedObjects++;
                UnityEngine.Object.Destroy(instance);
            }
        }
        private static void Check(this ParticleSystem instance, ref int scannedObjects, ref int destroyedObjects,ref int seenParticles)
        {
            scannedObjects++;
            seenParticles++;
            if (seenParticles > MaxParticleSystems)
            {
                destroyedObjects++;
                UnityEngine.Object.Destroy(instance);
            }
            if(instance.maxParticles > MaxParticles)
            {
                instance.maxParticles = MaxParticles;
            }
        }
        private static void Check(this Cloth instance, ref int scannedObjects, ref int destroyedObjects,ref int seenClothVertices)
        {
            scannedObjects++;

            instance.clothSolverFrequency = Mathf.Max(instance.clothSolverFrequency, 300f);

            var numVertices = 0;
            var skinnedMesh = instance.gameObject.GetComponent<SkinnedMeshRenderer>()?.sharedMesh;
            if (skinnedMesh == null || (seenClothVertices += (numVertices = skinnedMesh.vertexCount)) >= MaxClothVerticies)
            {
                seenClothVertices -= numVertices;
                destroyedObjects++;
                UnityEngine.Object.DestroyImmediate(instance, true);
            }
        }
        private static void Check(this Rigidbody instance, ref int scannedObjects, ref int destroyedObjects, ref int seenRigidbodies)
        {
            scannedObjects++;
            seenRigidbodies++;
            if (seenRigidbodies > MaxRidgitBodys)
            {
                destroyedObjects++;
                UnityEngine.Object.Destroy(instance);
            }
        }
        private static void Check(this IConstraint instance, ref int scannedObjects, ref int destroyedObjects, ref int seenContrains)
        {
            scannedObjects++;
            seenContrains++;
            if (seenContrains > MaxConstraints)
            {
                UnityEngine.Object.DestroyImmediate(instance.Cast<Behaviour>(), true);
                destroyedObjects++;
            }
        }
        private static void Check(this Collider instance, ref int scannedObjects, ref int destroyedObjects, ref int seenColliders)
        {
            scannedObjects++;
            seenColliders++;
            if (seenColliders > MaxColliders)
            {
                destroyedObjects++;
                UnityEngine.Object.Destroy(instance);
            }
        }
        private static void Check(this Transform instance, ref int scannedObjects, ref int destroyedObjects, ref int seenTransforms,Animator avataranimator)
        {
            scannedObjects++;
            seenTransforms++;
            if (avataranimator != null && seenTransforms > MaxTransforms && !avataranimator.IsBoneTransform(instance))
            {
                destroyedObjects++;
                UnityEngine.Object.Destroy(instance);
            }
        }
        private static void Check(this Component instance, ref int scannedObjects, ref int destroyedObjects, ref int seenComponents)
        {
            scannedObjects++;
            seenComponents++;
            if (seenComponents > MaxComponents)
            {
                destroyedObjects++;
                UnityEngine.Object.Destroy(instance);
            }
        }

    }
}
