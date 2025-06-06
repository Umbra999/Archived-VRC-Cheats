namespace Hexed.Interfaces
{
    internal interface IPlayerModule
    {
        void Initialize(VRCPlayer player);
        void OnUpdate();
    }
}
