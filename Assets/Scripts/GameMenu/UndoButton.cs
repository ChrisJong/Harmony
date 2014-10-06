namespace GameMenu {

    using System.Collections;

    using UnityEngine;

    using Player;
    using AI;

    public class UndoButton : MonoBehaviour {

        public Texture2D buttonEnterTexture;
        public Texture2D buttonExitTexture;

        private GUITexture _objectTexture;

        private void Awake() {
            this._objectTexture = this.transform.guiTexture;

            this._objectTexture.texture = this.buttonExitTexture;
            this.transform.gameObject.SetActive(false);
        }

        private void OnMouseEnter() {
            this._objectTexture.texture = this.buttonEnterTexture;
        }

        private void OnMouseExit() {
            this._objectTexture.texture = this.buttonExitTexture;
        }

        private void OnMouseUp() {
            PlayerController.instance.UndoMovement();
            AIController.instance.UndoMovement();
            this.transform.gameObject.SetActive(false);
        }
    }
}