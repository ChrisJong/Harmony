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

            this._blockMaterials[0] = this.blockUpMaterials[this.firstDirectionValue - 1];
            this._blockMaterials[1] = this.blockUpAlphaMaterials[this.secondDirectionValue - 1];
            this.blockRenderer.materials = this._blockMaterials;
        }

        public override void MoveDown() {
            this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
            this.isUp = false;
            this.blockState = BlockInfo.BlockState.NONE;

            this._blockMaterials[0] = this.blockDownMaterials[this.firstDirectionValue - 1];
            this._blockMaterials[1] = this.blockDownAlphaMaterials[this.secondDirectionValue - 1];
            this.blockRenderer.materials = this._blockMaterials;
        }

        public override void SetupType(BlockInfo.BlockTypes type, BlockInfo.BlockDirection firstDirection, BlockInfo.BlockDirection secondDirection) {
            base.SetupType(type, firstDirection, secondDirection);

            Material[] mats = this.blockRenderer.sharedMaterials;
            mats[0] = this.blockDownMaterials[(int)firstDirection - 1];
            mats[1] = this.blockDownAlphaMaterials[(int)secondDirection - 1];
            this.blockRenderer.sharedMaterials = mats;
        }

        public override void SetupType(BlockInfo.BlockTypes type, BlockInfo.BlockDirection firstDirection, BlockInfo.BlockDirection secondDirection, BlockInfo.BlockState state) {
            base.SetupType(type, firstDirection, secondDirection, state);

            Material[] mats = this.blockRenderer.sharedMaterials;

            if(state == BlockInfo.BlockState.UP){
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