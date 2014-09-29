// This Class will hold any block static values the game will have access to across all scripts (only place values/variables/rules here if the values never change).
namespace Constants {
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    public static class GlobalValues {
        public enum GameState {
            MENU,
            INGAME,
            PAUSED,
            NONE
        }

        public static int ScreenWidth = Screen.width;
        public static int ScreenHeight = Screen.height;
    }

    public static class GameMenu {

        public enum ButtonTypes {
            RESTART,
            MAINMENU,
            QUIT
        }

        public const int LogoWidth = 130;
        public const int LogoHeight = 130;
        public const int LogoFrameCount = 2;
        public const int GameMenuWidth = 260;
        public const int GameMenuHeight = 130;
        public static Rect GameMenuRect = new Rect(GlobalValues.ScreenWidth - (GameMenuWidth * 0.5f + 10), GlobalValues.ScreenHeight - (GameMenuHeight * 0.5f + 10), GameMenuWidth * 0.5f, GameMenuHeight * 0.5f);

        public const int MenuBGWidth = 400;
        public const int MenuBGHeight = 500;
        public static Rect MenuBGRect = new Rect((GlobalValues.ScreenWidth * 0.5f) - (MenuBGWidth * 0.5f), (GlobalValues.ScreenHeight * 0.5f) - (MenuBGHeight * 0.5f), MenuBGWidth, MenuBGHeight);

        public const int MenuButtonWidth = 300;
        public const int MenuButtonHeight = 100;
        public static Rect RestartButtonRect = new Rect(MenuBGRect.x + ((MenuBGWidth - MenuButtonWidth) * 0.5f), MenuBGRect.y + ((MenuBGHeight - MenuButtonHeight) * 0.9f), MenuButtonWidth, MenuButtonHeight);
        public static Rect MainMenuButtonRect = new Rect(MenuBGRect.x + ((MenuBGWidth - MenuButtonWidth) * 0.5f), MenuBGRect.y + ((MenuBGHeight - MenuButtonHeight) * 0.6f), MenuButtonWidth, MenuButtonHeight);
        public static Rect QuitButtonRect = new Rect(MenuBGRect.x + ((MenuBGWidth - MenuButtonWidth) * 0.5f), MenuBGRect.y + ((MenuBGHeight - MenuButtonHeight) * 0.3f), MenuButtonWidth, MenuButtonHeight);
    }

    public static class AssetPaths {
        public const string PathPrefab = "Assets/Prefabs/";
        public const string PathPrefabBlocks = PathPrefab + "Blocks/";
        public const string PathPrefabPlayer = PathPrefab + "Players/";
        public const string PathPrefabMisc = PathPrefab + "Misc/";
        
        public const string PathTextures = "Asset/Textures/";
        public const string PathTexturesGameMenu = PathTextures + "GameMenu/";

        public const string UpBlockName = "UpBlock";
        public const string RightBlockName = "RightBlock";
        public const string DownBlockName = "DownBlock";
        public const string LeftBlockName = "LeftBlock";
        public const string EmptyBlockName = "EmptyBlock";
        public const string MultiLeftRightBlockName = "MultiLeftRightBlock";
        public const string MultiUpDownBlockName = "MultiUpDownBlock";
        public const string InvisibleBlockName = "InvisibleBlock";

        public const string PlayerName = "Player";
        public const string AIName = "AI";

        public const string GameControllerName = "GameController";
        public const string GridControllerName = "GridController";
        public const string SoundControllerName = "SoundController";
    }

    public static class PlayerValues {
        public enum MovementDirection {
            NONE = 0,
            FORWARD,
            RIGHT,
            BACKWARD,
            LEFT
        };


        public const float MoveSpeed = 10.0f;
        public const float Gravity = 1000.0f;
        public const float TerminalVelocity = 20.0f;
    }

    public static class BlockValues {
        public enum BlockType {
            EMPTYUP = 0,
            EMPTYDOWN,
            UP,
            RIGHT,
            DOWN,
            LEFT,
            MULTILEFTRIGHT,
            MULTIUPDOWN
        };

        public enum BlockState {
            NONE = 0,
            UP,
            DOWN
        };

        public const float BlockWidth = 1.0f;
        public const float BlockBreadth = 1.0f;
        public const float BlockHeight = 1.0f;
    }

    public static class CameraValues {
        public static Color BackgroundColor = Color.black;
        public const float MinFOV = 60.0f;
        public const float MaxFOV = 170.0f;
        public const float FOVSpeed = 2.0f;
        public const float FOVMultiplier = 10.0f;
        public static Quaternion CameraRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
    }

    public static class SoundValues {
        public const string BlockUp = "blockup";
        public const string BlockDown = "blockdown";
        public const string BlockCollision = "blockcollision";
        public const string PlayerMovement = "playermovement";
    }

    public static class MenuValues {
        public enum MenuTypes {
            NEWGAME = 0,
            ABOUT,
            CREDITS,
            SOMETHING,
            MAINMENU
        }
    }
}