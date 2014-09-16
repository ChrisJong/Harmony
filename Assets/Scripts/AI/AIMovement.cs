namespace AI {

    using UnityEngine;
    using System.Collections;

    using Constants;
    using GridGenerator;
    using Blocks;
    using Player;

    public class AIMovement : MonoBehaviour {

        public GridController gridController;
        public CharacterController mainController;
        public PlayerMovement humanPlayer;
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

        void OnTriggerEnter(Collider obj) {
            if(obj.tag.Equals("Player")) {
                Debug.Log(obj.tag);
                UnityEditor.EditorApplication.isPlaying = false;
                Application.Quit();
            }
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
            if(humanPlayer == null)
                humanPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

            if(!isMoving) {
                if(humanPlayer.currentDirection == PlayerValues.PlayerDirection.FORWARD) {
                    previousDirection = currentDirection;
                    currentDirection = PlayerValues.PlayerDirection.BACKWARD;
                    CastRay();
                    ResetMovement();
                    isMoving = true;
                    CastRay();
                    
                    //moveVector = new Vector3(0, 0, -1);
                } else if(humanPlayer.currentDirection == PlayerValues.PlayerDirection.RIGHT) {
                    previousDirection = currentDirection;
                    currentDirection = PlayerValues.PlayerDirection.LEFT;
                    CastRay();
                    ResetMovement();
                    isMoving = true;
                    CastRay();
                    
                    //moveVector = new Vector3(-1, 0, 0);
                } else if(humanPlayer.currentDirection == PlayerValues.PlayerDirection.BACKWARD) {
                    previousDirection = currentDirection;
                    currentDirection = PlayerValues.PlayerDirection.FORWARD;
                    CastRay();
                    ResetMovement();
                    isMoving = true;
                    CastRay();
                    
                    
                    //moveVector = new Vector3(0, 0, 1);
                } else if(humanPlayer.currentDirection == PlayerValues.PlayerDirection.LEFT) {
                    previousDirection = currentDirection;
                    currentDirection = PlayerValues.PlayerDirection.RIGHT;
                    CastRay();
                    ResetMovement();
                    isMoving = true;
                    CastRay();
                    
                    
                    //moveVector = new Vector3(1, 0, 0);
                }
            } else
                return;
        }

        private void ProcessMovement() {
            GetCurrentBlock();
            if(isMoving) {
                switch(humanPlayer.currentDirection) {
                    case PlayerValues.PlayerDirection.FORWARD:
                    moveVector = new Vector3(0, 0, -1);
                    break;
                    case PlayerValues.PlayerDirection.RIGHT:
                    moveVector = new Vector3(-1, 0, 0);
                    break;
                    case PlayerValues.PlayerDirection.BACKWARD:
                    moveVector = new Vector3(0, 0, 1);
                    break;

                    case PlayerValues.PlayerDirection.LEFT:
                    moveVector = new Vector3(1, 0, 0);
                    break;
                }
                moveVector = transform.TransformDirection(moveVector);
            } else {
                moveVector = new Vector3(0, moveVector.y, 0);
            }

            if(moveVector.magnitude > 1.0f)
                moveVector = Vector3.Normalize(moveVector);

            moveVector *= moveSpeed;

            moveVector = new Vector3(moveVector.x, _verticalVelocity, moveVector.z);

            //ApplyGravity();

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
                this.moveVector = new Vector3(moveVector.x, 50.0f, moveVector.z);
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
                    if(hitInfo.collider.tag == "Player") {
                        return;
                    } else if(hitInfo.collider == transform.GetComponent<BoxCollider>()) {
                        Debug.Log("Hit Bpx");
                    } else {
                        Debug.Log("Stop Moving");
                        isMoving = false;
                    }
                    //ResetMovement();
                }
                Debug.Log(isMoving.ToString());
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
