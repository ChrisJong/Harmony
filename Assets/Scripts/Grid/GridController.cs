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
    using Input;
    
    [DisallowMultipleComponent]
    public class GridController : MonoBehaviour {

        public static GridController instance;

        public PlayerInfo.MovementDirection directionCurrent;
        public PlayerInfo.MovementDirection directionPrevious;
        public bool blocksReady = false;

        private GameObject _gridMap;

        private List<BlockClass> _upList = new List<BlockClass>();
        private List<BlockClass> _rightList = new List<BlockClass>();
        private List<BlockClass> _downList = new List<BlockClass>();
        private List<BlockClass> _leftList = new List<BlockClass>();

        private List<NormalBlock> _normalBlocks = new List<NormalBlock>();
        private List<MultiBlock> _multiBlocks = new List<MultiBlock>();
        private List<NumberBlock> _numberBlocks = new List<NumberBlock>();
        private List<BlockClass> _stunBlocks = new List<BlockClass>();
        private List<EmptyBlock> _emptyBlocks = new List<EmptyBlock>();

        private int _moveCount = 0;
        private int _maxMoves = 0;
        private float _endTimer = 0.0f;

        private SwipeInput _swipeController;
        private bool _endMenuActive = false;
        private bool _startEndAnimation = false;

        void Awake() {
            instance = this;
            this._gridMap = GameObject.FindGameObjectWithTag("GridMap").gameObject;
            GameController.FindOrCreate();
            GameMenuController.FindOrCreate();
            GameController.instance.PrepareNextLevel();

#if UNITY_IPHONE || UNITY_ANDROID
            this.transform.gameObject.AddComponent<SwipeInput>();
            this._swipeController = this.transform.GetComponent<SwipeInput>() as SwipeInput;
#endif
        }

        void Start() {
            this._upList.Clear();
            this._rightList.Clear();
            this._downList.Clear();
            this._leftList.Clear();
            this._normalBlocks.Clear();
            this._multiBlocks.Clear();
            this._numberBlocks.Clear();
            this._stunBlocks.Clear();
            this._emptyBlocks.Clear();

            this.FindBlocks();
            this.SortBlocks();

            GridMap.instance.GeneratePlayers();

            if(MazeInfo.MazeMoveValue != null)
                this._maxMoves = MazeInfo.MazeMoveValue[MazeInfo.CurrentMazeNumber - 1].maxMoves;
        }

        void Update() {
            if(GameController.instance.gameState == GlobalInfo.GameState.INGAME) {
                if(!GameController.instance.isStageFinished) {
                    this.GetInput();

                    /*if(this._moveCount > this._maxMoves * 3)
                        GameController.instance.isStageFinished = true;*/
                } else {
                    if(!this._endMenuActive) {
                        if(!this._startEndAnimation) {
                            if(this._moveCount <= this._maxMoves) {
                                GameObject fireworks = ResourceManager.instance.fireworkParticle;
                                fireworks.transform.position = new Vector3(GridMap.instance.columns * 0.5f, -5.5f, GridMap.instance.rows * 0.5f);
                                fireworks = Instantiate(fireworks) as GameObject;
                                PlayerController.instance.SpawnEndAnimation();
                                PlayerController.instance.gameObject.SetActive(false);
                                AIController.instance.gameObject.SetActive(false);
                                this._startEndAnimation = true;
                            } else if(this._moveCount > this._maxMoves * 3) {
                                Debug.Log("DEAD");
                            } else {
                                PlayerController.instance.SpawnEndAnimation();
                                PlayerController.instance.gameObject.SetActive(false);
                                AIController.instance.gameObject.SetActive(false);
                                this._startEndAnimation = true;
                            }
                        }
                        if(this._endTimer > 3.0f) {
                            GameMenuController.instance.ActivateEndMenu();
                            if(GridController.instance.MoveCount <= GridController.instance.MaxMoves * 3)
                                GameController.instance.UnlockNextLevel();
                            this._endMenuActive = true;
                        } else {
                            this._endTimer += Time.deltaTime;
                        }
                    }

                    this.DeactivateBlocks();
                }
            }
        }

        private void GetInput() {
            if(PlayerController.instance.isMoving || AIController.instance.isMoving)
                return;
            
#if UNITY_IPHONE || UNITY_ANDROID
            if(!this._swipeController.GetInput())
                GridController.instance.blocksReady = false;
#else
            if(Input.GetKeyDown(KeyCode.UpArrow)) {
                this.directionPrevious = this.directionCurrent;
                this.directionCurrent = PlayerInfo.MovementDirection.FORWARD;
                PlayerController.instance.GetInput(this.directionCurrent, this.directionPrevious);
                AIController.instance.GetInput(this.directionCurrent, this.directionPrevious);
                this.ActivateBlocks(this.directionCurrent, this.directionPrevious);
                SoundController.PlayerAudio(SoundInfo.PlayerMovement);
                this._moveCount++;
            } else if(Input.GetKeyDown(KeyCode.RightArrow)) {
                this.directionPrevious = this.directionCurrent;
                this.directionCurrent = PlayerInfo.MovementDirection.RIGHT;
                PlayerController.instance.GetInput(this.directionCurrent, this.directionPrevious);
                AIController.instance.GetInput(this.directionCurrent, this.directionPrevious);
                this.ActivateBlocks(this.directionCurrent, this.directionPrevious);
                SoundController.PlayerAudio(SoundInfo.PlayerMovement);
                this._moveCount++;
            } else if(Input.GetKeyDown(KeyCode.DownArrow)) {
                this.directionPrevious = this.directionCurrent;
                this.directionCurrent = PlayerInfo.MovementDirection.BACKWARD;
                PlayerController.instance.GetInput(this.directionCurrent, this.directionPrevious);
                AIController.instance.GetInput(this.directionCurrent, this.directionPrevious);
                this.ActivateBlocks(this.directionCurrent, this.directionPrevious);
                SoundController.PlayerAudio(SoundInfo.PlayerMovement);
                this._moveCount++;
            } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                this.directionPrevious = this.directionCurrent;
                this.directionCurrent = PlayerInfo.MovementDirection.LEFT;
                PlayerController.instance.GetInput(this.directionCurrent, this.directionPrevious);
                AIController.instance.GetInput(this.directionCurrent, this.directionPrevious);
                this.ActivateBlocks(this.directionCurrent, this.directionPrevious);
                SoundController.PlayerAudio(SoundInfo.PlayerMovement);
                this._moveCount++;
            } else {
                GridController.instance.blocksReady = false;
            }
#endif
            PlayerController.instance.CheckCurrentBlock();
            AIController.instance.CheckCurrentBlock();

            if(PlayerController.instance.CanUndo & AIController.instance.CanUndo)
                GameMenuController.instance.undoButton.SetActive(true);
            else
                GameMenuController.instance.undoButton.SetActive(false);

            if(MazeInfo.MazeMoveValue == null) {
                GameMenuController.instance.moveText.text = "Moves: " + this._moveCount.ToString();
            } else {
                if(MazeInfo.MazeMoveValue.ContainsKey(MazeInfo.CurrentMazeNumber - 1))
                    GameMenuController.instance.moveText.text = "Level: " + MazeInfo.CurrentMazeNumber + " of " + MazeInfo.MaxMazeLength.ToString() + '\n' + "Moves: " + this._moveCount.ToString() + " / " + this._maxMoves;
                else
                    GameMenuController.instance.moveText.text = "Moves: " + this._moveCount.ToString();
            }
        }

        public void UndoMovement() {
            this._moveCount -= 1;
            this.UndoBlocks(this.directionCurrent, this.directionPrevious);
            this.directionCurrent = this.directionPrevious;
            this.directionPrevious = PlayerInfo.MovementDirection.NONE;
        }

        /// <summary>
        /// This Function is called whenever the player moves around, so that we can activate the blocks connnected to each movement.
        /// </summary>
        /// <param name="current">The Current Direction The Player Is Moving At.</param>
        /// <param name="previous">The Previous Direction The Player Was Moving Before.</param>
        public void ActivateBlocks(PlayerInfo.MovementDirection currentDirection, PlayerInfo.MovementDirection previousDirection) {
            BlockInfo.BlockDirection current = (BlockInfo.BlockDirection)((int)currentDirection);
            BlockInfo.BlockDirection previous = (BlockInfo.BlockDirection)((int)previousDirection);

            if(currentDirection == previousDirection) {
                if(this._numberBlocks.Count > 0) {
                    int count = this._numberBlocks.Count;
                    for(int i = 0; i < count; i++) {
                        if(this._numberBlocks[i].currentCounter == 1)
                            this._numberBlocks[i].BlockState = BlockInfo.BlockState.UP;
                        else
                            this._numberBlocks[i].BlockState = BlockInfo.BlockState.DOWN;
                    }
                }
                this.blocksReady = true;
                return;
            }

            SoundController.PlayerAudio(SoundInfo.BlockUp);

            if(this._numberBlocks.Count > 0) {
                int count = this._numberBlocks.Count;
                for(int i = 0; i < count; i++) {
                    if(this._numberBlocks[i].currentCounter == 1)
                        this._numberBlocks[i].BlockState = BlockInfo.BlockState.UP;
                    else
                        this._numberBlocks[i].BlockState = BlockInfo.BlockState.DOWN;
                }
            }

            if(this._multiBlocks.Count > 0) {
                int count = this._multiBlocks.Count;
                for(int i = 0; i < count; i++) {
                    if(!this._multiBlocks[i].BlockDirections.Contains(current)){
                        if(this._multiBlocks[i].isUp)
                            this._multiBlocks[i].BlockState = BlockInfo.BlockState.DOWN;
                    }
                }
            }

            if(this._normalBlocks.Count > 0) {
                int count = this._normalBlocks.Count;
                for(int i = 0; i < count; i++) {
                    if(this._normalBlocks[i].FirstDirection != current){
                        if(this._normalBlocks[i].isUp)
                            this._normalBlocks[i].BlockState = BlockInfo.BlockState.DOWN;
                    }
                }
            }

            if(this._normalBlocks.Count > 0) {
                int count = this._normalBlocks.Count;
                for(int i = 0; i < count; i++) {
                    if(this._normalBlocks[i].FirstDirection == current){
                        if(this._normalBlocks[i].isUp)
                            continue;
                        else
                            this._normalBlocks[i].BlockState = BlockInfo.BlockState.UP;
                    }
                }
            }

            if(this._multiBlocks.Count > 0) {
                int count = this._multiBlocks.Count;
                for(int i = 0; i < count; i++) {
                    if(this._multiBlocks[i].BlockDirections.Contains(current)){
                        if(this._multiBlocks[i].isUp)
                            continue;
                        else
                            this._multiBlocks[i].BlockState = BlockInfo.BlockState.UP;
                    }
                }
            }

            this.blocksReady = true;
        }

        public void UndoBlocks(PlayerInfo.MovementDirection current, PlayerInfo.MovementDirection previous) {
            int i = 0;
            int count = 0;
            
            if(current == previous) {
                // Number Blocks
                if(this._numberBlocks.Count > 0) {
                    count = this._numberBlocks.Count;
                    for(i = 0; i < count; i++) {
                        this._numberBlocks[i].Undo();
                    }
                }
                this.blocksReady = true;
                return;
            }

            // Number Blocks
            if(this._numberBlocks.Count > 0) {
                count = this._numberBlocks.Count;
                for(i = 0; i < count; i++) {
                    this._numberBlocks[i].Undo();
                }
            }

            // Arrow Blocks - Down To Up (Previous)
            switch(previous) {
                case PlayerInfo.MovementDirection.FORWARD:
                    if(this._upList.Count > 0) {
                        count = this._upList.Count;
                        for(i = 0; i < count; i++) {
                            if(this._upList[i].isUp)
                                continue;

                            this._upList[i].BlockState = BlockInfo.BlockState.UP;
                        }
                    }
                    break;

                case PlayerInfo.MovementDirection.RIGHT:
                    if(this._rightList.Count > 0) {
                        count = this._rightList.Count;
                        for(i = 0; i < count; i++) {
                            if(this._rightList[i].isUp)
                                continue;

                            this._rightList[i].BlockState = BlockInfo.BlockState.UP;
                        }
                    }
                    break;

                case PlayerInfo.MovementDirection.BACKWARD:
                    if(this._downList.Count > 0) {
                        count = this._downList.Count;
                        for(i = 0; i < count; i++) {
                            if(this._downList[i].isUp)
                                continue;

                            this._downList[i].BlockState = BlockInfo.BlockState.UP;
                        }
                    }
                    break;


                case PlayerInfo.MovementDirection.LEFT:
                    if(this._leftList.Count > 0) {
                        count = this._leftList.Count;
                        for(i = 0; i < count; i++) {
                            if(this._leftList[i].isUp)
                                continue;

                            this._leftList[i].BlockState = BlockInfo.BlockState.UP;
                        }
                    }
                    break;
            }

            // Arrow Blocks - Up To Down (Current)
            switch(current) {
                case PlayerInfo.MovementDirection.FORWARD:
                    if(this._upList.Count > 0) {
                        count = this._upList.Count;
                        for(i = 0; i < count; i++) {
                            if(this._upList[i].isUp) {
                                if(this._upList[i].GetType() == typeof(MultiBlock)) {
                                    if((int)this._upList[i].FirstDirection != (int)current && (int)this._upList[i].SecondDirection != (int)current)
                                        this._upList[i].BlockState = BlockInfo.BlockState.DOWN;
                                }else
                                    this._upList[i].BlockState = BlockInfo.BlockState.DOWN;
                            }

                            continue;
                        }
                    }
                    break;

                case PlayerInfo.MovementDirection.RIGHT:
                    if(this._rightList.Count > 0) {
                        count = this._rightList.Count;
                        for(i = 0; i < count; i++) {
                            if(this._rightList[i].isUp) {
                                if(this._rightList[i].GetType() == typeof(MultiBlock)) {
                                    if((int)this._rightList[i].FirstDirection != (int)current && (int)this._rightList[i].SecondDirection != (int)current)
                                        this._rightList[i].BlockState = BlockInfo.BlockState.DOWN;
                                } else
                                    this._rightList[i].BlockState = BlockInfo.BlockState.DOWN;
                            }

                            continue;
                        }
                    }
                    break;

                case PlayerInfo.MovementDirection.BACKWARD:
                    if(this._downList.Count > 0) {
                        count = this._downList.Count;
                        for(i = 0; i < count; i++) {
                            if(this._downList[i].isUp) {
                                if(this._downList[i].GetType() == typeof(MultiBlock)) {
                                    if((int)this._downList[i].FirstDirection != (int)current && (int)this._downList[i].SecondDirection != (int)current)
                                        this._downList[i].BlockState = BlockInfo.BlockState.DOWN;
                                } else
                                    this._downList[i].BlockState = BlockInfo.BlockState.DOWN;
                            }
                        }
                    }
                    break;

                case PlayerInfo.MovementDirection.LEFT:
                    if(this._leftList.Count > 0) {
                        count = this._leftList.Count;
                        for(i = 0; i < count; i++) {
                            if(this._leftList[i].isUp) {
                                if(this._leftList[i].GetType() == typeof(MultiBlock)) {
                                    if((int)this._leftList[i].FirstDirection != (int)current && (int)this._leftList[i].SecondDirection != (int)current)
                                        this._leftList[i].BlockState = BlockInfo.BlockState.DOWN;
                                } else
                                    this._leftList[i].BlockState = BlockInfo.BlockState.DOWN;
                            }
                        }
                    }
                    break;
            }

            this.blocksReady = true;
        }

        public void DeactivateBlocks() {
            if(this._normalBlocks.Count > 0) {
                int count = this._normalBlocks.Count;
                for(int i = 0; i < count; i++)
                    this._normalBlocks[i].BlockState = BlockInfo.BlockState.DOWN;
            }

            if(this._multiBlocks.Count > 0) {
                int count = this._multiBlocks.Count;
                for(int i = 0; i < count; i++)
                    this._multiBlocks[i].BlockState = BlockInfo.BlockState.DOWN;
            }

            if(this._numberBlocks.Count > 0) {
                int count = this._numberBlocks.Count;
                for(int i = 0; i < count; i++)
                    this._numberBlocks[i].BlockState = BlockInfo.BlockState.DOWN;
            }

            if(this._stunBlocks.Count > 0) {
                int count = this._stunBlocks.Count;
                for(int i = 0; i < count; i++)
                    this._stunBlocks[i].BlockState = BlockInfo.BlockState.DOWN;
            }

            if(this._emptyBlocks.Count > 0) {
                int count = this._emptyBlocks.Count;
                for(int i = 0; i < count; i++)
                    if(this._emptyBlocks[i].isUp)
                        this._emptyBlocks[i].MoveDown();
            }
        }

        /// <summary>
        /// This Function Will be called to sort and group all the available blocks on the stage in a list.
        /// </summary>
        private void FindBlocks() {
            foreach(Transform child in this._gridMap.transform) {
                var childType = child.GetComponent<BlockClass>();
                switch(childType.BlockType) {
                    case BlockInfo.BlockTypes.NORMAL:
                        if(childType.BlockState == BlockInfo.BlockState.UP)
                            childType.MoveUp();
                        else
                            childType.MoveDown();
                        this._normalBlocks.Add(child.gameObject.GetComponent<NormalBlock>());
                        break;

                    case BlockInfo.BlockTypes.MULTI:
                        if(childType.BlockState == BlockInfo.BlockState.UP)
                            childType.MoveUp();
                        else
                            childType.MoveDown();
                        this._multiBlocks.Add(child.gameObject.GetComponent<MultiBlock>());
                        break;

                    case BlockInfo.BlockTypes.NUMBER:
                        ((NumberBlock)childType).Init();
                        this._numberBlocks.Add(child.gameObject.GetComponent<NumberBlock>());
                        break;

                    case BlockInfo.BlockTypes.STUN:
                        if(childType.BlockState == BlockInfo.BlockState.UP)
                            childType.MoveUp();
                        else
                            childType.MoveDown();
                        this._stunBlocks.Add(child.gameObject.GetComponent<BlockClass>());
                        break;

                    case BlockInfo.BlockTypes.EMPTY:
                        if(childType.BlockState == BlockInfo.BlockState.UP)
                            childType.MoveUp();
                        else
                            childType.MoveDown();
                        this._emptyBlocks.Add(child.gameObject.GetComponent<EmptyBlock>());
                        break;
                }
            }
        }

        private void SortBlocks() {
            if(this._normalBlocks.Count > 0) {
                foreach(NormalBlock block in this._normalBlocks) {
                    switch(block.FirstDirection){
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
                    switch(block.FirstDirection) {
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

                    switch(block.SecondDirection) {
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
                tempController = ResourceManager.instance.gridController;
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
            set { this._moveCount = value; }
        }

        public int MaxMoves {
            get { return this._maxMoves; }
        }

        public bool EndMenuActive {
            get { return this._endMenuActive; }
        }
        #endregion
    }
}
