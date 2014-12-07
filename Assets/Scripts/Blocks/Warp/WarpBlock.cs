namespace Blocks {

    using System.Collections;
    using System.Collections.Generic;

#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;

    using GameInfo;

    [System.Serializable]
    public class WarpBlock : BlockClass {

        public List<Material> warpUpMaterials;
        public List<Material> warpDownMaterials;

        [SerializeField]
        public GameObject warpNode;
        [SerializeField]
        public PlayerInfo.MovementDirection warpDirection;

        [HideInInspector]
        public Material[] blockMaterials;

        private float _timer = 0.0f;
        private float _maxTimer = 1.0f;
        private BoxCollider _warpCollider;

        void Awake() {
            this.blockMaterials = this.blockRenderer.materials;
            this._warpCollider = this.warpNode.transform.GetChild(0).GetComponent<BoxCollider>();
        }

        void Update() {
            if(!this._warpCollider.enabled) {
                this._timer += Time.deltaTime;
                if(this._timer >= _maxTimer) {
                    this.EnableCollider();
                    this._timer = 0.0f;
                }
            }
        }

        public void AddWarpNode(GameObject obj) {
            if(this.warpNode != null)
                return;

            this.warpNode = obj;

#if UNITY_EDITOR
            EditorUtility.SetDirty(this.gameObject.GetComponent<WarpBlock>());
#endif
        }

        public void RemoveWarpNode(ref List<GameObject> blockList) {
            if(blockList.Contains(this.warpNode)) {
                blockList.Remove(this.warpNode);
                UnityEngine.Object.DestroyImmediate(this.warpNode);
                this.warpNode = null;
            }
        }

        public void EnableCollider() {
            this.blockMaterials[1] = this.warpDownMaterials[this.firstDirectionValue - 1];
            this.blockRenderer.materials = this.blockMaterials;

            if(!this._warpCollider.enabled)
                this._warpCollider.enabled = true;
        }

        public void Init() {
            if(this.BlockState == BlockInfo.BlockState.UP) {
                this.isUp = true;
                this.transform.position = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
                this.BlockState = BlockInfo.BlockState.NONE;
            } else {
                this.isUp = false;
                this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
                this.BlockState = BlockInfo.BlockState.NONE;
            }
        }

        public override void MoveUp() {
        }

        public override void MoveDown() {
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockDirection direction) {
            base.SetupBlock(type, direction);

            this.warpDirection = (PlayerInfo.MovementDirection)((int)direction);

            Material[] mats = this.blockRenderer.sharedMaterials;
            mats[1] = this.warpDownMaterials[(int)direction - 1];
            this.blockRenderer.sharedMaterials = mats;
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockDirection direction, BlockInfo.BlockState state) {
            base.SetupBlock(type, direction, state);

            this.warpDirection = (PlayerInfo.MovementDirection)((int)direction);

            Material[] mats = this.blockRenderer.sharedMaterials;
            mats[1] = this.warpDownMaterials[(int)direction - 1];
            this.blockRenderer.sharedMaterials = mats;
        }

        #region Getter/Setter
        public PlayerInfo.MovementDirection WarpDirection {
            get { return this.warpDirection; }
        }
        #endregion
    }
}