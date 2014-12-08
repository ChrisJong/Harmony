namespace Blocks {

    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    using Grid;
    using GameInfo;
    using Helpers;

    [System.Serializable]
    public class GlassBlock : BlockClass {

        public GameObject fillerObject;
        public GameObject glassCollider;
        public List<Material> glassUpMaterials;
        public List<Material> glassDownMaterials;

        [SerializeField, HideInInspector]
        private int _maxCount;
        [SerializeField, HideInInspector]
        private int _previousBreakCount;
        [SerializeField, HideInInspector]
        private int _breakCount;

        public void BreakApart() {
            this.fillerObject = Instantiate(this.fillerObject) as GameObject;
            this.fillerObject.transform.position = new Vector3(this.transform.position.x, 1.5f, this.transform.position.z);
            this.fillerObject.transform.parent = GridMap.instance.transform;

            this.glassCollider.SetActive(false);
            this.blockRenderer.enabled = false;
        }

        public void DisableBlock() {
            this.blockRenderer.enabled = false;
        }

        public override void ResetUndoState() {
        }

        public override void MoveUp() {
        }

        public override void MoveDown() {
            this._previousBreakCount = this._breakCount;
            this._breakCount--;

            if(this._breakCount == 0)
                this.BreakApart();
            else {
                Debug.Log(this._breakCount);
            }
        }

        public override void Init() {
            base.Init();
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, int initCounter) {
            base.SetupBlock(type, initCounter);

            this._maxCount = initCounter;
            this._previousBreakCount = initCounter;
            this._breakCount = initCounter;
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockState state, int initCounter) {
            base.SetupBlock(type, state, initCounter);

            this._maxCount = initCounter;
            this._previousBreakCount = initCounter;
            this._breakCount = initCounter;

#if UNITY_EDITOR
            this.tileUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Empty/Balloon/Up/", "BalloonEmpty01-Up");
            this.tileDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Empty/Balloon/Down/", "BalloonEmpty01-Down");
            this.blockMaterials = this.blockRenderer.sharedMaterials;

            if(this.BlockState == BlockInfo.BlockState.UP) {
                this.blockMaterials[0] = this.tileUpMaterial;
            } else {
                this.blockMaterials[0] = this.tileDownMaterial;
            }
            this.blockRenderer.sharedMaterials = this.blockMaterials;
#endif
        }

        #region Getter/Setter
        public int PreviousBreakCount {
            get { return this._previousBreakCount; }
        }

        public int BreakCount {
            get { return this._breakCount; }
        }
        #endregion
    }
}