using UnityEngine;
using System.Collections;
using Constants;
using Helpers;

using GridGenerator;
using Sound;
using Player;
using AI;

[DisallowMultipleComponent]
public class GameController : MonoBehaviour {

    public static GameController instance;

    private PlayerValues.MovementDirection _directionCurrent;
    private PlayerValues.MovementDirection _directionPrevious;

    void Awake() {
        instance = this;
        MainCameraController.FindOrCreate();
        SoundController.FindOrCreate();
    }

    void Update() {
        this.GetInput();
    }

    private void GetInput() {
        if(PlayerController.instance.isMoving || AIController.instance.isMoving)
            return;

        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            this._directionPrevious = this._directionCurrent;
            this._directionCurrent = PlayerValues.MovementDirection.FORWARD;
            PlayerController.instance.GetInput(this._directionCurrent, this._directionPrevious);
            AIController.instance.GetInput(this._directionCurrent, this._directionPrevious);
            GridController.instance.ActivateBlocks(this._directionCurrent, this._directionPrevious);
            SoundController.PlayerAudio(SoundValues.PlayerMovement);
        } else if(Input.GetKeyDown(KeyCode.RightArrow)) {
            this._directionPrevious = this._directionCurrent;
            this._directionCurrent = PlayerValues.MovementDirection.RIGHT;
            PlayerController.instance.GetInput(this._directionCurrent, this._directionPrevious);
            AIController.instance.GetInput(this._directionCurrent, this._directionPrevious);
            GridController.instance.ActivateBlocks(this._directionCurrent, this._directionPrevious);
            SoundController.PlayerAudio(SoundValues.PlayerMovement);
        } else if(Input.GetKeyDown(KeyCode.DownArrow)) {
            this._directionPrevious = this._directionCurrent;
            this._directionCurrent = PlayerValues.MovementDirection.BACKWARD;
            PlayerController.instance.GetInput(this._directionCurrent, this._directionPrevious);
            AIController.instance.GetInput(this._directionCurrent, this._directionPrevious);
            GridController.instance.ActivateBlocks(this._directionCurrent, this._directionPrevious);
            SoundController.PlayerAudio(SoundValues.PlayerMovement);
        } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            this._directionPrevious = this._directionCurrent;
            this._directionCurrent = PlayerValues.MovementDirection.LEFT;
            PlayerController.instance.GetInput(this._directionCurrent, this._directionPrevious);
            AIController.instance.GetInput(this._directionCurrent, this._directionPrevious);
            GridController.instance.ActivateBlocks(this._directionCurrent, this._directionPrevious);
            SoundController.PlayerAudio(SoundValues.PlayerMovement);
        }
        PlayerController.instance.CheckCurrentBlock();
        AIController.instance.CheckCurrentBlock();
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
