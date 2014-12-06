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

        public override void MoveUp() {
            //throw new System.NotImplementedException();
        }

        public override void MoveDown() {
            //throw new System.NotImplementedException();
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