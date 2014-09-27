namespace Player {

    using UnityEngine;
    using System.Collections;

    using Sound;
    using Constants;

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

        void Awake() {
            instance = this;
        }

        void FixedUpdate() {
            this.ApplyGravity();
        }

        public void UpdateMovement() {
            this.ProcessMovement();
        }

        private void ProcessMovement() {
            if(PlayerController.instance.isMoving)
                this.MoveVector = this.transform.TransformDirection(this.MoveVector);

            if(MoveVector.magnitude > 1.0f)
                this.MoveVector = Vector3.Normalize(this.MoveVector);

            this.MoveVector *= PlayerValues.MoveSpeed;

            this.MoveVector = new Vector3(this.MoveVector.x, this.VerticalVelocity, this.MoveVector.z);

            this.ApplyGravity();
            
            PlayerController.characterController.Move(this.MoveVector * Time.deltaTime);
        }

        private void ApplyGravity() {
            if(this.MoveVector.y > -PlayerValues.TerminalVelocity) {
                this.MoveVector = new Vector3(this.MoveVector.x, this.MoveVector.y - PlayerValues.Gravity * Time.deltaTime, this.MoveVector.z);
            }

            if(PlayerController.characterController.isGrounded && this.MoveVector.y < -1.0f) {
                this.MoveVector = new Vector3(this.MoveVector.x, -1.0f, this.MoveVector.z);
            }
        }
    }
}