namespace MainMenu {

    using System.Collections;

    using UnityEngine;

    using GameInfo;

    public class LevelBlockButton : MonoBehaviour {
        private int _id = 0;

        public Material normalMaterial;
        public Material lockedMaterial;
        public Material ribbonMaterial;

        private TextMesh _blockText;

        private string _mazeTitle;
        private string _mazeInfo;

        private int _movesMade;
        private int _maxMoves;

        private bool _mazeLocked = true;

        void Awake() {
            this._blockText = this.transform.GetChild(0).GetComponent<TextMesh>() as TextMesh;
        }

        public void SetId(int id) {
            this._id = id;
            this._blockText.text = _id.ToString();

            this._mazeTitle = MazeInfo.MazeName + " " + this._id.ToString();
            this._movesMade = MazeInfo.MazeMoveValue[this._id - 1][0];
            this._maxMoves = MazeInfo.MazeMoveValue[this._id - 1][1];

            if(this._movesMade == -1) {
                this._mazeLocked = true;
                this.gameObject.renderer.material = this.lockedMaterial;
                this._mazeInfo = "Maze " + this._id.ToString() + " Locked";
                return;
            } else if(this._movesMade == 0) {
                this._mazeLocked = false;
                this.gameObject.renderer.material = normalMaterial;
                this._mazeInfo = "New Maze: " + this._id.ToString() + '\n' + "??" + " out of " + this._maxMoves + " moves";
                return;
            } else if(this._movesMade > 0 && this._movesMade <= this._maxMoves) {
                this._mazeLocked = false;
                this.gameObject.renderer.material = ribbonMaterial;
                this._mazeInfo = "Best Record: " + this._mazeTitle + '\n' + this._movesMade + " out of " + this._maxMoves + " moves";
                return;
            } else {
                this._mazeLocked = false;
                this.gameObject.renderer.material = normalMaterial;
                this._mazeInfo = "Completed: " + this._mazeTitle + '\n' + this._movesMade + " out of " + this._maxMoves + " moves";
                return;
            }
        }

        public void OnMouseEnter() {
            LevelSelectController.instance.informationField.text = this._mazeInfo;
        }

        public void OnMouseExit() {
            LevelSelectController.instance.informationField.text = null;
        }

        private void OnMouseUp() {
            if(!this._mazeLocked)
                GameController.instance.LoadLevelAt(this._id);
        }
    }
}