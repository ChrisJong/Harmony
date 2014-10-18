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

        private Dictionary<BlockInfo.BlockDirection, List<BlockClass>> _directionList;
        private List<BlockClass> _upList = new List<BlockClass>();
        private List<BlockClass> _rightList = new List<BlockClass>();
        private List<BlockClass> _downList = new List<BlockClass>();
        private List<BlockClass> _leftList = new List<BlockClass>();

        private List<NormalBlock> _normalBlocks = new List<NormalBlock>();
        private List<MultiBlock> _multiBlocks = new List<MultiBlock>();
        private List<NumberBlock> _numberBlocks = new List<NumberBlock>();
        private List<BlockClass> _stunBlocks = new List<BlockClass>();
        private List<EmptyBlock> _emptyBlocks = new List<EmptyBlock>();

        public PlayerInfo.MovementDirection _directionCurrent;
        public PlayerInfo.MovementDirection _directionPrevious;

        private int _moveCount = 0;

        void Awake() {
            instance = this;
            this._gridMap = GameObject.FindGameObjectWithTag("GridMap").gameObject;
            GameController.FindOrCreate();
            GameMenuController.FindOrCreate();
            GameController.instance.PrepareNextLevel();
        }

        void Start() {
            this._normalBlocks.Clear();
            this._multiBlocks.Clear();
            this._numberBlocks.Clear();
            this._stunBlocks.Clear();
            this._emptyBlocks.Clear();

            this.FindBlocks();
            this.SortBlocks();

            if(this._emptyBlocks.Count > 0) {
                foreach(EmptyBlock block in this._emptyBlocks) {
                    if(block.blockState == BlockInfo.BlockState.UP)
                        block.MoveDown();
                    
                    if(block.blockState == BlockInfo.BlockState.DOWN)
                        block.MoveUp();
                }
            }

            GridMap.instance.GeneratePlayers();
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

            if(PlayerController.instance.CanUndo & AIController.instance.CanUndo)
                GameMenuController.instance.undoButton.SetActive(true);
            else
                GameMenuController.instance.undoButton.SetActive(false);

            if(MazeInfo.MazeMoveValue.ContainsKey(MazeInfo.CurrentMazeNumber - 1))
                GameMenuController.instance.moveText.text = "Moves: " + this._moveCount.ToString() + " / " + MazeInfo.MazeMoveValue[MazeInfo.CurrentMazeNumber - 1][1].ToString();
            else
                GameMenuController.instance.moveText.text = "Moves: " + this._moveCount.ToString();
        }

        public void UndoMovement() {
            this._moveCount -= 1;
            this.ActivateBlocks(this._directionPrevious, this._directionCurrent);
            this._directionCurrent = this._directionPrevious;
            this._directionPrevious = PlayerInfo.MovementDirection.NONE;
        }

        /// <summary>
        /// This Function is called whenever the player moves around, so that we can activate the blocks connnected to each movement.
        /// </summary>
        /// <param name="current">The Current Direction The Player Is Moving At.</param>
        /// <param name="previous">The Previous Direction The Player Was Moving Before.</param>
        public void ActivateBlocks(PlayerInfo.MovementDirection currentDirection, PlayerInfo.MovementDirection previousDirection) {
            if(currentDirection == previousDirection)
                return;

            SoundController.PlayerAudio(SoundInfo.BlockUp);

            switch(currentDirection) {
                case PlayerInfo.MovementDirection.FORWARD:
                    if(this._upList.Count > 0) {
                        int count = this._upList.Count;
                        for(int i = 0; i < count; i++) {
                            if(this._upList[i].isUp)
                                continue;

                            this._upList[i].blockState = BlockInfo.BlockState.UP;
                        }
                    }
                    break;

                case PlayerInfo.MovementDirection.RIGHT:
                    if(this._rightList.Count > 0) {
                        int count = this._rightList.Count;
                        for(int i = 0; i < count; i++) {
                            if(this._rightList[i].isUp)
                                continue;

                            this._rightList[i].blockState = BlockInfo.BlockState.UP;
                        }
                    }
                    break;

                case PlayerInfo.MovementDirection.BACKWARD:
                    if(this._downList.Count > 0) {
                        int count = this._downList.Count;
                        for(int i = 0; i < count; i++) {
                            if(this._downList[i].isUp)
                                continue;

                            this._downList[i].blockState = BlockInfo.BlockState.UP;
                        }
                    }
                    break;

                case PlayerInfo.MovementDirection.LEFT:
                    if(this._leftList.Count > 0) {
                        int count = this._leftList.Count;
                        for(int i = 0; i < count; i++) {
                            if(this._leftList[i].isUp)
                                continue;

                            this._leftList[i].blockState = BlockInfo.BlockState.UP;
                        }
                    }
                    break;
            }

            switch(previousDirection) {
                case PlayerInfo.MovementDirection.FORWARD:
                    if(this._upList.Count > 0) {
                        int count = this._upList.Count;
                        for(int i = 0; i < count; i++) {
                            if(this._upList[i].isUp)
                                if((int)this._upList[i].firstDirection != (int)currentDirection && (int)this._upList[i].secondDirection != (int)currentDirection)
                                    this._upList[i].blockState = BlockInfo.BlockState.DOWN;

                            continue;
                        }
                    }
                    break;

                case PlayerInfo.MovementDirection.RIGHT:
                    if(this._rightList.Count > 0) {
                        int count = this._rightList.Count;
                        for(int i = 0; i < count; i++) {
                            if(this._rightList[i].isUp)
                                if((int)this._rightList[i].firstDirection != (int)currentDirection && (int)this._rightList[i].secondDirection != (int)currentDirection)
                                    this._rightList[i].blockState = BlockInfo.BlockState.DOWN;

                            continue;
                        }
                    }
                    break;

                case PlayerInfo.MovementDirection.BACKWARD:
                    if(this._downList.Count > 0) {
                        int count = this._downList.Count;
                        for(int i = 0; i < count; i++) {
                            if(this._downList[i].isUp)
                                if((int)this._downList[i].firstDirection != (int)currentDirection && (int)this._downList[i].secondDirection != (int)currentDirection)
                                    this._downList[i].blockState = BlockInfo.BlockState.DOWN;

                            continue;
                        }
                    }
                    break;

                case PlayerInfo.MovementDirection.LEFT:
                    if(this._leftList.Count > 0) {
                        int count = this._leftList.Count;
                        for(int i = 0; i < count; i++) {
                            if(this._leftList[i].isUp)
                                if((int)this._leftList[i].firstDirection != (int)currentDirection && (int)this._leftList[i].secondDirection != (int)currentDirection)
                                    this._leftList[i].blockState = BlockInfo.BlockState.DOWN;

                            continue;
                        }
                    }
                    break;
            }

            if(this._numberBlocks.Count > 0) {
                int count = this._numberBlocks.Count;
                for(int i = 0; i < count; i++) {
                    if(this._numberBlocks[i].currentCounter == 1)
                        this._numberBlocks[i].blockState = BlockInfo.BlockState.UP;
                    else
                        this._numberBlocks[i].blockState = BlockInfo.BlockState.DOWN;
                }
            }
        }

        /// <summary>
        /// This Function Will be called to sort and group all the available blocks on the stage in a list.
        /// </summary>
        private void FindBlocks() {
            foreach(Transform child in this._gridMap.transform) {
                var childType = child.GetComponent<BlockClass>().blockType;
                switch(childType) {
                    case BlockInfo.BlockTypes.NORMAL:
                        this._normalBlocks.Add(child.gameObject.GetComponent<NormalBlock>());
                        break;

                    case BlockInfo.BlockTypes.MULTI:
                        this._multiBlocks.Add(child.gameObject.GetComponent<MultiBlock>());
                        break;

                    case BlockInfo.BlockTypes.NUMBER:
                        this._numberBlocks.Add(child.gameObject.GetComponent<NumberBlock>());
                        break;

                    case BlockInfo.BlockTypes.STUN:
                        this._stunBlocks.Add(child.gameObject.GetComponent<BlockClass>());
                        break;

                    case BlockInfo.BlockTypes.EMPTY:
                        this._emptyBlocks.Add(child.gameObject.GetComponent<EmptyBlock>());
                        break;
                }
            }
        }

        private void SortBlocks() {
            if(this._normalBlocks.Count > 0) {
                foreach(NormalBlock block in this._normalBlocks) {
                    switch(block.firstDirection){
                        case BlockInfo.BlockDirection.UP:
                            this._upList.Add(block);
                            break;

                        case BlockInfo.BlockDirection.RIGHT:
                            this._rightList.Add(block);
                            break;

                        case BlockInfo.BlockDirection.DOWN:
                            this._downList.Add(block);
                            break;

                        case BlockInfo.BlockDirection.LEFT:
                            this._leftList.Add(block);
                            break;
                    }
                }
            }

            if(this._multiBlocks.Count > 0) {
                foreach(MultiBlock block in this._multiBlocks) {
                    switch(block.firstDirection) {
                        case BlockInfo.BlockDirection.UP:
                            this._upList.Add(block);
                            break;

                        case BlockInfo.BlockDirection.RIGHT:
                            this._rightList.Add(block);
                            break;

                        case BlockInfo.BlockDirection.DOWN:
                            this._downList.Add(block);
                            break;

                        case BlockInfo.BlockDirection.LEFT:
                            this._leftList.Add(block);
                            break;
                    }

                    switch(block.secondDirection) {
                        case BlockInfo.BlockDirection.UP:
                            this._upList.Add(block);
                            break;

                        case BlockInfo.BlockDirection.RIGHT:
                            this._rightList.Add(block);
                            break;

                        case BlockInfo.BlockDirection.DOWN:
                            this._downList.Add(block);
                            break;

                        case BlockInfo.BlockDirection.LEFT:
                            this._leftList.Add(block);
                            break;
                    }
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
