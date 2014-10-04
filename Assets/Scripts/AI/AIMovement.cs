namespace AI {

    using UnityEngine;
    using System.Collections;

    using Sound;
    using GameInfo;

    public class AIMovement : MonoBehaviour {

        public static AIMovement instance;

        public Vector3 MoveVector {
            get;
            set;
        }
        public float verticalVelocity {
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
            if(AIController.instance.isMoving)
                this.MoveVector = this.transform.TransformDirection(this.MoveVector);

            if(this.MoveVector.magnitude > 1.0f)
                this.MoveVector = Vector3.Normalize(this.MoveVector);

            this.MoveVector *= PlayerInfo.MoveSpeed;

            this.MoveVector = new Vector3(this.MoveVector.x, this.verticalVelocity, this.MoveVector.z);

            this.ApplyGravity();

            AIController.characterController.Move(this.MoveVector * Time.deltaTime);
        }

        private void ApplyGravity() {
            if(this.MoveVector.y > -PlayerInfo.TerminalVelocity) {
                this.MoveVector = new Vector3(this.MoveVector.x, this.MoveVector.y - PlayerInfo.Gravity * Time.deltaTime, this.MoveVector.z);
            }

            if(AIController.characterController.isGrounded && this.MoveVector.y < -1.0f) {
                this.MoveVector = new Vector3(this.MoveVector.x, -1.0f, this.MoveVector.z);
            }
        }
    }
}