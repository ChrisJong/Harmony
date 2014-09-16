namespace Player {

    using UnityEngine;
    using System.Collections;

    using Constants;
    using GridGenerator;
    using Blocks;
    using AI;

    public class PlayerMovement : MonoBehaviour {

        public GridController gridController;
        public CharacterController mainController;
        public AIMovement aiPlayer;
        public GameObject currentBlock;

        public float moveSpeed = 7.0f;
        public bool isMoving = false;
        public Vector3 moveVector;

        public Vector3 currentPosition;
        public Vector3 previousPosition;

        public PlayerValues.PlayerDirection currentDirection;
        public PlayerValues.PlayerDirection previousDirection;

        public float gravity = 21.0f;
        //private float _terminalVelocity = 1.0f;
        private float _verticalVelocity;

        private float _rayDistance = 0.5f;

        public int Round(float a) {
            return (int)(a + 0.5f);
        }

        void Awake() {
            gridController = GameObject.FindGameObjectWithTag("GridController").GetComponent<GridController>();
            mainController = (CharacterController)GetComponent("CharacterController");
        }

        void Update() {
            GetControlInput();
            CastRay();
            ProcessMovement();
        }

        public void ResetMovement() {
            _verticalVelocity = moveVector.y;
            moveVector = Vector3.zero;
            isMoving = false;
        }

        private void GetControlInput() {
            if(aiPlayer == null)
                aiPlayer = GameObject.FindGameObjectWithTag("AI").GetComponent<AIMovement>();

            if(!isMoving && !aiPlayer.isMoving) {
                if(Input.GetKeyDown(KeyCode.UpArrow)) {
                    previousDirection = currentDirection;
                    currentDirection = PlayerValues.PlayerDirection.FORWARD;
                    //aiPlayer.currentDirection = currentDirection;
                    gridController.ActivateBlocks(currentDirection, previousDirection);
                    ResetMovement();
                    moveVector = new Vector3(0, 0, 1);
                    isMoving = true;
                } else if(Input.GetKeyDown(KeyCode.RightArrow)) {
                    previousDirection = currentDirection;
                    currentDirection = PlayerValues.PlayerDirection.RIGHT;
                    //aiPlayer.currentDirection = currentDirection;
                    gridController.ActivateBlocks(currentDirection, previousDirection);
                    ResetMovement();
                    moveVector = new Vector3(1, 0, 0);
                    isMoving = true;
                } else if(Input.GetKeyDown(KeyCode.DownArrow)) {
                    previousDirection = currentDirection;
                    currentDirection = PlayerValues.PlayerDirection.BACKWARD;
                    //aiPlayer.currentDirection = currentDirection;
                    gridController.ActivateBlocks(currentDirection, previousDirection);
                    ResetMovement();
                    moveVector = new Vector3(0, 0, -1);
                    isMoving = true;
                } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                    previousDirection = currentDirection;
                    currentDirection = PlayerValues.PlayerDirection.LEFT;
                    //aiPlayer.currentDirection = currentDirection;
                    gridController.ActivateBlocks(currentDirection, previousDirection);
                    ResetMovement();
                    moveVector = new Vector3(-1, 0, 0);
                    isMoving = true;
                }
                
            } else
                return;
        }

        private void ProcessMovement() {
            GetCurrentBlock();
            if(isMoving) {
                
                moveVector = transform.TransformDirection(moveVector);
            } else {
                moveVector = new Vector3(0, moveVector.y, 0);
            }
            if(moveVector.magnitude > 1.0f)
                moveVector = Vector3.Normalize(moveVector);

            moveVector *= moveSpeed;

            moveVector = new Vector3(moveVector.x, _verticalVelocity, moveVector.z);

            ApplyGravity();

            mainController.Move(moveVector * Time.deltaTime);
            
        }

        private void ApplyGravity() {

            var distanceToGround = transform.position.y - currentBlock.transform.position.y;

            //Debug.Log(distanceToGround.ToString());

            float offset = 0.2f;

            if(distanceToGround >= 0.8f && distanceToGround <= 1.2f) {
                //Debug.Log("Reset");
                transform.position = new Vector3(transform.position.x, Round(transform.position.y), transform.position.z);
                return;
            }

            if(distanceToGround < (1.0f - offset)) {
                //Debug.Log("Up");
                moveVector = new Vector3(moveVector.x, 50.0f, moveVector.z);
            } else if(distanceToGround > 1.0f + offset)
                moveVector = new Vector3(moveVector.x, -20.0f, moveVector.z);
        }

        private void CastRay() {
            RaycastHit hitInfo;
            Vector3 rayDirection = Vector3.zero;

            switch(currentDirection) {
                case PlayerValues.PlayerDirection.FORWARD:
                rayDirection += transform.TransformDirection(Vector3.forward);
                break;
                case PlayerValues.PlayerDirection.RIGHT:
                rayDirection += transform.TransformDirection(Vector3.right);
                break;
                case PlayerValues.PlayerDirection.BACKWARD:
                rayDirection += transform.TransformDirection(Vector3.back);
                break;
                case PlayerValues.PlayerDirection.LEFT:
                rayDirection += transform.TransformDirection(Vector3.left);
                break;
            }
            Vector3 tempOrigin = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
            Debug.DrawRay(tempOrigin, rayDirection * 100.0f, Color.red);
            if(Physics.Raycast(tempOrigin, rayDirection, out hitInfo, _rayDistance)) {
                //Debug.Log(hitInfo.transform.name);
                if(hitInfo.collider != null) {
                    if(hitInfo.collider.tag == "AI") {
                        return;
                    } else
                        isMoving = false;
                    //ResetMovement();
                }
            }
        }

        private void GetCurrentBlock() {
            RaycastHit hitInfo;
            Vector3 rayDirection = transform.TransformDirection(Vector3.down);

            Debug.DrawRay(transform.position, rayDirection * 100.0f, Color.red);
            if(Physics.Raycast(transform.position, rayDirection, out hitInfo, 5.0f)) {
                //Debug.Log(hitInfo.transform.name);
                if(hitInfo.transform.tag == "Block") {
                    currentBlock = hitInfo.transform.gameObject;
                }
            }
        }
    }
}
