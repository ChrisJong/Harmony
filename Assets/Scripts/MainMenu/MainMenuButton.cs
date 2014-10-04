namespace MainMenu {

    using System.Collections;
    using UnityEngine;

    using GameInfo;

    public class MainMenuButton : MonoBehaviour {

        public MainMenuInfo.MenuTypes buttonType;

        public GameObject text;

        void Awake() {
            if(text != null)
                this.text.SetActive(false);
        }

        private void OnMouseEnter() {
            if(text != null)
                text.SetActive(true);
        }

        private void OnMouseExit() {
            if(text != null)
                text.SetActive(false);
        }

        private void OnMouseUp() {
            switch(buttonType) {
                case MainMenuInfo.MenuTypes.NEWGAME:
                text.SetActive(false);
                GameController.instance.gameState = GlobalInfo.GameState.INGAME;
                GameController.instance.LoadLevelAt(1);
                break;

                case MainMenuInfo.MenuTypes.INSTRUCTIONS:
                MainMenuController.instance.SetMenuScreen(MainMenuInfo.MenuTypes.INSTRUCTIONS);
                text.SetActive(false);
                break;

                case MainMenuInfo.MenuTypes.LEVELSELECT:
                MainMenuController.instance.SetMenuScreen(MainMenuInfo.MenuTypes.LEVELSELECT);
                text.SetActive(false);
                break;

                case MainMenuInfo.MenuTypes.CREDITS:
                MainMenuController.instance.SetMenuScreen(MainMenuInfo.MenuTypes.CREDITS);
                text.SetActive(false);
                break;

                case MainMenuInfo.MenuTypes.MAINMENU:
                MainMenuController.instance.SetMenuScreen(MainMenuInfo.MenuTypes.MAINMENU);
                break;
            }
        }
    }
}
