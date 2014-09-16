namespace GridGenerator {

    using UnityEngine;
    using System.Collections.Generic;

    using Constants;
    using Blocks;
    using Player;
    
    [DisallowMultipleComponent]
    public class GridController : MonoBehaviour {

        private List<GameObject> _upList = new List<GameObject>();
        private List<GameObject> _rightList = new List<GameObject>();
        private List<GameObject> _downList = new List<GameObject>();
        private List<GameObject> _leftList = new List<GameObject>();
        private List<GameObject> _emptyUpList = new List<GameObject>();

        void Awake() {
            GameController.FindOrCreate();
        }

        // Use this for initialization
        void Start() {
            _upList.Clear();
            _rightList.Clear();
            _downList.Clear();
            _leftList.Clear();
            FindBlocks();

            if(_emptyUpList.Count > 0){
                foreach(GameObject obj in _emptyUpList){
                    obj.GetComponent<Block>().MoveUp();
                }
            }
        }

        /// <summary>
        /// This Function is called whenever the player moves around, so that we can activate the blocks connnected to each movement.
        /// </summary>
        /// <param name="current">The Current Direction The Player Is Moving At.</param>
        /// <param name="previous">The Previous Direction The Player Was Moving Before.</param>
        public void ActivateBlocks(PlayerValues.PlayerDirection current, PlayerValues.PlayerDirection previous) {
            if(current == previous)
                return;

            switch(current) {
                case PlayerValues.PlayerDirection.FORWARD:
                if(_upList.Count > 0) {
                    foreach(GameObject obj in _upList) {
                        obj.GetComponent<Block>().blockState = BlockValues.BlockState.UP;
                        //obj.GetComponent<Block>().MoveUp();
                    }
                } else
                    return;
                break;

                case PlayerValues.PlayerDirection.RIGHT:
                if(_rightList.Count > 0) {
                    foreach(GameObject obj in _rightList) {
                        obj.GetComponent<Block>().blockState = BlockValues.BlockState.UP;
                        //obj.GetComponent<Block>().MoveUp();
                    }
                } else
                    return;
                break;

                case PlayerValues.PlayerDirection.BACKWARD:
                if(_downList.Count > 0) {
                    foreach(GameObject obj in _downList) {
                        obj.GetComponent<Block>().blockState = BlockValues.BlockState.UP;
                        //obj.GetComponent<Block>().MoveUp();
                    }
                } else
                    return;
                break;

                case PlayerValues.PlayerDirection.LEFT:
                if(_leftList.Count > 0) {
                    foreach(GameObject obj in _leftList) {
                        obj.GetComponent<Block>().blockState = BlockValues.BlockState.UP;
                        //obj.GetComponent<Block>().MoveUp();
                    }
                } else
                    return;
                break;
            }

            switch(previous) {
                case PlayerValues.PlayerDirection.FORWARD:
                if(_upList.Count > 0) {
                    foreach(GameObject obj in _upList) {
                        obj.GetComponent<Block>().blockState = BlockValues.BlockState.DOWN;
                        //obj.GetComponent<Block>().MoveDown();
                    }
                } else
                    return;
                break;

                case PlayerValues.PlayerDirection.RIGHT:
                if(_rightList.Count > 0) {
                    foreach(GameObject obj in _rightList) {
                        obj.GetComponent<Block>().blockState = BlockValues.BlockState.DOWN;
                        //obj.GetComponent<Block>().MoveDown();
                    }
                } else
                    return;
                break;

                case PlayerValues.PlayerDirection.BACKWARD:
                if(_downList.Count > 0) {
                    foreach(GameObject obj in _downList) {
                        obj.GetComponent<Block>().blockState = BlockValues.BlockState.DOWN;
                        //obj.GetComponent<Block>().MoveDown();
                    }
                } else
                    return;
                break;

                case PlayerValues.PlayerDirection.LEFT:
                if(_leftList.Count > 0) {
                    foreach(GameObject obj in _leftList) {
                        obj.GetComponent<Block>().blockState = BlockValues.BlockState.DOWN;
                        //obj.GetComponent<Block>().MoveDown();
                    }
                } else
                    return;
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
                    _upList.Add(child.gameObject);
                    break;

                    case BlockValues.BlockType.RIGHT:
                    _rightList.Add(child.gameObject);
                    break;

                    case BlockValues.BlockType.DOWN:
                    _downList.Add(child.gameObject);
                    break;

                    case BlockValues.BlockType.LEFT:
                    _leftList.Add(child.gameObject);
                    break;

                    case BlockValues.BlockType.EMPTYUP:
                    _emptyUpList.Add(child.gameObject);
                    break;
                }
            }
        }
    }
}
