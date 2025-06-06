using System.Collections.Generic;
using System.Reflection;
using Hexed.Interfaces;
using Hexed.Wrappers;

namespace Hexed.Core
{
    internal class GameManager
    {
        private static readonly List<IHook> LoadedHooks = new();
        public static void CreateHooks()
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (Type type in types)
            {
                if (type.IsAbstract) continue;

                if (!typeof(IHook).IsAssignableFrom(type)) continue;

                IHook instance = (IHook)Activator.CreateInstance(type);

                try
                {
                    if (GameUtils.IsInVr())
                    {
                        if (instance is IDesktopOnly updateableModule) continue;

                        instance.Initialize();
                        LoadedHooks.Add(instance);
                    }
                    else
                    {
                        if (instance is IVROnly updateableModule) continue;

                        instance.Initialize();
                        LoadedHooks.Add(instance);
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError($"Failed to Hook {instance.GetType().Name}: {e}");
                }
            }

            Logger.LogDebug($"Loaded {LoadedHooks.Count} Hooks");
        }


        private static readonly List<IGlobalModule> LoadedGlobalModules = new();
        public static void CreateGlobalModules()
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (Type type in types)
            {
                if (type.IsAbstract) continue;

                if (!typeof(IGlobalModule).IsAssignableFrom(type)) continue;

                IGlobalModule instance = (IGlobalModule)Activator.CreateInstance(type);

                try
                {
                    if (GameUtils.IsInVr())
                    {
                        if (instance is IDesktopOnly updateableModule) continue;

                        instance.Initialize();
                        LoadedGlobalModules.Add(instance);
                    }
                    else
                    {
                        if (instance is IVROnly updateableModule) continue;

                        instance.Initialize();
                        LoadedGlobalModules.Add(instance);
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError($"Failed to Initialize {instance.GetType().Name}: {e}");
                }
            }

            Logger.LogDebug($"Loaded {LoadedGlobalModules.Count} Global Modules");
        }

        private static float GlobalModuleDelayMs = UnityEngine.Time.time;
        public static void UpdateGlobalModules()
        {
            bool refreshTimer = false;

            foreach (IGlobalModule instance in LoadedGlobalModules)
            {
                if (instance is IDelayModule delayedModule)
                {
                    if (UnityEngine.Time.time > GlobalModuleDelayMs)
                    {
                        instance.OnUpdate();
                        refreshTimer = true;
                    }
                }
                else instance.OnUpdate();
            }

            if (refreshTimer) GlobalModuleDelayMs = UnityEngine.Time.time + 0.15f;
        }


        private static readonly Dictionary<int, List<IPlayerModule>> LoadedPlayerModules = new();
        public static void CreatePlayerModules(VRCPlayer player)
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (Type type in types)
            {
                if (type.IsAbstract) continue;

                if (!typeof(IPlayerModule).IsAssignableFrom(type)) continue;

                IPlayerModule instance = (IPlayerModule)Activator.CreateInstance(type);

                int Actor = player.GetPhotonPlayer().ActorID();

                try
                {
                    if (GameUtils.IsInVr())
                    {
                        if (instance is IDesktopOnly updateableModule) continue;

                        instance.Initialize(player);

                        if (!LoadedPlayerModules.ContainsKey(Actor)) LoadedPlayerModules.Add(Actor, new List<IPlayerModule>() { instance });
                        else LoadedPlayerModules[Actor].Add(instance);
                    }
                    else
                    {
                        if (instance is IVROnly updateableModule) continue;

                        instance.Initialize(player);

                        if (!LoadedPlayerModules.ContainsKey(Actor)) LoadedPlayerModules.Add(Actor, new List<IPlayerModule>() { instance });
                        else LoadedPlayerModules[Actor].Add(instance);
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError($"Failed to Initialize {instance.GetType().Name}: {e}");
                }
            }

            //Logger.LogDebug($"Loaded {LoadedPlayerModules.Count} Player Modules");
        }

        public static IPlayerModule GetPlayerModuleByClass(int Actor, Type Class)
        {
            if (LoadedPlayerModules.TryGetValue(Actor, out List<IPlayerModule> Modules))
            {
                return Modules.FirstOrDefault(x => x.GetType() == Class);
            }

            return null;
        }

        public static void DestroyPlayerModules(int Actor)
        {
            if (LoadedPlayerModules.ContainsKey(Actor)) LoadedPlayerModules.Remove(Actor);
            if (PlayerModuleDelay.ContainsKey(Actor)) PlayerModuleDelay.Remove(Actor);
        }

        public static void DestroyAllPlayerModules()
        {
            LoadedPlayerModules.Clear();
            PlayerModuleDelay.Clear();
        }

        private static readonly Dictionary<int, float> PlayerModuleDelay = new();
        public static void UpdatePlayerModules(int Actor)
        {
            bool refreshTimer = false;

            foreach (var instance in LoadedPlayerModules.Where(x => x.Value != null && x.Key == Actor))
            {
                foreach (var module in instance.Value)
                {
                    if (module is IDelayModule delayedModule)
                    {
                        if (!PlayerModuleDelay.ContainsKey(Actor))
                        {
                            PlayerModuleDelay.Add(Actor, UnityEngine.Time.time);
                        }

                        if (UnityEngine.Time.time > PlayerModuleDelay[Actor])
                        {
                            module.OnUpdate();

                            refreshTimer = true;
                        }
                    }
                    else module.OnUpdate();
                }
            }

            if (refreshTimer) PlayerModuleDelay[Actor] = UnityEngine.Time.time + 0.15f;
        }
    }
}
