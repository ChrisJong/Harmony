namespace MainMenu {

    using System.Collections.Generic;
    using System.Collections;

    using UnityEngine;

    using Input;
    using GameInfo;

    public class MainMenuCameraController : MonoBehaviour {

        private Vector3 _mainMenuPosition = new Vector3(0.0f, 0.0f, -10.0f);
        public Vector3 _newGamePosition = new Vector3(0.0f, 20.0f, 10.0f);
        private Vector3 _creditsPosition = new Vector3(-20.0f, 0.0f, -10.0f);
        private Vector3 _levelSelectPosition = new Vector3(0.0f, -20.0f, -10.0f);
        private Vector3 _instructionsPosition = new Vector3(20.0f, 0.0f, -10.0f);

        private Transform _transform;

        private GameObject touchCurrent;
        private GameObject touchOld;

        void Awake() {
            this._transform = this.transform;

#if UNITY_IPHONE || UNITY_ANDROID
            this.transform.gameObject.AddComponent<TouchInput>();
#endif
        }

        void LateUpdate() {
            this.PanCamera();
        }

        private void PanCamera() {
            if(MainMenuController.instance.currentMenuScene == MainMenuInfo.MenuTypes.LEVELSELECT)
                this._transform.position = Vector3.Lerp(this._transform.position, this._levelSelectPosition, Time.deltaTime * 6.0f);
            else if(MainMenuController.instance.currentMenuScene == MainMenuInfo.MenuTypes.INSTRUCTIONS)
                this._transform.position = Vector3.Lerp(this._transform.position, this._instructionsPosition, Time.deltaTime * 6.0f);
            else if(MainMenuController.instance.currentMenuScene == MainMenuInfo.MenuTypes.CREDITS)
                this._transform.position = Vector3.Lerp(this._transform.position, this._creditsPosition, Time.deltaTime * 6.0f);
            else if(MainMenuController.instance.currentMenuScene == MainMenuInfo.MenuTypes.MAINMENU)
                this._transform.position = Vector3.Lerp(this._transform.position, this._mainMenuPosition, Time.deltaTime * 6.0f);
            else if(MainMenuController.instance.currentMenuScene == MainMenuInfo.MenuTypes.NEWGAME) {
                this._transform.position = Vector3.Lerp(this._transform.position, this._newGamePosition, Time.deltaTime * 2.0f);
                if(this._transform.position.y > (this._newGamePosition.y - 0.1f)) {
                    GameController.instance.gameState = GlobalInfo.GameState.INGAME;
                    GameController.instance.LoadLevelAt(1);
                }
            }
        }
    }
}