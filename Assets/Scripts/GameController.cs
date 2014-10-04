using System.IO;
using System.Collections.Generic;

using UnityEngine;

using Grid;
using Sound;
using Player;
using AI;
using GameInfo;
using Helpers;

[DisallowMultipleComponent]
public class GameController : MonoBehaviour {

    public static GameController instance;

    public GlobalInfo.GameState gameState = GlobalInfo.GameState.MENU;

    void Awake() {
        instance = this;
        
        if(Application.loadedLevelName == "MainMenu") {
            gameState = GlobalInfo.GameState.MENU;
        } else {
            gameState = GlobalInfo.GameState.INGAME;
        }

        MainCameraController.FindOrCreate();
        SoundController.FindOrCreate();
    }

    void Update() {

    }

    public void PrepareNextLevel() {
        if(MazeInfo.CurrentMaze == null)
            MazeInfo.CurrentMaze = Application.loadedLevelName;

        string[] maze = MazeInfo.CurrentMaze.Split(new string[] { MazeInfo.MazeName, "level", "Level" }, System.StringSplitOptions.RemoveEmptyEntries);
        int mazeNumber;
        if(int.TryParse(maze[0], out mazeNumber)) {
            MazeInfo.CurrentMazeNumber = mazeNumber;
            mazeNumber++;
            if(mazeNumber > MazeInfo.MaxMazeCount)
                MazeInfo.NextMaze = "MainMenu";
            else
                MazeInfo.NextMaze = MazeInfo.MazeName + mazeNumber.ToString();
        }
    }

    public void LoadLevelAt(int mazeNumber) {
        MazeInfo.CurrentMaze = MazeInfo.MazeName + mazeNumber.ToString();
        Application.LoadLevel(MazeInfo.CurrentMaze);
    }

    public void LoadNextLevel() {
        /*currentScene = Application.loadedLevelName;
        string[] words;
        words = currentScene.Split(new string[] { MazeInfo.MazeName }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach(string s in words)
            Debug.Log(s);


        int number;
        if(int.TryParse(words[0], out number)){
            Debug.Log(number);
        }
        number++;
        nextScene = MazeInfo.MazeName + number.ToString();*/
        MazeInfo.MazeMoveValue[MazeInfo.CurrentMazeNumber - 1][0] = GridController.instance.MoveCount;
        MazeInfo.CurrentMaze = MazeInfo.NextMaze;
        Debug.Log(MazeInfo.NextMaze);
        Application.LoadLevel(MazeInfo.NextMaze);
    }

    public static void FindOrCreate() {
        GameObject tempController = GameObject.FindGameObjectWithTag("GameController");

        if(tempController == null) {
            tempController = AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabMisc, AssetPaths.GameControllerName);
            Instantiate(tempController).name = AssetPaths.GameControllerName;
            return;
        } else {
            return;
        }
    }
}
