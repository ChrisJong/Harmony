using UnityEngine;
using System.Collections;
using Constants;
using Helpers;

public class GameController : MonoBehaviour {

    void Awake() {
        MainCameraController.FindOrCreate();
    }

    public static void FindOrCreate() {
        GameObject tempController = GameObject.FindGameObjectWithTag("GameController");

        if(tempController == null) {
            tempController = AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabMisc, AssetPaths.GameControllerName);
            Instantiate(tempController);
            return;
        } else {
            return;
        }
    }
}
