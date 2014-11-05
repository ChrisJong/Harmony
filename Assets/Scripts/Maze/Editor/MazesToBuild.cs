namespace Maze {

    using System.IO;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEditor;

    using Grid;
    using Helpers;
    using Maze;
    using GameInfo;

    public class MazesToBuild : EditorWindow {

        [MenuItem("Rebuild Tools/Mazes And Data (Normal)")]
        static void RebuildNormal() {
            DeleteSaveData();
            List<EditorBuildSettingsScene> mazeList = new List<EditorBuildSettingsScene>();

            string mainFolder = "Assets/Scenes/";
            string mainMazeFolder = mainFolder + "Mazes/";
            string sceneType = ".unity";
            EditorBuildSettingsScene mainMenuToAdd = new EditorBuildSettingsScene(mainFolder + "MainMenu/" + "MainMenu" + sceneType, true);
            mazeList.Add(mainMenuToAdd);
            MazeInfo.MazeMoveValue = new Dictionary<int, MazeData>();
            for(int i = 0; i < MazeInfo.MaxMazeLength; i++) {
                EditorBuildSettingsScene mazeToAdd = new EditorBuildSettingsScene(mainMazeFolder + MazeInfo.MazeName + (i + 1) + sceneType, true);
                mazeList.Add(mazeToAdd);

                EditorApplication.OpenScene(mazeToAdd.path);

                GridMap obj = GameObject.FindWithTag("GridMap").GetComponent<GridMap>() as GridMap;
                if(i == 0) {
                    MazeInfo.MazeMoveValue.Add(i, new MazeData {
                        moveCount = 0,
                        maxMoves = obj.maxMoves
                    });
                } else {
                    MazeInfo.MazeMoveValue.Add(i, new MazeData {
                        moveCount = -1,
                        maxMoves = obj.maxMoves
                    });
                }
            }
            MazeDataHelper.SaveData();
            EditorBuildSettings.scenes = mazeList.ToArray();
            Debug.Log("REBUILDING OF DATA AND MAZES FINISHED! CONTINUE ON WHATEVER YOU WERE DOING.");
        }

        [MenuItem("Rebuild Tools/Mazes And Data (Unlock Everything)")]
        static void RebuildUnlock() {
            DeleteSaveData();
            List<EditorBuildSettingsScene> mazeList = new List<EditorBuildSettingsScene>();

            string mainFolder = "Assets/Scenes/";
            string mainMazeFolder = mainFolder + "Mazes/";
            string sceneType = ".unity";
            EditorBuildSettingsScene mainMenuToAdd = new EditorBuildSettingsScene(mainFolder + "MainMenu/" + "MainMenu" + sceneType, true);
            mazeList.Add(mainMenuToAdd);
            MazeInfo.MazeMoveValue = new Dictionary<int, MazeData>();
            for(int i = 0; i < MazeInfo.MaxMazeLength; i++) {
                EditorBuildSettingsScene mazeToAdd = new EditorBuildSettingsScene(mainMazeFolder + MazeInfo.MazeName + (i + 1) + sceneType, true);
                mazeList.Add(mazeToAdd);

                EditorApplication.OpenScene(mazeToAdd.path);

                GridMap obj = GameObject.FindWithTag("GridMap").GetComponent<GridMap>() as GridMap;
                MazeInfo.MazeMoveValue.Add(i, new MazeData {
                    moveCount = 0,
                    maxMoves = obj.maxMoves
                });
            }
            MazeDataHelper.SaveData();
            EditorBuildSettings.scenes = mazeList.ToArray();
            Debug.Log("REBUILDING OF DATA AND MAZES FINISHED! CONTINUE ON WHATEVER YOU WERE DOING.");
        }

        static void DeleteSaveData() {
            if(File.Exists(Application.persistentDataPath + "/GameData.csv")) {
#if UNITY_IPHONE
                File.Delete("/private" + Application.persistentDataPath + "/GameData.csv");
#else
                File.Delete(Application.persistentDataPath + "/GameData.csv");
#endif
            }
        }
    }
}