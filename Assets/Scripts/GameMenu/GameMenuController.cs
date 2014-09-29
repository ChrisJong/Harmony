namespace GameMenu {

    using System.Collections;
    using UnityEngine;

    using Constants;

    public class GameMenuController : MonoBehaviour {

        public GameObject menuBG;
        public GameObject gameMenuButton;

        void Awake() {
            menuBG.transform.guiTexture.pixelInset = GameMenu.MenuBGRect;
            menuBG.SetActive(false);
        }

        void FixedUpdate() {
            if(gameMenuButton.GetComponent<GameMenuButton>().GameMenuToggle) {
                menuBG.SetActive(true);
            } else {
                menuBG.SetActive(false);
            }
        }
    }
}
