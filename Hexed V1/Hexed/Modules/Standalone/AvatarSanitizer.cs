using Hexed.Core;
using Hexed.Wrappers;
using UnityEngine;
using UnityEngine.Animations;

namespace Hexed.Modules.Standalone
{
    internal class AvatarSanitizer
    {
        public static void SanitizeAvatarObject(GameObject gameObject, VRCPlayer player) // add position checks to all gameobjects?
        {
            int RemovedComponents = 0;

            RemovedComponents += SanitizeAudios(gameObject.GetComponentsInChildren<AudioSource>(true));

            RemovedComponents += SanitizeONSPAudios(gameObject.GetComponentsInChildren<ONSPAudioSource>(true));

            RemovedComponents += SanitizeLights(gameObject.GetComponentsInChildren<Light>(true));

            RemovedComponents += SanitizeParticleSystems(gameObject.GetComponentsInChildren<ParticleSystem>(true));

            RemovedComponents += SanitizeAnimators(gameObject.GetComponentsInChildren<Animator>(true));

            RemovedComponents += SanitizeColliders(gameObject.GetComponentsInChildren<Collider>(true));

            RemovedComponents += SanitizeRenderers(gameObject.GetComponentsInChildren<Renderer>(true));

            RemovedComponents += SanitizeRigidbodys(gameObject.GetComponentsInChildren<Rigidbody>(true));

            RemovedComponents += SanitizeCloths(gameObject.GetComponentsInChildren<Cloth>(true));

            RemovedComponents += SanitizeConstraints(gameObject.GetComponentsInChildren<AimConstraint>(true));

            RemovedComponents += SanitizeConstraints(gameObject.GetComponentsInChildren<ParentConstraint>(true));

            RemovedComponents += SanitizeConstraints(gameObject.GetComponentsInChildren<RotationConstraint>(true));

            RemovedComponents += SanitizeConstraints(gameObject.GetComponentsInChildren<PositionConstraint>(true));

            RemovedComponents += SanitizeConstraints(gameObject.GetComponentsInChildren<ScaleConstraint>(true));

            RemovedComponents += SanitizeConstraints(gameObject.GetComponentsInChildren<LookAtConstraint>(true));

            if (RemovedComponents > 0)
            {
                Wrappers.Logger.Log($"Prevented {player.DisplayName()} from overflow with {RemovedComponents} Avatar components", Wrappers.Logger.LogsType.Protection);
                VRConsole.Log($"{player.DisplayName()} --> {RemovedComponents} Avatar components", VRConsole.LogsType.Protection);
            }
        }

        private static int SanitizeAudios(AudioSource[] Audios)
        {
            int Count = 20;
            int Index = 0;

            for (int i = 0; i < Audios.Length; i++)
            {
                if (Index > Count) UnityEngine.Object.DestroyImmediate(Audios[i], true);
                else if (InternalSettings.NoSpawnsound && Audios[i].enabled && Audios[i].playOnAwake && !Audios[i].loop) UnityEngine.Object.DestroyImmediate(Audios[i], true);
                else
                {
                    Audios[i].outputAudioMixerGroup = null;
                }

                Index++;
            }

            return Audios.Length - Count > 0 ? Audios.Length - Count : 0;
        }

        private static int SanitizeONSPAudios(ONSPAudioSource[] Audios)
        {
            int Count = 20;
            int Index = 0;

            for (int i = 0; i < Audios.Length; i++)
            {
                if (Index > Count) UnityEngine.Object.DestroyImmediate(Audios[i], true);

                Index++;
            }

            return Audios.Length - Count > 0 ? Audios.Length - Count : 0;
        }

        private static int SanitizeLights(Light[] Lights)
        {
            int Count = 40;
            int Index = 0;

            for (int i = 0; i < Lights.Length; i++)
            {
                if (Index > Count) UnityEngine.Object.DestroyImmediate(Lights[i], true);
                else
                {
                    Lights[i].shadowResolution = UnityEngine.Rendering.LightShadowResolution.Low;
                    Lights[i].shadowSoftness = 0;
                    Lights[i].shadowSoftnessFade = 0;
                    Lights[i].shadowStrength = 0;
                    Lights[i].shadowNearPlane = 0;
                    Lights[i].shadowBias = 0;
                    Lights[i].shadowNormalBias = 0;
                    Lights[i].shadowObjectSizeBias = 0;
                    Lights[i].shadowConstantBias = 0;
                }

                Index++;

            }

            return Lights.Length - Count > 0 ? Lights.Length - Count : 0;
        }

