namespace GameMenu {

    using System.Collections;

    using UnityEngine;

    using GameInfo;

    public class FadeTransition : MonoBehaviour {
        public bool fadeToMax;
        public bool isActive;
        public bool playOnStart;

        public float duration = 5.0f;

        private GUITexture _transitionTexture;

        private bool _fadeFinished;

        private float _alpha = 0.0f;
        private float _alphaMinValue = 0.0f;
        private float _alphaMaxValue = 1.0f;

        private Color _minColor;
        private Color _maxColor;

        void Awake() {
            this._transitionTexture = this.transform.GetComponent<GUITexture>() as GUITexture;

            this.transform.position = new Vector3(0.5f, 0.5f, 1.0f);
            this._transitionTexture.pixelInset = new Rect(0.0f, 0.0f, GlobalInfo.ScreenWidth, GlobalInfo.ScreenHeight);

            this._minColor = new Color(0.5f, 0.5f, 0.5f, this._alphaMinValue);
            this._maxColor = new Color(0.5f, 0.5f, 0.5f, this._alphaMaxValue);

            if(this.fadeToMax) {
                this._transitionTexture.color = this._minColor;
                this._alpha = this._alphaMinValue;
            } else {
                this._transitionTexture.color = this._maxColor;
                this._alpha = this._alphaMaxValue;
            }

            if(!this.isActive)
                this.gameObject.SetActive(false);
        }

        void Start() {
            if(this.playOnStart)
                StartCoroutine("FadeTo");
        }

        void OnDisable() {
            StopCoroutine("FadeTo");
        }

        void OnDestroy() {
            StopCoroutine("FadeTo");
        }

        public IEnumerator FadeTo() {
            while(true) {
                if(this.fadeToMax) {
                    this.FadeToMax();
                    
                    if(this._alpha < (this._alphaMaxValue - 0.05f)) {
                        yield return null;
                    } else {
                        this._alpha = this._alphaMaxValue;
                        this._transitionTexture.color = this._maxColor;
                        this._fadeFinished = true;
                        this.transform.position = new Vector3(0.5f, 0.5f, 1.0f);
                        yield break;
                    }
                } else {
                    this.FadeToMin();
                    
                    if(this._alpha > (this._alphaMinValue + 0.02f)) {
                        yield return null;
                    } else {
                        this._alpha = this._alphaMaxValue;
                        this._transitionTexture.color = this._minColor;
                        this._fadeFinished = true;
                        this.transform.position = new Vector3(0.5f, 0.5f, 0.0f);
                        yield break;
                    }
                }
            }
        }

        public void PlayFadeToMax() {
            this.fadeToMax = true;
            this._fadeFinished = false;
            this._transitionTexture.color = this._minColor;
            this._alpha = this._alphaMinValue;
            this.transform.position = new Vector3(0.5f, 0.5f, 1.0f);

            StartCoroutine(this.FadeTo());
        }

        public void PlayFadeToMin() {
            this.fadeToMax = false;
            this._fadeFinished = false;
            this._transitionTexture.color = this._maxColor;
            this._alpha = this._alphaMaxValue;
            this.transform.position = new Vector3(0.5f, 0.5f, 1.0f);

            StartCoroutine(this.FadeTo());
        }

        private void FadeToMax() {
            var lerp = Mathf.PingPong(Time.deltaTime, this.duration) / this.duration;
            this._alpha = Mathf.Lerp(this._alpha, this._alphaMaxValue, lerp);
            this._transitionTexture.color = new Color(0.5f, 0.5f, 0.5f, this._alpha);
        }

        private void FadeToMin() {
            var lerp = Mathf.PingPong(Time.deltaTime, this.duration) / this.duration;
            this._alpha = Mathf.Lerp(this._alpha, this._alphaMinValue, lerp);
            this._transitionTexture.color = new Color(0.5f, 0.5f, 0.5f, this._alpha);
        }

        #region Getter/Setter
        public bool FadeFinished {
            get { return this._fadeFinished; }
        }
        #endregion
    }
}