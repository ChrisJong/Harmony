namespace MainMenu {

    using System.Collections;

    using UnityEngine;

    using GameMenu;
    using Helpers;
    using GameInfo;
    using Resource;

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

        private GlobalInfo.Skin _previousSkin = GlobalInfo.Skin.NONE;
        private GlobalInfo.Skin _currentSkin = GlobalInfo.Skin.NONE;

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
            TileManager.FindOrCreate();
            if(GameObject.FindGameObjectWithTag("EndGameToken") != null) {
                this.currentMenuScene = MainMenuInfo.MenuTypes.CREDITS;
            } else {
                this.currentMenuScene = MainMenuInfo.MenuTypes.MAINMENU;
            }
            MazeDataHelper.SaveGameData();
        }

        void Start() {
            this._currentSkin = TileManager.instance.CurrentSkin;
            this._previousSkin = this._currentSkin;
        }

        void Update() {
            ChangeSkin();

            if(this.isActive) {
                switch(currentMenuScene) {
                    case MainMenuInfo.MenuTypes.NEWGAME:
                        if(this.fade.FadeFinished) {
                            GameController.instance.gameState = GlobalInfo.GameState.INGAME;
                            Object.DontDestroyOnLoad(TileManager.instance.gameObject);
                            GameController.instance.LoadLevelAt(1);
                        }
                        break;

                    case MainMenuInfo.MenuTypes.LEVELSELECT:
                        if(this.fade.FadeFinished) {
                            Object.DontDestroyOnLoad(TileManager.instance.gameObject);
                            GameController.instance.LoadLevelAt(this.levelID);
                        }
                        break;
                }
            }
        }

        public void ChangeSkin() {
            // Switch Mechanic later for switching skins, need icons.

            if(this._currentSkin == this._previousSkin)
                return;

            TileManager.instance.ChangeSkin(this._currentSkin);
            this.mainMenu.BroadcastMessage("SetupSkin");
            this.levelSelect.BroadcastMessage("SetupSkin");
            this.instructions.BroadcastMessage("SetupSkin");
            this.credits.BroadcastMessage("SetupSkin");
            this._previousSkin = this._currentSkin;
        }
    }
}