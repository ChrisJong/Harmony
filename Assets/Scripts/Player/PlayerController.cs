﻿namespace Player {

    using UnityEngine;
    using System.Collections;

    using GridGenerator;
    using Constants;
    using Blocks;

    public class PlayerController : MonoBehaviour {

        public static PlayerController instance;
        public static CharacterController characterController;
        
        private GameObject _currentBlock;
        private PlayerValues.MovementDirection _currentDirection = PlayerValues.MovementDirection.NONE;
        private PlayerValues.MovementDirection _previousDirection = PlayerValues.MovementDirection.NONE;

        public bool isMoving = false;

        void Awake() {
            instance = this;
            characterController = GetComponent<CharacterController>() as CharacterController;

            this._currentBlock = null;
            this._currentDirection = PlayerValues.MovementDirection.NONE;
            this._previousDirection = PlayerValues.MovementDirection.NONE;
        }

        void Update() {
            if(Camera.main == null)
                return;

            this.CheckCurrentBlock();

            if(this.CastCollisionRays())
                PlayerMovement.instance.UpdateMovement();
        }

        public void GetInput(PlayerValues.MovementDirection current, PlayerValues.MovementDirection previous) {
            if(this.isMoving)
                return;

            this.GetCurrentBlock();
            this.CheckCurrentBlock();
            PlayerMovement.instance.VerticalVelocity = PlayerMovement.instance.MoveVector.y;
            PlayerMovement.instance.MoveVector = Vector3.zero;

            if(Input.GetKeyDown(KeyCode.UpArrow)) {
                this._previousDirection = this._currentDirection;
                this._currentDirection = current;
                PlayerMovement.instance.MoveVector = new Vector3(0, 0, 1);
                this.isMoving = true;
            } else if(Input.GetKeyDown(KeyCode.RightArrow)) {
                this._previousDirection = this._currentDirection;
                this._currentDirection = current;
                PlayerMovement.instance.MoveVector = new Vector3(1, 0, 0);
                this.isMoving = true;
            } else if(Input.GetKeyDown(KeyCode.DownArrow)) {
                this._previousDirection = this._currentDirection;
                this._currentDirection = current;
                PlayerMovement.instance.MoveVector = new Vector3(0, 0, -1);
                this.isMoving = true;
            } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                this._previousDirection = this._currentDirection;
                this._currentDirection = current;
                PlayerMovement.instance.MoveVector = new Vector3(-1, 0, 0);
                this.isMoving = true;
            }
            this.CheckCurrentBlock();
        }

        public void CheckCurrentBlock() {
            if(this._currentBlock == null)
                return;

            if(this._currentBlock.GetComponent<Block>().blockState == BlockValues.BlockState.UP)
                this.transform.position = new Vector3(this.transform.position.x, 2.5f, this.transform.position.z);
            else if(this._currentBlock.GetComponent<Block>().blockState == BlockValues.BlockState.DOWN)
                this.transform.position = new Vector3(this.transform.position.x, 1.5f, this.transform.position.z);
        }

        private bool CastCollisionRays() {
            RaycastHit hitInfo;
            Vector3 rayDirection = Vector3.zero;

            switch(this._currentDirection) {
                case PlayerValues.MovementDirection.FORWARD:
                rayDirection += this.transform.TransformDirection(Vector3.forward);
                break;
                case PlayerValues.MovementDirection.RIGHT:
                rayDirection += this.transform.TransformDirection(Vector3.right);
                break;
                case PlayerValues.MovementDirection.BACKWARD:
                rayDirection += this.transform.TransformDirection(Vector3.back);
                break;
                case PlayerValues.MovementDirection.LEFT:
                rayDirection += this.transform.TransformDirection(Vector3.left);
                break;
            }

            Vector3 tempOrigin = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            Debug.DrawRay(tempOrigin, rayDirection * 0.6f, Color.red);
            if(Physics.Raycast(tempOrigin, rayDirection, out hitInfo, 0.6f)) {
                if(hitInfo.collider != null) {
                    if(hitInfo.collider.tag == "AI") {
                        UnityEditor.EditorApplication.isPlaying = false;
                        Application.Quit();
                        return false;
                    } else {
                        this.isMoving = false;
                        PlayerMovement.instance.VerticalVelocity = PlayerMovement.instance.MoveVector.y;
                        PlayerMovement.instance.MoveVector = Vector3.zero;
                        return true;
                    }
                }
            }
            return true;
        }

        private void GetCurrentBlock() {
            RaycastHit hitInfo;
            this._currentBlock = null;
            Vector3 rayDirection = this.transform.TransformDirection(Vector3.down);

            Debug.DrawRay(this.transform.position, rayDirection * 100.0f, Color.red);
            if(Physics.Raycast(this.transform.position, rayDirection, out hitInfo, 5.0f)) {
                if(hitInfo.transform.tag == "Block") {
                    this._currentBlock = hitInfo.transform.gameObject;
                }
            }
        }
    }
}
