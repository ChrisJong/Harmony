namespace Firework {

    using System.Collections;

    using UnityEngine;

    using Sound;
    using GameInfo;

    public class ExplosionAudio : MonoBehaviour {

        public AudioSource audioSource;

        private float _delay = 0.0f;

        void Awake() {
            this.audioSource = this.gameObject.GetComponent<AudioSource>() as AudioSource;
            this.audioSource.rolloffMode = AudioRolloffMode.Linear;
            this.audioSource.volume = 0.3f;
        }

        void Start() {
            PlayAudio();
        }

        void Update() {
            PlayAudio();

            if(this._delay < 2.3f) {
                this._delay += Time.deltaTime;
            }
        }

        public void PlayAudio() {
            if(this.audioSource.isPlaying)
                return;
            if(this._delay >= 2.3) {
                this.audioSource.clip = SoundController.GetAudioType(SoundInfo.SoundTypes.FIREWORK_EXPLOSION);
                this.audioSource.Play();
            }
        }

        public void Stop() {
            this.audioSource.Stop();
        }
    }
}