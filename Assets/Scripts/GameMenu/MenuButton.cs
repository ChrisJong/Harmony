namespace GameMenu {

    using System.Collections;
    using UnityEngine;

    using GameInfo;

    public class MenuButton : MonoBehaviour {

        public Texture2D buttonEnter;
        public Texture2D buttonExit;
        public GameMenuInfo.ButtonTypes buttonType;

        private GUITexture _objectTexture;

        void Awake() {
            this._objectTexture = this.transform.guiTexture;
            this.transform.position = new Vector3(0.0f, 0.0f, 1.0f);
            this._objectTexture.texture = this.buttonExit;

            switch(this.buttonType) {
                case GameMenuInfo.ButtonTypes.RESTART:
                this._objectTexture.pixelInset = GameMenuInfo.RestartButtonRect;
                break;

                case GameMenuInfo.ButtonTypes.MAINMENU:
                this._objectTexture.pixelInset = GameMenuInfo.MainMenuButtonRect;
                break;

                case GameMenuInfo.ButtonTypes.QUIT:
                this._objectTexture.pixelInset = GameMenuInfo.QuitButtonRect;
                break;
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
                Application.LoadLevel(Application.loadedLevelName);
                break;

                case GameMenuInfo.ButtonTypes.MAINMENU:
                GameController.instance.gameState = GlobalInfo.GameState.MENU;
                Object.DestroyImmediate(Sound.SoundController.instance.gameObject);
                Application.LoadLevel("MainMenu");
                break;

                case GameMenuInfo.ButtonTypes.QUIT:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
                break;
            }
        }
    }
}