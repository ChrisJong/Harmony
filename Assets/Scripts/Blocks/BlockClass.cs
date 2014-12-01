namespace Blocks {

    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;

    [System.Serializable]
    public abstract class BlockClass : MonoBehaviour {

        public List<Material> tileUpMaterials;
        public List<Material> tileDownMaterials;

        public MeshRenderer blockRenderer;

        public int firstDirectionValue;
        public int secondDirectionValue;
        public bool isUp;

        private int _materialID;
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

        public abstract void MoveUp();

        public abstract void MoveDown();

        public virtual void Destruction() {
            this.gameObject.AddComponent<Rigidbody>();
            //this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, -20, this.transform.position.z), Time.deltaTime * 50.0f);
        }

        #region Setup / Init
        public virtual void ChangeTileMaterial() {
            throw new System.NotImplementedException();
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
            if(this.tileDownMaterials.Count == this.tileUpMaterials.Count)
                this._materialID = Random.Range(0, tileDownMaterials.Count);
            else
                this._materialID = 0;

            if(this._blockState == BlockInfo.BlockState.UP)
                this.blockRenderer.material = this.tileUpMaterials[this._materialID];
            else
                this.blockRenderer.material = this.tileDownMaterials[this._materialID];
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