namespace Animation {

    using System.Collections;

    using UnityEngine;

    public class AnimateTextureSheet : MonoBehaviour {
        public int columns = 5;
        public int rows = 5;
        public float framesPerSecond = 10.0f;
        public bool runOnce = true;
        public bool isPlaying = true;

        public float RunTimeInSeconds {
            get {
                return ((1.0f / this.framesPerSecond) * (this.columns * this.rows));
            }
        }

        private Material _materialCopy = null;

        void Start() {
            this._materialCopy = new Material(this.GetComponent<Renderer>().sharedMaterial);
            this.GetComponent<Renderer>().sharedMaterial = this._materialCopy;

            Vector2 size = new Vector2(1.0f / this.columns, 1.0f / this.rows);
            this.GetComponent<Renderer>().sharedMaterial.SetTextureScale("_MainTex", size);
        }

        void OnEnable() {
            this.isPlaying = true;
            this.StartCoroutine("UpdateTiling");
        }

        void OnDisable() {
            this.isPlaying = false;
            this.StopCoroutine("UpdateTiling");
        }

        public void StartCoroutineAnimation() {
            this.isPlaying = true;
            this.StartCoroutine("UpdateTiling");
        }

        public void StopCoroutineAnimation() {
            this.isPlaying = false;
            this.StopCoroutine("UpdateTiling");
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

                        this.GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
                        yield return new WaitForSeconds(1.0f / this.framesPerSecond);
                    }
                }

                if(this.runOnce) {
                    this.isPlaying = false;
                    yield break;
                }
            }
        }
    }
}