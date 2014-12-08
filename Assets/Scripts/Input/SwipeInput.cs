namespace Input {

    using System.Collections;

    using UnityEngine;

    using Input;
    using Player;
    using AI;
    using Grid;
    using GameInfo;
    using Sound;

    public class SwipeInput : MonoBehaviour {

        public const float CM_TO_INCH = 0.393700787f;
        public const float INCH_TO_CM = 1 / CM_TO_INCH;

        public float minSwipeDistY = 150.0f;
        public float minSwipeDistX = 150.0f;

        private float _currentDPI;
        private float _defaultScale = 160.0f;

        private Vector2 _startPosition;

        void OnEnable() {
            this._currentDPI = Screen.dpi;

            if(this._currentDPI < float.Epsilon)
                this.SetupDisplay();
        }

        public bool GetInput() {
            if(Input.touchCount > 0) {
                Touch touch = Input.GetTouch(0);

                switch(touch.phase) {
                    case TouchPhase.Began:
                    this._startPosition = touch.position;
                    break;

                    case TouchPhase.Ended:
                    float swipeDistVertical = (new Vector3(0.0f, touch.position.y, 0.0f) - new Vector3(0.0f, this._startPosition.y, 0.0f)).magnitude;

                    if(swipeDistVertical > minSwipeDistY) {
                        float swipeValue = Mathf.Sign(touch.position.y - this._startPosition.y);

                        if(swipeValue > 0.0f) {
                            GridController.instance.directionPrevious = GridController.instance.directionCurrent;
                            GridController.instance.directionCurrent = PlayerInfo.MovementDirection.FORWARD;
                            if(!PlayerController.instance.isStunned) {
                                PlayerController.instance.GetInput(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                                if(!AIController.instance.isStunned)
                                    AIController.instance.GetInput(GridController.instance.directionCurrent, GridController.instance.directionPrevious);

                                if(GridController.instance.MoveCount > 0)
                                    GridController.instance.ResetUndoStates();

                                GridController.instance.ActivateBlocks(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                                GridController.instance.MoveCount += 1;
                            }

                            if(MazeInfo.MazeMoveValue != null) {
                                if(GridController.instance.MoveCount >= GridController.instance.MaxMoves * 2) {
                                    GridController.instance.warningColor.a = (GridController.instance.MoveCount / GridController.instance.MaxMoves);
                                }
                            }
                            return true;
                        } else if(swipeValue < 0.0f) {
                            GridController.instance.directionPrevious = GridController.instance.directionCurrent;
                            GridController.instance.directionCurrent = PlayerInfo.MovementDirection.BACKWARD;
                            if(!PlayerController.instance.isStunned) {
                                PlayerController.instance.GetInput(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                                if(!AIController.instance.isStunned)
                                    AIController.instance.GetInput(GridController.instance.directionCurrent, GridController.instance.directionPrevious);

                                if(GridController.instance.MoveCount > 0)
                                    GridController.instance.ResetUndoStates();

                                GridController.instance.ActivateBlocks(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                                GridController.instance.MoveCount += 1;
                            }

                            if(MazeInfo.MazeMoveValue != null) {
                                if(GridController.instance.MoveCount >= GridController.instance.MaxMoves * 2) {
                                    GridController.instance.warningColor.a = (GridController.instance.MoveCount / GridController.instance.MaxMoves);
                                }
                            }
                            return true;
                        }
                    }

                    float swipeDistHorizontal = (new Vector3(touch.position.x, 0.0f, 0.0f) - new Vector3(this._startPosition.x, 0.0f, 0.0f)).magnitude;

                    if(swipeDistHorizontal > minSwipeDistX) {
                        float swipeValue = Mathf.Sign(touch.position.x - this._startPosition.x);

                        if(swipeValue > 0.0f) {
                            GridController.instance.directionPrevious = GridController.instance.directionCurrent;
                            GridController.instance.directionCurrent = PlayerInfo.MovementDirection.RIGHT;
                            if(!PlayerController.instance.isStunned) {
                                PlayerController.instance.GetInput(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                                if(!AIController.instance.isStunned)
                                    AIController.instance.GetInput(GridController.instance.directionCurrent, GridController.instance.directionPrevious);

                                if(GridController.instance.MoveCount > 0)
                                    GridController.instance.ResetUndoStates();

                                GridController.instance.ActivateBlocks(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                                GridController.instance.MoveCount += 1;
                            }

                            if(MazeInfo.MazeMoveValue != null) {
                                if(GridController.instance.MoveCount >= GridController.instance.MaxMoves * 2) {
                                    GridController.instance.warningColor.a = (GridController.instance.MoveCount / GridController.instance.MaxMoves);
                                }
                            }
                            return true;
                        } else if(swipeValue < 0.0f) {
                            GridController.instance.directionPrevious = GridController.instance.directionCurrent;
                            GridController.instance.directionCurrent = PlayerInfo.MovementDirection.LEFT;
                            if(!PlayerController.instance.isStunned) {
                                PlayerController.instance.GetInput(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                                if(!AIController.instance.isStunned)
                                    AIController.instance.GetInput(GridController.instance.directionCurrent, GridController.instance.directionPrevious);

                                if(GridController.instance.MoveCount > 0)
                                    GridController.instance.ResetUndoStates();
                                GridController.instance.ActivateBlocks(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                                GridController.instance.MoveCount += 1;
                            }

                            if(MazeInfo.MazeMoveValue != null) {
                                if(GridController.instance.MoveCount >= GridController.instance.MaxMoves * 2) {
                                    GridController.instance.warningColor.a = (GridController.instance.MoveCount / GridController.instance.MaxMoves);
                                }
                            }
                            return true;
                        }
                    }
                    break;
                }
            }
            return false;
        }

        private void SetupDisplay(){
            switch(Application.platform) {

                case RuntimePlatform.Android: {
                        var width = Mathf.Max(Screen.currentResolution.width, Screen.currentResolution.height);
                        var height = Mathf.Min(Screen.currentResolution.width, Screen.currentResolution.height);

                        if(width >= 1280) {
                            if(height >= 800)
                                this._currentDPI = 285.0f;
                            else
                                this._currentDPI = 312.0f;
                        } else if(width >= 1024)
                            this._currentDPI = 171.0f;
                        else if(width >= 960)
                            this._currentDPI = 256.0f;
                        else if(width >= 800)
                            this._currentDPI = 240.0f;
                        else
                            this._currentDPI = 160.0f;
                        break;
                    }

                case RuntimePlatform.IPhonePlayer: {
                        var width = Mathf.Max(Screen.currentResolution.width, Screen.currentResolution.height);
                        //var height = Mathf.Min(Screen.currentResolution.width, Screen.currentResolution.height);
                        if(width >= 2048)
                            this._currentDPI = 290.0f;
                        else if(width >= 1136)
                            this._currentDPI = 326.0f;
                        else if(width >= 1024)
                            this._currentDPI = 160.0f;
                        else if(width >= 960)
                            this._currentDPI = 326.0f;
                        else
                            this._currentDPI = 160.0f;
                        break;
                    }

                default:
                    this.minSwipeDistY = (this._defaultScale * 0.5f) * CM_TO_INCH;
                    this.minSwipeDistX = this._defaultScale * CM_TO_INCH;
                    return;
            }
            this.minSwipeDistY = (this._currentDPI * 0.5f) * CM_TO_INCH;
            this.minSwipeDistX = this._currentDPI * CM_TO_INCH;
        }

    }
}