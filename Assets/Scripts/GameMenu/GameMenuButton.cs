namespace GameMenu {
    
    using System.Collections.Generic;
    using UnityEngine;

    using Constants;
    using Helpers;

    public class GameMenuButton : MonoBehaviour {

        public List<Texture2D> menuTextureList = new List<Texture2D>();
        public List<Texture2D> backTextureList = new List<Texture2D>();

        private GUITexture _objectTexture;
        private bool _gameMenuToggle = false;
        private int _currentFrame;
        private int _minFrame = 0;
        private int _maxFrame = 0;

        private void Awake() {
            this._currentFrame = 0;
            this._minFrame = 0;
            this._maxFrame = GameMenu.LogoFrameCount;
            this._objectTexture = this.transform.guiTexture;

            this._objectTexture.texture = menuTextureList[0];
            this._objectTexture.pixelInset = GameMenu.GameMenuRect;
        }
        
        private void OnMouseOver() {
            if(this._currentFrame == this._maxFrame || this._gameMenuToggle)
                return;

            if(GlobalValues.ScreenWidth - Input.mousePosition.x < 75) {
                for(int i = this._currentFrame; i < this._maxFrame; i++) {
                    this._currentFrame = i;
                    this._objectTexture.texture = menuTextureList[i];
                }
            } else
                return;
        }

        private void OnMouseExit() {
            if(this._currentFrame == 0 || this._gameMenuToggle)
                return;

            for(int i = this._currentFrame; i >= this._minFrame; i--) {
                this._currentFrame = i;
                this._objectTexture.texture = menuTextureList[i];
            }
        }

        private void OnMouseUp() {
            if(!this._gameMenuToggle) {
                this._gameMenuToggle = true;
                //Time.timeScale = 0.0f;
                this._objectTexture.texture = backTextureList[0];
            } else {
                this._gameMenuToggle = false;
                //Time.timeScale = 1.0f;
                this._objectTexture.texture = menuTextureList[0];
            }
        }

        public bool GameMenuToggle {
            get { return this._gameMenuToggle; }
        }
    }
}