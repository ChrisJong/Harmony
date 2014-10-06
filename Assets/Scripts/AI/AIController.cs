namespace AI {

    using UnityEngine;
    using System.Collections;

    using Grid;
    using GameInfo;
    using Blocks;
    using Sound;

    public class AIController : MonoBehaviour {

        public static AIController instance;
        public static CharacterController characterController;

        private GameObject _currentBlock;
        private PlayerInfo.MovementDirection _currentDirection = PlayerInfo.MovementDirection.NONE;
        private bool _canUndo = false;
        public bool isMoving = false;

        void OnTriggerEnter(Collider obj) {
            if(obj.tag.Equals("Player")) {
                GameController.instance.LoadNextLevel();
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

        void Update() {
            if(Camera.main == null)
                return;

            this.CheckCurrentBlock();

            if(this.CastCollisionRays())
                AIMovement.instance.UpdateMovement();
        }

        public void GetInput(PlayerInfo.MovementDirection current, PlayerInfo.MovementDirection previous) {
            if(this.isMoving)
                return;
            this._canUndo = false;
            this.GetCurrentBlock();
            this.CheckCurrentBlock();
            AIMovement.instance.verticalVelocity = AIMovement.instance.MoveVector.y;
            AIMovement.instance.MoveVector = Vector3.zero;

            AIMovement.instance.UndoPosition = this.transform.position;

            if(Input.GetKeyDown(KeyCode.UpArrow)) {
                this._currentDirection = current;
                AIMovement.instance.RotateToMovement(90.0f);
                AIMovement.instance.MoveVector = new Vector3(0, 0, -1);
                this.isMoving = true;
            } else if(Input.GetKeyDown(KeyCode.RightArrow)) {
                this._currentDirection = current;
                AIMovement.instance.RotateToMovement(180.0f);
                AIMovement.instance.MoveVector = new Vector3(-1, 0, 0);
                this.isMoving = true;
            } else if(Input.GetKeyDown(KeyCode.DownArrow)) {
                this._currentDirection = current;
                AIMovement.instance.RotateToMovement(270.0f);
                AIMovement.instance.MoveVector = new Vector3(0, 0, 1);
                this.isMoving = true;
            } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                this._currentDirection = current;
                AIMovement.instance.RotateToMovement(0.0f);
                AIMovement.instance.MoveVector = new Vector3(1, 0, 0);
                this.isMoving = true;
            }
            this.CheckCurrentBlock();
            AIAudio.instance.Play();
        }

        public void CheckCurrentBlock() {
            if(this._currentBlock == null)
                return;

            if(this._currentBlock.GetComponent<Block>().blockState == BlockInfo.BlockState.UP) {
                this.transform.position = new Vector3(this.transform.position.x, 2.5f, this.transform.position.z);
            } else if(this._currentBlock.GetComponent<Block>().blockState == BlockInfo.BlockState.DOWN) {
                this.transform.position = new Vector3(this.transform.position.x, 1.5f, this.transform.position.z);
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

        private bool CastCollisionRays() {
            RaycastHit hitInfo;
            Vector3 rayDirection = Vector3.zero;

            switch(this._currentDirection) {
                case PlayerInfo.MovementDirection.FORWARD:
                rayDirection += this.transform.TransformDirection(Vector3.back);
                break;
                case PlayerInfo.MovementDirection.RIGHT:
                rayDirection += this.transform.TransformDirection(Vector3.left);
                break;
                case PlayerInfo.MovementDirection.BACKWARD:
                rayDirection += this.transform.TransformDirection(Vector3.forward);
                break;
                case PlayerInfo.MovementDirection.LEFT:
                rayDirection += this.transform.TransformDirection(Vector3.right);
                break;
            }

            Vector3 tempOrigin = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            Debug.DrawRay(tempOrigin, rayDirection * 0.6f, Color.red);
            if(Physics.Raycast(tempOrigin, rayDirection, out hitInfo, 0.6f)) {
                if(hitInfo.collider != null) {
                    if(hitInfo.collider.tag == "Player") {
                        //UnityEditor.EditorApplication.isPlaying = false;
                        //Application.Quit();
                        GameController.instance.LoadNextLevel();
                        return false;
                    } else {
                        if(isMoving)
                            SoundController.PlayerAudio(SoundInfo.PlayerCollision);
                        this._canUndo = true;
                        AIAudio.instance.Stop();
                        this.isMoving = false;
                        AIMovement.instance.verticalVelocity = AIMovement.instance.MoveVector.y;
                        AIMovement.instance.MoveVector = Vector3.zero;

                        this.GetCurrentBlock();
                        AIMovement.instance.CenterPlayer(this._currentBlock.transform);
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

        #region Getter / Setter
        public bool CanUndo {
            get { return this._canUndo; }
        }
        #endregion
    }
}
