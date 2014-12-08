namespace Blocks {

    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;
    using Helpers;

    [System.Serializable]
    public class NormalBlock : BlockClass {

        [SerializeField, HideInInspector]
        private Material _arrowUpMaterial;
        [SerializeField, HideInInspector]
        private Material _arrowDownMaterial;

        void Update() {
            if(this.BlockState == BlockInfo.BlockState.NONE)
                return;

            if(this.BlockState == BlockInfo.BlockState.UP)
                this.MoveUp();
            if(this.BlockState == BlockInfo.BlockState.DOWN)
                this.MoveDown();
        }

        public override void ResetUndoState() {
            this.PreviousState = BlockInfo.BlockState.NONE;
        }

        public override void MoveUp() {
            this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
            this.isUp = true;
            this.BlockState = BlockInfo.BlockState.NONE;
            this.PreviousState = BlockInfo.BlockState.DOWN;

            this.blockMaterials[0] = this.tileUpMaterial;
            this.blockMaterials[1] = this._arrowUpMaterial;
            this.blockRenderer.materials = this.blockMaterials;
        }

        public override void MoveDown() {
            this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
            this.isUp = false;
            this.BlockState = BlockInfo.BlockState.NONE;
            this.PreviousState = BlockInfo.BlockState.UP;

            this.blockMaterials[0] = this.tileDownMaterial;
            this.blockMaterials[1] = this._arrowDownMaterial;
            this.blockRenderer.materials = this.blockMaterials;
        }

        public override void Init() {
            base.Init();

            if(this.isUp) {
                this.blockMaterials[1] = this._arrowUpMaterial;
                this.blockRenderer.materials = this.blockMaterials;
            } else {
                this.blockMaterials[1] = this._arrowDownMaterial;
                this.blockRenderer.materials = this.blockMaterials;
            }
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockDirection direction) {
            base.SetupBlock(type, direction);

            this.isUp = false;
#if UNITY_EDITOR
            this.SetMiscMaterial();
#endif
            this.blockRenderer.sharedMaterials = this.blockMaterials;
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockDirection direction, BlockInfo.BlockState state) {
            base.SetupBlock(type, direction, state);

            if(state == BlockInfo.BlockState.UP) {
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
                    this._arrowUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Normal/Standard/Up/", "Up-Up");
                    this._arrowDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Normal/Standard/Down/", "Up-Down");
                    break;

                case BlockInfo.BlockDirection.RIGHT:
                    this._arrowUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Normal/Standard/Up/", "Right-Up");
                    this._arrowDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Normal/Standard/Down/", "Right-Down");
                    break;

                case BlockInfo.BlockDirection.DOWN:
                    this._arrowUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Normal/Standard/Up/", "Down-Up");
                    this._arrowDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Normal/Standard/Down/", "Down-Down");
                    break;

                case BlockInfo.BlockDirection.LEFT:
                    this._arrowUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Normal/Standard/Up/", "Left-Up");
                    this._arrowDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Normal/Standard/Down/", "Left-Down");
                    break;
            }

            if(this.BlockState == BlockInfo.BlockState.UP)
                this.blockMaterials[1] = this._arrowUpMaterial;
            else {
                if(this.isUp)
                    this.blockMaterials[1] = this._arrowUpMaterial;
                else
                    this.blockMaterials[1] = this._arrowDownMaterial;                    
            }
        }
#endif
    }
}