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

        private int _blockCount;
        //private bool _isFlipped;
        //private bool _isReversed;
        //private BlockInfo.BlockState _previousState;
        private Material[] _blockMaterials;

        void Awake() {
            this._blockMaterials = this.blockRenderer.materials;

            for(int i = 0; i < this._blockCount; i++) {
                this.emptySwitchScripts.Add(this.emptyBlocks[i].GetComponent<SwitchEmptyBlock>() as SwitchEmptyBlock);
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

        public void Init() {
            if(this.BlockState == BlockInfo.BlockState.UP) {
                this.isUp = true;
                this.BlockState = BlockInfo.BlockState.NONE;

                this._blockMaterials[0] = this.tileUpMaterials[this.MaterialID];
                this._blockMaterials[1] = this.switchUpMaterial;
                this.blockRenderer.materials = this._blockMaterials;
            } else {
                this.isUp = false;
                this.BlockState = BlockInfo.BlockState.NONE;

                this._blockMaterials[0] = this.tileDownMaterials[this.MaterialID];
                this._blockMaterials[1] = this.switchDownMaterial;
                this.blockRenderer.materials = this._blockMaterials;
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

        public override void MoveUp() {
            this.isUp = true;
            this.BlockState = BlockInfo.BlockState.NONE;

            this._blockMaterials[0] = this.tileUpMaterials[this.MaterialID];
            this._blockMaterials[1] = this.switchUpMaterial;
            this.blockRenderer.materials = this._blockMaterials;

            for(int i = 0; i < this._blockCount; i++) {
                this.emptySwitchScripts[i].BlockState = BlockInfo.BlockState.UP;
            }
        }

        public override void MoveDown() {
            this.isUp = false;
            this.BlockState = BlockInfo.BlockState.NONE;

            this._blockMaterials[0] = this.tileDownMaterials[this.MaterialID];
            this._blockMaterials[1] = this.switchDownMaterial;
            this.blockRenderer.materials = this._blockMaterials;

            for(int i = 0; i < this._blockCount; i++) {
                this.emptySwitchScripts[i].BlockState = BlockInfo.BlockState.DOWN;
            }
        }

        public override void SetupBlock(BlockInfo.BlockTypes type) {
            base.SetupBlock(type);

            Material[] mats = this.blockRenderer.sharedMaterials;
            
            //this._isReversed = false;

            mats[1] = this.switchDownMaterial;
            this.blockRenderer.materials = mats;
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockState state) {
            base.SetupBlock(type, state);

            Material[] mats = this.blockRenderer.sharedMaterials;

            if(state == BlockInfo.BlockState.UP) {
                //this._isReversed = true;

                mats[1] = this.switchUpMaterial;
                this.blockRenderer.materials = mats;
            } else {
                //this._isReversed = false;

                mats[1] = this.switchDownMaterial;
                this.blockRenderer.materials = mats;
            }
        }

        #region Getter/Setter
        public int BlockCount {
            get { return this._blockCount; }
        }

        /*public bool IsFlipped {
            get { return this._isFlipped; }
        }*/

        /*public bool IsReversed {
            get { return this._isReversed; }
        }*/

        /*public BlockInfo.BlockState PreviousState {
            get { return this._previousState; }
        }*/
        #endregion
    }
}