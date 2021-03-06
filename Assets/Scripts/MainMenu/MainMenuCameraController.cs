﻿namespace MainMenu {

    using System.Collections.Generic;
    using System.Collections;

    using UnityEngine;

    using Input;
    using GameInfo;

    public class MainMenuCameraController : MonoBehaviour {

        private Vector3 _mainMenuPosition = new Vector3(0.0f, 0.0f, -10.0f);
        private Vector3 _newGamePosition = new Vector3(0.0f, 20.0f, 10.0f);
        private Vector3 _creditsPosition = new Vector3(-20.0f, 0.0f, -10.0f);
        private Vector3 _levelSelectPosition = new Vector3(0.0f, -20.0f, -10.0f);
        private Vector3 _levelSelectPositionOver = new Vector3(0.0f, -40.0f, -10.0f);
        private Vector3 _instructionsPosition = new Vector3(20.0f, 0.0f, -10.0f);

        private Transform _transform;

        private SwipeMenuInput _swipeMenuController;
        private GameObject touchCurrent;
        private GameObject touchOld;

        void Awake() {
            this._transform = this.transform;

#if UNITY_IPHONE || UNITY_ANDROID
            this.transform.gameObject.AddComponent<TouchInput>();
            this.transform.gameObject.AddComponent<SwipeMenuInput>();
            this._swipeMenuController = this.transform.GetComponent<SwipeMenuInput>() as SwipeMenuInput;
#endif
        }

        void Update() {
#if UNITY_IPHONE || UNITY_ANDROID
            this._swipeMenuController.GetInput();
#endif
            this.PanCamera();
        }

        private void PanCamera() {
            if(MainMenuController.instance.currentMenuScene == MainMenuInfo.MenuTypes.LEVELSELECT) {
                
                if(MainMenuController.instance.isActive) {
                    this._transform.position = Vector3.Lerp(this._transform.position, this._levelSelectPositionOver, Time.deltaTime * 2.0f);
                } else {
                    this._transform.position = Vector3.Lerp(this._transform.position, this._levelSelectPosition, Time.deltaTime * 6.0f);
                }
            } else if(MainMenuController.instance.currentMenuScene == MainMenuInfo.MenuTypes.INSTRUCTIONS)
                this._transform.position = Vector3.Lerp(this._transform.position, this._instructionsPosition, Time.deltaTime * 6.0f);
            else if(MainMenuController.instance.currentMenuScene == MainMenuInfo.MenuTypes.CREDITS)
                this._transform.position = Vector3.Lerp(this._transform.position, this._creditsPosition, Time.deltaTime * 6.0f);
            else if(MainMenuController.instance.currentMenuScene == MainMenuInfo.MenuTypes.MAINMENU)
                this._transform.position = Vector3.Lerp(this._transform.position, this._mainMenuPosition, Time.deltaTime * 6.0f);
            else if(MainMenuController.instance.currentMenuScene == MainMenuInfo.MenuTypes.NEWGAME) {
                this._transform.position = Vector3.Lerp(this._transform.position, this._newGamePosition, Time.deltaTime * 2.0f);
                if(!MainMenuController.instance.isActive) {
                    MainMenuController.instance.isActive = true;
                    MainMenuController.instance.fade.PlayFadeToMax();
                }
            }
        }
    }
}