namespace LunaR.Loaders
{
    public class MainLoader
    {
        public static unsafe void OnApplicationStart()
        {
            LoaderClasses.OnStart();
        }

        public static void OnLevelWasLoaded(int level)
        {
            LoaderClasses.LevelLoaded(level);
        }

        public static void OnLevelWasInitialized(int level)
        {
            LoaderClasses.LevelInit(level);
        }

        public static void OnUpdate()
        {
            LoaderClasses.OnUpdate();
        }

        public static void OnApplicationQuit()
        {
            LoaderClasses.OnExit();
        }
    }
}