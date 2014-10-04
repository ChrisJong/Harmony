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

        void Start() {
            this.audioSource.clip = SoundController.instance.playerHover as AudioClip;
        }

        public void Play() {
            //this.audioSource.PlayDelayed(0.5f);
            this.audioSource.Play();
        }

        public void Stop() {
            this.audioSource.Stop();
        }
    }
}