namespace Player {

    using UnityEngine;
    using System.Collections;

    using Sound;
    using GameInfo;

    public class PlayerMovement : MonoBehaviour {

        public static PlayerMovement instance;

        public Vector3 MoveVector {
            get;
            set;
        }

        public float VerticalVelocity {
            get;
            set;
        }

        public Vector3 UndoPosition {
            get;
            set;
        }

        private GameObject _objectRotation;

        void Awake() {
            instance = this;
            this._objectRotation = this.transform.GetChild(0).gameObject;
        }

        void LateUpdate() {
            this.ApplyGravity();
        }

        public void UpdateMovement() {
            this.ProcessMovement();
        }

        public void RotateToMovement(float newRotation) {
            this._objectRotation.transform.rotation = Quaternion.Euler(0, newRotation, 0);
        }

        private void ProcessMovement() {
            if(PlayerController.instance.isMoving)
                this.MoveVector = this.transform.TransformDirection(this.MoveVector);
            else
                this.MoveVector = new Vector3(0, this.MoveVector.y, 0);

            if(MoveVector.magnitude > 1.0f)
                this.MoveVector = Vector3.Normalize(this.MoveVector);

            this.MoveVector *= PlayerInfo.MoveSpeed;

            this.MoveVector = new Vector3(this.MoveVector.x, this.VerticalVelocity, this.MoveVector.z);

            this.ApplyGravity();
            
            PlayerController.characterController.Move(this.MoveVector * Time.deltaTime);
        }

        public void CenterPlayer(Transform currentBlock) {
            this.transform.position = new Vector3(currentBlock.transform.position.x, this.transform.position.y, currentBlock.transform.position.z + 0.5f);
        }

        public void UndoMovement() {
            this.transform.position = this.UndoPosition;
        }

        private void ApplyGravity() {
            if(this.MoveVector.y > -PlayerInfo.TerminalVelocity) {
                this.MoveVector = new Vector3(this.MoveVector.x, this.MoveVector.y - PlayerInfo.Gravity * Time.deltaTime, this.MoveVector.z);
            }

            if(PlayerController.characterController.isGrounded && this.MoveVector.y < -1.0f) {
                this.MoveVector = new Vector3(this.MoveVector.x, -1.0f, this.MoveVector.z);
            }
        }
    }
}