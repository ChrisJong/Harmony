namespace Sound {

    using System.Collections.Generic;
    using UnityEngine;

    using Helpers;
    using GameInfo;
    using Resource;

    [DisallowMultipleComponent]
    public class SoundController : MonoBehaviour {

        public static SoundController instance;

        public List<AudioClip> musicGame;
        public List<AudioClip> musicMenu;

        public List<AudioClip> blockMovement;
        public List<AudioClip> playerMovement;
        public List<AudioClip> playerCollision;
		public List<AudioClip> playerStunned;

        public List<AudioClip> fireworkEmit;
        public List<AudioClip> fireworkExplosion;

        private AudioSource _currentSong;

        void Awake() {
            instance = this;
            this._currentSong = GetComponent<AudioSource>() as AudioSource;
        }

        void Start() {
            this.StartMusic(GameController.instance.gameState);
        }

        void Update() {
            this.MusicPlayer();
        }

        public void StartMusic(GlobalInfo.GameState gameState) {
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
            if(_currentSong.time >= GetComponent<AudioSource>().clip.length) {
                this.StartMusic(GameController.instance.gameState);
            } else
                return;
        }

        public static void PlayerAudio(SoundInfo.SoundTypes type, Vector3 position) {
            switch(type){
                case SoundInfo.SoundTypes.PLAYER_MOVEMENT:
                    int playerMovementCount = SoundController.instance.playerMovement.Count;
                    if(playerMovementCount > 0) {
                        int randomMove = Random.Range(0, playerMovementCount);
                        AudioSource.PlayClipAtPoint(SoundController.instance.playerMovement[randomMove], position, 0.5f);
                    }
                    break;
            }
        }

        public static void PlayerAudio(SoundInfo.SoundTypes type) {
            switch(type) {
                case SoundInfo.SoundTypes.PLAYER_MOVEMENT:
                    int playerMovementCount = SoundController.instance.playerMovement.Count;
                    if(playerMovementCount > 0) {
                        int randomMove = Random.Range(0, playerMovementCount);
                        AudioSource.PlayClipAtPoint(SoundController.instance.playerMovement[randomMove], Vector3.zero, 0.5f);
                    }
                    break;
            }
        }

        public static AudioClip GetAudioType(SoundInfo.SoundTypes type) {
            int count = 0;
            switch(type) {
                case SoundInfo.SoundTypes.BLOCK_MOVEMENT:
                    count = SoundController.instance.blockMovement.Count;
                    if(count > 0) {
                        int random = Random.Range(0, count);
                        return SoundController.instance.blockMovement[random];
                    }
                    break;

                case SoundInfo.SoundTypes.PLAYER_MOVEMENT:
                    count = SoundController.instance.playerMovement.Count;
                    if(count > 0) {
                        int random = Random.Range(0, count);
                        return SoundController.instance.playerMovement[random];
                    }
                    break;

                case SoundInfo.SoundTypes.PLAYER_COLLISION:
                    count = SoundController.instance.playerCollision.Count;
                    if(count > 0) {
                        int random = Random.Range(0, count);
                        return SoundController.instance.playerCollision[random];
                    }
                    break;
					
				case SoundInfo.SoundTypes.PLAYER_STUNNED:
                    count = SoundController.instance.playerStunned.Count;
					if(count > 0){
						int random = Random.Range(0, count);
						return SoundController.instance.playerStunned[random];
					}
					break;

                case SoundInfo.SoundTypes.FIREWORK_EMIT:
                    count = SoundController.instance.fireworkEmit.Count;
                    if(count > 0) {
                        int random = Random.Range(0, count);
                        return SoundController.instance.fireworkEmit[random];
                    }
                    break;

                case SoundInfo.SoundTypes.FIREWORK_EXPLOSION:
                    count = SoundController.instance.fireworkExplosion.Count;
                    if(count > 0) {
                        int random = Random.Range(0, count);
                        return SoundController.instance.fireworkExplosion[random];
                    }
                    break;
            }
            return null;
        }

        public static void FindOrCreate() {
            GameObject tempController = GameObject.FindGameObjectWithTag("SoundController");

            if(tempController == null) {
#if UNITY_EDITOR
                tempController = AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabMisc, AssetPaths.SoundControllerName);
#else
                tempController = ResourceManager.instance.soundController;
#endif
                var audio = tempController.GetComponent<AudioSource>() as AudioSource;
                audio.rolloffMode = AudioRolloffMode.Linear;
                audio.minDistance = 50.0f;
                audio.volume = 0.2f;
                tempController = Instantiate(tempController) as GameObject;
                tempController.name = AssetPaths.SoundControllerName;
                if(GameController.instance.gameState == GlobalInfo.GameState.MENU)
                    tempController.transform.parent = Camera.main.transform;
            }
        }
    }
}