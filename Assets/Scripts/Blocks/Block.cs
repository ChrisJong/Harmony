namespace Blocks {

    using UnityEngine;
    using System.Collections;

    using Grid;
    using Constants;

    [DisallowMultipleComponent]
    public class Block : MonoBehaviour {

        private float _blockSpeed;
        private Vector3 _blockPosition;
        //private float _blockHeight;

        public BlockValues.BlockType blockType;
        public BlockValues.BlockState blockState;
        public bool frozen;

        void Awake() {
            this._blockPosition = this.transform.position;
        }

        void Update() {
            if(this.blockState == BlockValues.BlockState.NONE)
                return;

            if(this.blockState == BlockValues.BlockState.UP)
                MoveUp();
            if(this.blockState == BlockValues.BlockState.DOWN)
                MoveDown();
        }

        public void MoveUp() {
            this._blockPosition = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
            this.transform.position = this._blockPosition;
            if(this.transform.position.y >= 1.0f) {
                this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
                this._blockPosition = this.transform.position;
                this.blockState = BlockValues.BlockState.NONE;
                return;
            }
        }

        public void MoveDown() {
            this._blockPosition = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
            this.transform.position = this._blockPosition;
            if(this.transform.position.y <= 0.0f) {
                this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
                this._blockPosition = this.transform.position;
                this.blockState = BlockValues.BlockState.NONE;
                return;
            }
        }

        public void SetType(BlockValues.BlockType type) {
            switch(type) {
                case BlockValues.BlockType.UP:
                this.blockType = BlockValues.BlockType.UP;
                break;

                case BlockValues.BlockType.RIGHT:
                this.blockType = BlockValues.BlockType.RIGHT;
                break;

                case BlockValues.BlockType.DOWN:
                this.blockType = BlockValues.BlockType.DOWN;
                break;

                case BlockValues.BlockType.LEFT:
                this.blockType = BlockValues.BlockType.LEFT;
                break;

                case BlockValues.BlockType.MULTILEFTRIGHT:
                this.blockType = BlockValues.BlockType.MULTILEFTRIGHT;
                break;

                case BlockValues.BlockType.MULTIUPDOWN:
                this.blockType = BlockValues.BlockType.MULTIUPDOWN;
                break;

                case BlockValues.BlockType.EMPTYDOWN:
                this.blockType = BlockValues.BlockType.EMPTYDOWN;
                this.frozen = true;
                break;

                case BlockValues.BlockType.EMPTYUP:
                this.blockState = BlockValues.BlockState.UP;
                this.blockType = BlockValues.BlockType.EMPTYUP;
                this.frozen = true;
                break;

                default:
                this.blockState = BlockValues.BlockState.NONE;
                this.blockType = BlockValues.BlockType.EMPTYDOWN;
                break;
            }
        }
    }
}
