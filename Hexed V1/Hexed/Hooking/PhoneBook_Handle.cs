using CoreRuntime.Manager;
using Hexed.Interfaces;
using Hexed.Modules.Standalone;
using Hexed.Wrappers;
using Transmtn;
using Transmtn.DTO;
using VRC.Core;

namespace Hexed.Hooking
{
    internal class PhoneBook_Handle : IHook
    {
        private delegate IntPtr _HandleDelegate(nint instance, nint __0, FriendMessageType __1);
        private static _HandleDelegate originalMethod;

        public void Initialize()
        {
            originalMethod = HookManager.Detour<_HandleDelegate>(typeof(PhoneBook).GetMethod(nameof(PhoneBook.Handle)), Patch);
        }

        private static IntPtr Patch(nint instance, nint __0, FriendMessageType __1)
        {
            IntPtr result = originalMethod(instance, __0, __1);

            User user = __0 == IntPtr.Zero ? null : new User(__0);
            if (user == null) return result;

            switch (__1)
            {
                case FriendMessageType.Add:
                    {
                        Wrappers.Logger.Log($"You and {user.displayName} friended", Wrappers.Logger.LogsType.Friends);
                        VRConsole.Log($"{user.displayName} --> Friend", VRConsole.LogsType.Friend);
                    }
                    break;

                case FriendMessageType.Delete:
                    {
                        APIUser.FetchUser(user.id, new Action<APIUser>((user) =>
                        {
                            Wrappers.Logger.Log($"You and {user.DisplayName()} unfriended", Wrappers.Logger.LogsType.Friends);
                            VRConsole.Log($"{user.DisplayName()} --> Unfriend", VRConsole.LogsType.Friend);
                        }), new Action<string>((error) =>
                        {
                            Wrappers.Logger.Log($"You and {user.id} unfriended", Wrappers.Logger.LogsType.Friends);
                            VRConsole.Log($"{user.id} --> Unfriend", VRConsole.LogsType.Friend);
                        }));
                    }
                    break;

                case FriendMessageType.Location:
                    {
                        if (user.location.isPrivate)
                        {
                            Wrappers.Logger.Log($"{user.displayName} is now in PRIVATE", Wrappers.Logger.LogsType.Friends);
                            VRConsole.Log($"{user.displayName} --> PRIVATE", VRConsole.LogsType.World);
                        }
                        else if (user.location != null && user.location.isTraveling && user.location.WorldId != null)
                        {
                            InstanceAccessType Type = GameUtils.GetWorldType(user.location.WorldId);
                            string LocalizedType = GameUtils.GetLocalizedWorldType(Type);

                            ApiWorld world = new() { id = user.location.WorldId.Split(':')[0] };
                            world.Fetch(new Action<ApiContainer>((container) =>
                            {
                                ApiModelContainer<ApiWorld> apiWorld = new();
                                apiWorld.setFromContainer(container);
                                ApiWorld World = container.Model.Cast<ApiWorld>();

                                Wrappers.Logger.Log($"{user.displayName} is now in {World.name} [{LocalizedType}] [{user.location.WorldId}]", Wrappers.Logger.LogsType.Friends);
                                VRConsole.Log($"{user.displayName} --> {World.name} [{LocalizedType}]", VRConsole.LogsType.World);
                            }), new Action<ApiContainer>((container) =>
                            {
                                Wrappers.Logger.Log($"{user.displayName} is now in UNKNOWN [{LocalizedType}] [{user.location.WorldId}]", Wrappers.Logger.LogsType.Friends);
                                VRConsole.Log($"{user.displayName} --> UNKNOWN [{LocalizedType}]", VRConsole.LogsType.World);
                            }));
                        }
                    }
                    break;

                case FriendMessageType.Online:
                    {
                        PlayerUtils.APIDevice Platform = PlayerUtils.GetAPIDevice(user.last_platform); 
                        Wrappers.Logger.Log($"{user.displayName} is now Online [{Platform}]", Wrappers.Logger.LogsType.Friends);
                        VRConsole.Log($"{user.displayName} [{Platform}]", VRConsole.LogsType.Online);
                    }
                    break;

                case FriendMessageType.Offline:
                    {
                        APIUser.FetchUser(user.id, new Action<APIUser>((user) =>
                        {
                            Wrappers.Logger.Log($"{user.DisplayName()} is now Offline", Wrappers.Logger.LogsType.Friends);
                            VRConsole.Log(user.DisplayName(), VRConsole.LogsType.Offline);
                        }), new Action<string>((error) =>
                        {
                            Wrappers.Logger.Log($"{user.id} is now Offline", Wrappers.Logger.LogsType.Friends);
                            VRConsole.Log(user.id, VRConsole.LogsType.Offline);
                        }));
                    }
                    break;

                case FriendMessageType.Update:
                    {

                    }
                    break;

                case FriendMessageType.Active:
                    {
                        Wrappers.Logger.Log($"{user.displayName} is now Online [Website]", Wrappers.Logger.LogsType.Friends);
                        VRConsole.Log($"{user.displayName} [Website]", VRConsole.LogsType.Online);
                    }
                    break;

                case FriendMessageType.SelfUpdate:
                    {

                    }
                    break;
            }

            return result;
        }
    }
}
