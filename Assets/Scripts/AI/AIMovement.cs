namespace AI {

    using UnityEngine;
    using System.Collections;

    using Constants;

    public class AIMovement : MonoBehaviour {

        public static AIMovement instance;

        public float moveSpeed = 10.0f;
        public float gravity = 1000.0f;
        public float terminalVelocity = 20.0f;

        public Vector3 moveVector;
        public float verticalVelocity;

        void Awake() {
            instance = this;
        }

        public void UpdateMovement() {
            ProcessMovement();
        }

        private void ProcessMovement() {
            if(AIController.instance.isMoving)
                moveVector = transform.TransformDirection(moveVector);

            if(moveVector.magnitude > 1)
                moveVector = Vector3.Normalize(moveVector);

            moveVector *= moveSpeed;

            moveVector = new Vector3(moveVector.x, verticalVelocity, moveVector.z);

            ApplyGravity();

            AIController.characterController.Move(moveVector * Time.deltaTime);
        }

        private void ApplyGravity() {
            if(moveVector.y > -terminalVelocity) {
                moveVector = new Vector3(moveVector.x, moveVector.y - gravity * Time.deltaTime, moveVector.z);
            }

            if(AIController.characterController.isGrounded && moveVector.y < -1) {
                moveVector = new Vector3(moveVector.x, -1, moveVector.z);
            }
        }

        public Vector3 MoveVector {
            get { return moveVector; }
            set { moveVector = value; }
        }

        public float VerticalVelocity {
            get { return verticalVelocity; }
            set { verticalVelocity = value; }
        }
    }
}