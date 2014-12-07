namespace Blocks {

    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;

    public class StunBlock : BlockClass {

        public Material stunUpMaterial;
        public Material stunDownMaterial;
        public Material[] blockMaterials;

        public int stunCounter;
        public bool isEnabled = false;

        private float _timer = 0.0f;
        private float _maxTimer = 1.0f;

        void Awake() {
            this.blockMaterials = this.blockRenderer.materials;
        }

        void Update() {
            if(this.isEnabled) {
                this._timer += Time.deltaTime;
                if(this._timer >= this._maxTimer) {
                    this.isEnabled = false;
                    this._timer = 0.0f;
                    this.blockMaterials[1] = this.stunDownMaterial;
                    this.blockRenderer.materials = this.blockMaterials;
                }
            }
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