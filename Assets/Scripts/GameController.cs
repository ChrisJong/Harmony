using UnityEngine;
using System.Collections;
using Constants;
using Helpers;

using Grid;
using Sound;
using Player;
using AI;

[DisallowMultipleComponent]
public class GameController : MonoBehaviour {

    public static GameController instance;

    public GlobalValues.GameState gameState = GlobalValues.GameState.MENU;

    void Awake() {
        instance = this;

        if(UnityEditor.EditorApplication.currentScene == "Assets/Scenes/MainMenu/MainMenu.unity") {
            gameState = GlobalValues.GameState.MENU;
        } else if(Application.loadedLevelName == "MainMenu") {
            gameState = GlobalValues.GameState.MENU;
        } else {
            gameState = GlobalValues.GameState.INGAME;
        }

        MainCameraController.FindOrCreate();
        SoundController.FindOrCreate();
    }

    void Update() {
    }

    public void LoadLevel() {

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
