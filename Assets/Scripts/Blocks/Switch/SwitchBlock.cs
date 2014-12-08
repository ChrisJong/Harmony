namespace Blocks {

    using System.Collections;
    using System.Collections.Generic;

#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;

    using Grid;
    using GameInfo;

    [System.Serializable]
    public class SwitchBlock : BlockClass {

        public Material switchUpMaterial;
        public Material switchDownMaterial;

        [SerializeField]
        private List<GameObject> emptyBlocks;

        [SerializeField]
        public List<SwitchEmptyBlock> emptySwitchScripts;

        [SerializeField]
        private int _blockCount = 0;
        private bool _isFlipped;
        //private bool _isReversed;
        private BlockInfo.BlockState _previousState;
        private Material[] _blockMaterials;

        void Awake() {
            this._blockMaterials = this.blockRenderer.materials;
            this.emptySwitchScripts = new List<SwitchEmptyBlock>();

            for(int i = 0; i < this._blockCount; i++) {
                this.emptySwitchScripts.Add(this.emptyBlocks[i].GetComponent<SwitchEmptyBlock>());
            }
        }

        void Update() {
            if(this.BlockState == BlockInfo.BlockState.NONE)
                return;

            if(this.BlockState == BlockInfo.BlockState.UP)
                this.MoveUp();
            else
                this.MoveDown();
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

        public void ResetUndoState() {
            this._isFlipped = false;
            this._previousState = BlockInfo.BlockState.NONE;
        }

        public override void MoveUp() {
            this.isUp = true;
            this.BlockState = BlockInfo.BlockState.NONE;

            this._blockMaterials[0] = this.tileUpMaterial;
            this._blockMaterials[1] = this.switchUpMaterial;
            this.blockRenderer.materials = this._blockMaterials;

            for(int i = 0; i < this._blockCount; i++) {
                this.emptySwitchScripts[i].BlockState = BlockInfo.BlockState.UP;
            }
        }

        public override void MoveDown() {
            this.isUp = false;
            this.BlockState = BlockInfo.BlockState.NONE;

            this._blockMaterials[0] = this.tileDownMaterial;
            this._blockMaterials[1] = this.switchDownMaterial;
            this.blockRenderer.materials = this._blockMaterials;

            for(int i = 0; i < this._blockCount; i++) {
                this.emptySwitchScripts[i].BlockState = BlockInfo.BlockState.DOWN;
            }
        }

        public void Init() {
            //base.Init();

            if(this.BlockState == BlockInfo.BlockState.UP) {
                this.isUp = true;
                this.BlockState = BlockInfo.BlockState.NONE;

                this._blockMaterials[0] = this.tileUpMaterial;
                this._blockMaterials[1] = this.switchDownMaterial;
                this.blockRenderer.materials = this._blockMaterials;
            } else {
                this.isUp = false;
                this.BlockState = BlockInfo.BlockState.NONE;

                this._blockMaterials[0] = this.tileDownMaterial;
                this._blockMaterials[1] = this.switchDownMaterial;
                this.blockRenderer.materials = this._blockMaterials;
            }
        }

        public override void SetupBlock(BlockInfo.BlockTypes type) {
            base.SetupBlock(type);

            Material[] mats = this.blockRenderer.sharedMaterials;

            mats[1] = this.switchDownMaterial;
            this.blockRenderer.materials = mats;
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockState state) {
            base.SetupBlock(type, state);

            Material[] mats = this.blockRenderer.sharedMaterials;

            if(state == BlockInfo.BlockState.UP) {
                mats[1] = this.switchDownMaterial;
                this.blockRenderer.materials = mats;
            } else {
                mats[1] = this.switchDownMaterial;
                this.blockRenderer.materials = mats;
            }
        }

        #region Getter/Setter
        public int BlockCount {
            get { return this._blockCount; }
        }

        public bool IsFlipped {
            get { return this._isFlipped; }
            set { this._isFlipped = value; }
        }

        /*public bool IsReversed {
            get { return this._isReversed; }
        }*/

        public BlockInfo.BlockState PreviousState {
            get { return this._previousState; }
            set { this._previousState = value; }
        }
        #endregion
    }
}