namespace Blocks {

    using UnityEngine;
    using System.Collections;

    using GridGenerator;
    using Constants;

    [DisallowMultipleComponent]
    public class Block : MonoBehaviour {

        private float _blockSpeed;
        private Vector3 _blockVelocity;
        private Vector3 _blockPosition;
        //private float _blockHeight;

        public BlockValues.BlockType blockType;
        public BlockValues.BlockState blockState;
        public bool frozen;

        public Vector3 pass;

        void Awake() {
            _blockSpeed = 20.0f;
            _blockVelocity = Vector3.zero;
            _blockPosition = this.transform.position;
            pass = _blockVelocity = _blockSpeed * transform.TransformDirection(Vector3.up);
            
            //_blockHeight = 1.0f;
        }

        void Start() {
            //GridMap temp = GameObject.Find("GridMap").GetComponent<GridMap>();
            //_blockHeight = temp.realBlockHeight;
        }

        // Update is called once per frame
        void Update() {
            if(blockState == BlockValues.BlockState.NONE)
                return;

            if(blockState == BlockValues.BlockState.UP)
                MoveUp();
            if(blockState == BlockValues.BlockState.DOWN)
                MoveDown();
        }

        public void MoveUp() {
            //blockState = BlockState.UP;
            //while(blockState == BlockState.UP) {
                //_blockVelocity = _blockSpeed * transform.TransformDirection(Vector3.up);
                //_blockPosition += _blockVelocity * Time.deltaTime;
            _blockPosition = new Vector3(this.transform.position.x, 1.0f, this.transform.position.z);
                this.transform.position = _blockPosition;
                if(this.transform.position.y >= 1.0f) {
                    this.transform.position = new Vector3(this.transform.position.x, 0.95f, this.transform.position.z);
                    _blockVelocity = Vector3.zero;
                    _blockPosition = this.transform.position;
                    blockState = BlockValues.BlockState.NONE;
                    return;
                }
            //}
        }

        public void MoveDown() {
            //blockState = BlockState.DOWN;
            //while(blockState == BlockState.DOWN) {
                //_blockVelocity = _blockSpeed * transform.TransformDirection(Vector3.down);
                //_blockPosition += _blockVelocity * Time.deltaTime;
            _blockPosition = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
                this.transform.position = _blockPosition;
                if(this.transform.position.y <= 0.0f) {
                    this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
                    _blockVelocity = Vector3.zero;
                    _blockPosition = this.transform.position;
                    blockState = BlockValues.BlockState.NONE;
                    return;
                }
            //}
        }

        public void SetType(BlockValues.BlockType type) {
            switch(type) {
                case BlockValues.BlockType.UP:
                this.blockType = BlockValues.BlockType.UP;
                break;

                case BlockValues.BlockType.RIGHT:
                this.blockType = BlockValues.BlockType.RIGHT;
                break;

                case BlockValues.BlockType.DOWN:
                this.blockType = BlockValues.BlockType.DOWN;
                break;

                case BlockValues.BlockType.LEFT:
                this.blockType = BlockValues.BlockType.LEFT;
                break;

                case BlockValues.BlockType.EMPTYDOWN:
                this.blockType = BlockValues.BlockType.EMPTYDOWN;
                this.frozen = true;
                break;

                case BlockValues.BlockType.EMPTYUP:
                this.blockState = BlockValues.BlockState.UP;
                this.blockType = BlockValues.BlockType.EMPTYUP;
                this.frozen = true;
                break;

                case BlockValues.BlockType.AI:
                this.blockType = BlockValues.BlockType.AI;
                this.frozen = true;
                break;

                case BlockValues.BlockType.HUMAN:
                this.blockType = BlockValues.BlockType.HUMAN;
                this.frozen = true;
                break;

                default:
                this.blockState = BlockValues.BlockState.NONE;
                this.blockType = BlockValues.BlockType.EMPTYDOWN;
                break;
            }
        }

        public float BlockSpeed {
            get { return _blockSpeed; }
        }

        public Vector3 BlockVelocity {
            get { return _blockVelocity; }
        }
    }
}
