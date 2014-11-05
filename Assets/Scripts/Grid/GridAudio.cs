namespace Grid {

    using System.Collections;

    using UnityEngine;

    using Sound;
    using GameInfo;

    public class GridAudio : MonoBehaviour {

        public static GridAudio instance;
        public AudioSource audioSource;

        void Awake() {
            instance = this;
            this.audioSource = this.gameObject.GetComponent<AudioSource>() as AudioSource;
            this.audioSource.rolloffMode = AudioRolloffMode.Linear;
            this.audioSource.volume = 0.3f;
        }

        public void PlayMovement() {
            if(this.audioSource.isPlaying)
                this.audioSource.Stop();

            this.audioSource.clip = SoundController.GetAudioType(SoundInfo.SoundTypes.BLOCK_MOVEMENT);
            this.audioSource.Play();
        }

        public void Stop() {
            this.audioSource.Stop();
        }
    }
}