        private static int SanitizeParticleSystems(ParticleSystem[] Particles)
        {
            int Count = 150;
            int Index = 0;

            for (int i = 0; i < Particles.Length; i++)
            {
                if (Index > Count) UnityEngine.Object.DestroyImmediate(Particles[i], true);
                else
                {
                    if (Particles[i].maxParticles > 5000) Particles[i].maxParticles = 5000;
                    if (Particles[i].collision.maxCollisionShapes > 256) Particles[i].collision.maxCollisionShapes = 256;
                    if (Particles[i].trails.ribbonCount > 64) Particles[i].maxParticles = 64;
                }

                Index++;
            }

            return Particles.Length - Count > 0 ? Particles.Length - Count : 0;
        }

        private static int SanitizeAnimators(Animator[] Animators)
        {
            int Count = 80;
            int Index = 0;

            for (int i = 0; i < Animators.Length; i++)
            {
                if (Index > Count) UnityEngine.Object.DestroyImmediate(Animators[i], true);

                Index++;
            }

            return Animators.Length - Count > 0 ? Animators.Length - Count : 0;
        }

        private static int SanitizeColliders(Collider[] Colliders)
        {
            int Count = 50;
            int Index = 0;

            for (int i = 0; i < Colliders.Length; i++)
            {
                if (Index > Count) UnityEngine.Object.DestroyImmediate(Colliders[i], true);

                Index++;
            }

            return Colliders.Length - Count > 0 ? Colliders.Length - Count : 0;
        }

        private static int SanitizeRenderers(Renderer[] Renderers)
        {
            int Count = 400;
            int Index = 0;

            for (int i = 0; i < Renderers.Length; i++)
            {
                if (Index > Count) UnityEngine.Object.DestroyImmediate(Renderers[i], true);
                else
                {
                    Renderers[i].rayTracingMode = UnityEngine.Experimental.Rendering.RayTracingMode.Off;
                    Renderers[i].receiveShadows = false;
                    Renderers[i].castShadows = false;
                    Renderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    Renderers[i].sortingOrder = 0;
                    Renderers[i].sortingLayerID = 0;
                    Renderers[i].rendererPriority = 0;
                    Renderers[i].useLightProbes = false;
                    Renderers[i].reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

                    SkinnedMeshRenderer skinnedRenderer = Renderers[i].TryCast<SkinnedMeshRenderer>();
                    if (skinnedRenderer != null)
                    {
                        skinnedRenderer.updateWhenOffscreen = false;

                        Transform zeroScaleRoot = null;
                        Transform[] bones = skinnedRenderer.bones;
                        for (int b = 0; b < bones.Length; b++)
                        {
                            if (bones[b] != null) continue;

                            if (zeroScaleRoot == null)
                            {
                                GameObject newGo = new($"hexed-zero-scale");
                                zeroScaleRoot = newGo.transform;
                                zeroScaleRoot.SetParent(skinnedRenderer.rootBone, false);
                                zeroScaleRoot.localScale = Vector3.zero;
                            }

                            bones[b] = zeroScaleRoot;
                        }

                        if (zeroScaleRoot != null) skinnedRenderer.bones = bones;
                    }
                }

                Index++;
            }

            return Renderers.Length - Count > 0 ? Renderers.Length - Count : 0;
        }

        private static int SanitizeRigidbodys(Rigidbody[] Rigidbodys)
        {
            int Count = 30;
            int Index = 0;

            for (int i = 0; i < Rigidbodys.Length; i++)
            {
                if (Index > Count) UnityEngine.Object.DestroyImmediate(Rigidbodys[i], true);

                Index++;
            }

            return Rigidbodys.Length - Count > 0 ? Rigidbodys.Length - Count : 0;
        }

        private static int SanitizeCloths(Cloth[] Cloths)
        {
            int Count = 30;
            int Index = 0;

            for (int i = 0; i < Cloths.Length; i++)
            {
                if (Index > Count) UnityEngine.Object.DestroyImmediate(Cloths[i], true);

                Index++;
            }

            return Cloths.Length - Count > 0 ? Cloths.Length - Count : 0;
        }

        private static int SanitizeConstraints(Behaviour[] constraints)
        {
            int Count = 250;
            int Index = 0;

            for (int i = 0; i < constraints.Length; i++)
            {
                if (Index > Count) UnityEngine.Object.DestroyImmediate(constraints[i], true);

                Index++;
            }

            return constraints.Length - Count > 0 ? constraints.Length - Count : 0;
        }
    }
}
