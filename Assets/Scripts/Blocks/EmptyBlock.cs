namespace Blocks {

    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;

    public class EmptyBlock : BlockClass {

        public override void MoveUp() {
            this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
            this.isUp = true;
            this.blockState = BlockInfo.BlockState.NONE;
        }

        public override void MoveDown() {
            this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
            this.isUp = false;
            this.blockState = BlockInfo.BlockState.NONE;
        }

        public override void SetupType(BlockInfo.BlockTypes type) {
            base.SetupType(type);
        }

        public override void SetupType(BlockInfo.BlockTypes type, BlockInfo.BlockState state) {
            base.SetupType(type, state);

            if(state == BlockInfo.BlockState.UP)
                this.blockRenderer.material = this.blockUpMaterials[0];
            else
                this.blockRenderer.material = this.blockDownMaterials[0];
        }
    }
}