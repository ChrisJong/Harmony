namespace MainMenu {

    using System.Collections;
    using UnityEngine;

    using Input;
    using Helpers;
    using GameInfo;

#if UNITY_IPHONE || UNITY_ANDROID
    public class MainMenuButton : MonoBehaviour, ITouchable {
#else
    public class MainMenuButton : MonoBehaviour {
#endif
        public Material buttonEnter;
        public Material buttonExit;
        public MainMenuInfo.MenuTypes buttonType;

        private MeshRenderer _renderer;

        void Awake() {
            this._renderer = this.transform.GetComponent<MeshRenderer>() as MeshRenderer;
            this._renderer.material = this.buttonExit;
        }

#if UNITY_IPHONE || UNITY_ANDROID
        public void OnTouchBegan() {
            this._renderer.material = this.buttonEnter;
        }

        public void OnTouchEnded() {
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

                case MainMenuInfo.MenuTypes.PREV:
                if(LevelSelectController.instance.currentPage <= 0) {
                    LevelSelectController.instance.currentPage = 0;
                } else {
                    LevelSelectController.instance.currentPage -= 1;
                }
                this._renderer.material = this.buttonExit;
                break;

                case MainMenuInfo.MenuTypes.NEXT:
                if(LevelSelectController.instance.currentPage >= LevelSelectController.instance.totalPages) {
                    LevelSelectController.instance.currentPage += 1;
                } else {
                    LevelSelectController.instance.currentPage = 1;
                }
                this._renderer.material = this.buttonExit;
                break;

                case MainMenuInfo.MenuTypes.EXIT:
                MazeDataHelper.SaveGameData();
                Application.Quit();
                break;
            }
        }

        public void OnTouchMoved() {
            this._renderer.material = this.buttonExit;
        }

        public void OnTouchStayed() {
            this._renderer.material = this.buttonEnter;
        }

        public void OnTouchCanceled() {
            this._renderer.material = this.buttonExit;
        }

        public void OnTouchEndedGlobal() {
            this._renderer.material = this.buttonExit;
        }

        public void OnTouchMovedGlobal() {
            this._renderer.material = this.buttonExit;
        }

        public void OnTouchStayedGlobal() {
            this._renderer.material = this.buttonEnter;
        }

        public void OnTouchCanceledGlobal() {
            this._renderer.material = this.buttonExit;
        }
#else
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

                case MainMenuInfo.MenuTypes.PREV:
                    if(LevelSelectController.instance.currentPage <= 0) {
                        LevelSelectController.instance.currentPage = 0;
                    } else {
                        LevelSelectController.instance.currentPage -= 1;
                    }
                    break;

                case MainMenuInfo.MenuTypes.NEXT:
                    if(LevelSelectController.instance.currentPage >= LevelSelectController.instance.totalPages) {
                        LevelSelectController.instance.currentPage += 1;
                    } else {
                        LevelSelectController.instance.currentPage = 1;
                    }
                    break;

                case MainMenuInfo.MenuTypes.EXIT:
                    MazeDataHelper.SaveGameData();
                    Application.Quit();
                    break;
            }
        }
#endif
    }
}
