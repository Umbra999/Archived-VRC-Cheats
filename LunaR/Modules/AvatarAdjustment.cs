using LunaR.ConsoleUtils;
using LunaR.Wrappers;
using System.Collections.Generic;
using UnityEngine;

namespace LunaR.Modules
{
    internal class AvatarAdjustment
    {
        public static bool AntiSpawnsound = false;

        public static List<string> DisabledShaders = new();
        public static List<string> DisabledParticles = new();
        public static List<string> DisabledAudios = new();
        public static List<string> DisabledColliders = new();

        public static void DisableAvatarFeatures(GameObject avatarObject, VRCPlayer player)
        {
            if (DisabledAudios.Contains(player.UserID()))
            {
                foreach (AudioSource renderer in avatarObject.GetComponentsInChildren<AudioSource>(true))
                {
                    Object.DestroyImmediate(renderer, true);
                }
            }

            if (DisabledShaders.Contains(player.UserID()))
            {
                foreach (Renderer renderer in avatarObject.GetComponentsInChildren<Renderer>(true))
                {
                    foreach (Material material in renderer.materials)
                    {
                        material.shader = Shader.Find("Diffuse");
                    }
                }
            }

            if (DisabledParticles.Contains(player.UserID()))
            {
                foreach (ParticleSystem renderer in avatarObject.GetComponentsInChildren<ParticleSystem>(true))
                {
                    Object.DestroyImmediate(renderer, true);
                }
            }

            if (DisabledColliders.Contains(player.UserID()))
            {
                foreach (Collider renderer in avatarObject.GetComponentsInChildren<Collider>(true))
                {
                    Object.DestroyImmediate(renderer, true);
                }

                foreach (BoxCollider renderer in avatarObject.GetComponentsInChildren<BoxCollider>(true))
                {
                   Object.DestroyImmediate(renderer, true);
                }

                foreach (CapsuleCollider renderer in avatarObject.GetComponentsInChildren<CapsuleCollider>(true))
                {
                    Object.DestroyImmediate(renderer, true);
                }

                foreach (SphereCollider renderer in avatarObject.GetComponentsInChildren<SphereCollider>(true))
                {
                    Object.DestroyImmediate(renderer, true);
                }
            }
        }

        public static void AdjustAvatar(GameObject gameObject)
        {
            gameObject.SetActive(false);

            AudioSource[] AudioSources = gameObject.GetComponentsInChildren<AudioSource>(true);
            HandleAudios(AudioSources);

            Light[] Lights = gameObject.GetComponentsInChildren<Light>(true);
            HandleLights(Lights);

            ParticleSystem[] Particles = gameObject.GetComponentsInChildren<ParticleSystem>(true);
            HandleParticles(Particles);

            Animator[] Animators = gameObject.GetComponentsInChildren<Animator>(true);
            HandleAnimators(Animators);

            Collider[] Colliders = gameObject.GetComponentsInChildren<Collider>(true);
            HandleColliders(Colliders);

            BoxCollider[] BoxColliders = gameObject.GetComponentsInChildren<BoxCollider>(true);
            HandleColliders(BoxColliders);

            CapsuleCollider[] CapsuleColliders = gameObject.GetComponentsInChildren<CapsuleCollider>(true);
            HandleColliders(CapsuleColliders);

            SphereCollider[] SphereColliders = gameObject.GetComponentsInChildren<SphereCollider>(true);
            HandleColliders(SphereColliders);

            Renderer[] Renderers = gameObject.GetComponentsInChildren<Renderer>(true);
            HandleRenderers(Renderers);

            SkinnedMeshRenderer[] MeshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            HandleMeshRenderers(MeshRenderers);

            Rigidbody[] Rigidbodys = gameObject.GetComponentsInChildren<Rigidbody>(true);
            HandleRigidbodys(Rigidbodys);;

            gameObject.SetActive(true);
        }

        private static void HandleAudios(AudioSource[] Audios)
        {
            int Count = 30;

            if (Audios.Length > Count)
            {
                for (int i = Count; i < Audios.Length; i++)
                {
                    Object.DestroyImmediate(Audios[i], true);
                }
                Extensions.Logger.Log($"Avatar with Overflow of {Audios} {Audios.Length}", Extensions.Logger.LogsType.Protection);
                VRConsole.Log(VRConsole.LogsType.Protection, $"Avatar --> Overflow of {Audios} [{Audios.Length}]");
            }

            if (AntiSpawnsound)
            {
                foreach (AudioSource Source in Audios)
                {
                    if (Source == null) continue;
                    if (Source.enabled && Source.minDistance > 10 && !Source.loop) Object.DestroyImmediate(Source, true);
                }
            }
        }

