namespace Blocks {

    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;

    public class NumberBlock : BlockClass {

        public int previousCounter;
        public int currentCounter;
        public int maxCounter;
        public bool isReversed;

        void Update() {
            if(this.blockState == BlockInfo.BlockState.NONE)
                return;

            if(this.blockState == BlockInfo.BlockState.UP)
                this.MoveUp();
            if(this.blockState == BlockInfo.BlockState.DOWN)
                this.MoveDown();
        }

        public void Init() {
            if(this.blockState == BlockInfo.BlockState.UP) {
                //this.blockState = BlockInfo.BlockState.UP;
                this.isUp = true;
                this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
                this.blockState = BlockInfo.BlockState.NONE;
            } else {
                //this.blockState = BlockInfo.BlockState.DOWN;
                this.isUp = false;
                this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
                this.blockState = BlockInfo.BlockState.NONE;
            }
        }

        public override void MoveUp() {
            if(this.isReversed) {
                //this.blockState = BlockInfo.BlockState.DOWN;
                this.currentCounter--;
                this.blockRenderer.material = this.blockDownMaterials[currentCounter];
                this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
                this.isUp = false;
                this.blockState = BlockInfo.BlockState.NONE;
            } else {
                //this.blockState = BlockInfo.BlockState.UP;
                this.currentCounter--;
                this.blockRenderer.material = this.blockUpMaterials[currentCounter];
                this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
                this.isUp = true;
                this.blockState = BlockInfo.BlockState.NONE;
            }
        }

        public override void MoveDown() {
            if(this.isReversed) {
                if(!this.isUp) {
                    //this.blockState = BlockInfo.BlockState.UP;
                    this.currentCounter = this.maxCounter;
                    this.blockRenderer.material = this.blockUpMaterials[currentCounter];
                    this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
                    this.isUp = true;
                    this.blockState = BlockInfo.BlockState.NONE;
                } else {
                    this.currentCounter--;
                    this.blockRenderer.material = this.blockUpMaterials[currentCounter];
                    this.blockState = BlockInfo.BlockState.NONE;
                }
            } else {
                if(this.isUp) {
                    //this.blockState = BlockInfo.BlockState.DOWN;
                    this.currentCounter = this.maxCounter;
                    this.blockRenderer.material = this.blockDownMaterials[currentCounter];
                    this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
                    this.isUp = false;
                    this.blockState = BlockInfo.BlockState.NONE;
                } else {
                    this.currentCounter--;
                    this.blockRenderer.material = this.blockDownMaterials[currentCounter];
                    this.blockState = BlockInfo.BlockState.NONE;
                }
            }
        }

        public override void SetupType(BlockInfo.BlockTypes type, int initCounter) {
            base.SetupType(type, initCounter);

            this.currentCounter = initCounter;
            this.previousCounter = initCounter;
            this.maxCounter = initCounter;
            this.blockRenderer.material = this.blockDownMaterials[initCounter];
            this.isReversed = false;
        }

        public override void SetupType(BlockInfo.BlockTypes type, BlockInfo.BlockState state, int initCounter) {
            base.SetupType(type, state, initCounter);

            this.currentCounter = initCounter;
            this.previousCounter = initCounter;
            this.maxCounter = initCounter;

            if(state == BlockInfo.BlockState.UP) {
                this.blockRenderer.material = this.blockUpMaterials[initCounter];
                this.isReversed = true;
            } else {
                this.blockRenderer.material = this.blockDownMaterials[initCounter];
                this.isReversed = false;
            }
        }
    }
}