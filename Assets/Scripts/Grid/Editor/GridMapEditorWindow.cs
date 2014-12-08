namespace Grid {

    using UnityEngine;
    using UnityEditor;

    using GameInfo;

    public class GridMapEditorWindow : EditorWindow {

        private GridMap _target;

        void Awake() {
            this.title = "Place Blocks";
        }

        void Update() {
            if(EditorApplication.isPlaying) {
                this.CloseWindow();
            } else
                return;
        }

        void OnDisable() {
            this.ResetValues();
        }

        public void CloseWindow() {
            this.ResetValues();
            this.Close();
        }

        public void Init() {
            this._target = (GridMap)FindObjectOfType(typeof(GridMap));
        }

        void OnGUI() {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Block Type: " + this._target.blockToPlace.ToString());
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("INVISIBLE")) {
                this.ResetValues();
                this._target.blockToPlace = BlockInfo.BlockTypes.INVISIBLE;
            }

            if(GUILayout.Button("EMPTY")) {
                this.ResetValues();
                this._target.blockToPlace = BlockInfo.BlockTypes.EMPTY;
            }

            if(GUILayout.Button("NORMAL")) {
                this.ResetValues();
                this._target.blockToPlace = BlockInfo.BlockTypes.NORMAL;
            }

            if(GUILayout.Button("MULTI")) {
                this.ResetValues();
                this._target.blockToPlace = BlockInfo.BlockTypes.MULTI;
            }

            
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("NUMBER")) {
                this.ResetValues();
                this._target.blockToPlace = BlockInfo.BlockTypes.NUMBER;
            }

            if(GUILayout.Button("SWITCH")) {
                this.ResetValues();
                this._target.blockToPlace = BlockInfo.BlockTypes.SWITCH;
            }

            if(GUILayout.Button("STUN")) {
                this.ResetValues();
                this._target.blockToPlace = BlockInfo.BlockTypes.STUN;
            }

            if(GUILayout.Button("WARP")) {
                this.ResetValues();
                this._target.blockToPlace = BlockInfo.BlockTypes.WARP;
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);

            this.BlockInformation();
        }

        private void BlockInformation() {
            switch(this._target.blockToPlace){
                case BlockInfo.BlockTypes.INVISIBLE:
                    EditorGUILayout.LabelField("Place a 4 Story Invisible Block.");
                    break;

                case BlockInfo.BlockTypes.EMPTY:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("EMPTY BLOCK TYPE: ");
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if(GUILayout.Button("NORMAL")) {
                        this._target.blockToPlace = BlockInfo.BlockTypes.EMPTY;
                    }

                    if(GUILayout.Button("TALL")) {
                        this._target.blockToPlace = BlockInfo.BlockTypes.EMPTY_TALL;
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Block State:" + this._target.blockState);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if(GUILayout.Button("UP"))
                        this._target.blockState = BlockInfo.BlockState.UP;
                    if(GUILayout.Button("DOWN"))
                        this._target.blockState = BlockInfo.BlockState.DOWN;
                    EditorGUILayout.EndHorizontal();
                    break;

                case BlockInfo.BlockTypes.EMPTY_TALL:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("EMPTY BLOCK TYPE: ");
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if(GUILayout.Button("NORMAL")) {
                        this._target.blockToPlace = BlockInfo.BlockTypes.EMPTY;
                    }

                    if(GUILayout.Button("TALL")) {
                        this._target.blockToPlace = BlockInfo.BlockTypes.EMPTY_TALL;
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if(this._target.blockToPlace == BlockInfo.BlockTypes.EMPTY_TALL)
                        EditorGUILayout.LabelField("Place a 3 Story Empty Block.");
                    EditorGUILayout.EndHorizontal();
                    break;

                case BlockInfo.BlockTypes.NORMAL:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Direction:" + this._target.blockOneDirection);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if(GUILayout.Button("UP"))
                        this._target.blockOneDirection = BlockInfo.BlockDirection.UP;
                    if(GUILayout.Button("RIGHT"))
                        this._target.blockOneDirection = BlockInfo.BlockDirection.RIGHT;
                    if(GUILayout.Button("DOWN"))
                        this._target.blockOneDirection = BlockInfo.BlockDirection.DOWN;
                    if(GUILayout.Button("LEFT"))
                        this._target.blockOneDirection = BlockInfo.BlockDirection.LEFT;
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Block State:" + this._target.blockState);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if(GUILayout.Button("UP"))
                        this._target.blockState = BlockInfo.BlockState.UP;
                    if(GUILayout.Button("DOWN"))
                        this._target.blockState = BlockInfo.BlockState.DOWN;
                    EditorGUILayout.EndHorizontal();
                    break;

                case BlockInfo.BlockTypes.MULTI:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Direction One:" + this._target.blockOneDirection);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if(this._target.blockTwoDirection == BlockInfo.BlockDirection.UP || this._target.blockOneDirection == BlockInfo.BlockDirection.UP) {
                        GUILayout.Button("N/A");
                    } else {
                        if(GUILayout.Button("UP"))
                            this._target.blockOneDirection = BlockInfo.BlockDirection.UP;
                    }

                    if(this._target.blockTwoDirection == BlockInfo.BlockDirection.RIGHT || this._target.blockOneDirection == BlockInfo.BlockDirection.RIGHT) {
                        GUILayout.Button("N/A");
                    } else {
                        if(GUILayout.Button("RIGHT"))
                            this._target.blockOneDirection = BlockInfo.BlockDirection.RIGHT;
                    }

                    if(this._target.blockTwoDirection == BlockInfo.BlockDirection.DOWN || this._target.blockOneDirection == BlockInfo.BlockDirection.DOWN) {
                        GUILayout.Button("N/A");
                    } else {
                        if(GUILayout.Button("DOWN"))
                            this._target.blockOneDirection = BlockInfo.BlockDirection.DOWN;
                    }

                    if(this._target.blockTwoDirection == BlockInfo.BlockDirection.LEFT || this._target.blockOneDirection == BlockInfo.BlockDirection.LEFT) {
                        GUILayout.Button("N/A");
                    } else {
                        if(GUILayout.Button("LEFT"))
                            this._target.blockOneDirection = BlockInfo.BlockDirection.LEFT;
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Direction Two:" + this._target.blockTwoDirection);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if(this._target.blockOneDirection == BlockInfo.BlockDirection.UP || this._target.blockTwoDirection == BlockInfo.BlockDirection.UP) {
                        GUILayout.Button("N/A");
                    } else {
                        if(GUILayout.Button("UP"))
                            this._target.blockTwoDirection = BlockInfo.BlockDirection.UP;
                    }

                    if(this._target.blockOneDirection == BlockInfo.BlockDirection.RIGHT || this._target.blockTwoDirection == BlockInfo.BlockDirection.RIGHT) {
                        GUILayout.Button("N/A");
                    } else {
                        if(GUILayout.Button("RIGHT"))
                            this._target.blockTwoDirection = BlockInfo.BlockDirection.RIGHT;
                    }

                    if(this._target.blockOneDirection == BlockInfo.BlockDirection.DOWN || this._target.blockTwoDirection == BlockInfo.BlockDirection.DOWN) {
                        GUILayout.Button("N/A");
                    } else {
                        if(GUILayout.Button("DOWN"))
                            this._target.blockTwoDirection = BlockInfo.BlockDirection.DOWN;
                    }

                    if(this._target.blockOneDirection == BlockInfo.BlockDirection.LEFT || this._target.blockTwoDirection == BlockInfo.BlockDirection.LEFT) {
                        GUILayout.Button("N/A");
                    } else {
                        if(GUILayout.Button("LEFT"))
                            this._target.blockTwoDirection = BlockInfo.BlockDirection.LEFT;
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Block State:" + this._target.blockState);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if(GUILayout.Button("UP"))
                        this._target.blockState = BlockInfo.BlockState.UP;
                    if(GUILayout.Button("DOWN"))
                        this._target.blockState = BlockInfo.BlockState.DOWN;
                    EditorGUILayout.EndHorizontal();
                    break;

                case BlockInfo.BlockTypes.NUMBER:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Countdown Number:" + this._target.blockNumber);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    this._target.blockNumber = EditorGUILayout.IntSlider(this._target.blockNumber, 1, 10);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Block State:" + this._target.blockState);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if(GUILayout.Button("UP"))
                        this._target.blockState = BlockInfo.BlockState.UP;
                    if(GUILayout.Button("DOWN"))
                        this._target.blockState = BlockInfo.BlockState.DOWN;
                    EditorGUILayout.EndHorizontal();
                    break;

                case BlockInfo.BlockTypes.SWITCH:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Switch Block Count:" + this._target.switchBlockCount);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    this._target.switchBlockCount = EditorGUILayout.IntSlider(this._target.switchBlockCount, 1, 10);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Block State:" + this._target.blockState);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if(GUILayout.Button("UP"))
                        this._target.blockState = BlockInfo.BlockState.UP;
                    if(GUILayout.Button("DOWN"))
                        this._target.blockState = BlockInfo.BlockState.DOWN;
                    EditorGUILayout.EndHorizontal();
                    break;

                case BlockInfo.BlockTypes.SWTICH_EMPTY:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Block Start State:" + this._target.blockState);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if(GUILayout.Button("UP"))
                        this._target.blockState = BlockInfo.BlockState.UP;
                    if(GUILayout.Button("DOWN"))
                        this._target.blockState = BlockInfo.BlockState.DOWN;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.LabelField("PLEASE PLACE EMPTY SWITCH BLOCKS.");
                    break;

                case BlockInfo.BlockTypes.STUN:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Countdown Number:" + this._target.blockNumber);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    this._target.blockNumber = EditorGUILayout.IntSlider(this._target.blockNumber, 1, 10);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Block State:" + this._target.blockState);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if(GUILayout.Button("UP"))
                        this._target.blockState = BlockInfo.BlockState.UP;
                    if(GUILayout.Button("DOWN"))
                        this._target.blockState = BlockInfo.BlockState.DOWN;
                    EditorGUILayout.EndHorizontal();

                    break;

                case BlockInfo.BlockTypes.WARP:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Warp Direction:" + this._target.blockOneDirection);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if(GUILayout.Button("UP"))
                        this._target.blockOneDirection = BlockInfo.BlockDirection.UP;
                    if(GUILayout.Button("RIGHT"))
                        this._target.blockOneDirection = BlockInfo.BlockDirection.RIGHT;
                    if(GUILayout.Button("DOWN"))
                        this._target.blockOneDirection = BlockInfo.BlockDirection.DOWN;
                    if(GUILayout.Button("LEFT"))
                        this._target.blockOneDirection = BlockInfo.BlockDirection.LEFT;
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Block State:" + this._target.blockState);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if(GUILayout.Button("UP"))
                        this._target.blockState = BlockInfo.BlockState.UP;
                    if(GUILayout.Button("DOWN"))
                        this._target.blockState = BlockInfo.BlockState.DOWN;
                    EditorGUILayout.EndHorizontal();
                    break;

                case BlockInfo.BlockTypes.WARP_NODE:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("PLACE A WARP NODE: ");
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Warp Direction:" + this._target.blockOneDirection);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if(GUILayout.Button("UP"))
                        this._target.blockOneDirection = BlockInfo.BlockDirection.UP;
                    if(GUILayout.Button("RIGHT"))
                        this._target.blockOneDirection = BlockInfo.BlockDirection.RIGHT;
                    if(GUILayout.Button("DOWN"))
                        this._target.blockOneDirection = BlockInfo.BlockDirection.DOWN;
                    if(GUILayout.Button("LEFT"))
                        this._target.blockOneDirection = BlockInfo.BlockDirection.LEFT;
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Block State:" + this._target.blockState);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if(GUILayout.Button("UP"))
                        this._target.blockState = BlockInfo.BlockState.UP;
                    if(GUILayout.Button("DOWN"))
                        this._target.blockState = BlockInfo.BlockState.DOWN;
                    EditorGUILayout.EndHorizontal();
                    break;

                default:
                    EditorGUILayout.LabelField("Please Select A Block Type.");
                    break;
                    
            }
        }

        private void ResetValues() {
            this._target.blockToPlace = BlockInfo.BlockTypes.NONE;
            this._target.blockOneDirection = BlockInfo.BlockDirection.NONE;
            this._target.blockTwoDirection = BlockInfo.BlockDirection.NONE;
            this._target.blockState = BlockInfo.BlockState.NONE;
            this._target.blockNumber = 0;
            this._target.switchBlockCount = 0;
        }
    }
}