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
                this.ResetValues();
                this.Close();
            } else
                return;
        }

        public void Init() {
            this._target = (GridMap)FindObjectOfType(typeof(GridMap));
        }

        void OnGUI() {
            GUILayout.Space(20);
            this._target.blockToPlace = (BlockInfo.BlockTypes)EditorGUILayout.EnumPopup("Block Types: ", this._target.blockToPlace);
            GUILayout.Space(20);

            if(this._target.blockToPlace == BlockInfo.BlockTypes.MULTI) {
                this._target.blockOneDirection = (BlockInfo.BlockDirection)EditorGUILayout.EnumPopup("Direction One: ", this._target.blockOneDirection);
                this._target.blockTwoDirection = (BlockInfo.BlockDirection)EditorGUILayout.EnumPopup("Direction Two: ", this._target.blockTwoDirection);
                this._target.blockState = (BlockInfo.BlockState)EditorGUILayout.EnumPopup("Block State: ", this._target.blockState);
            } else if(this._target.blockToPlace == BlockInfo.BlockTypes.NUMBER) {
                this._target.blockNumber = EditorGUILayout.IntSlider("Countdown Number: ",this._target.blockNumber, 1, 10);
                this._target.blockState = (BlockInfo.BlockState)EditorGUILayout.EnumPopup("Block State: ", this._target.blockState);
            } else if(this._target.blockToPlace == BlockInfo.BlockTypes.STUN) {
                Debug.LogError("Not Implemented Yet.");
            } else if(this._target.blockToPlace == BlockInfo.BlockTypes.EMPTY) {
                this._target.blockState = (BlockInfo.BlockState)EditorGUILayout.EnumPopup("Block State: ", this._target.blockState);
            } else if(this._target.blockToPlace == BlockInfo.BlockTypes.NONE) {
                EditorGUILayout.LabelField("Please Select A Block Type.");
            }else{
                this._target.blockOneDirection = (BlockInfo.BlockDirection)EditorGUILayout.EnumPopup("Direction: ", this._target.blockOneDirection);
                this._target.blockState = (BlockInfo.BlockState)EditorGUILayout.EnumPopup("Block State: ", this._target.blockState);
            }
        }

        private void ResetValues() {
            this._target.blockToPlace = BlockInfo.BlockTypes.NONE;
            this._target.blockOneDirection = BlockInfo.BlockDirection.NONE;
            this._target.blockTwoDirection = BlockInfo.BlockDirection.NONE;
            this._target.blockState = BlockInfo.BlockState.NONE;
        }
    }
}