// This Class will hold any block static values the game will have access to across all scripts (only place values/variables/rules here if the values never change).
namespace Constants {
    using System;
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;

    public static class AssetPaths {
        public const string PathPrefab = "Assets/Prefabs/";
        public const string PathPrefabBlocks = PathPrefab + "Blocks/";
        public const string PathPrefabPlayer = PathPrefab + "Players/";
        public const string PathPrefabMisc = PathPrefab + "Misc/";

        public const string UpBlockName = "UpBlock";
        public const string RightBlockName = "RightBlock";
        public const string DownBlockName = "DownBlock";
        public const string LeftBlockName = "LeftBlock";
        public const string EmptyBlockName = "EmptyBlock";
        public const string SpawnBlockHumanName = "SpawnHumanBlock";
        public const string SpawnBlockAIName = "SpawnAIBlock";
        public const string InvisibleBlockName = "InvisibleBlock";

        public const string PlayerName = "Player";
        public const string AIName = "AI";

        public const string GameControllerName = "GameController";
    }

    public static class PlayerValues {
        public enum PlayerDirection {
            NONE = 0,
            FORWARD,
            RIGHT,
            BACKWARD,
            LEFT
        };
    }

    public static class BlockValues {
        /// <summary>
        /// A Enum that holds the type of blocks the game will have. this will help us keep track of what blocks we have available.
        /// </summary>
        public enum BlockType {
            EMPTYUP = 0,
            EMPTYDOWN,
            UP,
            RIGHT,
            DOWN,
            LEFT,
            HUMAN,
            AI
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
        public const float minFOW = 60.0f;
        public const float maxFOX = 150.0f;
        public const float FOVMultiplier = 10.0f;
        public static Quaternion CameraRotation = Quaternion.Euler(75.0f, 0.0f, 0.0f);
    }
}