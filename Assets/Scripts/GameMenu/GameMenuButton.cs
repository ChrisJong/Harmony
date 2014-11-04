namespace GameMenu {

    using System.Collections;
    using UnityEngine;

    using GameInfo;
    using Player;
    using AI;
    using Input;

    public class GameMenuButton : GUITouchInput {

        public Texture2D buttonEnter;
        public Texture2D buttonExit;
        public GameMenuInfo.ButtonTypes buttonType;

        public bool _disableTouch = false;
        private GUITexture _objectTexture;

        void Awake() {
#if UNITY_IPHONE || UNITY_ANDROID
            this._disableTouch = false;
#else
            this._disableTouch = true;
#endif

            this._objectTexture = this.transform.guiTexture;
            this.transform.position = new Vector3(0.0f, 0.0f, 0.1f);
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

        public override void Update() {
            if(this._disableTouch) {
                return;
            } else {
                base.Update();
            }
        }

#if UNITY_IPHONE || UNITY_ANDROID
        public override void OnNoTouch() {
            this._objectTexture.texture = this.buttonExit;
            GameMenuController.instance.buttonStateText.text = "";
        }

        public override void OnTouchBegan() {
            this._objectTexture.texture = this.buttonEnter;
            switch(this.buttonType) {
                case GameMenuInfo.ButtonTypes.RESTART:
                    GameMenuController.instance.buttonStateText.text = "RESTART";
                    break;

                case GameMenuInfo.ButtonTypes.MAINMENU:
                    GameMenuController.instance.buttonStateText.text = "MAIN MENU";
                    break;

                case GameMenuInfo.ButtonTypes.NEXTLEVEL:
                    GameMenuController.instance.buttonStateText.text = "NEXT LEVEL";
                    break;
            }
        }

        public override void OnTouchEnded() {
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
            GameMenuController.instance.buttonStateText.text = "";
        }

        public override void OnTouchMoved() {
            this._objectTexture.texture = this.buttonEnter;
        }

        public override void OnTouchStayed() {
            this._objectTexture.texture = this.buttonEnter;
            switch(this.buttonType) {
                case GameMenuInfo.ButtonTypes.RESTART:
                    GameMenuController.instance.buttonStateText.text = "RESTART";
                    break;

                case GameMenuInfo.ButtonTypes.MAINMENU:
                    GameMenuController.instance.buttonStateText.text = "MAIN MENU";
                    break;

                case GameMenuInfo.ButtonTypes.NEXTLEVEL:
                    GameMenuController.instance.buttonStateText.text = "NEXT LEVEL";
                    break;
            }
        }

        public override void OnTouchCanceled(){
            this._objectTexture.texture = this.buttonExit;
            GameMenuController.instance.buttonStateText.text = "";
        }
#else
        private void OnMouseEnter() {
            this._objectTexture.texture = this.buttonEnter;
            switch(this.buttonType) {
                case GameMenuInfo.ButtonTypes.RESTART:
                    GameMenuController.instance.buttonStateText.text = "RESTART";
                    break;

                case GameMenuInfo.ButtonTypes.MAINMENU:
                    GameMenuController.instance.buttonStateText.text = "MAIN MENU";
                    break;

                case GameMenuInfo.ButtonTypes.NEXTLEVEL:
                    GameMenuController.instance.buttonStateText.text = "NEXT LEVEL";
                    break;
            }
        }

        private void OnMouseExit() {
            this._objectTexture.texture = this.buttonExit;
            GameMenuController.instance.buttonStateText.text = "";
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
#endif
    }
}