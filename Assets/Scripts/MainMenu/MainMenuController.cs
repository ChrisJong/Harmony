namespace MainMenu {

    using System.Collections;

    using UnityEngine;

    using Helpers;
    using GameInfo;

    public class MainMenuController : MonoBehaviour {

        public static MainMenuController instance;
        
        public GameObject mainMenu;
        public GameObject levelSelect;
        public GameObject instructions;
        public GameObject credits;

        public MainMenuInfo.MenuTypes currentMenuScene;

        void Awake() {
            if(!GlobalInfo.GameDataLoaded) {
                MazeDataHelper.LoadGameData();
                GlobalInfo.GameDataLoaded = true;
            }

            instance = this;
            Instantiate(Resources.Load("ResourceManager") as GameObject);
            this.currentMenuScene = MainMenuInfo.MenuTypes.MAINMENU;
            MazeDataHelper.SaveGameData();
        }
    }
}