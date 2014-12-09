namespace Blocks {

    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    using Grid;
    using GameInfo;
    using Helpers;

    [System.Serializable]
    public class GlassBlock : BlockClass {

        public GameObject glassBlock;

        //[SerializeField, HideInInspector]
        //private int _maxCount;
        [SerializeField]
        public int previousBreakCount;
        [SerializeField]
        public int breakCount;

        public void DisableBlock() {
            this.blockRenderer.enabled = false;
        }

        public override void ResetUndoState() {
        }

        public override void MoveUp() {
        }

        public override void MoveDown() {
        }

        public override void Init() {
            base.Init();
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, int initCounter) {
            base.SetupBlock(type, initCounter);

            //this._maxCount = initCounter;
            this.previousBreakCount = initCounter;
            this.breakCount = initCounter;

#if UNITY_EDITOR
            this.SetMiscMaterial();
#endif
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockState state, int initCounter) {
            base.SetupBlock(type, state, initCounter);

            //this._maxCount = initCounter;
            this.previousBreakCount = initCounter;
            this.breakCount = initCounter;

#if UNITY_EDITOR
            this.SetMiscMaterial();
            if(this.BlockState == BlockInfo.BlockState.UP) {
                this.blockMaterials[0] = this.tileUpMaterial;
            } else {
                this.blockMaterials[0] = this.tileDownMaterial;
            }
            this.blockRenderer.sharedMaterials = this.blockMaterials;
#endif
        }

#if UNITY_EDITOR
        private void SetMiscMaterial() {
            Material glassMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Glass/", "Glass");

            if(this.glassBlock != null) {
                this.glassBlock.GetComponent<GlassCollider>().blockRenderer = this.glassBlock.GetComponent<MeshRenderer>();
                this.glassBlock.GetComponent<GlassCollider>().blockRenderer.sharedMaterial = glassMaterial;
            } else {
                this.glassBlock = this.transform.GetChild(0).gameObject;
                if(this.glassBlock != null) {
                    this.glassBlock.GetComponent<GlassCollider>().blockRenderer = this.glassBlock.GetComponent<MeshRenderer>();
                    this.glassBlock.GetComponent<GlassCollider>().blockRenderer.sharedMaterial = glassMaterial;
                } else
                    throw new System.ArgumentNullException("Can Not Find The Glass Block.");
            }
        }
#endif
    }
}