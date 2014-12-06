namespace Blocks {

    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;

    public class StunBlock : BlockClass {

        public Material stunUpMaterial;
        public Material stunDownMaterial;

        public int stunCounter;

        private Material[] _blockMaterials;

        void Awake() {
            this._blockMaterials = this.blockRenderer.materials;
        }

        public void Init() {
            if(this.BlockState == BlockInfo.BlockState.UP) {
                this.isUp = true;
                this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
                this.BlockState = BlockInfo.BlockState.NONE;
            } else {
                this.isUp = false;
                this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
                this.BlockState = BlockInfo.BlockState.NONE;
            }
        }

        public override void MoveUp() {
        }

        public override void MoveDown() {
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, int initCounter) {
            base.SetupBlock(type, initCounter);

            Material[] mats = this.blockRenderer.sharedMaterials;

            this.stunCounter = initCounter;

            mats[1] = stunDownMaterial;
            this.blockRenderer.materials = mats;
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockState state, int initCounter) {
            base.SetupBlock(type, state, initCounter);

            Material[] mats = this.blockRenderer.sharedMaterials;

            this.stunCounter = initCounter;

            mats[1] = stunDownMaterial;
            this.blockRenderer.materials = mats;
        }
    }
}