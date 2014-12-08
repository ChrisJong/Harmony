namespace Blocks {

    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;
    using Helpers;
    using Resource;

    [System.Serializable]
    public abstract class BlockClass : MonoBehaviour {

        [HideInInspector]
        public Material tileUpMaterial;
        [HideInInspector]
        public Material tileDownMaterial;
        [HideInInspector]
        public Material[] blockMaterials;
        [HideInInspector]
        public MeshRenderer blockRenderer;

        public int firstDirectionValue;
        public int secondDirectionValue;
        public bool isUp;

        [SerializeField, HideInInspector]
        private int _materialID;
        /*[SerializeField, HideInInspector]
        private bool _isReversed;*/
        [SerializeField, HideInInspector]
        private BlockInfo.BlockTypes _blockType;
        [SerializeField, HideInInspector]
        private BlockInfo.BlockState _blockState;
        [SerializeField, HideInInspector]
        private List<BlockInfo.BlockDirection> _blockDirections = new List<BlockInfo.BlockDirection>();
        [SerializeField, HideInInspector]
        private BlockInfo.BlockDirection _firstDirection;
        [SerializeField, HideInInspector]
        private BlockInfo.BlockDirection _secondDirection;
        /*[SerializeField, HideInInspector]
        private BlockInfo.BlockState _previousState;*/

        //public abstract void ResetUndoState();

        public abstract void MoveUp();

        public abstract void MoveDown();

        public virtual void Destruction() {
            this.gameObject.AddComponent<Rigidbody>();
            //this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, -20, this.transform.position.z), Time.deltaTime * 50.0f);
        }

        #region Setup / Init
        /*public virtual void Init() {

        }*/

        public virtual void ChangeTileMaterial() {
            this._materialID = TileManager.instance.ChangeMaterialID();
            this.tileUpMaterial = null;
            this.tileUpMaterial = TileManager.instance.GetCurrentSkinMaterial("up", this._materialID);
            this.tileDownMaterial = null;
            this.tileDownMaterial = TileManager.instance.GetCurrentSkinMaterial("down", this._materialID);

            this.SetTileMaterial();
        }

        public virtual void SetupBlock() {
            this._blockType = BlockInfo.BlockTypes.NONE;
            this._blockState = BlockInfo.BlockState.NONE;

            this._firstDirection = BlockInfo.BlockDirection.NONE;
            this._secondDirection = BlockInfo.BlockDirection.NONE;

            this.SetTileMaterial();
        }

        public virtual void SetupBlock(BlockInfo.BlockTypes type) {
            this._blockType = type;
            this._blockState = BlockInfo.BlockState.NONE;

            this._firstDirection = BlockInfo.BlockDirection.NONE;
            this._secondDirection = BlockInfo.BlockDirection.NONE;

            this.SetTileMaterial();
        }

        public virtual void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockState state) {
            this._blockType = type;
            this._blockState = state;

            this._firstDirection = BlockInfo.BlockDirection.NONE;
            this._secondDirection = BlockInfo.BlockDirection.NONE;

            this.SetTileMaterial();
        }

        public virtual void SetupBlock(BlockInfo.BlockTypes type, int initCounter) {
            this._blockType = type;
            this._blockState = BlockInfo.BlockState.NONE;

            this._firstDirection = BlockInfo.BlockDirection.NONE;
            this._secondDirection = BlockInfo.BlockDirection.NONE;

            this.SetTileMaterial();
        }

        public virtual void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockState state, int initCounter) {
            this._blockType = type;
            this._blockState = state;

            this._firstDirection = BlockInfo.BlockDirection.NONE;
            this._secondDirection = BlockInfo.BlockDirection.NONE;

            this.SetTileMaterial();
        }

        public virtual void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockDirection direction) {
            this._blockType = type;
            this._blockState = BlockInfo.BlockState.NONE;

            this._firstDirection = direction;
            this._secondDirection = BlockInfo.BlockDirection.NONE;

            this._blockDirections.Add(direction);

            this.firstDirectionValue = (int)direction;

            this.SetTileMaterial();
        }

        public virtual void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockDirection direction, BlockInfo.BlockState state) {
            this._blockType = type;
            this._blockState = state;

            this._firstDirection = direction;
            this._secondDirection = BlockInfo.BlockDirection.NONE;

            this._blockDirections.Add(direction);

            this.firstDirectionValue = (int)direction;

            this.SetTileMaterial();
        }

        public virtual void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockDirection firstDirection, BlockInfo.BlockDirection secondDirection) {
            this._blockType = type;
            this._blockState = BlockInfo.BlockState.NONE;

            this._firstDirection = firstDirection;
            this._secondDirection = secondDirection;

            this._blockDirections.Add(firstDirection);
            this._blockDirections.Add(secondDirection);

            this.firstDirectionValue = (int)firstDirection;
            this.secondDirectionValue = (int)secondDirection;

            this.SetTileMaterial();
        }

        public virtual void SetupBlock(BlockInfo.BlockTypes type, BlockInfo.BlockDirection firstDirection, BlockInfo.BlockDirection secondDirection, BlockInfo.BlockState state) {
            this._blockType = type;
            this._blockState = state;

            this._firstDirection = firstDirection;
            this._secondDirection = secondDirection;

            this._blockDirections.Add(firstDirection);
            this._blockDirections.Add(secondDirection);

            this.firstDirectionValue = (int)firstDirection;
            this.secondDirectionValue = (int)secondDirection;

            this.SetTileMaterial();
        }

        private void SetTileMaterial() {
#if UNITY_EDITOR
            this.blockRenderer = this.gameObject.GetComponent<MeshRenderer>() as MeshRenderer;
            this.blockMaterials = this.blockRenderer.sharedMaterials;

            this._materialID = Random.Range(1, 6);
            this.tileUpMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Empty/Standard/Up/", "Empty" + this._materialID.ToString().PadLeft(2, '0') + "-Up");
            this.tileDownMaterial = AssetProcessor.FindAsset<Material>("Assets/Models/Block/Material/Empty/Standard/Down/", "Empty" + this._materialID.ToString().PadLeft(2, '0') + "-Down");
#else
            this.blockMaterials = this.blockRenderer.materials
#endif
            if(this._blockState == BlockInfo.BlockState.UP)
                this.blockMaterials[0] = this.tileUpMaterial;
            else {
                if(this.isUp)
                    this.blockMaterials[0] = this.tileUpMaterial;
                else
                    this.blockMaterials[0] = this.tileDownMaterial;
            }
#if UNITY_EDITOR
            this.blockRenderer.sharedMaterials = this.blockMaterials;
#else
            this.blockRenderer.materials = this.blockMaterials;
#endif
        }
        #endregion

        #region Getter/Setters
        public int MaterialID {
            get { return this._materialID; }
        }

        public BlockInfo.BlockState BlockState {
            get { return this._blockState; }
            set { this._blockState = value; }
        }

        public BlockInfo.BlockTypes BlockType {
            get { return this._blockType; }
        }

        public List<BlockInfo.BlockDirection> BlockDirections {
            get { return this._blockDirections; }
        }

        public BlockInfo.BlockDirection FirstDirection {
            get { return this._firstDirection; }
        }

        public BlockInfo.BlockDirection SecondDirection {
            get { return this._secondDirection; }
        }
        #endregion
    }
}