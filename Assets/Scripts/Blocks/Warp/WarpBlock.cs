namespace Blocks {

    using System.Collections;
    using System.Collections.Generic;

#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;

    using GameInfo;
    using Helpers;

    [System.Serializable]
    public class WarpBlock : BlockClass {

        [HideInInspector]
        public Material warpUpMaterial;
        [HideInInspector]
        public Material warpDownMaterial;

        [SerializeField]
        public GameObject warpNode;
        [SerializeField]
        public PlayerInfo.MovementDirection warpDirection;

        private float _timer = 0.0f;
        private float _maxTimer = 0.25f;
        private BoxCollider _warpCollider;

        void Awake() {
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
            if(this.isUp) {
                this.blockMaterials[0] = this.tileUpMaterial;
                this.blockMaterials[1] = this.warpDownMaterial;
            } else {
                this.blockMaterials[0] = this.tileDownMaterial;
                this.blockMaterials[1] = this.warpDownMaterial;
            }

            this.blockRenderer.materials = this.blockMaterials;

            if(!this._warpCollider.enabled)
                this._warpCollider.enabled = true;
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

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockDirection direction) {
            base.SetupBlock(type, direction);

            this.warpDirection = (PlayerInfo.MovementDirection)((int)direction);
#if UNITY_EDITOR
            this.SetMiscMaterial();
#endif
            this.blockMaterials[1] = this.warpDownMaterial;
            this.blockRenderer.sharedMaterials = this.blockMaterials;
        }

        public override void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockDirection direction, BlockInfo.BlockState state) {
            base.SetupBlock(type, direction, state);

            this.warpDirection = (PlayerInfo.MovementDirection)((int)direction);
#if UNITY_EDITOR
            this.SetMiscMaterial();
#endif
            this.blockMaterials[1] = this.warpDownMaterial;
            this.blockRenderer.sharedMaterials = this.blockMaterials;
        }

#if UNITY_EDITOR
        private void SetMiscMaterial() {
            this.warpUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Warp/Standard/Up/", "Up-Up");
            this.warpDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Warp/Standard/Down/", "Up-Down");

            // OLD WARP DIRECTION CODE.
            /*switch(this.FirstDirection){
                case BlockInfo.BlockDirection.UP:
                    this.warpUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Warp/Standard/Up/", "Up-Up");
                    this.warpDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Warp/Standard/Down/", "Up-Down");
                    break;

                case BlockInfo.BlockDirection.RIGHT:
                    this.warpUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Warp/Standard/Up/", "Right-Up");
                    this.warpDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Warp/Standard/Down/", "Right-Down");
                    break;

                case BlockInfo.BlockDirection.DOWN:
                    this.warpUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Warp/Standard/Up/", "Down-Up");
                    this.warpDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Warp/Standard/Down/", "Down-Down");
                    break;

                case BlockInfo.BlockDirection.LEFT:
                    this.warpUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Warp/Standard/Up/", "Left-Up");
                    this.warpDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Warp/Standard/Down/", "Left-Down");
                    break;
            }*/
        }
#endif

        #region Getter/Setter
        public PlayerInfo.MovementDirection WarpDirection {
            get { return this.warpDirection; }
        }
        #endregion
    }
}