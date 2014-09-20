namespace AI {

    using UnityEngine;
    using System.Collections;

    using GridGenerator;
    using Constants;
    using Blocks;

    public class AIController : MonoBehaviour {

        public static CharacterController characterController;
        public static GridController gridController;
        public static AIController instance;
        public GameObject currentBlock;

        public PlayerValues.PlayerDirection currentDirection;
        public PlayerValues.PlayerDirection previousDirection;

        public bool isMoving = false;

        void OnTriggerEnter(Collider obj) {
            if(obj.tag.Equals("Player")) {
                Debug.Log(obj.tag);
                UnityEditor.EditorApplication.isPlaying = false;
                Application.Quit();
            }
        }

        void Awake() {
            characterController = GetComponent<CharacterController>() as CharacterController;
            gridController = GameObject.FindGameObjectWithTag("GridController").GetComponent<GridController>();
            instance = this;
        }

        void Update() {
            if(Camera.main == null)
                return;

            GetInput();

            if(CastCollisionRays())
                AIMovement.instance.UpdateMovement();
        }

        private void GetInput() {
            if(isMoving)
                return;

            GetCurrentBlock();
            AIMovement.instance.verticalVelocity = AIMovement.instance.moveVector.y;
            AIMovement.instance.moveVector = Vector3.zero;

            if(Input.GetKeyDown(KeyCode.UpArrow)) {
                previousDirection = currentDirection;
                currentDirection = PlayerValues.PlayerDirection.FORWARD;
                //gridController.ActivateBlocks(currentDirection, previousDirection);
                AIMovement.instance.moveVector = new Vector3(0, 0, -1);
                isMoving = true;
            } else if(Input.GetKeyDown(KeyCode.RightArrow)) {
                previousDirection = currentDirection;
                currentDirection = PlayerValues.PlayerDirection.RIGHT;
                //gridController.ActivateBlocks(currentDirection, previousDirection);
                AIMovement.instance.moveVector = new Vector3(-1, 0, 0);
                isMoving = true;
            } else if(Input.GetKeyDown(KeyCode.DownArrow)) {
                previousDirection = currentDirection;
                currentDirection = PlayerValues.PlayerDirection.BACKWARD;
                //gridController.ActivateBlocks(currentDirection, previousDirection);
                AIMovement.instance.moveVector = new Vector3(0, 0, 1);
                isMoving = true;
            } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                previousDirection = currentDirection;
                currentDirection = PlayerValues.PlayerDirection.LEFT;
                //gridController.ActivateBlocks(currentDirection, previousDirection);
                AIMovement.instance.moveVector = new Vector3(1, 0, 0);
                isMoving = true;
            }
            CheckCurrentBlock();
        }

        private void CheckCurrentBlock() {
            if(currentBlock.GetComponent<Block>().blockState == BlockValues.BlockState.UP)
                this.transform.position = new Vector3(transform.position.x, 2.75f, transform.position.z);
            else if(currentBlock.GetComponent<Block>().blockState == BlockValues.BlockState.DOWN)
                this.transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
        }

        private bool CastCollisionRays() {
            RaycastHit hitInfo;
            Vector3 rayDirection = Vector3.zero;

            switch(currentDirection) {
                case PlayerValues.PlayerDirection.FORWARD:
                rayDirection += transform.TransformDirection(Vector3.back);
                break;
                case PlayerValues.PlayerDirection.RIGHT:
                rayDirection += transform.TransformDirection(Vector3.left);
                break;
                case PlayerValues.PlayerDirection.BACKWARD:
                rayDirection += transform.TransformDirection(Vector3.forward);
                break;
                case PlayerValues.PlayerDirection.LEFT:
                rayDirection += transform.TransformDirection(Vector3.right);
                break;
            }

            Vector3 tempOrigin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Debug.DrawRay(tempOrigin, rayDirection * 0.5f, Color.red);
            if(Physics.Raycast(tempOrigin, rayDirection, out hitInfo, 0.5f)) {
                if(hitInfo.collider != null) {
                    if(hitInfo.collider.tag == "AI") {
                        return false;
                    } else {
                        isMoving = false;
                        AIMovement.instance.verticalVelocity = AIMovement.instance.moveVector.y;
                        AIMovement.instance.moveVector = Vector3.zero;
                        return true;
                    }
                }
            }
            return true;
        }

        private void GetCurrentBlock() {
            RaycastHit hitInfo;
            currentBlock = null;
            Vector3 rayDirection = transform.TransformDirection(Vector3.down);

            Debug.DrawRay(transform.position, rayDirection * 100.0f, Color.red);
            if(Physics.Raycast(transform.position, rayDirection, out hitInfo, 5.0f)) {
                if(hitInfo.transform.tag == "Block") {
                    currentBlock = hitInfo.transform.gameObject;
                }
            }
        }
    }
}
