namespace Sound {

    using System.Collections.Generic;
    using UnityEngine;

    using Helpers;
    using Constants;

    [DisallowMultipleComponent]
    public class SoundController : MonoBehaviour {

        public static SoundController instance;

        public List<AudioClip> musicGame;
        public List<AudioClip> musicMenu;

        public AudioClip blockUp;
        public AudioClip blockDown;
        public AudioClip blockCollision;

        public AudioClip playerMovement;

        private AudioSource _currentSong;

        void Awake() {
            instance = this;
            this._currentSong = GetComponent<AudioSource>() as AudioSource;
        }

        void Start() {
            this.StartMusic(GameController.instance.gameState);
        }

        void FixedUpdate() {
            this.MusicPlayer();
        }

        private void StartMusic(GlobalValues.GameState gameState) {
            if(gameState == GlobalValues.GameState.INGAME) {
                int randomSong = Random.Range(0, this.musicGame.Count);
                this._currentSong.clip = musicGame[randomSong] as AudioClip;
                this._currentSong.Play();
            } else if(gameState == GlobalValues.GameState.MENU) {
                int randomSong = Random.Range(0, this.musicMenu.Count);
                this._currentSong.clip = musicMenu[randomSong] as AudioClip;
                this._currentSong.Play();
            }
        }

        private void MusicPlayer() {
            if(_currentSong.time >= audio.clip.length) {
                this.StartMusic(GameController.instance.gameState);
            } else
                return;
        }

        public static void PlayerAudio(string type, Vector3 position) {
            switch(type.ToLower()){
                case SoundValues.BlockUp:
                AudioSource.PlayClipAtPoint(SoundController.instance.blockUp, position);
                break;

                case SoundValues.BlockDown:
                AudioSource.PlayClipAtPoint(SoundController.instance.blockDown, position);
                break;

                case SoundValues.BlockCollision:
                AudioSource.PlayClipAtPoint(SoundController.instance.blockCollision, position);
                break;

                case SoundValues.PlayerMovement:
                AudioSource.PlayClipAtPoint(SoundController.instance.playerMovement, position);
                break;
            }
        }

        public static void PlayerAudio(string type) {
            switch(type.ToLower()) {
                case SoundValues.BlockUp:
                AudioSource.PlayClipAtPoint(SoundController.instance.blockUp, Vector3.zero);
                break;

                case SoundValues.BlockDown:
                AudioSource.PlayClipAtPoint(SoundController.instance.blockDown, Vector3.zero);
                break;

                case SoundValues.BlockCollision:
                AudioSource.PlayClipAtPoint(SoundController.instance.blockCollision, Vector3.zero);
                break;

                case SoundValues.PlayerMovement:
                AudioSource.PlayClipAtPoint(SoundController.instance.playerMovement, Vector3.zero);
                break;
            }
        }

        public static void FindOrCreate() {
            GameObject tempController = GameObject.FindGameObjectWithTag("SoundController");

            if(tempController == null) {
                tempController = AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabMisc, AssetPaths.SoundControllerName);
                var audio = tempController.GetComponent<AudioSource>() as AudioSource;
                audio.rolloffMode = AudioRolloffMode.Linear;
                audio.minDistance = 50.0f;
                audio.volume = 0.5f;
                Instantiate(tempController).name = AssetPaths.SoundControllerName;
                return;
            } else {
                return;
            }
        }
    }
}