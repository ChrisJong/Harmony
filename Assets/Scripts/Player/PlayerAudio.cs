namespace Player {

    using System.Collections;

    using UnityEngine;

    using Sound;
    using GameInfo;

    public class PlayerAudio : MonoBehaviour {

        public static PlayerAudio instance;

        private AudioSource _collisionAudioSource;
        private AudioSource _movementAudioSource;

        void Awake() {
            instance = this;

            this.AddAudioSource();
        }

        public void PlayMovement() {
            if(this._movementAudioSource.isPlaying)
                return;

            this._movementAudioSource.clip = SoundController.GetAudioType(SoundInfo.SoundTypes.PLAYER_MOVEMENT);
            this._movementAudioSource.Play();
        }

        public void PlayCollision() {
            if(this._collisionAudioSource.isPlaying)
                this._collisionAudioSource.Stop();

            this._collisionAudioSource.clip = SoundController.GetAudioType(SoundInfo.SoundTypes.PLAYER_COLLISION);
            this._collisionAudioSource.Play();
        }

        public void StopMovement() {
            this._movementAudioSource.Stop();
        }

        public void StopCollision() {
            this._collisionAudioSource.Stop();
        }

        private void AddAudioSource() {
            this._movementAudioSource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
            this._movementAudioSource.rolloffMode = AudioRolloffMode.Linear;
            this._movementAudioSource.volume = 1.0f;
            this._movementAudioSource.playOnAwake = false;

            this._collisionAudioSource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
            this._collisionAudioSource.rolloffMode = AudioRolloffMode.Linear;
            this._collisionAudioSource.volume = 0.2f;
            this._collisionAudioSource.playOnAwake = false;
        }
    }
}