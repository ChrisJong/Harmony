namespace Blocks {

    using System.Collections;

    using UnityEngine;

    using GameInfo;

    [System.Serializable]
    public class SwitchEmptyBlock : BlockClass {
        public GameObject parentNode;

        private Material _blockMaterial;

        void Awake() {
            this._blockMaterial = this.blockRenderer.material;
        }

        void Update() {
            if(this.BlockState == BlockInfo.BlockState.NONE)
                return;

            if(this.BlockState == BlockInfo.BlockState.UP)
                this.MoveUp();
            if(this.BlockState == BlockInfo.BlockState.DOWN)
                this.MoveDown();
        }

        public void SetParentNode(GameObject parent) {
            if(parent == null)
                return;

            this.parentNode = parent;
        }

        public void RemoveParentNode() {
            if(this.parentNode == null)
                return;

            this.parentNode = null;
        }

        public void RemoveFromParent() {
            if(this.parentNode == null)
                return;

            this.parentNode.GetComponent<SwitchBlock>().RemoveEmptyBlock(this.gameObject);
            this.parentNode = null;
        }

        public override void MoveUp() {
            this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
            this.isUp = true;
            this.BlockState = BlockInfo.BlockState.NONE;

            this._blockMaterial = this.tileUpMaterials[this.MaterialID];
            this.blockRenderer.material = this._blockMaterial;
        }

        public override void MoveDown() {
            this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
            this.isUp = false;
            this.BlockState = BlockInfo.BlockState.NONE;

            this._blockMaterial = this.tileDownMaterials[this.MaterialID];
            this.blockRenderer.material = this._blockMaterial;
        }

        public override void SetupBlock(BlockInfo.BlockTypes type) {
            base.SetupBlock(type);
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockState state) {
            base.SetupBlock(type, state);
        }
    }
}