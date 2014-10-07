﻿// This Class will hold any block static values the game will have access to across all scripts (only place values/variables/rules here if the values never change).
namespace GameInfo {
    using System;
    using System.Collections.Generic;
    
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public static class MazeInfo {
        public static Dictionary<int, List<int>> MazeMoveValue = new Dictionary<int, List<int>>() {
            // First Value Is The Maze Number (0 is Maze 1 and so on).
            // Second Number Is The Number Of Moves The Player Has Made for that maze. 0 is a new game / unlocked content. -1 is locked content(will be implemented).
            // Thrid Number Is The Max Number Of Moves For That Stage.
            {0, new List<int>{0, 3}},
            {1, new List<int>{-1, 3}},
            {2, new List<int>{-1, 4}},
            {3, new List<int>{-1, 6}},
            {4, new List<int>{-1, 3}},
            {5, new List<int>{-1, 4}},
            {6, new List<int>{-1, 6}},
            {7, new List<int>{-1, 5}},
            {8, new List<int>{-1, 5}},
            {9, new List<int>{-1, 6}},
            {10, new List<int>{-1, 5}},
            {11, new List<int>{-1, 7}},
            {12, new List<int>{-1, 6}},
            {13, new List<int>{-1, 6}},
            {14, new List<int>{-1, 7}},
            {15, new List<int>{-1, 5}},
            {16, new List<int>{-1, 7}},
            {17, new List<int>{-1, 6}},
            {18, new List<int>{-1, 6}},
            {19, new List<int>{-1, 7}},
            {20, new List<int>{-1, 10}},
            {21, new List<int>{-1, 7}},
            {22, new List<int>{-1, 8}}
        };

        public const string MazeName = "Maze";
        public static string PreviousMaze;
        public static string CurrentMaze;
        public static string NextMaze;
        public static int CurrentMazeNumber;
        public static int MaxMazeCount = Application.levelCount - 1;
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
    }

    public static class MainMenuInfo {
        public enum MenuTypes {
            MAINMENU = 0,
            NEWGAME = 1,
            LEVELSELECT = 2,
            INSTRUCTIONS = 3,
            CREDITS = 4, 
            NONE
        }
    }

    public static class GameMenuInfo {
        public enum ButtonTypes {
            RESTART,
            MAINMENU,
            UNDO,
            NONE
        }

        public const int GameMenuButtonWidth = 75;
        public const int GameMenuButtonHeight = 75;
        public static Rect RestartButtonRect = new Rect(GlobalInfo.ScreenWidth - (GameMenuButtonWidth + 5), GlobalInfo.ScreenHeight - (GameMenuButtonHeight + 5), GameMenuButtonWidth, GameMenuButtonHeight);
        public static Rect MainMenuButtonRect = new Rect(RestartButtonRect.x, RestartButtonRect.y - (GameMenuButtonHeight + 5), GameMenuButtonWidth, GameMenuButtonHeight);
        public static Rect UndoButtonRect = new Rect(RestartButtonRect.x, 0.0f + 5.0f, GameMenuButtonWidth, GameMenuButtonHeight);
        //public static Vector3 MenuVector = new Vector3((GlobalInfo.ScreenWidth * 0.98f) / GlobalInfo.ScreenWidth, (GlobalInfo.ScreenHeight * 0.98f) / GlobalInfo.ScreenHeight, 1.0f);
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

        public const string UpBlockName = "UpBlock";
        public const string RightBlockName = "RightBlock";
        public const string DownBlockName = "DownBlock";
        public const string LeftBlockName = "LeftBlock";
        public const string EmptyBlockName = "EmptyBlock";
        public const string MultiLeftRightBlockName = "MultiLeftRightBlock";
        public const string MultiUpDownBlockName = "MultiUpDownBlock";
        public const string InvisibleBlockName = "InvisibleBlock";
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
            FORWARD,
            RIGHT,
            BACKWARD,
            LEFT
        };


        public const float MaxMoveSpeed = 20.0f;
        public const float MinMoveSpeed = 5.0f;
        public const float Gravity = 1000.0f;
        public const float TerminalVelocity = 20.0f;
    }

    public static class BlockInfo {
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

    public static class CameraInfo {
        public static Color BackgroundColor = Color.black;
        public const float MinFOV = 60.0f;
        public const float MaxFOV = 170.0f;
        public const float FOVSpeed = 2.0f;
        public const float FOVMultiplier = 10.0f;
        public static Quaternion CameraRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
    }

    public static class SoundInfo {
        public const string BlockUp = "blockup";
        public const string BlockDown = "blockdown";
        public const string PlayerCollision = "playercollision";
        public const string PlayerMovement = "playermovement";
    }
}