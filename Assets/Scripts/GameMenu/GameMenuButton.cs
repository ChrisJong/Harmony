namespace GameMenu {

    using System.Collections;
    using UnityEngine;

    using GameInfo;
    using Player;
    using AI;
    using Input;

    public class GameMenuButton : MonoBehaviour {

        public Texture2D buttonEnter;
        public Texture2D buttonExit;
        public GameMenuInfo.ButtonTypes buttonType;

        private GUITexture _objectTexture;
        public static int currentTouchID = 0;
        private int touchID = 64;

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

#if UNITY_IPHONE || UNITY_ANDROID
        void Update() {
            if(Input.touchCount > 0) {
                foreach(Touch touch in Input.touches) {
                    currentTouchID = touch.fingerId;
                    if(this._objectTexture != null && (this._objectTexture.HitTest(touch.position))) {
                        if(touch.phase == TouchPhase.Began) {
                            this.OnTouchBegan();
                            this.touchID = currentTouchID;
                        }
                        if(touch.phase == TouchPhase.Ended) {
                            this.OnTouchEnded();
                        }
                        if(touch.phase == TouchPhase.Moved) {
                            this.OnTouchMoved();
                        }
                        if(touch.phase == TouchPhase.Stationary) {
                            this.OnTouchStayed();
                        }
                        if(touch.phase == TouchPhase.Canceled) {
                            this.OnTouchCanceled();
                        }
                    }

                    switch(touch.phase) {
                        case TouchPhase.Ended:
                        this.OnTouchEndedGlobal();
                        break;

                        case TouchPhase.Moved:
                        this.OnTouchMovedGlobal();
                        break;

                        case TouchPhase.Stationary:
                        this.OnTouchStayedGlobal();
                        break;

                        case TouchPhase.Canceled:
                        this.OnTouchCanceledGlobal();
                        break;
                    }
                }
            }
        }

        public void OnTouchBegan() {
            this._objectTexture.texture = this.buttonEnter;
        }

        public void OnTouchEnded() {
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

        public void OnTouchMoved() {
            this._objectTexture.texture = this.buttonExit;
        }

        public void OnTouchStayed() {
            this._objectTexture.texture = this.buttonEnter;
        }

        public void OnTouchCanceled() {
            this._objectTexture.texture = this.buttonExit;
        }

        public void OnTouchEndedGlobal() {
            this._objectTexture.texture = this.buttonExit;
        }

        public void OnTouchMovedGlobal() {
            this._objectTexture.texture = this.buttonEnter;
        }

        public void OnTouchStayedGlobal() {
            this._objectTexture.texture = this.buttonEnter;
        }

        public void OnTouchCanceledGlobal() {
            this._objectTexture.texture = this.buttonExit;
        }
#else

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
#endif
    }
}