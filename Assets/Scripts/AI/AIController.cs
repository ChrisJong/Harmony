﻿namespace AI {

    using UnityEngine;
    using System.Collections;

    using Player;
    using Grid;
    using GameInfo;
    using Blocks;
    using Sound;

    public class AIController : MonoBehaviour {

        public static AIController instance;
        public static CharacterController characterController;

        public Animator charactorAnimator;

        public bool isMoving = false;
        public bool isStunned = false;

        public Vector2 currentPos;
        public Vector2 previousPos;

        private GameObject _currentBlock;
        private PlayerInfo.MovementDirection _currentDirection = PlayerInfo.MovementDirection.NONE;
        private bool _canUndo = false;

        void OnTriggerEnter(Collider obj) {
            if(obj.tag.Equals("Player")) {
                GameController.instance.isStageFinished = true;
                //GameController.instance.LoadNextLevel();
                //Debug.Log(obj.tag);
                //UnityEditor.EditorApplication.isPlaying = false;
                //Application.Quit();
            }
        }

        void Awake() {
            instance = this;
            characterController = GetComponent<CharacterController>() as CharacterController;

            this._currentBlock = null;
            this._currentDirection = PlayerInfo.MovementDirection.NONE;
        }

        void Start() {
            this.currentPos = new Vector2(this.transform.position.x, this.transform.position.z);
        }

        void Update() {
            if(Camera.main == null)
                return;

            this.CheckCurrentBlock();

            if(GridController.instance.blocksReady)
                this.CastCollisionRays();

            AIMovement.instance.UpdateMovement();
        }

        public void GetInput(PlayerInfo.MovementDirection current, PlayerInfo.MovementDirection previous) {
            if(this.isMoving)
                return;

            this._canUndo = false;
            this.GetCurrentBlock();
            this.CheckCurrentBlock();

            AIMovement.instance.ResetMovement();
            AIMovement.instance.UndoPosition = this.transform.position;

            if(current == PlayerInfo.MovementDirection.FORWARD) {
                this._currentDirection = PlayerInfo.MovementDirection.BACKWARD;
                AIMovement.instance.RotateToMovement(180.0f);
                AIMovement.instance.MoveVector = new Vector3(0, 0, -1);
            } else if(current == PlayerInfo.MovementDirection.RIGHT) {
                this._currentDirection = PlayerInfo.MovementDirection.LEFT;
                AIMovement.instance.RotateToMovement(270.0f);
                AIMovement.instance.MoveVector = new Vector3(-1, 0, 0);
            } else if(current == PlayerInfo.MovementDirection.BACKWARD) {
                this._currentDirection = PlayerInfo.MovementDirection.FORWARD;
                AIMovement.instance.RotateToMovement(0.0f);
                AIMovement.instance.MoveVector = new Vector3(0, 0, 1);
            } else if(current == PlayerInfo.MovementDirection.LEFT) {
                this._currentDirection = PlayerInfo.MovementDirection.RIGHT;
                AIMovement.instance.RotateToMovement(90.0f);
                AIMovement.instance.MoveVector = new Vector3(1, 0, 0);
            }

            this.CheckCurrentBlock();
            //AIAudio.instance.PlayMovement();
            this.isMoving = true;
            this.previousPos = this.currentPos;
            this.currentPos = new Vector2(this.transform.position.x, this.transform.position.z);
        }

        public void CheckCurrentBlock() {
            if(this._currentBlock == null)
                return;

            if(this._currentBlock.GetComponent<BlockClass>() != null) {
                if(this._currentBlock.GetComponent<BlockClass>().BlockState == BlockInfo.BlockState.UP) {
                    this.transform.position = new Vector3(this.transform.position.x, 3.0f, this.transform.position.z);
                } else if(this._currentBlock.GetComponent<BlockClass>().BlockState == BlockInfo.BlockState.DOWN) {
                    this.transform.position = new Vector3(this.transform.position.x, 2.0f, this.transform.position.z);
                }
            }
        }

        public void UndoMovement() {
            if(this._canUndo) {
                this.isMoving = false;
                AIMovement.instance.UndoMovement();
                this._currentDirection = PlayerInfo.MovementDirection.NONE;
                this._canUndo = false;
            }
        }

        private void CastCollisionRays() {
            RaycastHit hitInfo;
            Vector3 rayDirection = Vector3.zero;

            switch(this._currentDirection) {
                case PlayerInfo.MovementDirection.FORWARD:
                rayDirection += this.transform.TransformDirection(Vector3.forward);
                break;
                case PlayerInfo.MovementDirection.RIGHT:
                rayDirection += this.transform.TransformDirection(Vector3.right);
                break;
                case PlayerInfo.MovementDirection.BACKWARD:
                rayDirection += this.transform.TransformDirection(Vector3.back);
                break;
                case PlayerInfo.MovementDirection.LEFT:
                rayDirection += this.transform.TransformDirection(Vector3.left);
                break;
            }

            Vector3 tempOrigin = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            Debug.DrawRay(tempOrigin, rayDirection * 0.5f, Color.red);
            if(Physics.Raycast(tempOrigin, rayDirection, out hitInfo, 0.5f)) {
                if(hitInfo.collider != null) {
                    if(hitInfo.collider.tag == "Player") {
                        //UnityEditor.EditorApplication.isPlaying = false;
                        //Application.Quit();
                        //GameController.instance.LoadNextLevel();
                        GameController.instance.isStageFinished = true;
                    } else {
                        if(isMoving) {
                            if(hitInfo.collider.tag == "Block")
                                AIAudio.instance.PlayCollision();
                        }

                        if(hitInfo.collider.tag == "Block" || hitInfo.collider.tag == "Wall" || hitInfo.collider.tag == "Glass") {
                            if(!this.isStunned)
                                this._canUndo = true;

                            this.isMoving = false;
                            AIMovement.instance.VerticalVelocity = AIMovement.instance.MoveVector.y;
                            AIMovement.instance.MoveVector = Vector3.zero;

                            this.GetCurrentBlock();
                            AIMovement.instance.CenterPlayer(this._currentBlock.transform);

                            this.currentPos = new Vector2(this.transform.position.x, this.transform.position.z);
                            //Debug.Log(Vector2.Distance(this.previousPos, this.currentPos));
                            if(Vector2.Distance(this.previousPos, this.currentPos) > 0.9f) {
                                if(hitInfo.collider.tag == "Glass")
                                    hitInfo.transform.GetComponent<GlassCollider>().MoveDown();
                            }
                        }
                    }
                }
            }
        }

        private void GetCurrentBlock() {
            RaycastHit hitInfo;
            Vector3 rayDirection = this.transform.TransformDirection(Vector3.down);

            Debug.DrawRay(this.transform.position, rayDirection * 100.0f, Color.red);
            if(Physics.Raycast(this.transform.position, rayDirection, out hitInfo, 5.0f)) {
                if(hitInfo.transform.tag == "Block" || hitInfo.transform.tag == "Glass") {
                    this._currentBlock = hitInfo.transform.gameObject;
                }
            }
        }

        #region Getter / Setter
        public bool CanUndo {
            get { return this._canUndo; }
        }

        public PlayerInfo.MovementDirection CurrentDirection {
            get { return this._currentDirection; }
        }
        #endregion
    }
}
