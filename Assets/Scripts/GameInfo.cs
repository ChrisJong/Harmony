// This Class will hold any block static values the game will have access to across all scripts (only place values/variables/rules here if the values never change).
namespace GameInfo {
    using System;
    using System.Collections.Generic;
    
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    using Maze;

    public static class MazeInfo {

        public static Dictionary<int, MazeData> MazeMoveValue;

        public static int MaxMazeLength = 48;

        public const string MazeName = "Maze";
        public static string PreviousMaze;
        public static string CurrentMaze;
        public static string NextMaze;
        public static int CurrentMazeNumber;
    }

    public static class GlobalInfo {
        public enum GameState {
            MENU,
            INGAME,
            PAUSED,
            NONE
        }

        public static int ScreenWidth = Screen.width;
        public static int ScreenHeight = Screen.height;

        public static bool GameDataLoaded = false;
    }

    public static class MainMenuInfo {
        public enum MenuTypes {
            MAINMENU = 0,
            NEWGAME = 1,
            LEVELSELECT = 2,
            INSTRUCTIONS = 3,
            CREDITS = 4,
            NEXT = 5,
            PREV = 6,
            EXIT = 7,
            NONE
        }
    }

    public static class GameMenuInfo {
        public enum ButtonTypes {
            RESTART,
            MAINMENU,
            UNDO,
            NEXTLEVEL,
            NONE
        }

        public const int GameMenuButtonWidth = 100;
        public const int GameMenuButtonHeight = 100;
        public static Rect RestartButtonRect = new Rect(GlobalInfo.ScreenWidth - (GameMenuButtonWidth + 30.0f), GlobalInfo.ScreenHeight - (GameMenuButtonHeight + 30.0f), GameMenuButtonWidth, GameMenuButtonHeight);
        public static Rect MainMenuButtonRect = new Rect(RestartButtonRect.x, RestartButtonRect.y - (GameMenuButtonHeight + 5.0f), GameMenuButtonWidth, GameMenuButtonHeight);
        public static Rect UndoButtonRect = new Rect(RestartButtonRect.x, 30.0f, GameMenuButtonWidth, GameMenuButtonHeight);

        public const int EndBillboardWidth = 450;
        public const int EndBillboardHeight = 225;
        public const int StartAnimationWidth = 100;
        public const int StartAnimationHeight = 100;
        public static Rect EndBillboardRect = new Rect(GlobalInfo.ScreenWidth * 0.5f - EndBillboardWidth * 0.5f, GlobalInfo.ScreenHeight * 0.6f - EndBillboardHeight * 0.5f, EndBillboardWidth, EndBillboardHeight);
        public static Rect EndRestartButtonRect = new Rect(EndBillboardRect.x + (GameMenuButtonWidth * 1.5f - 10.0f) * 0.5f, EndBillboardRect.y - (GameMenuButtonHeight + 50.0f), GameMenuButtonWidth, GameMenuButtonHeight);
        public static Rect EndMainMenuButtonRect = new Rect(EndRestartButtonRect.x + GameMenuButtonWidth + 5.0f, EndRestartButtonRect.y, GameMenuButtonWidth, GameMenuButtonHeight);
        public static Rect EndNextLevelButtonRect = new Rect(EndMainMenuButtonRect.x + GameMenuButtonWidth + 5.0f, EndRestartButtonRect.y, GameMenuButtonWidth, GameMenuButtonHeight);
        public static Rect StarAnimationRect = new Rect(EndBillboardRect.x + ((EndBillboardWidth * 0.5f) - (StartAnimationWidth * 0.5f)), EndBillboardRect.y + 10.0f, StartAnimationWidth, StartAnimationHeight);
    }

    public static class AssetPaths {
        public const string PathPrefab = "Assets/Prefabs/";
        public const string PathPrefabBlocks = PathPrefab + "Blocks/";
        public const string PathPrefabPlayer = PathPrefab + "Players/";
        public const string PathPrefabGameMenu = PathPrefab + "GameMenu/";
        public const string PathPrefabMainMenu = PathPrefab + "MainMenu/";
        public const string PathPrefabMisc = PathPrefab + "Misc/";
        
        public const string PathTextures = "Asset/Textures/";
        public const string PathTexturesGameMenu = PathTextures + "GameMenu/";

        public const string EmptyBlockName = "EmptyBlock";
        public const string NormalBlockName = "NormalBlock";
        public const string MultiBlockName = "MultiBlock";
        public const string NumberBlockName = "NumberBlock";
        public const string StunBlockName = "StunBlock";
        public const string LevelNumberBlockName = "LevelNumberBlock";

        public const string PlayerName = "Player";
        public const string AIName = "AI";

        public const string GameControllerName = "GameController";
        public const string GridControllerName = "GridController";
        public const string GameMenuControllerName = "GameMenuController";
        public const string SoundControllerName = "SoundController";
    }

    public static class PlayerInfo {
        public enum MovementDirection {
            NONE = 0,
            FORWARD = 1,
            RIGHT = 2,
            BACKWARD = 3,
            LEFT = 4
        };


        public const float MaxMoveSpeed = 20.0f;
        public const float MinMoveSpeed = 5.0f;
        public const float Gravity = 1000.0f;
        public const float TerminalVelocity = 20.0f;
    }

    public static class BlockInfo {
        public enum BlockTypes {
            NONE = 0,
            EMPTY = 1,
            NORMAL = 2,
            MULTI = 3,
            NUMBER = 4,
            STUN = 5
            
        };

        public enum BlockDirection {
            NONE = 0,
            UP = 1,
            RIGHT = 2,
            DOWN = 3,
            LEFT = 4
        }

        public enum BlockState {
            NONE = 0,
            UP = 1,
            DOWN = 2
        };

        public const float BlockWidth = 1.0f;
        public const float BlockBreadth = 1.0f;
        public const float BlockHeight = 1.0f;
    }

    public static class CameraInfo {
        public static Color BackgroundColor = Color.black;
        public const float MinFOV = 60.0f;
        public const float MaxFOV = 170.0f;
        public const float FOVSpeed = 2.0f;
        public const float FOVMultiplier = 10.0f;
        public static Quaternion CameraRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);

        public const float SkyboxRotationSpeed = 0.75f;
    }

    public static class SoundInfo {
        public const string BlockUp = "blockup";
        public const string BlockDown = "blockdown";
        public const string PlayerCollision = "playercollision";
        public const string PlayerMovement = "playermovement";
        public const string StarAnimation = "staranimation";
    }
}