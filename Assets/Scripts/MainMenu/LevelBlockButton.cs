namespace MainMenu {

    using System.Collections;

    using UnityEngine;

    using GameInfo;

    public class LevelBlockButton : MonoBehaviour {
        private int _id = 0;

        public Material normalMaterial;
        public Material betterMaterial;

        private TextMesh _blockText;

        private string _mazeTitle;
        private string _mazeInfo;

        private int _movesMade;
        private int _maxMoves;

        void Awake() {
            this._blockText = this.transform.GetChild(0).GetComponent<TextMesh>() as TextMesh;
        }

        public void SetId(int id) {
            this._id = id;
            this._blockText.text = _id.ToString();

            this._mazeTitle = MazeInfo.MazeName + " " + this._id.ToString();
            this._movesMade = MazeInfo.MazeMoveValue[this._id - 1][0];
            this._maxMoves = MazeInfo.MazeMoveValue[this._id - 1][1];

            if(this._movesMade == 0) {
                this.gameObject.renderer.material = normalMaterial;
                this._mazeInfo = "New Maze: " + this._id.ToString() + '\n' + "??" + " out of " + this._maxMoves + " moves";
            } else if(this._movesMade > 0 && this._movesMade <= this._maxMoves) {
                this.gameObject.renderer.material = betterMaterial;
                this._mazeInfo = "Best Record: " + this._mazeTitle + '\n' + this._movesMade + " out of " + this._maxMoves + " moves";
            } else {
                this.gameObject.renderer.material = normalMaterial;
                this._mazeInfo = "Completed: " + this._mazeTitle + '\n' + this._movesMade + " out of " + this._maxMoves + " moves";
            }
        }

        public void OnMouseEnter() {
            LevelSelectController.instance.informationField.text = this._mazeInfo;
        }

        public void OnMouseExit() {
            LevelSelectController.instance.informationField.text = null;
        }

        private void OnMouseUp() {
            GameController.instance.LoadLevelAt(this._id);
        }
    }
}