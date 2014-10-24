namespace MainMenu {

    using System.Collections;
    using UnityEngine;

    using GameInfo;

    public class MainMenuButton : MonoBehaviour {

        public Material buttonEnter;
        public Material buttonExit;
        public MainMenuInfo.MenuTypes buttonType;

        private MeshRenderer _renderer;

        void Awake() {
            this._renderer = this.transform.GetComponent<MeshRenderer>() as MeshRenderer;
            this._renderer.material = this.buttonExit;
        }

        private void OnMouseEnter() {
            this._renderer.material = this.buttonEnter;
        }

        private void OnMouseExit() {
            this._renderer.material = this.buttonExit;
        }

        private void OnMouseUp() {
            switch(buttonType) {
                case MainMenuInfo.MenuTypes.NEWGAME:
                    this._renderer.material = this.buttonExit;
                    MainMenuController.instance.currentMenuScene = buttonType;
                    break;

                case MainMenuInfo.MenuTypes.INSTRUCTIONS:
                    this._renderer.material = this.buttonExit;
                    MainMenuController.instance.currentMenuScene = buttonType;
                    break;

                case MainMenuInfo.MenuTypes.LEVELSELECT:
                    this._renderer.material = this.buttonExit;
                    MainMenuController.instance.currentMenuScene = buttonType;
                    break;

                case MainMenuInfo.MenuTypes.CREDITS:
                    this._renderer.material = this.buttonExit;
                    MainMenuController.instance.currentMenuScene = buttonType;
                    break;

                case MainMenuInfo.MenuTypes.MAINMENU:
                    this._renderer.material = this.buttonExit;
                    MainMenuController.instance.currentMenuScene = buttonType;
                    break;
            }
        }
    }
}
