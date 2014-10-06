namespace Grid {

    using System.Collections;

    using UnityEngine;
    using UnityEditor;


    public class GridMapEditorWindow : EditorWindow {

        private GridMap _target;

        void Awake() {
            this.title = "Place Blocks";
        }

        public void Init() {
            this._target = (GridMap)FindObjectOfType(typeof(GridMap));
        }

        void OnGUI() {

        }
    }
}