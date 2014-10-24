namespace Animation {

    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    [RequireComponent(typeof(GUITexture))]
    public class AnimateTexture : MonoBehaviour {
        public List<Texture> textures;

        public float framesPerSecond = 24.0f;
        public bool runOnce = true;

        private int _currentFrame = 0;
        private int _frameCount = 0;
        private GUITexture _guiTexture;

        void Awake() {
            this._guiTexture = this.transform.GetComponent<GUITexture>() as GUITexture;
            this._currentFrame = 0;
            this._frameCount = textures.Count;
            this._guiTexture.texture = this.textures[this._currentFrame];
            //this.gameObject.SetActive(false);
        }

        void Start() {
            this.StartCoroutine(UpdateAnimation());
        }

        public void SetRect(Rect pixelInset) {
            this._guiTexture.pixelInset = pixelInset;
        }

        private IEnumerator UpdateAnimation() {
            while(true) {
                for(this._currentFrame = 0; this._currentFrame < this._frameCount; this._currentFrame++) {
                    this._guiTexture.texture = this.textures[this._currentFrame];
                    yield return new WaitForSeconds(1.0f / this.framesPerSecond);
                }

                if(this.runOnce)
                    yield break;
            }
        }
    }
}