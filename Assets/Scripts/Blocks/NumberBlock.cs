namespace Blocks {

    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;

    public class NumberBlock : BlockClass {

        public List<Material> numberMaterials;

        public int previousCounter;
        public int currentCounter;
        public int maxCounter;
        public bool isReversed;
        public bool wasUp;

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

        public void Init() {
            if(this.BlockState == BlockInfo.BlockState.UP) {
                //this._blockState = BlockInfo.BlockState.UP;
                this.isUp = true;
                this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
                this.BlockState = BlockInfo.BlockState.NONE;
            } else {
                //this._blockState = BlockInfo.BlockState.DOWN;
                this.isUp = false;
                this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
                this.BlockState = BlockInfo.BlockState.NONE;
            }
        }

        public void Undo() {
            if(this.isUp){
                this.currentCounter = previousCounter;
                this._blockMaterials[0] = this.tileDownMaterial;
                this._blockMaterials[1] = this.numberMaterials[currentCounter];
                this.blockRenderer.materials = this._blockMaterials;
                this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
                this.isUp = false;
                this.BlockState = BlockInfo.BlockState.NONE;
            } else {
                this.currentCounter = previousCounter;
                if(this.previousCounter == 0) {
                    this._blockMaterials[0] = this.tileUpMaterial;
                    this._blockMaterials[1] = this.numberMaterials[currentCounter];
                    this.blockRenderer.materials = this._blockMaterials;
                    this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
                    this.isUp = true;
                } else {
                    this._blockMaterials[0] = this.tileDownMaterial;
                    this._blockMaterials[1] = this.numberMaterials[currentCounter];
                    this.blockRenderer.materials = this._blockMaterials;
                    this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
                    this.isUp = false;
                }
                this.BlockState = BlockInfo.BlockState.NONE;
            }
        }

        public override void MoveUp() {
            if(this.isReversed) {
                //this._blockState = BlockInfo.BlockState.DOWN;
                this.currentCounter--;
                this._blockMaterials[0] = this.tileDownMaterial;
                this._blockMaterials[1] = this.numberMaterials[currentCounter];
                this.blockRenderer.materials = this._blockMaterials;
                this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
                this.isUp = false;
                this.BlockState = BlockInfo.BlockState.NONE;
            } else {
                //this._blockState = BlockInfo.BlockState.UP;
                this.previousCounter = currentCounter;
                this.currentCounter--;
                this._blockMaterials[0] = this.tileUpMaterial;
                this._blockMaterials[1] = this.numberMaterials[currentCounter];
                this.blockRenderer.materials = this._blockMaterials;
                this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
                this.isUp = true;
                this.BlockState = BlockInfo.BlockState.NONE;
            }
        }

        public override void MoveDown() {
            if(this.isReversed) {
                if(!this.isUp) {
                    //this._blockState = BlockInfo.BlockState.UP;
                    this.currentCounter = this.maxCounter;
                    this.blockRenderer.material = this.tileUpMaterial;
                    this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
                    this.isUp = true;
                    this.BlockState = BlockInfo.BlockState.NONE;
                } else {
                    this.currentCounter--;
                    this.blockRenderer.material = this.tileUpMaterial;
                    this.BlockState = BlockInfo.BlockState.NONE;
                }
            } else {
                if(this.isUp) {
                    //this._blockState = BlockInfo.BlockState.DOWN;
                    this.previousCounter = currentCounter;
                    this.currentCounter = this.maxCounter;
                    this._blockMaterials[0] = this.tileDownMaterial;
                    this._blockMaterials[1] = this.numberMaterials[currentCounter];
                    this.blockRenderer.materials = this._blockMaterials;
                    this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
                    this.isUp = false;
                    this.BlockState = BlockInfo.BlockState.NONE;
                } else {
                    this.previousCounter = currentCounter;
                    this.currentCounter--;
                    this._blockMaterials[0] = this.tileDownMaterial;
                    this._blockMaterials[1] = this.numberMaterials[currentCounter];
                    this.blockRenderer.materials = this._blockMaterials;
                    this.BlockState = BlockInfo.BlockState.NONE;
                }
            }
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, int initCounter) {
            base.SetupBlock(type, initCounter);

            Material[] mats = this.blockRenderer.sharedMaterials;

            this.currentCounter = initCounter;
            this.previousCounter = initCounter;
            this.maxCounter = initCounter;

            mats[1] = this.numberMaterials[currentCounter];
            this.blockRenderer.materials = mats;

            this.isReversed = false;
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockState state, int initCounter) {
            base.SetupBlock(type, state, initCounter);

            Material[] mats = this.blockRenderer.sharedMaterials;

            this.currentCounter = initCounter;
            this.previousCounter = initCounter;
            this.maxCounter = initCounter;

            if(state == BlockInfo.BlockState.UP) {
                this.isUp = true;

                mats[1] = this.numberMaterials[currentCounter];
                this.blockRenderer.materials = mats;

                this.isReversed = true;
            } else {
                mats[1] = this.numberMaterials[currentCounter];
                this.blockRenderer.materials = mats;

                this.isReversed = false;
            }
        }
    }
}