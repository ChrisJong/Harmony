namespace Blocks {

    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;
    using Helpers;

    public class StunBlock : BlockClass {

        [HideInInspector]
        public Material stunUpMaterial;
        [HideInInspector]
        public Material stunDownMaterial;

        public int stunCounter;
        public bool isEnabled = false;

        private float _timer = 0.0f;
        private float _maxTimer = 0.5f;

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

            this.stunCounter = initCounter;
#if UNITY_EDITOR
            this.SetMiscMaterial();
#endif
            this.blockMaterials[1] = stunDownMaterial;
            this.blockRenderer.materials = this.blockMaterials;
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockState state, int initCounter) {
            base.SetupBlock(type, state, initCounter);

            this.stunCounter = initCounter;
#if UNITY_EDITOR
            this.SetMiscMaterial();
#endif
            this.blockMaterials[1] = stunDownMaterial;
            this.blockRenderer.materials = this.blockMaterials;
        }

#if UNITY_EDITOR
        private void SetMiscMaterial() {
            this.stunUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Stun/Standard/", "Stun-Up");
            this.stunDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Stun/Standard/", "Stun-Down");
        }
#endif
    }
}