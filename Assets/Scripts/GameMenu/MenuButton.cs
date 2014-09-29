namespace GameMenu {

    using System.Collections;
    using UnityEngine;

    using Constants;

    public class MenuButton : MonoBehaviour {

        public Texture2D buttonEnter;
        public Texture2D buttonExit;
        public GameMenu.ButtonTypes buttonType;

        private GUITexture _objectTexture;

        void Awake() {
            this._objectTexture = this.transform.guiTexture;
            this.transform.position = new Vector3(0.0f, 0.0f, 1.0f);
            this._objectTexture.texture = this.buttonExit;

            switch(this.buttonType) {
                case GameMenu.ButtonTypes.RESTART:
                this._objectTexture.pixelInset = GameMenu.RestartButtonRect;
                break;

                case GameMenu.ButtonTypes.MAINMENU:
                this._objectTexture.pixelInset = GameMenu.MainMenuButtonRect;
                break;

                case GameMenu.ButtonTypes.QUIT:
                this._objectTexture.pixelInset = GameMenu.QuitButtonRect;
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
                case GameMenu.ButtonTypes.RESTART:
                Application.LoadLevel(Application.loadedLevelName);
                break;

                case GameMenu.ButtonTypes.MAINMENU:
                GameController.instance.gameState = GlobalValues.GameState.MENU;
                Debug.Log(GameController.instance.gameState.ToString());
                Application.LoadLevel(0);
                break;

                case GameMenu.ButtonTypes.QUIT:
                UnityEditor.EditorApplication.isPlaying = false;
                Application.Quit();
                break;
            }
        }
    }
}