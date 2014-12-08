namespace Blocks {

    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;
    using Helpers;

    [System.Serializable]
    public class MultiBlock : BlockClass {

        [SerializeField, HideInInspector]
        private Material _firstArrowUpMaterial;
        [SerializeField, HideInInspector]
        private Material _firstArrowDownMaterial;
        [SerializeField, HideInInspector]
        private Material _secondArrowUpMaterial;
        [SerializeField, HideInInspector]
        private Material _secondArrowDownMaterial;

        void Update() {
            if(this.BlockState == BlockInfo.BlockState.NONE)
                return;

            if(this.BlockState == BlockInfo.BlockState.UP)
                this.MoveUp();
            if(this.BlockState == BlockInfo.BlockState.DOWN)
                this.MoveDown();
        }

        public override void ResetUndoState() {
        }

        public override void MoveUp() {
            this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
            this.isUp = true;
            this.BlockState = BlockInfo.BlockState.NONE;
            this.PreviousState = BlockInfo.BlockState.DOWN;

            this.blockMaterials[0] = this.tileUpMaterial;
            this.blockMaterials[1] = this._firstArrowUpMaterial;
            this.blockMaterials[2] = this._secondArrowUpMaterial;
            this.blockRenderer.materials = this.blockMaterials;
        }

        public override void MoveDown() {
            this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
            this.isUp = false;
            this.BlockState = BlockInfo.BlockState.NONE;
            this.PreviousState = BlockInfo.BlockState.UP;

            this.blockMaterials[0] = this.tileDownMaterial;
            this.blockMaterials[1] = this._firstArrowDownMaterial;
            this.blockMaterials[2] = this._secondArrowDownMaterial;
            this.blockRenderer.materials = this.blockMaterials;
        }

        public override void Init() {
            base.Init();

            if(this.isUp) {
                this.blockMaterials[1] = this._firstArrowUpMaterial;
                this.blockMaterials[2] = this._secondArrowUpMaterial;
                this.blockRenderer.materials = this.blockMaterials;
            } else {
                this.blockMaterials[1] = this._firstArrowDownMaterial;
                this.blockMaterials[2] = this._secondArrowDownMaterial;
                this.blockRenderer.materials = this.blockMaterials;
            }
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockDirection firstDirection, BlockInfo.BlockDirection secondDirection) {
            base.SetupBlock(type, firstDirection, secondDirection);

            this.isUp = false;
#if UNITY_EDITOR
            this.SetMiscMaterial();
#endif
            this.blockRenderer.sharedMaterials = this.blockMaterials;
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockDirection firstDirection, BlockInfo.BlockDirection secondDirection, BlockInfo.BlockState state) {
            base.SetupBlock(type, firstDirection, secondDirection, state);

            if(state == BlockInfo.BlockState.UP){
                this.isUp = true;
#if UNITY_EDITOR
                this.SetMiscMaterial();
#endif
                this.blockRenderer.sharedMaterials = this.blockMaterials;
            } else {
#if UNITY_EDITOR
                this.SetMiscMaterial();
#endif
                this.blockRenderer.sharedMaterials = this.blockMaterials;
            }
        }

#if UNITY_EDITOR
        private void SetMiscMaterial() {
            switch(this.FirstDirection) {
                case BlockInfo.BlockDirection.UP:
                    this._firstArrowUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Multi/Standard/Up/", "Up-Up");
                    this._firstArrowDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Multi/Standard/Down/", "Up-Down");
                    break;

                case BlockInfo.BlockDirection.RIGHT:
                    this._firstArrowUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Multi/Standard/Up/", "Right-Up");
                    this._firstArrowDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Multi/Standard/Down/", "Right-Down");
                    break;

                case BlockInfo.BlockDirection.DOWN:
                    this._firstArrowUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Multi/Standard/Up/", "Down-Up");
                    this._firstArrowDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Multi/Standard/Down/", "Down-Down");
                    break;

                case BlockInfo.BlockDirection.LEFT:
                    this._firstArrowUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Multi/Standard/Up/", "Left-Up");
                    this._firstArrowDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Multi/Standard/Down/", "Left-Down");
                    break;
            }

            switch(this.SecondDirection) {
                case BlockInfo.BlockDirection.UP:
                    this._secondArrowUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Multi/Standard/Up/", "Up-Up");
                    this._secondArrowDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Multi/Standard/Down/", "Up-Down");
                    break;

                case BlockInfo.BlockDirection.RIGHT:
                    this._secondArrowUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Multi/Standard/Up/", "Right-Up");
                    this._secondArrowDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Multi/Standard/Down/", "Right-Down");
                    break;

                case BlockInfo.BlockDirection.DOWN:
                    this._secondArrowUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Multi/Standard/Up/", "Down-Up");
                    this._secondArrowDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Multi/Standard/Down/", "Down-Down");
                    break;

                case BlockInfo.BlockDirection.LEFT:
                    this._secondArrowUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Multi/Standard/Up/", "Left-Up");
                    this._secondArrowDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Multi/Standard/Down/", "Left-Down");
                    break;
            }

            if(this.BlockState == BlockInfo.BlockState.UP) {
                this.blockMaterials[1] = this._firstArrowUpMaterial;
                this.blockMaterials[2] = this._secondArrowUpMaterial;
            } else {
                if(this.isUp) {
                    this.blockMaterials[1] = this._firstArrowUpMaterial;
                    this.blockMaterials[2] = this._secondArrowUpMaterial;
                } else {
                    this.blockMaterials[1] = this._firstArrowDownMaterial;
                    this.blockMaterials[2] = this._secondArrowDownMaterial;
                }
            }
        }
#endif
    }
}