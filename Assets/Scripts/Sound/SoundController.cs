namespace Sound {

    using System.Collections.Generic;
    using UnityEngine;

    using Helpers;
    using GameInfo;

    [DisallowMultipleComponent]
    public class SoundController : MonoBehaviour {

        public static SoundController instance;

        public List<AudioClip> musicGame;
        public List<AudioClip> musicMenu;

        public AudioClip blockUp;
        public AudioClip blockDown;

        public List<AudioClip> playerMovement;
        public AudioClip playerCollision;
        public AudioClip playerHover;

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

        private void StartMusic(GlobalInfo.GameState gameState) {
            if(gameState == GlobalInfo.GameState.INGAME) {
                int randomSong = Random.Range(0, this.musicGame.Count);
                this._currentSong.clip = musicGame[randomSong] as AudioClip;
                this._currentSong.Play();
            } else if(gameState == GlobalInfo.GameState.MENU) {
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
                case SoundInfo.BlockUp:
                AudioSource.PlayClipAtPoint(SoundController.instance.blockUp, position);
                break;

                case SoundInfo.BlockDown:
                AudioSource.PlayClipAtPoint(SoundController.instance.blockDown, position);
                break;

                case SoundInfo.PlayerCollision:
                AudioSource.PlayClipAtPoint(SoundController.instance.playerCollision, position, 0.5f);
                break;

                case SoundInfo.PlayerMovement:
                int randomMove = Random.Range(0, SoundController.instance.playerMovement.Count);
                AudioSource.PlayClipAtPoint(SoundController.instance.playerMovement[randomMove], position, 0.5f);
                break;
            }
        }

        public static void PlayerAudio(string type) {
            switch(type.ToLower()) {
                case SoundInfo.BlockUp:
                AudioSource.PlayClipAtPoint(SoundController.instance.blockUp, Vector3.zero);
                break;

                case SoundInfo.BlockDown:
                AudioSource.PlayClipAtPoint(SoundController.instance.blockDown, Vector3.zero);
                break;

                case SoundInfo.PlayerCollision:
                AudioSource.PlayClipAtPoint(SoundController.instance.playerCollision, Vector3.zero, 0.5f);
                break;

                case SoundInfo.PlayerMovement:
                int randomMove = Random.Range(0, SoundController.instance.playerMovement.Count);
                AudioSource.PlayClipAtPoint(SoundController.instance.playerMovement[randomMove], Vector3.zero, 0.5f);
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