namespace GameMenu {

    using System.Collections;
    using UnityEngine;

    using AI;
    using Player;
    using Grid;
    using GameInfo;
    using Helpers;
    using Sound;

    public class GameMenuController : MonoBehaviour {

        public static GameMenuController instance;

        public GameMenuInfo.ButtonTypes buttonPressed = GameMenuInfo.ButtonTypes.NONE;
        public bool isActive = false;

        public GameObject menu;
        public GameObject endMenu;
        public GameObject undoButton;
        public GameObject nextLevelButton;
        public GUIText moveText;
        public GUIText buttonStateText;
        public GUIText billboardText;

        public GameObject starAnimation;
        public GameObject noStar;
        public FadeTransition fade;

        void Awake() {
            instance = this;

            this.moveText.pixelOffset = new Vector2(30.0f, GlobalInfo.ScreenHeight - 20.0f);
            this.buttonStateText.pixelOffset = new Vector2(GameMenuInfo.EndMainMenuButtonRect.x + 55.0f, GameMenuInfo.EndRestartButtonRect.y - 15.0f);
            this.buttonStateText.text = "";
            this.billboardText.pixelOffset = new Vector2(GameMenuInfo.EndMainMenuButtonRect.x + 55.0f, GameMenuInfo.EndBillboardRect.y + (GameMenuInfo.EndBillboardHeight));

            this.starAnimation.transform.guiTexture.pixelInset = GameMenuInfo.StarAnimationRect;
            this.noStar.transform.guiTexture.pixelInset = GameMenuInfo.NoStarRect;

            this.menu.SetActive(true);
            this.endMenu.SetActive(false);

        }

        void Update() {
            if(this.isActive) {
                if(this.fade.FadeFinished) {
                    switch(this.buttonPressed) {
                        case GameMenuInfo.ButtonTypes.RESTART:
                            Application.LoadLevel(Application.loadedLevelName);
                            break;

                        case GameMenuInfo.ButtonTypes.MAINMENU:
                            Object.DestroyImmediate(Sound.SoundController.instance.gameObject);
                            Application.LoadLevel("MainMenu");
                            break;

                        case GameMenuInfo.ButtonTypes.NEXTLEVEL:
                            GameController.instance.LoadNextLevel();
                            break;
                    }
                }
            }
        }

        public void ActivateEndMenu() {
            this.menu.SetActive(false);
            this.endMenu.SetActive(true);
            this.noStar.SetActive(false);
            this.starAnimation.SetActive(false);

            if(PlayerController.instance.isDeath) {
                this.billboardText.text = "Your Actions Have Disrupted The Balance" + '\n' + "You Must Reunite The Forces Of The Universe!" + '\n' + "Try Again!";
                this.nextLevelButton.SetActive(false);
            } else {
                if(GridController.instance.MoveCount <= GridController.instance.MaxMoves) {
                    this.billboardText.text = "You Have Attained The Path To True Harmony!" + '\n' + "Perfect Score!";
                    this.starAnimation.SetActive(true);
                } else {
                    this.billboardText.text = "Unification Process Complete" + '\n' + "Although You Have Strayed From The Light" + '\n' + (GridController.instance.MoveCount - GridController.instance.MaxMoves).ToString() + " Moves(s) Over" + '\n' + "Try To Find The Path To Harmony";
                    this.noStar.SetActive(true);
                }
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
