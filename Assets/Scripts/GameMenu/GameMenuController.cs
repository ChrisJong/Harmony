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

        public GameObject menu;
        public GameObject endMenu;
        public GameObject undoButton;
        public GameObject nextLevelButton;
        public GUIText moveText;
        public GUIText buttonStateText;
        public GUIText billboardText;

        private GameObject _starAnimation;
        private GameObject _noStar;

        void Awake() {
            instance = this;
            if(this.transform.GetChild(0).gameObject.name == "Menu")
                this.menu = this.transform.GetChild(0).gameObject;

            this.moveText.pixelOffset = new Vector2(30.0f, GlobalInfo.ScreenHeight - 20.0f);
            this.buttonStateText.pixelOffset = new Vector2(GameMenuInfo.EndMainMenuButtonRect.x + 55.0f, GameMenuInfo.EndRestartButtonRect.y);
            this.buttonStateText.text = "";
            this.billboardText.pixelOffset = new Vector2(GameMenuInfo.EndMainMenuButtonRect.x + 55.0f, GameMenuInfo.EndBillboardRect.y + (GameMenuInfo.EndBillboardHeight - 35));

            this._starAnimation = this.endMenu.transform.GetChild(1).gameObject;
            this._starAnimation.transform.guiTexture.pixelInset = GameMenuInfo.StarAnimationRect;

            this._noStar = this.endMenu.transform.GetChild(2).gameObject;
            this._noStar.transform.guiTexture.pixelInset = GameMenuInfo.NoStarRect;

            this.menu.SetActive(true);
            this.endMenu.SetActive(false);

        }

        public void ActivateEndMenu() {
            this.menu.SetActive(false);
            this.endMenu.SetActive(true);
            this._noStar.SetActive(false);
            this.billboardText.text = "You have attained the path to true Harmony!" + '\n' + "Perfect score!";
            if(GridController.instance.MoveCount > GridController.instance.MaxMoves) {
                if(GridController.instance.MoveCount > GridController.instance.MaxMoves * 3) {
                    this.billboardText.text = "Your actions have disrupted the balance" + '\n' + "You must reunite the forces of the universe!" + '\n' + (GridController.instance.MoveCount - GridController.instance.MaxMoves).ToString() + " moves(s) over" + '\n' + "Try Again";
                    this.nextLevelButton.SetActive(false);
                    this._starAnimation.SetActive(false);
                    this._noStar.SetActive(true);
                } else {
                    this.billboardText.text = "Unification process complete" + '\n' + "Although you have strayed from the light" + '\n' + (GridController.instance.MoveCount - GridController.instance.MaxMoves).ToString() + " moves(s) over" + '\n' + "Try to find the path to Harmony";
                    this._starAnimation.SetActive(false);
                    this._noStar.SetActive(true);
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
