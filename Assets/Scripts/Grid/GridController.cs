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
    using Resource;
    
    [DisallowMultipleComponent]
    public class GridController : MonoBehaviour {

        public static GridController instance;

        public PlayerInfo.MovementDirection directionCurrent;
        public PlayerInfo.MovementDirection directionPrevious;
        public bool blocksReady = false;

        public Color warningColor;
        private GUITexture _warningTexture;

        private GameObject _gridMap;

        private List<BlockClass> _upList = new List<BlockClass>();
        private List<BlockClass> _rightList = new List<BlockClass>();
        private List<BlockClass> _downList = new List<BlockClass>();
        private List<BlockClass> _leftList = new List<BlockClass>();

        private List<NormalBlock> _normalBlocks = new List<NormalBlock>();
        private List<MultiBlock> _multiBlocks = new List<MultiBlock>();
        private List<NumberBlock> _numberBlocks = new List<NumberBlock>();
        private List<StunBlock> _stunBlocks = new List<StunBlock>();
        private List<SwitchBlock> _switchBlocks = new List<SwitchBlock>();
        private List<WarpBlock> _warpBlocks = new List<WarpBlock>();
        private List<GlassBlock> _glassBlocks = new List<GlassBlock>();
        private List<EmptyBlock> _emptyBlocks = new List<EmptyBlock>();

        private List<BlockClass> _allBlocks = new List<BlockClass>();

        private int _moveCount = 0;
        private int _maxMoves = 0;
        private float _endTimer = 0.0f;
        private float _stunTimer = 0.0f;

        private Vector3 _explosionPosition = Vector3.zero;
        private float _explosionRadius = 0.0f;

        private SwipeInput _swipeController;
        private bool _startDestruction = true;
        private bool _endMenuActive = false;
        private bool _startEndAnimation = false;

        void Awake() {
            instance = this;
            this._gridMap = GameObject.FindGameObjectWithTag("GridMap").gameObject;
            GameController.FindOrCreate();
            GameMenuController.FindOrCreate();
            GameController.instance.PrepareNextLevel();
            GameMenuController.instance.fade.PlayFadeToMin();
            this._explosionPosition = new Vector3(GridMap.instance.columns * 0.5f, 0.5f, GridMap.instance.rows * 0.5f);
            this._explosionRadius = (GridMap.instance.rows * GridMap.instance.columns);

            this._warningTexture = ((GameObject)Instantiate(ResourceManager.instance.warningTexture) as GameObject).GetComponent<GUITexture>();
            this._warningTexture.pixelInset = new Rect(0.0f, 0.0f, GlobalInfo.ScreenWidth, GlobalInfo.ScreenHeight);
            this.warningColor = new Color(0.8f, 0.1f, 0.1f, 0.0f);
            this._warningTexture.color = this.warningColor;

            this.gameObject.AddComponent<GridAudio>();

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
            this._switchBlocks.Clear();
            this._warpBlocks.Clear();
            this._glassBlocks.Clear();
            this._emptyBlocks.Clear();

            this.FindBlocks();
            this.SortBlocks();

            GridMap.instance.GeneratePlayers();

            if(MazeInfo.MazeMoveValue != null)
                this._maxMoves = MazeInfo.MazeMoveValue[MazeInfo.CurrentMazeNumber - 1].maxMoves;
        }

        void Update() {
            this.warningColor.a -= 0.3f;
            this._warningTexture.color = this.warningColor;
            if(GameController.instance.gameState == GlobalInfo.GameState.INGAME) {
                if(!GameController.instance.isStageFinished) {
                    this.GetInput();
                } else {
                    if(!this._endMenuActive) {
                        if(this._endTimer > 4.0f) {
                            GameMenuController.instance.ActivateEndMenu();
                            if(!PlayerController.instance.isDeath)
                                GameController.instance.UnlockNextLevel();
                            this._endMenuActive = true;
                        } else {
                            GameMenuController.instance.menu.SetActive(false);
                            this._endTimer += Time.deltaTime;
                        }
                    }

                    if(!PlayerController.instance.isDeath) {
                        if(!this._startEndAnimation) {
                            if(this._moveCount <= this._maxMoves) {
                                GameObject fireworks = ResourceManager.instance.fireworkParticle;
                                fireworks.transform.position = new Vector3(GridMap.instance.columns * 0.5f, Camera.main.transform.position.y - 13.0f, GridMap.instance.rows * 0.5f);
                                fireworks = Instantiate(fireworks) as GameObject;
                                PlayerController.instance.SpawnEndAnimation();
                                PlayerController.instance.gameObject.SetActive(false);
                                AIController.instance.gameObject.SetActive(false);
                                this._startEndAnimation = true;
                            } else {
                                PlayerController.instance.SpawnEndAnimation();
                                PlayerController.instance.gameObject.SetActive(false);
                                AIController.instance.gameObject.SetActive(false);
                                this._startEndAnimation = true;
                            }
                        }
                    }
                    //this.DeactivateBlocks();
                    this.DestructBlocks();
                }
            }
        }

        private void GetInput() {
            if(PlayerController.instance.isMoving || AIController.instance.isMoving)
                return;

            if(PlayerController.instance.isStunned || AIController.instance.isStunned){
                this._stunTimer += Time.deltaTime;

                if(this._stunTimer > 2.0f) {
                    PlayerController.instance.isStunned = false;
                    AIController.instance.isStunned = false;
                    this._moveCount++;
                    this._stunTimer = 0.0f;
                }
            }

            if(MazeInfo.MazeMoveValue != null) {
                if(this._moveCount >= (this._maxMoves * 3)) {
                    PlayerController.instance.isMoving = false;
                    AIController.instance.isMoving = false;
                    PlayerController.instance.isDeath = true;

                    PlayerController.instance.charactorAnimator.SetBool("IsDeath", PlayerController.instance.isDeath);
                    AIController.instance.charactorAnimator.SetBool("IsDeath", PlayerController.instance.isDeath);
                    GameController.instance.isStageFinished = true;
                }
            }

#if UNITY_IPHONE || UNITY_ANDROID
            if(!this._swipeController.GetInput())
                GridController.instance.blocksReady = false;
#else
            if(Input.GetKeyDown(KeyCode.UpArrow)) {
                this.directionPrevious = this.directionCurrent;
                this.directionCurrent = PlayerInfo.MovementDirection.FORWARD;
                if(!PlayerController.instance.isStunned) {
                    PlayerController.instance.GetInput(this.directionCurrent, this.directionPrevious);
                    if(!AIController.instance.isStunned)
                        AIController.instance.GetInput(this.directionCurrent, this.directionPrevious);
                    
                    if(this._moveCount > 0)
                        this.ResetUndoStates();

                    this.ActivateBlocks(this.directionCurrent, this.directionPrevious);
                    this._moveCount++;
                }
                
                if(MazeInfo.MazeMoveValue != null) {
                    if(this._moveCount >= this._maxMoves * 2) {
                        this.warningColor.a = (this._moveCount / this._maxMoves);
                    }
                }
            } else if(Input.GetKeyDown(KeyCode.RightArrow)) {
                this.directionPrevious = this.directionCurrent;
                this.directionCurrent = PlayerInfo.MovementDirection.RIGHT;
                if(!PlayerController.instance.isStunned) {
                    PlayerController.instance.GetInput(this.directionCurrent, this.directionPrevious);
                    if(!AIController.instance.isStunned)
                        AIController.instance.GetInput(this.directionCurrent, this.directionPrevious);
                    
                    if(this._moveCount > 0)
                        this.ResetUndoStates();

                    this.ActivateBlocks(this.directionCurrent, this.directionPrevious);
                    this._moveCount++;
                }

                if(MazeInfo.MazeMoveValue != null) {
                    if(this._moveCount >= this._maxMoves * 2) {
                        this.warningColor.a = (this._moveCount / this._maxMoves);
                    }
                }
            } else if(Input.GetKeyDown(KeyCode.DownArrow)) {
                this.directionPrevious = this.directionCurrent;
                this.directionCurrent = PlayerInfo.MovementDirection.BACKWARD;
                if(!PlayerController.instance.isStunned) {
                    PlayerController.instance.GetInput(this.directionCurrent, this.directionPrevious);
                    if(!AIController.instance.isStunned)
                        AIController.instance.GetInput(this.directionCurrent, this.directionPrevious);

                    if(this._moveCount > 0)
                        this.ResetUndoStates();

                    this.ActivateBlocks(this.directionCurrent, this.directionPrevious);
                    this._moveCount++;
                }

                if(MazeInfo.MazeMoveValue != null) {
                    if(this._moveCount >= this._maxMoves * 2) {
                        this.warningColor.a = (this._moveCount / this._maxMoves);
                    }
                }
            } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                this.directionPrevious = this.directionCurrent;
                this.directionCurrent = PlayerInfo.MovementDirection.LEFT;

                if(!PlayerController.instance.isStunned) {
                    PlayerController.instance.GetInput(this.directionCurrent, this.directionPrevious);
                    if(!AIController.instance.isStunned)
                        AIController.instance.GetInput(this.directionCurrent, this.directionPrevious);

                    if(this._moveCount > 0)
                        this.ResetUndoStates();

                    this.ActivateBlocks(this.directionCurrent, this.directionPrevious);
                    this._moveCount++;
                }
                    
                if(MazeInfo.MazeMoveValue != null) {
                    if(this._moveCount >= this._maxMoves * 2) {
                        this.warningColor.a = (this._moveCount / this._maxMoves);
                    }
                }
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
                if(PlayerController.instance.isStunned)
                    GameMenuController.instance.moveText.text = "Moves: " + this._moveCount.ToString() + " (Player Stunned)";
                else
                    GameMenuController.instance.moveText.text = "Moves: " + this._moveCount.ToString();
            } else {
                if(MazeInfo.MazeMoveValue.ContainsKey(MazeInfo.CurrentMazeNumber - 1)) {
                    if(PlayerController.instance.isStunned)
                        GameMenuController.instance.moveText.text = "Maze: " + MazeInfo.CurrentMazeNumber.ToString() + " / " + MazeInfo.MaxMazeLength.ToString() + '\n' + "Moves: " + this._moveCount.ToString() + " / " + (this._maxMoves * 3).ToString() + " (Player Stunned)";
                    else
                        GameMenuController.instance.moveText.text = "Maze: " + MazeInfo.CurrentMazeNumber.ToString() + " / " + MazeInfo.MaxMazeLength.ToString() + '\n' + "Moves: " + this._moveCount.ToString() + " / " + (this._maxMoves * 3).ToString();
                } else
                    GameMenuController.instance.moveText.text = "Moves: " + this._moveCount.ToString() + " (Player Stunned)";
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
        /// <param name="currentDirection">The Current Direction The Player Is Moving At.</param>
        /// <param name="previousDirection">The Previous Direction The Player Was Moving Before.</param>
        public void ActivateBlocks(PlayerInfo.MovementDirection currentDirection, PlayerInfo.MovementDirection previousDirection) {
            int i = 0;
            int count = 0;

            BlockInfo.BlockDirection current = (BlockInfo.BlockDirection)((int)currentDirection);
            //BlockInfo.BlockDirection previous = (BlockInfo.BlockDirection)((int)previousDirection);

            if(currentDirection == previousDirection) {
                if(this._numberBlocks.Count > 0) {
                    count = this._numberBlocks.Count;
                    for(i = 0; i < count; i++) {
                        if(this._numberBlocks[i].currentCounter == 1) {
                            if(this._numberBlocks[i].isReversed)
                                this._numberBlocks[i].BlockState = BlockInfo.BlockState.DOWN;
                            else
                                this._numberBlocks[i].BlockState = BlockInfo.BlockState.UP;
                        } else {
                            if(this._numberBlocks[i].isReversed)
                                this._numberBlocks[i].BlockState = BlockInfo.BlockState.UP;
                            else
                                this._numberBlocks[i].BlockState = BlockInfo.BlockState.DOWN;
                        }
                    }
                }
                this.blocksReady = true;
                return;
            }
            if((this._numberBlocks.Count | this._normalBlocks.Count | this._multiBlocks.Count) > 0)
                GridAudio.instance.PlayMovement();

            if(this._numberBlocks.Count > 0) {
                count = this._numberBlocks.Count;
                for(i = 0; i < count; i++) {
                    if(this._numberBlocks[i].currentCounter == 1) {
                        if(this._numberBlocks[i].isReversed)
                            this._numberBlocks[i].BlockState = BlockInfo.BlockState.DOWN;
                        else
                            this._numberBlocks[i].BlockState = BlockInfo.BlockState.UP;
                    } else {
                        if(this._numberBlocks[i].isReversed)
                            this._numberBlocks[i].BlockState = BlockInfo.BlockState.UP;
                        else
                            this._numberBlocks[i].BlockState = BlockInfo.BlockState.DOWN;
                    }
                }
            }

            if(this._normalBlocks.Count > 0) {
                count = this._normalBlocks.Count;
                for(i = 0; i < count; i++) {
                    if(this._normalBlocks[i].FirstDirection == current){
                        if(this._normalBlocks[i].isUp)
                            continue;
                        else
                            this._normalBlocks[i].BlockState = BlockInfo.BlockState.UP;
                    } else {
                        if(this._normalBlocks[i].isUp)
                            this._normalBlocks[i].BlockState = BlockInfo.BlockState.DOWN;
                    }
                }
            }

            if(this._multiBlocks.Count > 0) {
                count = this._multiBlocks.Count;
                for(i = 0; i < count; i++) {
                    if(this._multiBlocks[i].BlockDirections.Contains(current)){
                        if(this._multiBlocks[i].isUp) {
                            this._multiBlocks[i].PreviousState = BlockInfo.BlockState.UP;
                            continue;
                        } else
                            this._multiBlocks[i].BlockState = BlockInfo.BlockState.UP;
                    } else {
                        if(this._multiBlocks[i].isUp)
                            this._multiBlocks[i].BlockState = BlockInfo.BlockState.DOWN;
                    }
                }
            }

            this.blocksReady = true;
        }

        public void ResetUndoStates() {
            int i = 0;
            int count = 0;

            if(this._switchBlocks.Count > 0) {
                count = this._switchBlocks.Count;
                for(i = 0; i < count; i++) {
                    this._switchBlocks[i].ResetUndoState();
                }
            }
        }

        public void UndoBlocks(PlayerInfo.MovementDirection currentDirection, PlayerInfo.MovementDirection previousDirection) {
            int i = 0;
            int count = 0;

            BlockInfo.BlockDirection current = (BlockInfo.BlockDirection)((int)currentDirection);
            BlockInfo.BlockDirection previous = (BlockInfo.BlockDirection)((int)previousDirection);

            if(currentDirection == previousDirection) {
                if(this._numberBlocks.Count > 0) {
                    count = this._numberBlocks.Count;
                    for(i = 0; i < count; i++) {
                        this._numberBlocks[i].Undo();
                    }
                }
                this.blocksReady = true;
                return;
            }

            if(this._numberBlocks.Count > 0) {
                count = this._numberBlocks.Count;
                for(i = 0; i < count; i++) {
                    this._numberBlocks[i].Undo();
                }
            }

            if(this._normalBlocks.Count > 0) {
                count = this._normalBlocks.Count;
                for(i = 0; i < count; i++) {
                    if(this._normalBlocks[i].FirstDirection == current) {
                        this._normalBlocks[i].BlockState = this._normalBlocks[i].PreviousState;
                    }

                    if(this._normalBlocks[i].FirstDirection == previous) {
                        this._normalBlocks[i].BlockState = this._normalBlocks[i].PreviousState;
                    } else if(previous == BlockInfo.BlockDirection.NONE) {
                        this._normalBlocks[i].BlockState = this._normalBlocks[i].FirstState;
                    }
                }
            }

            if(this._multiBlocks.Count > 0) {
                count = this._multiBlocks.Count;
                for(i = 0; i < count; i++) {
                    if(this._multiBlocks[i].BlockDirections.Contains(current) && !this._multiBlocks[i].BlockDirections.Contains(previous)) {
                        this._multiBlocks[i].BlockState = this._multiBlocks[i].PreviousState;
                    }

                    if(this._multiBlocks[i].BlockDirections.Contains(previous)) {
                        if(!this._multiBlocks[i].isUp)
                            this._multiBlocks[i].BlockState = this._multiBlocks[i].PreviousState;
                    } else if(previous == BlockInfo.BlockDirection.NONE) {
                        this._multiBlocks[i].BlockState = this._multiBlocks[i].FirstState;
                    }
                }
            }

            if(this._switchBlocks.Count > 0) {
                count = this._switchBlocks.Count;
                for(i = 0; i < count; i++) {
                    if(this._switchBlocks[i].IsFlipped) {
                        this._switchBlocks[i].Undo();
                        this._switchBlocks[i].IsFlipped = false;
                    }
                }
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

        public void DestructBlocks() {
            if(!this._startDestruction)
                return;
            else {
                Destroy(GridMap.instance.wallOrigin);
                GridMap.instance.wallOrigin = null;

                this._allBlocks.ForEach(x => x.Destruction());
                Collider[] colliders = Physics.OverlapSphere(this._explosionPosition, this._explosionRadius);

                foreach(Collider hit in colliders) {
                    if(hit && hit.GetComponent<Rigidbody>())
                        hit.GetComponent<Rigidbody>().AddExplosionForce(50.0f, this._explosionPosition, this._explosionRadius, 3.0f);
                }
                this._startDestruction = false;
            }
        }

        /// <summary>
        /// This Function Will be called to sort and group all the available blocks on the stage in a list.
        /// </summary>
        private void FindBlocks() {
            foreach(Transform child in this._gridMap.transform) {
                var childType = child.GetComponent<BlockClass>();
                if(childType == null)
                    continue;

                this._allBlocks.Add(childType);
                switch(childType.BlockType) {
                    case BlockInfo.BlockTypes.NORMAL:
                        ((NormalBlock)childType).Init();
                        this._normalBlocks.Add(child.gameObject.GetComponent<NormalBlock>());
                        break;

                    case BlockInfo.BlockTypes.MULTI:
                        ((MultiBlock)childType).Init();
                        this._multiBlocks.Add(child.gameObject.GetComponent<MultiBlock>());
                        break;

                    case BlockInfo.BlockTypes.NUMBER:
                        ((NumberBlock)childType).Init();
                        this._numberBlocks.Add(child.gameObject.GetComponent<NumberBlock>());
                        break;

                    case BlockInfo.BlockTypes.STUN:
                        ((StunBlock)childType).Init();
                        this._stunBlocks.Add(child.gameObject.GetComponent<StunBlock>());
                        break;

                    case BlockInfo.BlockTypes.SWITCH:
                        ((SwitchBlock)childType).Init();
                        this._switchBlocks.Add(child.gameObject.GetComponent<SwitchBlock>());
                        break;

                    case BlockInfo.BlockTypes.WARP:
                        ((WarpBlock)childType).Init();
                        this._warpBlocks.Add(child.gameObject.GetComponent<WarpBlock>());
                        break;

                    case BlockInfo.BlockTypes.GLASS:
                        ((GlassBlock)childType).Init();
                        this._glassBlocks.Add(child.gameObject.GetComponent<GlassBlock>());
                        break;

                    case BlockInfo.BlockTypes.EMPTY:
                        ((EmptyBlock)childType).Init();
                        this._emptyBlocks.Add(child.gameObject.GetComponent<EmptyBlock>());
                        break;

                    case BlockInfo.BlockTypes.EMPTY_TALL:
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
