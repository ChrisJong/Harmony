namespace GameMenu {

    using System.Collections;
    using UnityEngine;

    using AI;
    using Player;
    using Grid;
    using GameInfo;
    using Helpers;

    public class GameMenuController : MonoBehaviour {

        public static GameMenuController instance;

        public GameObject menu;
        public GameObject endMenu;
        public GameObject undoButton;
        public GUIText moveText;

        private GameObject _starAnimation;

        void Awake() {
            instance = this;
            if(this.transform.GetChild(0).gameObject.name == "Menu")
                this.menu = this.transform.GetChild(0).gameObject;

            this.moveText.pixelOffset = new Vector2(30.0f, GlobalInfo.ScreenHeight - 20.0f);

            this.endMenu.transform.GetChild(4).transform.guiTexture.pixelInset = GameMenuInfo.EndBillboardRect;
            this._starAnimation = this.endMenu.transform.GetChild(0).gameObject;
            this._starAnimation.transform.guiTexture.pixelInset = GameMenuInfo.StarAnimationRect;

            this.menu.SetActive(true);
            this.endMenu.SetActive(false);

        }

#if UNITY_EDTITOR || UNITY_STANDALONE_WIN
        void Update() {
            if(Input.GetKeyDown(KeyCode.M)) {
                GameController.instance.gameState = GlobalInfo.GameState.MENU;
                Object.DestroyImmediate(Sound.SoundController.instance.gameObject);
                Application.LoadLevel("MainMenu");
            }

            if(Input.GetKeyDown(KeyCode.N)) {
                Application.LoadLevel(Application.loadedLevelName);
            }

            if(Input.GetKeyDown(KeyCode.Space)) {
                PlayerController.instance.UndoMovement();
                AIController.instance.UndoMovement();
                this.undoButton.transform.gameObject.SetActive(false);
            }

            if(GameController.instance.isStageFinished) {
                if(Input.GetKeyDown(KeyCode.RightArrow)) {
                    GameController.instance.LoadNextLevel();
                }
            }
        }
#endif
        public void ActivateEndMenu() {
            this.menu.SetActive(false);
            this.endMenu.SetActive(true);

            if(GridController.instance.MoveCount > GridController.instance.MaxMoves) {
                this._starAnimation.SetActive(false);
            }
        }

        public static void FindOrCreate() {
            GameObject tempController = GameObject.FindGameObjectWithTag("GameMenuController");
            
            if(tempController == null) {
#if UNITY_EDITOR
                tempController = AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabGameMenu, AssetPaths.GameMenuControllerName);
#else
                tempController = ResourceManager.instance.gameMenuController;
#endif
                Instantiate(tempController).name = AssetPaths.GameMenuControllerName;
                return;
            } else {
                return;
            }
        }
    }
}
