namespace Player {

    using UnityEngine;
    using System.Collections;

    using AI;
    using Grid;
    using GameInfo;
    using Blocks;
    using Sound;
    using Animation;
    using Resource;

    public class PlayerController : MonoBehaviour {

        public static PlayerController instance;
        public static CharacterController characterController;

        public GameObject indication;
        public AnimateTextureSheet indicationTexture;
        public Animator charactorAnimator;

        public bool isMoving = false;
        public bool isStunned = false;
        public bool isDeath = false;

        private GameObject _currentBlock;
        private PlayerInfo.MovementDirection _currentDirection = PlayerInfo.MovementDirection.NONE;
        private bool _canUndo = false;

        void Awake() {
            instance = this;
            characterController = GetComponent<CharacterController>() as CharacterController;

            this.indicationTexture = this.indication.GetComponent<AnimateTextureSheet>() as AnimateTextureSheet;
            this._currentBlock = null;
            this._currentDirection = PlayerInfo.MovementDirection.NONE;            
        }

        void Update() {
            if(Camera.main == null)
                return;

            this.CheckCurrentBlock();

            if(!this.isMoving && !this.isDeath) {
                if(!this.indicationTexture.isPlaying)
                    this.indication.SetActive(true);
            }

            if(GridController.instance.blocksReady)
                this.CastCollisionRays();

            PlayerMovement.instance.UpdateMovement();
        }

        public void SpawnEndAnimation() {
            GameObject endAnimation = Instantiate(ResourceManager.instance.endAnimation) as GameObject;
            endAnimation.transform.rotation = PlayerMovement.instance.characterModel.transform.rotation;
            endAnimation.transform.position = PlayerMovement.instance.characterModel.transform.position;
        }

        public void GetInput(PlayerInfo.MovementDirection current, PlayerInfo.MovementDirection previous) {
            if(this.isMoving)
                return;


            if(this.indicationTexture.isPlaying)
                this.indication.SetActive(false);

            this._canUndo = false;
            this.GetCurrentBlock();
            this.CheckCurrentBlock();
            
            PlayerMovement.instance.ResetMovement();
            PlayerMovement.instance.UndoPosition = this.transform.position;

            if(current == PlayerInfo.MovementDirection.FORWARD) {
                this._currentDirection = current;
                PlayerMovement.instance.RotateToMovement(0.0f);
                PlayerMovement.instance.MoveVector = new Vector3(0, 0, 1);
            } else if(current == PlayerInfo.MovementDirection.RIGHT) {
                this._currentDirection = current;
                PlayerMovement.instance.RotateToMovement(90.0f);
                PlayerMovement.instance.MoveVector = new Vector3(1, 0, 0);
            } else if(current == PlayerInfo.MovementDirection.BACKWARD) {
                this._currentDirection = current;
                PlayerMovement.instance.RotateToMovement(180.0f);
                PlayerMovement.instance.MoveVector = new Vector3(0, 0, -1);
            } else if(current == PlayerInfo.MovementDirection.LEFT) {
                this._currentDirection = current;
                PlayerMovement.instance.RotateToMovement(270.0f);
                PlayerMovement.instance.MoveVector = new Vector3(-1, 0, 0);
            }

            this.CheckCurrentBlock();
            PlayerAudio.instance.PlayMovement();
            this.isMoving = true;
        }

        public void CheckCurrentBlock() {
            if(this._currentBlock == null)
                return;

            if(this._currentBlock.GetComponent<BlockClass>().BlockState == BlockInfo.BlockState.UP)
                this.transform.position = new Vector3(this.transform.position.x, 2.85f, this.transform.position.z);
            else if(this._currentBlock.GetComponent<BlockClass>().BlockState == BlockInfo.BlockState.DOWN)
                this.transform.position = new Vector3(this.transform.position.x, 1.85f, this.transform.position.z);
        }

        public void UndoMovement() {
            if(this._canUndo) {
                this.isMoving = false;
                GridController.instance.UndoMovement();
                PlayerMovement.instance.UndoMovement();
                this._currentDirection = PlayerInfo.MovementDirection.NONE;
                this._canUndo = false;
            }
        }

        private void CastCollisionRays() {
            RaycastHit hitInfo;
            Vector3 rayDirection = Vector3.zero;
            
            switch(this._currentDirection) {
                case PlayerInfo.MovementDirection.FORWARD:
                rayDirection += this.transform.TransformDirection(Vector3.forward);
                break;
                case PlayerInfo.MovementDirection.RIGHT:
                rayDirection += this.transform.TransformDirection(Vector3.right);
                break;
                case PlayerInfo.MovementDirection.BACKWARD:
                rayDirection += this.transform.TransformDirection(Vector3.back);
                break;
                case PlayerInfo.MovementDirection.LEFT:
                rayDirection += this.transform.TransformDirection(Vector3.left);
                break;
            }

            Vector3 tempOrigin = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            Debug.DrawRay(tempOrigin, rayDirection * 0.5f, Color.red);
            if(Physics.Raycast(tempOrigin, rayDirection, out hitInfo, 0.5f)) {
                if(hitInfo.collider != null) {
                    if(hitInfo.collider.tag == "AI") {
                        //Debug.Log("AI");
                        //GameController.instance.LoadNextLevel();
                        //UnityEditor.EditorApplication.isPlaying = false;
                        //Application.Quit();
                        GameController.instance.isStageFinished = true;
                    } else {
                        if(isMoving) {
                            if(hitInfo.collider.tag == "Block")
                                PlayerAudio.instance.PlayCollision();
                        }

                        if(hitInfo.collider.tag == "Block" || hitInfo.collider.tag == "Wall") {
                            this._canUndo = true;
                            this.isMoving = false;
                            PlayerMovement.instance.VerticalVelocity = PlayerMovement.instance.MoveVector.y;
                            PlayerMovement.instance.MoveVector = Vector3.zero;

                            this.GetCurrentBlock();
                            PlayerMovement.instance.CenterPlayer(this._currentBlock.transform);
                        }
                    }
                }
            }
        }

        private void GetCurrentBlock() {
            RaycastHit hitInfo;
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
