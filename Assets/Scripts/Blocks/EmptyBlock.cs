namespace Blocks {

    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;

    public class EmptyBlock : BlockClass {

        private int _materialID = 0;

        public override void MoveUp() {
            this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
            this.isUp = true;
            this.BlockState = BlockInfo.BlockState.NONE;
        }

        public override void MoveDown() {
            this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
            this.isUp = false;
            this.BlockState = BlockInfo.BlockState.NONE;
        }

        public override void SetupBlock(BlockInfo.BlockTypes type) {
            base.SetupBlock(type);
            this._materialID = Random.Range(0, this.blockUpMaterials.Count);
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockState state) {
            base.SetupBlock(type, state);
            this._materialID = Random.Range(0, this.blockUpMaterials.Count);

            if(state == BlockInfo.BlockState.UP)
                this.blockRenderer.material = this.blockUpMaterials[_materialID];
            else
                this.blockRenderer.material = this.blockDownMaterials[_materialID];
        }
    }
}