namespace Player {

    using UnityEngine;
    using System.Collections;

    using Constants;

    public class PlayerMovement : MonoBehaviour {

        public static PlayerMovement instance;

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
            if(PlayerController.instance.isMoving)
                moveVector = transform.TransformDirection(moveVector);

            if(moveVector.magnitude > 1)
                moveVector = Vector3.Normalize(moveVector);

            moveVector *= moveSpeed;

            moveVector = new Vector3(moveVector.x, verticalVelocity, moveVector.z);

            ApplyGravity();

            PlayerController.characterController.Move(moveVector * Time.deltaTime);
        }

        private void ApplyGravity() {
            if(moveVector.y > -terminalVelocity) {
                moveVector = new Vector3(moveVector.x, moveVector.y - gravity * Time.deltaTime, moveVector.z);
            }

            if(PlayerController.characterController.isGrounded && moveVector.y < -1) {
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


/*namespace Player {

    using UnityEngine;
    using System.Collections;

    using Constants;
    using GridGenerator;
    using Blocks;

    public class PlayerMovement : MonoBehaviour {

        public GridController gridController;
        public CharacterController mainController;
        public GameObject currentBlock;

        public float moveSpeed = 7.0f;
        public bool isMoving = false;
        public Vector3 moveVector;

        public Vector3 currentPosition;
        public Vector3 previousPosition;

        public PlayerValues.PlayerDirection currentDirection;
        public PlayerValues.PlayerDirection previousDirection;

        public float gravity = 100.0f;
        //private float _terminalVelocity = 1.0f;
        public float _verticalVelocity;

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
            if(isMoving)
                ProcessMovement();
            else
                ResetMovement();
            
            //ProcessMovement();
        }

        public void ResetMovement() {
            _verticalVelocity = moveVector.y;
            moveVector = Vector3.zero;
            isMoving = false;
        }

        private void GetControlInput() {
            GetCurrentBlock();
            CastRay();
            if(!isMoving) {
                if(Input.GetKeyDown(KeyCode.UpArrow)) {
                    previousDirection = currentDirection;
                    currentDirection = PlayerValues.PlayerDirection.FORWARD;
                    gridController.ActivateBlocks(currentDirection, previousDirection);
                    moveVector = new Vector3(0, 0, 1);
                    isMoving = true;
                } else if(Input.GetKeyDown(KeyCode.RightArrow)) {
                    previousDirection = currentDirection;
                    currentDirection = PlayerValues.PlayerDirection.RIGHT;
                    gridController.ActivateBlocks(currentDirection, previousDirection);
                    moveVector = new Vector3(1, 0, 0);
                    isMoving = true;
                } else if(Input.GetKeyDown(KeyCode.DownArrow)) {
                    previousDirection = currentDirection;
                    currentDirection = PlayerValues.PlayerDirection.BACKWARD;
                    gridController.ActivateBlocks(currentDirection, previousDirection);
                    ResetMovement();
                    moveVector = new Vector3(0, 0, -1);
                    isMoving = true;
                } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                    previousDirection = currentDirection;
                    currentDirection = PlayerValues.PlayerDirection.LEFT;
                    gridController.ActivateBlocks(currentDirection, previousDirection);
                    ResetMovement();
                    moveVector = new Vector3(-1, 0, 0);
                    isMoving = true;
                }
                CheckCurrentBlock();
                CastRay();
            }
        }

        private void ProcessMovement() {
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
            
            CastRay();
        }

        private void ApplyGravity() {
            if(mainController.isGrounded){
                _verticalVelocity = 0.0f;
            } else {
                _verticalVelocity -= 20;
            }
        }

        private void CheckCurrentBlock() {
            if(currentBlock.GetComponent<Block>().blockState == BlockValues.BlockState.UP)
                this.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            else if(currentBlock.GetComponent<Block>().blockState == BlockValues.BlockState.DOWN)
                this.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
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

            Vector3 tempOrigin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Debug.DrawRay(tempOrigin, rayDirection * _rayDistance, Color.red);
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
                if(hitInfo.transform.tag == "Block") {
                    currentBlock = hitInfo.transform.gameObject;
                }
            }
        }
    }
}*/
