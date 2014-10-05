namespace Grid {

    using UnityEngine;
    using System.Collections.Generic;

    using Blocks;
    using Player;
    using AI;
    using GameMenu;
    using Sound;
    using GameInfo;
    using Helpers;
    
    [DisallowMultipleComponent]
    public class GridController : MonoBehaviour {

        public static GridController instance;

        private GameObject _gridMap;

        private List<GameObject> _upList = new List<GameObject>();
        private List<GameObject> _rightList = new List<GameObject>();
        private List<GameObject> _downList = new List<GameObject>();
        private List<GameObject> _leftList = new List<GameObject>();
        private List<GameObject> _multiLeftRightList = new List<GameObject>();
        private List<GameObject> _multiUpDownList = new List<GameObject>();
        private List<GameObject> _emptyUpList = new List<GameObject>();

        private PlayerInfo.MovementDirection _directionCurrent;
        private PlayerInfo.MovementDirection _directionPrevious;

        private int _moveCount = 0;

        void Awake() {
            instance = this;
            this._gridMap = GameObject.FindGameObjectWithTag("GridMap").gameObject;
            GameController.FindOrCreate();
            GameMenuController.FindOrCreate();
            GameController.instance.PrepareNextLevel();
        }

        void Start() {
            this._upList.Clear();
            this._rightList.Clear();
            this._downList.Clear();
            this._leftList.Clear();
            this._multiLeftRightList.Clear();
            this._multiUpDownList.Clear();
            this.FindBlocks();

            if(this._emptyUpList.Count > 0) {
                foreach(GameObject obj in this._emptyUpList) {
                    obj.GetComponent<Block>().MoveUp();
                }
            }
        }

        void OnGUI() {
            if(MazeInfo.MazeMoveValue.ContainsKey(MazeInfo.CurrentMazeNumber-1))
                GUI.Label(new Rect(10, 10, 100, 30), "Moves: " + this._moveCount.ToString() + " / " + MazeInfo.MazeMoveValue[MazeInfo.CurrentMazeNumber-1][1].ToString());
            else
                GUI.Label(new Rect(10, 10, 100, 30), "Moves: " + this._moveCount.ToString());
        }

        void Update() {
            if(GameController.instance.gameState == GlobalInfo.GameState.INGAME)
                this.GetInput();
        }

        private void GetInput() {
            if(PlayerController.instance.isMoving || AIController.instance.isMoving)
                return;

            if(Input.GetKeyDown(KeyCode.UpArrow)) {
                this._directionPrevious = this._directionCurrent;
                this._directionCurrent = PlayerInfo.MovementDirection.FORWARD;
                PlayerController.instance.GetInput(this._directionCurrent, this._directionPrevious);
                AIController.instance.GetInput(this._directionCurrent, this._directionPrevious);
                this.ActivateBlocks(this._directionCurrent, this._directionPrevious);
                SoundController.PlayerAudio(SoundInfo.PlayerMovement);
                this._moveCount++;
            } else if(Input.GetKeyDown(KeyCode.RightArrow)) {
                this._directionPrevious = this._directionCurrent;
                this._directionCurrent = PlayerInfo.MovementDirection.RIGHT;
                PlayerController.instance.GetInput(this._directionCurrent, this._directionPrevious);
                AIController.instance.GetInput(this._directionCurrent, this._directionPrevious);
                this.ActivateBlocks(this._directionCurrent, this._directionPrevious);
                SoundController.PlayerAudio(SoundInfo.PlayerMovement);
                this._moveCount++;
            } else if(Input.GetKeyDown(KeyCode.DownArrow)) {
                this._directionPrevious = this._directionCurrent;
                this._directionCurrent = PlayerInfo.MovementDirection.BACKWARD;
                PlayerController.instance.GetInput(this._directionCurrent, this._directionPrevious);
                AIController.instance.GetInput(this._directionCurrent, this._directionPrevious);
                this.ActivateBlocks(this._directionCurrent, this._directionPrevious);
                SoundController.PlayerAudio(SoundInfo.PlayerMovement);
                this._moveCount++;
            } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                this._directionPrevious = this._directionCurrent;
                this._directionCurrent = PlayerInfo.MovementDirection.LEFT;
                PlayerController.instance.GetInput(this._directionCurrent, this._directionPrevious);
                AIController.instance.GetInput(this._directionCurrent, this._directionPrevious);
                this.ActivateBlocks(this._directionCurrent, this._directionPrevious);
                SoundController.PlayerAudio(SoundInfo.PlayerMovement);
                this._moveCount++;
            }
            
            PlayerController.instance.CheckCurrentBlock();
            AIController.instance.CheckCurrentBlock();
        }

        /// <summary>
        /// This Function is called whenever the player moves around, so that we can activate the blocks connnected to each movement.
        /// </summary>
        /// <param name="current">The Current Direction The Player Is Moving At.</param>
        /// <param name="previous">The Previous Direction The Player Was Moving Before.</param>
        public void ActivateBlocks(PlayerInfo.MovementDirection current, PlayerInfo.MovementDirection previous) {
            if(current == previous)
                return;

            SoundController.PlayerAudio(SoundInfo.BlockUp);
            switch(current) {
                case PlayerInfo.MovementDirection.FORWARD:
                if(this._upList.Count > 0) {
                    foreach(GameObject obj in this._upList) {
                        obj.GetComponent<Block>().blockState = BlockInfo.BlockState.UP;
                        //obj.GetComponent<Block>().MoveUp();
                    }
                }

                if(this._multiUpDownList.Count > 0) {
                    foreach(GameObject obj in this._multiUpDownList) {
                        obj.GetComponent<Block>().blockState = BlockInfo.BlockState.UP;
                    }
                }
                break;

                case PlayerInfo.MovementDirection.RIGHT:
                if(this._rightList.Count > 0) {
                    foreach(GameObject obj in this._rightList) {
                        obj.GetComponent<Block>().blockState = BlockInfo.BlockState.UP;
                        //obj.GetComponent<Block>().MoveUp();
                    }
                }

                if(this._multiLeftRightList.Count > 0) {
                    foreach(GameObject obj in this._multiLeftRightList) {
                        obj.GetComponent<Block>().blockState = BlockInfo.BlockState.UP;
                    }
                }
                break;

                case PlayerInfo.MovementDirection.BACKWARD:
                if(this._downList.Count > 0) {
                    foreach(GameObject obj in this._downList) {
                        obj.GetComponent<Block>().blockState = BlockInfo.BlockState.UP;
                        //obj.GetComponent<Block>().MoveUp();
                    }
                }

                if(this._multiUpDownList.Count > 0) {
                    foreach(GameObject obj in this._multiUpDownList) {
                        obj.GetComponent<Block>().blockState = BlockInfo.BlockState.UP;
                    }
                }
                break;

                case PlayerInfo.MovementDirection.LEFT:
                if(this._leftList.Count > 0) {
                    foreach(GameObject obj in this._leftList) {
                        obj.GetComponent<Block>().blockState = BlockInfo.BlockState.UP;
                        //obj.GetComponent<Block>().MoveUp();
                    }
                }

                if(this._multiLeftRightList.Count > 0) {
                    foreach(GameObject obj in this._multiLeftRightList) {
                        obj.GetComponent<Block>().blockState = BlockInfo.BlockState.UP;
                    }
                }
                break;
            }

            switch(previous) {
                case PlayerInfo.MovementDirection.FORWARD:
                if(this._upList.Count > 0) {
                    foreach(GameObject obj in this._upList) {
                        obj.GetComponent<Block>().blockState = BlockInfo.BlockState.DOWN;
                        //obj.GetComponent<Block>().MoveDown();
                    }
                }

                if(current != PlayerInfo.MovementDirection.BACKWARD) {
                    if(this._multiUpDownList.Count > 0) {
                        foreach(GameObject obj in this._multiUpDownList) {
                            obj.GetComponent<Block>().blockState = BlockInfo.BlockState.DOWN;
                        }
                    }
                }
                break;

                case PlayerInfo.MovementDirection.RIGHT:
                if(this._rightList.Count > 0) {
                    foreach(GameObject obj in this._rightList) {
                        obj.GetComponent<Block>().blockState = BlockInfo.BlockState.DOWN;
                        //obj.GetComponent<Block>().MoveDown();
                    }
                }

                if(current != PlayerInfo.MovementDirection.LEFT){
                    if(this._multiLeftRightList.Count > 0) {
                        foreach(GameObject obj in this._multiLeftRightList) {
                            obj.GetComponent<Block>().blockState = BlockInfo.BlockState.DOWN;
                        }
                    }
                }
                break;

                case PlayerInfo.MovementDirection.BACKWARD:
                if(this._downList.Count > 0) {
                    foreach(GameObject obj in this._downList) {
                        obj.GetComponent<Block>().blockState = BlockInfo.BlockState.DOWN;
                        //obj.GetComponent<Block>().MoveDown();
                    }
                }

                if(current != PlayerInfo.MovementDirection.FORWARD) {
                    if(this._multiUpDownList.Count > 0) {
                        foreach(GameObject obj in this._multiUpDownList) {
                            obj.GetComponent<Block>().blockState = BlockInfo.BlockState.DOWN;
                        }
                    }
                }
                break;

                case PlayerInfo.MovementDirection.LEFT:
                if(this._leftList.Count > 0) {
                    foreach(GameObject obj in this._leftList) {
                        obj.GetComponent<Block>().blockState = BlockInfo.BlockState.DOWN;
                        //obj.GetComponent<Block>().MoveDown();
                    }
                }

                if(current != PlayerInfo.MovementDirection.RIGHT) {
                    if(this._multiLeftRightList.Count > 0) {
                        foreach(GameObject obj in this._multiLeftRightList) {
                            obj.GetComponent<Block>().blockState = BlockInfo.BlockState.DOWN;
                        }
                    }
                }
                break;
            }
        }

        /// <summary>
        /// On Project Start This Function Will be called to sort and group all the available blocks on the stage in a list.
        /// </summary>
        private void FindBlocks() {
            foreach(Transform child in this._gridMap.transform) {
                var childType = child.GetComponent<Block>().blockType;
                switch(childType) {
                    case BlockInfo.BlockType.UP:
                    this._upList.Add(child.gameObject);
                    break;

                    case BlockInfo.BlockType.RIGHT:
                    this._rightList.Add(child.gameObject);
                    break;

                    case BlockInfo.BlockType.DOWN:
                    this._downList.Add(child.gameObject);
                    break;

                    case BlockInfo.BlockType.LEFT:
                    this._leftList.Add(child.gameObject);
                    break;

                    case BlockInfo.BlockType.EMPTYUP:
                    this._emptyUpList.Add(child.gameObject);
                    break;

                    case BlockInfo.BlockType.MULTILEFTRIGHT:
                    this._multiLeftRightList.Add(child.gameObject);
                    break;

                    case BlockInfo.BlockType.MULTIUPDOWN:
                    this._multiUpDownList.Add(child.gameObject);
                    break;
                }
            }
        }

        public static void FindOrCreate() {
            GameObject tempController = GameObject.FindGameObjectWithTag("GridController");

            if(tempController == null) {
#if UNITY_EDITOR
                tempController = AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabMisc, AssetPaths.GridControllerName);
#else
                tempController = Controller.instance.gridController;
#endif
                Instantiate(tempController).name = AssetPaths.GridControllerName;
                return;
            } else {
                return;
            }
        }

        #region Getter/Setter
        public int MoveCount {
            get { return this._moveCount; }
        }
        #endregion
    }
}
