using Hexed.Core;
using Hexed.Modules.Standalone;
using Hexed.UIApi;
using Hexed.Wrappers;
using System.IO;

namespace Hexed.CustomUI.QM.BotMenus
{
    internal class PhysicsMenu
    {
        private static QMMenuPage PhysicsPage;
        public static void Init()
        {
            PhysicsPage = new("Bot Physics");

            QMSingleButton OpenMenu = new(BotMenu.BotsPage, 2, 1.5f, "Physics", PhysicsPage.OpenMe, "Bot Physic Options", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Physics"));

            new QMSingleButton(PhysicsPage, 0.75f, 0, "+", delegate
            {
                BotConnection.ChangePositionOffset("X", true);
            }, "Increase the X Offset", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(PhysicsPage, 1.25f, 0, "-", delegate
            {
                BotConnection.ChangePositionOffset("X", false);
            }, "Decrease the X Offset", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(PhysicsPage, 1, 0.5f, "X Position", delegate
            {
                BotConnection.ResetPositionOffset("X");
            }, "Reset the X Offset", ButtonAPI.ButtonSize.Half);


            new QMSingleButton(PhysicsPage, 1.75f, 0, "+", delegate
            {
                BotConnection.ChangePositionOffset("Y", true);
            }, "Increase the Y Offset", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(PhysicsPage, 2.25f, 0, "-", delegate
            {
                BotConnection.ChangePositionOffset("Y", false);
            }, "Decrease the Y Offset", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(PhysicsPage, 2, 0.5f, "Y Position", delegate
            {
                BotConnection.ResetPositionOffset("Y");
            }, "Reset the Y Offset", ButtonAPI.ButtonSize.Half);


            new QMSingleButton(PhysicsPage, 2.75f, 0, "+", delegate
            {
                BotConnection.ChangePositionOffset("Z", true);
            }, "Increase the Z Offset", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(PhysicsPage, 3.25f, 0, "-", delegate
            {
                BotConnection.ChangePositionOffset("Z", false);
            }, "Decrease the Z Offset", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(PhysicsPage, 3, 0.5f, "Z Position", delegate
            {
                BotConnection.ResetPositionOffset("Z");
            }, "Reset the Z Offset", ButtonAPI.ButtonSize.Half);


            new QMToggleButton(PhysicsPage, 4, 0, "Freeze \nPosition", delegate
            {
                BotConnection.FreezePosition(true);
            }, delegate
            {
                BotConnection.FreezePosition(false);
            }, "Freeze the current Position");


            new QMSingleButton(PhysicsPage, 0.75f, 1, "+", delegate
            {
                BotConnection.ChangeRotationOffset("X", true);
            }, "Increase the X Offset", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(PhysicsPage, 1.25f, 1, "-", delegate
            {
                BotConnection.ChangeRotationOffset("X", false);
            }, "Decrease the X Offset", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(PhysicsPage, 1, 1.5f, "X Rotation", delegate
            {
                BotConnection.ResetRotationOffset("X");
            }, "Reset the X Offset", ButtonAPI.ButtonSize.Half);


            new QMSingleButton(PhysicsPage, 1.75f, 1, "+", delegate
            {
                BotConnection.ChangeRotationOffset("Y", true);
            }, "Increase the Y Offset", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(PhysicsPage, 2.25f, 1, "-", delegate
            {
                BotConnection.ChangeRotationOffset("Y", false);
            }, "Decrease the Y Offset", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(PhysicsPage, 2, 1.5f, "Y Rotation", delegate
            {
                BotConnection.ResetRotationOffset("Y");
            }, "Reset the Y Offset", ButtonAPI.ButtonSize.Half);


            new QMSingleButton(PhysicsPage, 2.75f, 1, "+", delegate
            {
                BotConnection.ChangeRotationOffset("Z", true);
            }, "Increase the Z Offset", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(PhysicsPage, 3.25f, 1, "-", delegate
            {
                BotConnection.ChangeRotationOffset("Z", false);
            }, "Decrease the Z Offset", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(PhysicsPage, 3, 1.5f, "Z Rotation", delegate
            {
                BotConnection.ResetRotationOffset("Z");
            }, "Reset the Z Offset", ButtonAPI.ButtonSize.Half);


            new QMToggleButton(PhysicsPage, 4, 1, "Freeze \nRotation", delegate
            {
                BotConnection.FreezeRotation(true);
            }, delegate
            {
                BotConnection.FreezeRotation(false);
            }, "Freeze the current Rotation");


            new QMSingleButton(PhysicsPage, 0.75f, 2, "+", delegate
            {
                BotConnection.ChangeOrbitRange(true);
            }, "Increase the Orbit Range", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(PhysicsPage, 1.25f, 2, "-", delegate
            {
                BotConnection.ChangeOrbitRange(false);
            }, "Decrease the Orbit Range", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(PhysicsPage, 1, 2.5f, "Range", BotConnection.ResetOrbitRange, "Reset the Orbit Range", ButtonAPI.ButtonSize.Half);


            new QMSingleButton(PhysicsPage, 1.75f, 2, "+", delegate
            {
                BotConnection.ChangeOrbitSpeed(true);
            }, "Increase the Orbit Speed", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(PhysicsPage, 2.25f, 2, "-", delegate
            {
                BotConnection.ChangeOrbitSpeed(false);
            }, "Decrease the Orbit Speed", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(PhysicsPage, 2, 2.5f, "Speed", BotConnection.ResetOrbitSpeed, "Reset the Orbit Speed", ButtonAPI.ButtonSize.Half);


            new QMToggleButton(PhysicsPage, 3, 2, "AFK \nMode", delegate
            {
                BotConnection.AFKMode(true);
            }, delegate
            {
                BotConnection.AFKMode(false);
            }, "Switch the Bot to AFK Mode");

            new QMToggleButton(PhysicsPage, 4, 2, "Follow \nCamera", delegate
            {
                BotConnection.CameraFollowMode(true);
            }, delegate
            {
                BotConnection.CameraFollowMode(false);
            }, "Switch the Bot to follow the Camera");

            QMScrollButton MotionScroll = new(PhysicsPage, 1, 3, "Motion \nFiles", "Motion Files", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Options"));
            QMScrollButton DirScroll = new(PhysicsPage, 1, 3, "Motion Options", "Motion File Options");
            DirScroll.MainButton.GetGameObject().SetActive(false);

            MotionScroll.SetAction(delegate
            {
                foreach (string Dir in Directory.GetDirectories("Hexed\\Bots\\Movements"))
                {
                    string FolderName = Dir.Replace("Hexed\\Bots\\Movements\\", "");
                    MotionScroll.Add(FolderName, "Open the Subfolder", delegate
                    {
                        DirScroll.SetAction(delegate
                        {
                            foreach (string MotionFile in Directory.GetFiles($"Hexed\\Bots\\Movements\\{FolderName}"))
                            {
                                string Audio = MotionFile.Replace($"Hexed\\Bots\\Movements\\{FolderName}\\", "").Split('.')[0];
                                DirScroll.Add(Audio, "Play the Motion", delegate
                                {
                                    BotConnection.PlayMotionFile(MotionFile.Replace("Hexed\\Bots\\", ""));
                                });
                            }
                        });
                        DirScroll.MainButton.ClickMe();
                    });
                }

                foreach (string MotionFile in Directory.GetFiles("Hexed\\Bots\\Movements"))
                {
                    string Audio = MotionFile.Replace("Hexed\\Bots\\Movements\\", "").Split('.')[0];
                    MotionScroll.Add(Audio, "Play the Motion", delegate
                    {
                        BotConnection.PlayMotionFile(MotionFile.Replace("Hexed\\Bots\\", ""));
                    });
                }
            });

            new QMToggleButton(PhysicsPage, 2, 3, "Loop \nReplay", delegate
            {
                BotConnection.LoopAnimationRecord(true);
            }, delegate
            {
                BotConnection.LoopAnimationRecord(false);
            }, "Loop Motion Records");

            new QMSingleButton(PhysicsPage, 3, 3, "Stop \nMotion", BotConnection.StopMotionReplay, "Stop the currently played Motion", ButtonAPI.ButtonSize.Half);
        }
    }
}
