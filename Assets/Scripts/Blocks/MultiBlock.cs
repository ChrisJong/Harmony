namespace Blocks {

    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;

    public class MultiBlock : BlockClass {

        public List<Material> blockUpAlphaMaterials;
        public List<Material> blockDownAlphaMaterials;
        private Material[] _blockMaterials;

        void Awake() {
            this._blockMaterials = this.blockRenderer.materials;
        }

        void Update() {
            if(this.BlockState == BlockInfo.BlockState.NONE)
                return;

            if(this.BlockState == BlockInfo.BlockState.UP)
                this.MoveUp();
            if(this.BlockState == BlockInfo.BlockState.DOWN)
                this.MoveDown();
        }

        public override void MoveUp() {
            this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
            this.isUp = true;
            this.BlockState = BlockInfo.BlockState.NONE;

            this._blockMaterials[0] = this.blockUpMaterials[this.firstDirectionValue - 1];
            this._blockMaterials[1] = this.blockUpAlphaMaterials[this.secondDirectionValue - 1];
            this.blockRenderer.materials = this._blockMaterials;
        }

        public override void MoveDown() {
            this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
            this.isUp = false;
            this.BlockState = BlockInfo.BlockState.NONE;

            this._blockMaterials[0] = this.blockDownMaterials[this.firstDirectionValue - 1];
            this._blockMaterials[1] = this.blockDownAlphaMaterials[this.secondDirectionValue - 1];
            this.blockRenderer.materials = this._blockMaterials;
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockDirection firstDirection, BlockInfo.BlockDirection secondDirection) {
            base.SetupBlock(type, firstDirection, secondDirection);

            Material[] mats = this.blockRenderer.sharedMaterials;
            mats[0] = this.blockDownMaterials[(int)firstDirection - 1];
            mats[1] = this.blockDownAlphaMaterials[(int)secondDirection - 1];
            this.blockRenderer.sharedMaterials = mats;
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockDirection firstDirection, BlockInfo.BlockDirection secondDirection, BlockInfo.BlockState state) {
            base.SetupBlock(type, firstDirection, secondDirection, state);

            Material[] mats = this.blockRenderer.sharedMaterials;

            if(state == BlockInfo.BlockState.UP){
                this.isUp = true;
                mats[0] = this.blockUpMaterials[(int)firstDirection - 1];
                mats[1] = this.blockUpAlphaMaterials[(int)secondDirection - 1];
                this.blockRenderer.sharedMaterials = mats;
            } else {
                mats[0] = this.blockDownMaterials[(int)firstDirection - 1];
                mats[1] = this.blockDownAlphaMaterials[(int)secondDirection - 1];
                this.blockRenderer.sharedMaterials = mats;
            }
        }
    }
}