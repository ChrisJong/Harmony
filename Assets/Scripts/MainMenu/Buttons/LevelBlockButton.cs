namespace MainMenu {

    using System.Collections;

    using UnityEngine;

    using Input;
    using GameInfo;
    using Resource;

#if UNITY_IPHONE || UNITY_ANDROID
    public class LevelBlockButton : MonoBehaviour, ITouchable {
#else
    public class LevelBlockButton : MonoBehaviour {
#endif
        private int _id = 0;

        public Material tileEnter;
        public Material tileExit;
        public Material lockedMaterial;
        public Material starMaterial;

        private int _movesMade;
        private int _maxMoves;
        private int _materialID;

        private bool _mazeLocked = true;

        public MeshRenderer _renderer;
        public Material[] _blockMaterials;

        private TextMesh _blockText;

        void Awake() {
            this._blockText = this.transform.GetChild(0).GetComponent<TextMesh>() as TextMesh;
            this._blockMaterials = new Material[2];
            this._renderer = this.renderer as MeshRenderer;
        }

        public void SetupSkin() {
            this._materialID = TileManager.instance.ChangeMaterialID();
            this.tileEnter = TileManager.instance.GetCurrentSkinMaterial("up", this._materialID);
            this.tileExit = TileManager.instance.GetCurrentSkinMaterial("down", this._materialID);

            if(this._mazeLocked) {
                this._blockMaterials[0] = this.tileExit;
                this._blockMaterials[1] = this.lockedMaterial;
                this._renderer.materials = new Material[2];
                this._renderer.materials = this._blockMaterials;
            } else {
                if(this._movesMade == 0) {
                    this._renderer.material = this.tileExit;
                } else if(this._movesMade > 0 && this._movesMade <= this._maxMoves) {
                    this._blockMaterials[0] = this.tileExit;
                    this._blockMaterials[1] = this.starMaterial;
                    this._renderer.materials = new Material[2];
                    this._renderer.materials = this._blockMaterials;
                } else {
                    this._renderer.material = this.tileExit;
                }
            }
        }

        public void SetID(int id) {
            this._id = id;
            this._blockText.text = _id.ToString();

            this._movesMade = MazeInfo.MazeMoveValue[this._id - 1].moveCount;
            this._maxMoves = MazeInfo.MazeMoveValue[this._id - 1].maxMoves;

            if(this._movesMade == -1) {
                this._mazeLocked = true;
                this.SetupSkin();
            } else if(this._movesMade == 0) {
                this._mazeLocked = false;
                this.SetupSkin();
            } else if(this._movesMade > 0 && this._movesMade <= this._maxMoves) {
                this._mazeLocked = false;
                this.SetupSkin();
            } else {
                this._mazeLocked = false;
                this.SetupSkin();
            }
        }

#if UNITY_IPHONE || UNITY_ANDROID
        public void OnTouchBegan() {
            this._renderer.material = this.tileEnter;
        }

        public void OnTouchEnded() {
            this._renderer.material = this.tileExit;
            if(!this._mazeLocked)
                GameController.instance.LoadLevelAt(this._id);
        }

        public void OnTouchMoved() {
            this._renderer.material = this.tileExit;
        }

        public void OnTouchStayed() {
            this._renderer.material = this.tileEnter;
        }

        public void OnTouchCanceled() {
            this._renderer.material = this.tileExit;
        }

        public void OnTouchEndedGlobal() {
            this._renderer.material = this.tileExit;
        }

        public void OnTouchMovedGlobal() {
            this._renderer.material = this.tileExit;
        }

        public void OnTouchStayedGlobal() {
            this._renderer.material = this.tileEnter;
        }

        public void OnTouchCanceledGlobal() {
            this._renderer.material = this.tileExit;
        }
#else
        public void OnMouseEnter() {
            this._renderer.material = this.tileEnter;
        }

        public void OnMouseExit() {
            this._renderer.material = this.tileExit;
        }

        private void OnMouseUp() {
            this._renderer.material = this.tileExit;
            if(!MainMenuController.instance.isActive) {
                if(!this._mazeLocked) {
                    MainMenuController.instance.levelID = this._id;
                    MainMenuController.instance.isActive = true;
                    MainMenuController.instance.fade.PlayFadeToMax();
                }
            }
        }
#endif
    }
}