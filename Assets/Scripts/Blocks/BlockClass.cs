namespace Blocks {

    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;

    public abstract class BlockClass : MonoBehaviour {

        public List<Material> blockMaterials;
        public MeshRenderer blockRenderer;

        public BlockInfo.BlockTypes blockType;
        public BlockInfo.BlockState blockState;

        public BlockInfo.BlockDirection firstDirection;
        public BlockInfo.BlockDirection secondDirection;

        public bool isUp;

        public abstract void MoveUp();

        public abstract void MoveDown();

        public virtual void SetupType(BlockInfo.BlockTypes type) {
            this.blockType = type;
            this.blockState = BlockInfo.BlockState.NONE;

            this.firstDirection = BlockInfo.BlockDirection.NONE;
            this.secondDirection = BlockInfo.BlockDirection.NONE;
        }

        public virtual void SetupType(BlockInfo.BlockTypes type, BlockInfo.BlockState state) {
            this.blockType = type;
            this.blockState = state;

            this.firstDirection = BlockInfo.BlockDirection.NONE;
            this.secondDirection = BlockInfo.BlockDirection.NONE;
        }

        public virtual void SetupType(BlockInfo.BlockTypes type, int initCounter) {
            this.blockType = type;
            this.blockState = BlockInfo.BlockState.NONE;

            this.firstDirection = BlockInfo.BlockDirection.NONE;
            this.secondDirection = BlockInfo.BlockDirection.NONE;
        }

        public virtual void SetupType(BlockInfo.BlockTypes type, BlockInfo.BlockState state, int initCounter) {
            this.blockType = type;
            this.blockState = state;

            this.firstDirection = BlockInfo.BlockDirection.NONE;
            this.secondDirection = BlockInfo.BlockDirection.NONE;
        }

        public virtual void SetupType(BlockInfo.BlockTypes type, BlockInfo.BlockDirection direction) {
            this.blockType = type;
            this.blockState = BlockInfo.BlockState.NONE;

            this.firstDirection = direction;
            this.secondDirection = BlockInfo.BlockDirection.NONE;
        }

        public virtual void SetupType(BlockInfo.BlockTypes type, BlockInfo.BlockDirection direction, BlockInfo.BlockState state) {
            this.blockType = type;
            this.blockState = state;

            this.firstDirection = direction;
            this.secondDirection = BlockInfo.BlockDirection.NONE;
        }

        public virtual void SetupType(BlockInfo.BlockTypes type, BlockInfo.BlockDirection firstDirection, BlockInfo.BlockDirection secondDirection) {
            this.blockType = type;
            this.blockState = BlockInfo.BlockState.NONE;

            this.firstDirection = firstDirection;
            this.secondDirection = secondDirection;
            
        }

        public virtual void SetupType(BlockInfo.BlockTypes type, BlockInfo.BlockDirection firstDirection, BlockInfo.BlockDirection secondDirection, BlockInfo.BlockState state) {
            this.blockType = type;
            this.blockState = state;

            this.firstDirection = firstDirection;
            this.secondDirection = secondDirection;
        }
    }
}