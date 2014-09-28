namespace Menu {

    using System.Collections;
    using UnityEngine;

    using Constants;

    public class MenuButton : MonoBehaviour {

        public MenuValues.MenuTypes buttonType;

        public GameObject text;

        private void OnMouseEnter() {
            switch(buttonType) {
                case MenuValues.MenuTypes.NEWGAME:
                text.GetComponent<TextMesh>().text = "New Game";
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
                GameController.instance.gameState = GlobalValues.GameState.INGAME;
                break;

                case MenuValues.MenuTypes.CREDITS:
                text.GetComponent<TextMesh>().text = "Credits";
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
                break;

                case MenuValues.MenuTypes.ABOUT:
                text.GetComponent<TextMesh>().text = "About";
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
                break;

                case MenuValues.MenuTypes.SOMETHING:
                text.GetComponent<TextMesh>().text = "Something";
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
                break;
            }
        }

        private void OnMouseExit() {
            text.GetComponent<TextMesh>().text = "";
            switch(buttonType) {
                case MenuValues.MenuTypes.NEWGAME:
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, this.transform.position.z);
                break;

                case MenuValues.MenuTypes.CREDITS:
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, this.transform.position.z);
                break;

                case MenuValues.MenuTypes.ABOUT:
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, this.transform.position.z);
                break;

                case MenuValues.MenuTypes.SOMETHING:
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, this.transform.position.z);
                break;
            }
        }

        private void OnMouseUp() {
            text.GetComponent<TextMesh>().text = "Clicked";
            Debug.Log("Clicked");
        }
    }
}
