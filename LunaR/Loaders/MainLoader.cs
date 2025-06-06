using LunaR.Loaders;
using MelonLoader;

namespace LunaR.MainLoader
{
    public class MainLoader : MelonMod
    {
        public override unsafe void OnApplicationStart() // Runs after Game Initialization.
        {
            LoaderClasses.OnStart();
        }

        public override void OnLevelWasLoaded(int level) // Runs when a Scene has Loaded.
        {
            LoaderClasses.LevelLoaded(level);
        }

        public override void OnLevelWasInitialized(int level) // Runs when a Scene has Initialized.
        {
            LoaderClasses.LevelInit(level);
        }

        public override void OnUpdate() // Runs once per frame.
        {
            LoaderClasses.OnUpdate();
        }

        public override void OnFixedUpdate() // Can run multiple times per frame. Mostly used for Physics.
        {
        }

        public override void OnLateUpdate() // Runs once per frame after OnUpdate and OnFixedUpdate have finished.
        {
        }

        public override void OnGUI()
        {
        }

        public override void OnApplicationQuit() // Runs when the Game is told to Close.
        {
            LoaderClasses.OnExit();
        }

        public override void OnModSettingsApplied() // Runs when Mod Preferences get saved to UserData/modprefs.ini.
        {
        }
    }
}