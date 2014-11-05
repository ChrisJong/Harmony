namespace Player {

    using System.Collections;

    using UnityEngine;

    using Sound;
    using GameInfo;

    public class PlayerAudio : MonoBehaviour {

        public static PlayerAudio instance;
        public AudioSource audioSource;

        void Awake() {
            instance = this;
            this.audioSource = this.transform.GetComponent<AudioSource>() as AudioSource;
        }

        public void PlayMovement() {
            if(audioSource.isPlaying)
                this.audioSource.Stop();

            this.audioSource.clip = SoundController.GetAudioType(SoundInfo.SoundTypes.PLAYER_MOVEMENT);
            this.audioSource.volume = 1.0f;
            this.audioSource.Play();
        }

        public void PlayCollision() {
            if(audioSource.isPlaying)
                this.audioSource.Stop();

            this.audioSource.clip = SoundController.GetAudioType(SoundInfo.SoundTypes.PLAYER_COLLISION);
            this.audioSource.volume = 0.2f;
            this.audioSource.Play();
        }

        public void Stop() {
            this.audioSource.Stop();
        }
    }
}