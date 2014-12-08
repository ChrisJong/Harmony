namespace Blocks {

    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;

    public class EmptyBlock : BlockClass {

        public override void ResetUndoState() {
        }

        public override void MoveUp() {
        }

        public override void MoveDown() {
        }

        public override void Init() {
            base.Init();

            this.blockRenderer.materials = this.blockMaterials;
        }

        public override void SetupBlock(BlockInfo.BlockTypes type) {
            base.SetupBlock(type);
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockState state) {
            base.SetupBlock(type, state);
        }
    }
}