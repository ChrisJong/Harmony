namespace Blocks {

    using System.Collections;
    using System.Collections.Generic;

#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;

    using Grid;
    using GameInfo;
    using Helpers;

    [System.Serializable]
    public class SwitchBlock : BlockClass {

        [HideInInspector]
        public Material switchUpMaterial;
        [HideInInspector]
        public Material switchDownMaterial;

        [SerializeField]
        private List<GameObject> emptyBlocks;

        [SerializeField]
        public List<SwitchEmptyBlock> emptySwitchScripts;

        [SerializeField]
        private int _blockCount = 0;
        private bool _isFlipped;

        void Awake() {
            this.emptySwitchScripts = new List<SwitchEmptyBlock>();

            for(int i = 0; i < this._blockCount; i++) {
                this.emptySwitchScripts.Add(this.emptyBlocks[i].GetComponent<SwitchEmptyBlock>());
            }
        }

        public void AddEmptyBlock(GameObject obj) {
            if(this.emptyBlocks.Contains(obj))
                return;

            this.emptyBlocks.Add(obj);
            this._blockCount++;

#if UNITY_EDITOR
            EditorUtility.SetDirty(this.gameObject.GetComponent<SwitchBlock>());
#endif
        }

        public void RemoveEmptyBlock(GameObject obj) {
            if(this.emptyBlocks.Contains(obj)) {
                this.emptyBlocks.Remove(obj);
                this._blockCount--;
            }

            if(this.emptyBlocks.Count == 0)
                UnityEngine.Object.DestroyImmediate(this.gameObject);
        }

        public void RemoveAllBlocks(ref List<GameObject> blockList) {
            for(int i = this._blockCount - 1; i >= 0; i--) {
                this.emptyBlocks[i].GetComponent<SwitchEmptyBlock>().RemoveParentNode();
                blockList.Remove(this.emptyBlocks[i]);
                UnityEngine.Object.DestroyImmediate(this.emptyBlocks[i]);
            }

            this.emptyBlocks.Clear();
        }

        public void Undo() {
            if(this.PreviousState != BlockInfo.BlockState.NONE) {
                switch(this.PreviousState) {
                    case BlockInfo.BlockState.DOWN:
                        this.MoveDown();
                        break;

                    case BlockInfo.BlockState.UP:
                        this.MoveUp();
                        break;
                }
            }
        }

        public override void ResetUndoState() {
            this._isFlipped = false;
            this.PreviousState = BlockInfo.BlockState.NONE;
        }

        public override void MoveUp() {
            this.isUp = true;
            this.BlockState = BlockInfo.BlockState.NONE;
            this.PreviousState = BlockInfo.BlockState.DOWN;

            this.blockMaterials[0] = this.tileUpMaterial;
            this.blockMaterials[1] = this.switchUpMaterial;
            this.blockRenderer.materials = this.blockMaterials;

            for(int i = 0; i < this._blockCount; i++) {
                if(this.emptySwitchScripts[i].isReversed)
                    this.emptySwitchScripts[i].BlockState = BlockInfo.BlockState.DOWN;
                else
                    this.emptySwitchScripts[i].BlockState = BlockInfo.BlockState.UP;
            }
        }

        public override void MoveDown() {
            this.isUp = false;
            this.BlockState = BlockInfo.BlockState.NONE;
            this.PreviousState = BlockInfo.BlockState.UP;

            this.blockMaterials[0] = this.tileDownMaterial;
            this.blockMaterials[1] = this.switchDownMaterial;
            this.blockRenderer.materials = this.blockMaterials;

            for(int i = 0; i < this._blockCount; i++) {
                if(this.emptySwitchScripts[i].isReversed)
                    this.emptySwitchScripts[i].BlockState = BlockInfo.BlockState.UP;
                else
                    this.emptySwitchScripts[i].BlockState = BlockInfo.BlockState.DOWN;
            }
        }

        public override void Init() {
            base.Init();

            if(this.isUp) {
                this.blockMaterials[1] = this.switchDownMaterial;
                this.blockRenderer.materials = this.blockMaterials;
            } else {
                this.blockMaterials[1] = this.switchDownMaterial;
                this.blockRenderer.materials = this.blockMaterials;
            }
        }

        public override void SetupBlock(BlockInfo.BlockTypes type) {
            base.SetupBlock(type);
#if UNITY_EDITOR
            this.SetMiscMaterial();
#endif
            this.blockMaterials[1] = this.switchDownMaterial;
            this.blockRenderer.materials = this.blockMaterials;
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockState state) {
            base.SetupBlock(type, state);
#if UNITY_EDITOR
            this.SetMiscMaterial();
#endif
            this.blockMaterials[1] = this.switchDownMaterial;
            this.blockRenderer.materials = this.blockMaterials;
        }

#if UNITY_EDITOR
        private void SetMiscMaterial() {
            this.switchUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Switch/Standard/", "Up");
            this.switchDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Switch/Standard/", "Down");
        }
#endif

        #region Getter/Setter
        public int BlockCount {
            get { return this._blockCount; }
        }

        public bool IsFlipped {
            get { return this._isFlipped; }
            set { this._isFlipped = value; }
        }
        #endregion
    }
}