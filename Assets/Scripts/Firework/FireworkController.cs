namespace Firework {

    using System.Collections;

    using UnityEngine;

    using Sound;
    using GameInfo;

    public class FireworkController : MonoBehaviour {

        public AudioSource audioSource;

        public int _numberOfParticles = 0;
        public int _count = 0;

        void Awake() {
            this.audioSource = this.gameObject.GetComponent<AudioSource>() as AudioSource;
            this.audioSource.rolloffMode = AudioRolloffMode.Linear;
            this.audioSource.volume = 0.3f;
        }

        void Update() {
            this._count = particleSystem.particleCount;

            if(this._count < this._numberOfParticles) {
                this.PlayExplosionAudio();
            } else if(this._count > this._numberOfParticles) {
                this.PlayEmitAudio();
            }

            this._numberOfParticles = this._count;
        }

        public void PlayEmitAudio() {
            if(this.audioSource.isPlaying)
                return;

            this.audioSource.clip = SoundController.GetAudioType(SoundInfo.SoundTypes.FIREWORK_EMIT);
            this.audioSource.Play();
        }

        public void PlayExplosionAudio() {
            if(this.audioSource.isPlaying)
                return;

            this.audioSource.clip = SoundController.GetAudioType(SoundInfo.SoundTypes.FIREWORK_EMIT);
            this.audioSource.Play();
        }
    }
}