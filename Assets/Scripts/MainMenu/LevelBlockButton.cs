namespace MainMenu {

    using System.Collections;

    using UnityEngine;

    using Input;
    using GameInfo;

#if UNITY_IPHONE || UNITY_ANDROID
    public class LevelBlockButton : MonoBehaviour, ITouchable {
#else
    public class LevelBlockButton : MonoBehaviour {
#endif
        private int _id = 0;

        public Material buttonEnter;
        public Material buttonExit;
        public Material lockedMaterial;
        public Material starMaterial;
        private Material[] _blockMaterials;
        private MeshRenderer _blockRenderer;

        private TextMesh _blockText;

        private int _movesMade;
        private int _maxMoves;

        private bool _mazeLocked = true;

        void Awake() {
            this._blockText = this.transform.GetChild(0).GetComponent<TextMesh>() as TextMesh;
            this._blockMaterials = this.renderer.materials;
            this._blockRenderer = this.renderer as MeshRenderer;
        }

        public void SetId(int id) {
            this._id = id;
            this._blockText.text = _id.ToString();

            this._movesMade = MazeInfo.MazeMoveValue[this._id - 1].moveCount;
            this._maxMoves = MazeInfo.MazeMoveValue[this._id - 1].maxMoves;

            if(this._movesMade == -1) {
                this._mazeLocked = true;
                this._blockMaterials[1] = this.buttonExit;
                this._blockMaterials[0] = this.lockedMaterial;
                this._blockRenderer.materials = this._blockMaterials;
            } else if(this._movesMade == 0) {
                this._mazeLocked = false;
                this._blockMaterials[1] = this.buttonExit;
                this._blockMaterials[0] = null;
                this._blockRenderer.materials = this._blockMaterials;
            } else if(this._movesMade > 0 && this._movesMade <= this._maxMoves) {
                this._mazeLocked = false;
                this._blockMaterials[1] = this.buttonExit;
                this._blockMaterials[0] = this.starMaterial;
                this._blockRenderer.materials = this._blockMaterials;
            } else {
                this._mazeLocked = false;
                this._blockMaterials[1] = this.buttonExit;
                this._blockMaterials[0] = null;
                this._blockRenderer.materials = this._blockMaterials;
            }
        }

#if UNITY_IPHONE || UNITY_ANDROID
        public void OnTouchBegan() {
            this._blockMaterials[1] = this.buttonEnter;
            this._blockRenderer.materials = this._blockMaterials;
        }

        public void OnTouchEnded() {
            this._blockMaterials[1] = this.buttonExit;
            this._blockRenderer.materials = this._blockMaterials;
            if(!this._mazeLocked)
                GameController.instance.LoadLevelAt(this._id);
        }

        public void OnTouchMoved() {
            this._blockMaterials[1] = this.buttonExit;
            this._blockRenderer.materials = this._blockMaterials;
        }

        public void OnTouchStayed() {
            this._blockMaterials[1] = this.buttonEnter;
            this._blockRenderer.materials = this._blockMaterials;
        }

        public void OnTouchCanceled() {
            this._blockMaterials[1] = this.buttonExit;
            this._blockRenderer.materials = this._blockMaterials;
        }

        public void OnTouchEndedGlobal() {
            this._blockMaterials[1] = this.buttonExit;
            this._blockRenderer.materials = this._blockMaterials;
        }

        public void OnTouchMovedGlobal() {
            this._blockMaterials[1] = this.buttonExit;
            this._blockRenderer.materials = this._blockMaterials;
        }

        public void OnTouchStayedGlobal() {
            this._blockMaterials[1] = this.buttonEnter;
            this._blockRenderer.materials = this._blockMaterials;
        }

        public void OnTouchCanceledGlobal() {
            this._blockMaterials[1] = this.buttonExit;
            this._blockRenderer.materials = this._blockMaterials;
        }
#else
        public void OnMouseEnter() {
            this._blockMaterials[1] = this.buttonEnter;
            this._blockRenderer.materials = this._blockMaterials;
        }

        public void OnMouseExit() {
            this._blockMaterials[1] = this.buttonExit;
            this._blockRenderer.materials = this._blockMaterials;
        }

        private void OnMouseUp() {
            this._blockMaterials[1] = this.buttonExit;
            this._blockRenderer.materials = this._blockMaterials;
            if(!this._mazeLocked)
                GameController.instance.LoadLevelAt(this._id);
        }
#endif
    }
}