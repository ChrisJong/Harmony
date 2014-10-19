namespace GameMenu {

    using System.Collections;
    using UnityEngine;

    using GameInfo;
    using Player;
    using AI;

    public class GameMenuButton : MonoBehaviour {

        public Texture2D buttonEnter;
        public Texture2D buttonExit;
        public GameMenuInfo.ButtonTypes buttonType;

        private GUITexture _objectTexture;

        void Awake() {
            this._objectTexture = this.transform.guiTexture;
            this.transform.position = new Vector3(0.0f, 0.0f, 1.0f);
            this._objectTexture.texture = this.buttonExit;

            if(GameController.instance.isStageFinished) {
                switch(this.buttonType) {
                    case GameMenuInfo.ButtonTypes.RESTART:
                        this._objectTexture.pixelInset = GameMenuInfo.EndRestartButtonRect;
                        break;

                    case GameMenuInfo.ButtonTypes.MAINMENU:
                        this._objectTexture.pixelInset = GameMenuInfo.EndMainMenuButtonRect;
                        break;

                    case GameMenuInfo.ButtonTypes.NEXTLEVEL:
                        this._objectTexture.pixelInset = GameMenuInfo.EndNextLevelButtonRect;
                        break;
                }
            } else {
                switch(this.buttonType) {
                    case GameMenuInfo.ButtonTypes.RESTART:
                        this._objectTexture.pixelInset = GameMenuInfo.RestartButtonRect;
                        break;

                    case GameMenuInfo.ButtonTypes.MAINMENU:
                        this._objectTexture.pixelInset = GameMenuInfo.MainMenuButtonRect;
                        break;

                    case GameMenuInfo.ButtonTypes.UNDO:
                        this.transform.gameObject.SetActive(false);
                        this._objectTexture.pixelInset = GameMenuInfo.UndoButtonRect;
                        break;
                }
            }
        }

        private void OnMouseEnter() {
            this._objectTexture.texture = this.buttonEnter;
        }

        private void OnMouseExit() {
            this._objectTexture.texture = this.buttonExit;
        }

        private void OnMouseUp(){
            switch(this.buttonType) {
                case GameMenuInfo.ButtonTypes.RESTART:
                    this._objectTexture.texture = this.buttonExit;
                    Application.LoadLevel(Application.loadedLevelName);
                    break;

                case GameMenuInfo.ButtonTypes.MAINMENU:
                    this._objectTexture.texture = this.buttonExit;
                    GameController.instance.gameState = GlobalInfo.GameState.MENU;
                    Object.DestroyImmediate(Sound.SoundController.instance.gameObject);
                    Application.LoadLevel("MainMenu");
                    break;

                case GameMenuInfo.ButtonTypes.UNDO:
                    this._objectTexture.texture = this.buttonExit;
                    PlayerController.instance.UndoMovement();
                    AIController.instance.UndoMovement();
                    this.transform.gameObject.SetActive(false);
                    break;

                case GameMenuInfo.ButtonTypes.NEXTLEVEL:
                    GameController.instance.LoadNextLevel();
                    break;
            }
        }
    }
}