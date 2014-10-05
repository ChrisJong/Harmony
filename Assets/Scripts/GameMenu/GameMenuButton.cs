namespace GameMenu {
    
    using System.Collections.Generic;
    using UnityEngine;

    using GameInfo;
    using Helpers;

    public class GameMenuButton : MonoBehaviour {

        public Texture2D buttonEnterTexture;
        public Texture2D buttonExitTexture;
        public Texture2D bannerTexture;
        
        private GUITexture _objectTexture;
        private GUITexture _bannerObjectTexture;

        private GameObject _bannerObject;
        private bool _gameMenuToggle = false;

        private void Awake() {
            //this.transform.position = GameMenuInfo.GameMenuVector;
            this._objectTexture = this.transform.guiTexture;

            this._objectTexture.texture = this.buttonExitTexture;
            this._objectTexture.pixelInset = GameMenuInfo.GameMenuRect;

            this._bannerObject = this.transform.GetChild(0).gameObject;
            this._bannerObjectTexture = this._bannerObject.guiTexture;
            this._bannerObjectTexture.texture = this.bannerTexture;
            this._bannerObjectTexture.pixelInset = GameMenuInfo.BannerRect;
            this._bannerObject.SetActive(false);
        }
        
        private void OnMouseEnter() {
            if(this._gameMenuToggle) {
                this._objectTexture.texture = this.buttonExitTexture;
            } else {
                this._objectTexture.texture = this.buttonEnterTexture;
                this._bannerObjectTexture.gameObject.SetActive(true);
            }
        }

        private void OnMouseExit() {
            if(this._gameMenuToggle) {
                this._objectTexture.texture = this.buttonEnterTexture;
            } else {
                this._objectTexture.texture = this.buttonExitTexture;
                this._bannerObjectTexture.gameObject.SetActive(false);
            }
        }

        private void OnMouseUp() {
            if(!this._gameMenuToggle) {
                this._gameMenuToggle = true;
                this._bannerObjectTexture.gameObject.SetActive(true);
                GameMenuController.instance.SetMenu(true);
                this._objectTexture.texture = this.buttonEnterTexture;
            } else {
                this._gameMenuToggle = false;
                this._bannerObjectTexture.gameObject.SetActive(false);
                GameMenuController.instance.SetMenu(false);
                this._objectTexture.texture = this.buttonExitTexture;
            }
        }

        public bool GameMenuToggle {
            get { return this._gameMenuToggle; }
        }
    }
}