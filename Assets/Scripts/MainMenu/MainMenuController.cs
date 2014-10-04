namespace MainMenu {

    using System.Collections;

    using UnityEngine;
    
    using GameInfo;

    public class MainMenuController : MonoBehaviour {

        public static MainMenuController instance;
        
        public GameObject mainMenu;
        public GameObject levelSelect;
        public GameObject instructions;
        public GameObject credits;

        public MainMenuInfo.MenuTypes currentMenuType;
        public GameObject currentMenuScreen;

        void Awake() {
            instance = this;
            this.currentMenuType = MainMenuInfo.MenuTypes.MAINMENU;
            this.currentMenuScreen = mainMenu;

            this.mainMenu.SetActive(true);
            this.levelSelect.SetActive(false);
            this.instructions.SetActive(false);
            this.credits.SetActive(false);
        }

        public void SetMenuScreen(MainMenuInfo.MenuTypes menu) {
            switch(menu) {
                case MainMenuInfo.MenuTypes.MAINMENU:
                this.currentMenuScreen.SetActive(false);
                this.currentMenuScreen = this.mainMenu;
                this.currentMenuType = menu;
                this.currentMenuScreen.SetActive(true);
                break;

                case MainMenuInfo.MenuTypes.LEVELSELECT:
                this.currentMenuScreen.SetActive(false);
                this.currentMenuScreen = this.levelSelect;
                this.currentMenuType = menu;
                this.currentMenuScreen.SetActive(true);
                break;

                case MainMenuInfo.MenuTypes.INSTRUCTIONS:
                this.currentMenuScreen.SetActive(false);
                this.currentMenuScreen = this.instructions;
                this.currentMenuType = menu;
                this.currentMenuScreen.SetActive(true);
                break;

                case MainMenuInfo.MenuTypes.CREDITS:
                this.currentMenuScreen.SetActive(false);
                this.currentMenuScreen = this.credits;
                this.currentMenuType = menu;
                this.currentMenuScreen.SetActive(true);
                break;
            }
        }
    }
}