﻿namespace Animation {

    using System.Collections;

    using UnityEngine;

    public class AnimateTextureSheet : MonoBehaviour {
        public int columns = 5;
        public int rows = 5;
        public float framesPerSecond = 10.0f;
        public bool runOnce = true;

        public float RunTimeInSeconds {
            get {
                return ((1.0f / this.framesPerSecond) * (this.columns * this.rows));
            }
        }

        private Material _materialCopy = null;

        void Start() {
            this._materialCopy = new Material(this.renderer.sharedMaterial);
            this.renderer.sharedMaterial = this._materialCopy;

            Vector2 size = new Vector2(1.0f / this.columns, 1.0f / this.rows);
            this.renderer.sharedMaterial.SetTextureScale("_MainTex", size);
        }

        void OnEnable() {
            StartCoroutine(this.UpdateTiling());
        }

        private IEnumerator UpdateTiling() {
            float x = 0.0f;
            float y = 0.0f;
            Vector2 offset = Vector2.zero;

            while(true) {

                // Y
                for(int i = this.rows - 1; i >= 0; i--){
                    y = (float)i / this.rows;

                    // X
                    for(int j = 0; j <= columns - 1; j++){
                        x = (float)j / this.columns;

                        offset.Set(x, y);

                        this.renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
                        yield return new WaitForSeconds(1.0f / this.framesPerSecond);
                    }
                }

                if(this.runOnce)
                    yield break;
            }
        }
    }
}