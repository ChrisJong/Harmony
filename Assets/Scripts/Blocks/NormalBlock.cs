namespace Blocks {

    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;

    public class NormalBlock : BlockClass {

        public List<Material> arrowUpMaterials;
        public List<Material> arrowDownMaterials;
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

            //this.blockRenderer.material = this.tileUpMaterials[this.firstDirectionValue - 1];
            this._blockMaterials[0] = this.tileUpMaterials[this.MaterialID];
            this._blockMaterials[1] = this.arrowUpMaterials[this.firstDirectionValue - 1];
            this.blockRenderer.materials = this._blockMaterials;
        }

        public override void MoveDown() {
            this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
            this.isUp = false;
            this.BlockState = BlockInfo.BlockState.NONE;

            //this.blockRenderer.material = this.tileDownMaterials[this.firstDirectionValue - 1];
            this._blockMaterials[0] = this.tileDownMaterials[this.MaterialID];
            this._blockMaterials[1] = this.arrowDownMaterials[this.firstDirectionValue - 1];
            this.blockRenderer.materials = this._blockMaterials;

        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockDirection direction) {
            base.SetupBlock(type, direction);

            Material[] mats = this.blockRenderer.sharedMaterials;

            mats[0] = this.tileDownMaterials[this.MaterialID];
            mats[1] = this.arrowDownMaterials[(int)direction - 1];
            this.blockRenderer.sharedMaterials = mats;
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockDirection direction, BlockInfo.BlockState state) {
            base.SetupBlock(type, direction, state);

            Material[] mats = this.blockRenderer.sharedMaterials;

            if(state == BlockInfo.BlockState.UP) {
                this.isUp = true;
                mats[0] = this.tileUpMaterials[this.MaterialID];
                mats[1] = this.arrowUpMaterials[(int)direction - 1];
                this.blockRenderer.sharedMaterials = mats;
                //this.blockRenderer.material = this.tileUpMaterials[(int)direction - 1];

            } else {
                mats[0] = this.tileDownMaterials[this.MaterialID];
                mats[1] = this.arrowDownMaterials[(int)direction - 1];
                this.blockRenderer.sharedMaterials = mats;
                //this.blockRenderer.material = this.tileDownMaterials[(int)direction - 1];
            }
        }
    }
}