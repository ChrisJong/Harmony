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
    }
}