        private static void HandleLights(Light[] Lights)
        {
            int Count = 20;

            if (Lights.Length > Count)
            {
                for (int i = Count; i < Lights.Length; i++)
                {
                    Object.DestroyImmediate(Lights[i], true);
                }
                Extensions.Logger.Log($"Avatar with Overflow of {Lights} {Lights.Length}", Extensions.Logger.LogsType.Protection);
                VRConsole.Log(VRConsole.LogsType.Protection, $"Avatar --> Overflow of {Lights} [{Lights.Length}]");
            }
        }

        private static void HandleParticles(ParticleSystem[] Particles)
        {
            int Count = 90;

            if (Particles.Length > Count)
            {
                for (int i = Count; i < Particles.Length; i++)
                {
                    Object.DestroyImmediate(Particles[i], true);
                }
                Extensions.Logger.Log($"Avatar with Overflow of {Particles} {Particles.Length}", Extensions.Logger.LogsType.Protection);
                VRConsole.Log(VRConsole.LogsType.Protection, $"Avatar --> Overflow of {Particles} [{Particles.Length}]");
            }

            foreach (ParticleSystem particle in Particles)
            {
                if (Particles == null) continue;
                if (particle.maxParticles > 10000) particle.maxParticles = 10000;
            }
        }

        private static void HandleAnimators(Animator[] Animators)
        {
            int Count = 120;

            if (Animators.Length > Count)
            {
                for (int i = Count; i < Animators.Length; i++)
                {
                    Object.DestroyImmediate(Animators[i], true);
                }
                Extensions.Logger.Log($"Avatar with Overflow of {Animators} {Animators.Length}", Extensions.Logger.LogsType.Protection);
                VRConsole.Log(VRConsole.LogsType.Protection, $"Avatar --> Overflow of {Animators} [{Animators.Length}]");
            }
        }

        private static void HandleColliders(Collider[] Colliders)
        {
            int Count = 50;

            if (Colliders.Length > Count)
            {
                for (int i = Count; i < Colliders.Length; i++)
                {
                    Object.DestroyImmediate(Colliders[i], true);
                }
                Extensions.Logger.Log($"Avatar with Overflow of {Colliders} {Colliders.Length}", Extensions.Logger.LogsType.Protection);
                VRConsole.Log(VRConsole.LogsType.Protection, $"Avatar --> Overflow of {Colliders} [{Colliders.Length}]");
            }

            foreach (Collider collider in Colliders)
            {
                if (collider == null) continue;
                if (PatchExtensions.IsBadPosition(collider.transform.position) || PatchExtensions.IsBadRotation(collider.transform.rotation)) Object.DestroyImmediate(collider, true);
            }
        }

        private static void HandleRenderers(Renderer[] Renderers)
        {
            int Count = 350;

            if (Renderers.Length > Count)
            {
                for (int i = Count; i < Renderers.Length; i++)
                {
                    Object.DestroyImmediate(Renderers[i], true);
                }
                Extensions.Logger.Log($"Avatar with Overflow of {Renderers} {Renderers.Length}", Extensions.Logger.LogsType.Protection);
                VRConsole.Log(VRConsole.LogsType.Protection, $"Avatar --> Overflow of {Renderers} [{Renderers.Length}]");
            }
        }

        private static void HandleMeshRenderers(SkinnedMeshRenderer[] Renderers)
        {
            int Count = 45;

            if (Renderers.Length > Count)
            {
                for (int i = Count; i < Renderers.Length; i++)
                {
                    Object.DestroyImmediate(Renderers[i], true);
                }
                Extensions.Logger.Log($"Avatar with Overflow of {Renderers} {Renderers.Length}", Extensions.Logger.LogsType.Protection);
                VRConsole.Log(VRConsole.LogsType.Protection, $"Avatar --> Overflow of {Renderers} [{Renderers.Length}]");
            }

            foreach (SkinnedMeshRenderer renderer in Renderers)
            {
                if (renderer == null) continue;
                renderer.updateWhenOffscreen = false;
                renderer.sortingOrder = 0;
                renderer.sortingLayerID = 0;
                renderer.rendererPriority = 0;
            }
        }

        private static void HandleRigidbodys(Rigidbody[] Rigidbodys)
        {
            int Count = 30;

            if (Rigidbodys.Length > Count)
            {
                for (int i = Count; i < Rigidbodys.Length; i++)
                {
                    Object.DestroyImmediate(Rigidbodys[i], true);
                }
                Extensions.Logger.Log($"Avatar with Overflow of {Rigidbodys} {Rigidbodys.Length}", Extensions.Logger.LogsType.Protection);
                VRConsole.Log(VRConsole.LogsType.Protection, $"Avatar --> Overflow of {Rigidbodys} [{Rigidbodys.Length}]");
            }
        }
    }
}