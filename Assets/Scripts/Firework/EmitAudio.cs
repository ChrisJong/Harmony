namespace Firework {

    using System.Collections;

    using UnityEngine;

    using Sound;
    using GameInfo;

    public class EmitAudio : MonoBehaviour {
        public AudioSource audioSource;

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
        }

        public void PlayAudio() {
            if(this.audioSource.isPlaying)
                return;

            this.audioSource.clip = SoundController.GetAudioType(SoundInfo.SoundTypes.FIREWORK_EMIT);
            this.audioSource.Play();
        }

        public void Stop() {
            this.audioSource.Stop();
        }
    }
}