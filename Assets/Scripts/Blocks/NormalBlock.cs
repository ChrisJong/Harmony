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
            this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
            this.isUp = true;
            this.blockState = BlockInfo.BlockState.NONE;
            this.blockRenderer.material = this.blockUpMaterials[this.firstDirectionValue - 1];

        }

        public override void MoveDown() {
            this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
            this.isUp = false;
            this.blockState = BlockInfo.BlockState.NONE;
            this.blockRenderer.material = this.blockDownMaterials[this.firstDirectionValue - 1];

        }

        public override void SetupType(BlockInfo.BlockTypes type, BlockInfo.BlockDirection direction) {
            base.SetupType(type, direction);

            this.blockRenderer.material = this.blockDownMaterials[(int)direction - 1];
        }

        public override void SetupType(BlockInfo.BlockTypes type, BlockInfo.BlockDirection direction, BlockInfo.BlockState state) {
            base.SetupType(type, direction, state);

            if(state == BlockInfo.BlockState.UP)
                this.blockRenderer.material = this.blockUpMaterials[(int)direction - 1];
            else
                this.blockRenderer.material = this.blockDownMaterials[(int)direction - 1];
        }
    }
}