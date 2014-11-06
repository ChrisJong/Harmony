namespace MainMenu {

    using System.Collections;

    using UnityEngine;

    using GameMenu;
    using Helpers;
    using GameInfo;

    public class MainMenuController : MonoBehaviour {

        public static MainMenuController instance;
        
        public GameObject mainMenu;
        public GameObject levelSelect;
        public GameObject instructions;
        public GameObject credits;

        public FadeTransition fade;
        public bool isActive = false;
        public int levelID = 0;

        public MainMenuInfo.MenuTypes currentMenuScene;

        void Awake() {
#if UNITY_IPHONE || UNITY_ANDROID
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
#endif

            if(!GlobalInfo.GameDataLoaded) {
                MazeDataHelper.LoadGameData();
                GlobalInfo.GameDataLoaded = true;
            }

            instance = this;
            Instantiate(Resources.Load("ResourceManager") as GameObject);
            this.currentMenuScene = MainMenuInfo.MenuTypes.MAINMENU;
            MazeDataHelper.SaveGameData();
        }

        void Update() {
            if(this.isActive) {
                switch(currentMenuScene) {
                    case MainMenuInfo.MenuTypes.NEWGAME:
                        if(this.fade.FadeFinished) {
                            GameController.instance.gameState = GlobalInfo.GameState.INGAME;
                            GameController.instance.LoadLevelAt(1);
                        }
                        break;

                    case MainMenuInfo.MenuTypes.LEVELSELECT:
                        if(this.fade.FadeFinished) {
                            GameController.instance.LoadLevelAt(this.levelID);
                        }
                        break;
                }

            }
        }
    }
}