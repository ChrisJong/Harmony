namespace GridGenerator {

    using UnityEngine;
    using System.Collections.Generic;

    using Blocks;
    using Player;
    using Sound;
    using Constants;
    
    [DisallowMultipleComponent]
    public class GridController : MonoBehaviour {

        public static GridController instance;

        private List<GameObject> _upList = new List<GameObject>();
        private List<GameObject> _rightList = new List<GameObject>();
        private List<GameObject> _downList = new List<GameObject>();
        private List<GameObject> _leftList = new List<GameObject>();
        private List<GameObject> _emptyUpList = new List<GameObject>();

        void Awake() {
            instance = this;
            GameController.FindOrCreate();
        }

        void Start() {
            this._upList.Clear();
            this._rightList.Clear();
            this._downList.Clear();
            this._leftList.Clear();
            this.FindBlocks();

            if(this._emptyUpList.Count > 0) {
                foreach(GameObject obj in this._emptyUpList) {
                    obj.GetComponent<Block>().MoveUp();
                }
            }
        }

        /// <summary>
        /// This Function is called whenever the player moves around, so that we can activate the blocks connnected to each movement.
        /// </summary>
        /// <param name="current">The Current Direction The Player Is Moving At.</param>
        /// <param name="previous">The Previous Direction The Player Was Moving Before.</param>
        public void ActivateBlocks(PlayerValues.MovementDirection current, PlayerValues.MovementDirection previous) {
            if(current == previous)
                return;

            SoundController.PlayerAudio(SoundValues.BlockUp);
            switch(current) {
                case PlayerValues.MovementDirection.FORWARD:
                if(this._upList.Count > 0) {
                    foreach(GameObject obj in this._upList) {
                        obj.GetComponent<Block>().blockState = BlockValues.BlockState.UP;
                        //obj.GetComponent<Block>().MoveUp();
                    }
                }
                break;

                case PlayerValues.MovementDirection.RIGHT:
                if(this._rightList.Count > 0) {
                    foreach(GameObject obj in this._rightList) {
                        obj.GetComponent<Block>().blockState = BlockValues.BlockState.UP;
                        //obj.GetComponent<Block>().MoveUp();
                    }
                }
                break;

                case PlayerValues.MovementDirection.BACKWARD:
                if(this._downList.Count > 0) {
                    foreach(GameObject obj in this._downList) {
                        obj.GetComponent<Block>().blockState = BlockValues.BlockState.UP;
                        //obj.GetComponent<Block>().MoveUp();
                    }
                }
                break;

                case PlayerValues.MovementDirection.LEFT:
                if(this._leftList.Count > 0) {
                    foreach(GameObject obj in this._leftList) {
                        obj.GetComponent<Block>().blockState = BlockValues.BlockState.UP;
                        //obj.GetComponent<Block>().MoveUp();
                    }
                }
                break;
            }

            SoundController.PlayerAudio(SoundValues.BlockDown);
            switch(previous) {
                case PlayerValues.MovementDirection.FORWARD:
                if(this._upList.Count > 0) {
                    foreach(GameObject obj in this._upList) {
                        obj.GetComponent<Block>().blockState = BlockValues.BlockState.DOWN;
                        //obj.GetComponent<Block>().MoveDown();
                    }
                }
                break;

                case PlayerValues.MovementDirection.RIGHT:
                if(this._rightList.Count > 0) {
                    foreach(GameObject obj in this._rightList) {
                        obj.GetComponent<Block>().blockState = BlockValues.BlockState.DOWN;
                        //obj.GetComponent<Block>().MoveDown();
                    }
                }
                break;

                case PlayerValues.MovementDirection.BACKWARD:
                if(this._downList.Count > 0) {
                    foreach(GameObject obj in this._downList) {
                        obj.GetComponent<Block>().blockState = BlockValues.BlockState.DOWN;
                        //obj.GetComponent<Block>().MoveDown();
                    }
                }
                break;

                case PlayerValues.MovementDirection.LEFT:
                if(this._leftList.Count > 0) {
                    foreach(GameObject obj in this._leftList) {
                        obj.GetComponent<Block>().blockState = BlockValues.BlockState.DOWN;
                        //obj.GetComponent<Block>().MoveDown();
                    }
                }
                break;
            }
        }

        /// <summary>
        /// On Project Start This Function Will be called to sort and group all the available blocks on the stage in a list.
        /// </summary>
        private void FindBlocks() {
            foreach(Transform child in transform) {
                var childType = child.GetComponent<Block>().blockType;
                switch(childType) {
                    case BlockValues.BlockType.UP:
                    this._upList.Add(child.gameObject);
                    break;

                    case BlockValues.BlockType.RIGHT:
                    this._rightList.Add(child.gameObject);
                    break;

                    case BlockValues.BlockType.DOWN:
                    this._downList.Add(child.gameObject);
                    break;

                    case BlockValues.BlockType.LEFT:
                    this._leftList.Add(child.gameObject);
                    break;

                    case BlockValues.BlockType.EMPTYUP:
                    this._emptyUpList.Add(child.gameObject);
                    break;
                }
            }
        }
    }
}
