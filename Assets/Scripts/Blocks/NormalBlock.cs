namespace Blocks {

    using UnityEngine;

    using GameInfo;

    public class NormalBlock : BlockClass {

        void Update() {
            if(this.blockState == BlockInfo.BlockState.NONE)
                return;

            if(this.blockState == BlockInfo.BlockState.UP)
                this.MoveUp();
            if(this.blockState == BlockInfo.BlockState.DOWN)
                this.MoveDown();
        }

        public override void MoveUp() {
            if(this.transform.position.y >= 1.0f)
                return;

            this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
            this.isUp = true;
            this.blockState = BlockInfo.BlockState.NONE;
        }

        public override void MoveDown() {
            if(this.transform.position.y <= 0.0f)
                return;

            this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
            this.isUp = false;
            this.blockState = BlockInfo.BlockState.NONE;
        }

        public override void SetupType(BlockInfo.BlockTypes type, BlockInfo.BlockDirection direction) {
            base.SetupType(type, direction);

            this.blockRenderer.material = this.blockMaterials[(int)direction - 1];
        }

        public override void SetupType(BlockInfo.BlockTypes type, BlockInfo.BlockDirection direction, BlockInfo.BlockState state) {
            base.SetupType(type, direction, state);

            this.blockRenderer.material = this.blockMaterials[(int)direction - 1];
        }
    }
}