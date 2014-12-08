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

        public void Undo() {
            if(this.isReversed) {
                if(!this.isUp) {
                    this.currentCounter = previousCounter;
                    this._blockMaterials[0] = this.tileUpMaterial;
                    this._blockMaterials[1] = this.numberMaterials[currentCounter];
                    this.blockRenderer.materials = this._blockMaterials;
                    this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
                    this.isUp = true;
                    this.BlockState = BlockInfo.BlockState.NONE;
                } else {
                    this.currentCounter = previousCounter;
                    if(this.previousCounter == 0) {
                        this._blockMaterials[0] = this.tileDownMaterial;
                        this._blockMaterials[1] = this.numberMaterials[currentCounter];
                        this.blockRenderer.materials = this._blockMaterials;
                        this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
                        this.isUp = false;
                    } else {
                        this._blockMaterials[0] = this.tileUpMaterial;
                        this._blockMaterials[1] = this.numberMaterials[currentCounter];
                        this.blockRenderer.materials = this._blockMaterials;
                        this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
                        this.isUp = true;
                    }
                    this.BlockState = BlockInfo.BlockState.NONE;
                }
            } else {
                if(this.isUp) {
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
        }

        public override void ResetUndoState() {
        }

        public override void MoveUp() {
            if(this.isReversed) {
                if(!this.isUp) {
                    this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
                    this.isUp = true;

                    this.previousCounter = currentCounter;
                    this.currentCounter = this.maxCounter;

                    this._blockMaterials[0] = this.tileUpMaterial;
                    this._blockMaterials[1] = this.numberMaterials[currentCounter];
                    this.blockRenderer.materials = this._blockMaterials;

                    this.BlockState = BlockInfo.BlockState.NONE;
                } else {
                    this.isUp = true;

                    this.previousCounter = currentCounter;
                    this.currentCounter--;

                    this._blockMaterials[0] = this.tileUpMaterial;
                    this._blockMaterials[1] = this.numberMaterials[currentCounter];
                    this.blockRenderer.materials = this._blockMaterials;

                    this.BlockState = BlockInfo.BlockState.NONE;

                    this.BlockState = BlockInfo.BlockState.NONE;
                }
            } else {
                this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
                this.isUp = true;

                this.previousCounter = currentCounter;
                this.currentCounter--;

                this._blockMaterials[0] = this.tileUpMaterial;
                this._blockMaterials[1] = this.numberMaterials[currentCounter];
                this.blockRenderer.materials = this._blockMaterials;
                
                this.BlockState = BlockInfo.BlockState.NONE;
            }
        }

        public override void MoveDown() {
            if(this.isReversed) {
                this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
                this.isUp = false;

                this.previousCounter = currentCounter;
                this.currentCounter--;

                this._blockMaterials[0] = this.tileDownMaterial;
                this._blockMaterials[1] = this.numberMaterials[currentCounter];
                this.blockRenderer.materials = this._blockMaterials;

                this.BlockState = BlockInfo.BlockState.NONE;
            } else {
                if(this.isUp) {
                    this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
                    this.isUp = false;

                    this.previousCounter = currentCounter;
                    this.currentCounter = this.maxCounter;

                    this._blockMaterials[0] = this.tileDownMaterial;
                    this._blockMaterials[1] = this.numberMaterials[currentCounter];
                    this.blockRenderer.materials = this._blockMaterials;
                    
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

        public override void Init() {
            base.Init();

            if(this.isUp)
                this.isReversed = true;
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, int initCounter) {
            base.SetupBlock(type, initCounter);

            this.currentCounter = initCounter;
            this.previousCounter = initCounter;
            this.maxCounter = initCounter;

            this.blockMaterials[1] = this.numberMaterials[currentCounter];
            this.blockRenderer.materials = this.blockMaterials;

            this.isReversed = false;
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockState state, int initCounter) {
            base.SetupBlock(type, state, initCounter);

            this.currentCounter = initCounter;
            this.previousCounter = initCounter;
            this.maxCounter = initCounter;

            if(state == BlockInfo.BlockState.UP) {
                this.blockMaterials[1] = this.numberMaterials[currentCounter];
                this.blockRenderer.materials = this.blockMaterials;

                this.isReversed = true;
            } else {
                this.blockMaterials[1] = this.numberMaterials[currentCounter];
                this.blockRenderer.materials = this.blockMaterials;

                this.isReversed = false;
            }
        }
    }
}