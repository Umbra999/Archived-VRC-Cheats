namespace WengaPort.Utility
{
    enum MenuButtonType
    {
        PlaylistButton,
        AvatarFavButton,
        ReportButton    
    }
    enum MenuType
    {
        UserInfo,
        AvatarMenu,
        SettingsMenu,
        SocialMenu,
        WorldMenu,
        WorldInfoMenu
    }
    public enum HTTPMethods : byte
    {
        Get = 0,
        Head = 1,
        Post = 2,
        Put = 3,
        Delete = 4,
        Patch = 5
    }
}