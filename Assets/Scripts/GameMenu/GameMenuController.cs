namespace GameMenu {

    using System.Collections;
    using UnityEngine;

    using Constants;

    public class GameMenuController : MonoBehaviour {

        public static GameMenuController instance;

        public GameObject menuBG;
        public GameObject gameMenuButton;

        void Awake() {
            instance = this;
            this.menuBG.transform.guiTexture.pixelInset = GameMenu.MenuBGRect;
            this.menuBG.SetActive(false);
        }

        public void SetMenu() {
            if(gameMenuButton.GetComponent<GameMenuButton>().GameMenuToggle) {
                menuBG.SetActive(true);
            } else {
                menuBG.SetActive(false);
            }
        }
    }
}
