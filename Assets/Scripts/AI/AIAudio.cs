namespace AI {

    using System.Collections;

    using UnityEngine;

    using Sound;

    public class AIAudio : MonoBehaviour {

        public static AIAudio instance;
        public AudioSource audioSource;

        void Awake() {
            instance = this;
            this.audioSource = this.transform.GetComponent<AudioSource>() as AudioSource;
        }

        public void PlayMovement() {
            //this.audioSource.clip = SoundController.instance.playerHover as AudioClip;
            this.audioSource.Play();
        }

        public void PlayCollision() {
            //this.audioSource.clip = SoundController.instance.playerHover as AudioClip;
            this.audioSource.Play();
        }

        public void Stop() {
            this.audioSource.Stop();
        }
    }
}