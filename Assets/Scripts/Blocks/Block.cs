namespace Blocks {

    using UnityEngine;
    using System.Collections;

    using Grid;
    using GameInfo;

    [DisallowMultipleComponent]
    public class Block : MonoBehaviour {

        private float _blockSpeed;
        private Vector3 _blockPosition;
        //private float _blockHeight;

        public BlockInfo.BlockType blockType;
        public BlockInfo.BlockState blockState;
        public bool frozen;

        void Awake() {
            this._blockPosition = this.transform.position;
        }

        void Update() {
            if(this.blockState == BlockInfo.BlockState.NONE)
                return;

            if(this.blockState == BlockInfo.BlockState.UP)
                MoveUp();
            if(this.blockState == BlockInfo.BlockState.DOWN)
                MoveDown();
        }

        public void MoveUp() {
            this._blockPosition = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
            this.transform.position = this._blockPosition;
            if(this.transform.position.y >= 1.0f) {
                this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
                this._blockPosition = this.transform.position;
                this.blockState = BlockInfo.BlockState.NONE;
                return;
            }
        }

        public void MoveDown() {
            this._blockPosition = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
            this.transform.position = this._blockPosition;
            if(this.transform.position.y <= 0.0f) {
                this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
                this._blockPosition = this.transform.position;
                this.blockState = BlockInfo.BlockState.NONE;
                return;
            }
        }

        public void SetType(BlockInfo.BlockType type) {
            switch(type) {
                case BlockInfo.BlockType.UP:
                this.blockType = BlockInfo.BlockType.UP;
                break;

                case BlockInfo.BlockType.RIGHT:
                this.blockType = BlockInfo.BlockType.RIGHT;
                break;

                case BlockInfo.BlockType.DOWN:
                this.blockType = BlockInfo.BlockType.DOWN;
                break;

                case BlockInfo.BlockType.LEFT:
                this.blockType = BlockInfo.BlockType.LEFT;
                break;

                case BlockInfo.BlockType.MULTILEFTRIGHT:
                this.blockType = BlockInfo.BlockType.MULTILEFTRIGHT;
                break;

                case BlockInfo.BlockType.MULTIUPDOWN:
                this.blockType = BlockInfo.BlockType.MULTIUPDOWN;
                break;

                case BlockInfo.BlockType.EMPTYDOWN:
                this.blockType = BlockInfo.BlockType.EMPTYDOWN;
                this.frozen = true;
                break;

                case BlockInfo.BlockType.EMPTYUP:
                this.blockState = BlockInfo.BlockState.UP;
                this.blockType = BlockInfo.BlockType.EMPTYUP;
                this.frozen = true;
                break;

                default:
                this.blockState = BlockInfo.BlockState.NONE;
                this.blockType = BlockInfo.BlockType.EMPTYDOWN;
                break;
            }
        }
    }
}
