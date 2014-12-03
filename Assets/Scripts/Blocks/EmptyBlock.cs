namespace Blocks {

    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;

    public class EmptyBlock : BlockClass {

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
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockState state) {
            base.SetupBlock(type, state);
        }
    }
}