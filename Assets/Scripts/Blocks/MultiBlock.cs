namespace Blocks {

    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;

    public class MultiBlock : BlockClass {

        public List<Material> blockAlphaMaterial;

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

        public override void SetupType(BlockInfo.BlockTypes type, BlockInfo.BlockDirection firstDirection, BlockInfo.BlockDirection secondDirection) {
            base.SetupType(type, firstDirection, secondDirection);

            Material[] mats = this.blockRenderer.sharedMaterials;
            mats[0] = this.blockMaterials[(int)firstDirection - 1];
            mats[1] = this.blockAlphaMaterial[(int)secondDirection - 1];
            this.blockRenderer.sharedMaterials = mats;
        }

        public override void SetupType(BlockInfo.BlockTypes type, BlockInfo.BlockDirection firstDirection, BlockInfo.BlockDirection secondDirection, BlockInfo.BlockState state) {
            base.SetupType(type, firstDirection, secondDirection, state);

            Material[] mats = this.blockRenderer.sharedMaterials;
            mats[0] = this.blockMaterials[(int)firstDirection - 1];
            mats[1] = this.blockAlphaMaterial[(int)secondDirection - 1];
            this.blockRenderer.sharedMaterials = mats;
        }
    }
}