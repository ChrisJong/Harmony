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

        [MenuItem("Build Tools/Rebuild Mazes And Data")]
        static void Init() {
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
                    //Debug.Log(MazeInfo.MazeName + (i + 1) + ": " + obj.moves + " Max Moves");
                    //Debug.Log(MazeInfo.MazeName + (i + 1) + ": " + MazeInfo.MazeMoveValue[i][0] + " / " + MazeInfo.MazeMoveValue[i][1]);
                } else {
                    MazeInfo.MazeMoveValue.Add(i, new MazeData {
                        moveCount = -1,
                        maxMoves = obj.maxMoves
                    });
                    //Debug.Log(MazeInfo.MazeName + (i + 1) + ": " + obj.moves + " Max Moves");
                    //Debug.Log(MazeInfo.MazeName + (i + 1) + ": " + MazeInfo.MazeMoveValue[i][0] + " / " + MazeInfo.MazeMoveValue[i][1]);
                }
            }
            MazeDataHelper.SaveData();
            EditorBuildSettings.scenes = mazeList.ToArray();
            Debug.Log("REBUILDING OF DATA AND MAZES FINISHED! CONTINUE ON WHATEVER YOU WERE DOING.");
        }
    }
}