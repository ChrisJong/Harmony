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
        public float minSwipeDistY = 150.0f;
        public float minSwipeDistX = 150.0f;

        private Vector2 _startPosition;

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
                            PlayerController.instance.GetInput(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                            AIController.instance.GetInput(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                            GridController.instance.ActivateBlocks(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                            SoundController.PlayerAudio(SoundInfo.PlayerMovement);
                            GridController.instance.MoveCount += 1;
                            return true;
                        } else if(swipeValue < 0.0f) {
                            GridController.instance.directionPrevious = GridController.instance.directionCurrent;
                            GridController.instance.directionCurrent = PlayerInfo.MovementDirection.BACKWARD;
                            PlayerController.instance.GetInput(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                            AIController.instance.GetInput(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                            GridController.instance.ActivateBlocks(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                            SoundController.PlayerAudio(SoundInfo.PlayerMovement);
                            GridController.instance.MoveCount += 1;
                            return true;
                        }
                    }

                    float swipeDistHorizontal = (new Vector3(touch.position.x, 0.0f, 0.0f) - new Vector3(this._startPosition.x, 0.0f, 0.0f)).magnitude;

                    if(swipeDistHorizontal > minSwipeDistX) {
                        float swipeValue = Mathf.Sign(touch.position.x - this._startPosition.x);

                        if(swipeValue > 0.0f) {
                            GridController.instance.directionPrevious = GridController.instance.directionCurrent;
                            GridController.instance.directionCurrent = PlayerInfo.MovementDirection.RIGHT;
                            PlayerController.instance.GetInput(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                            AIController.instance.GetInput(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                            GridController.instance.ActivateBlocks(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                            SoundController.PlayerAudio(SoundInfo.PlayerMovement);
                            GridController.instance.MoveCount += 1;
                            return true;
                        } else if(swipeValue < 0.0f) {
                            GridController.instance.directionPrevious = GridController.instance.directionCurrent;
                            GridController.instance.directionCurrent = PlayerInfo.MovementDirection.LEFT;
                            PlayerController.instance.GetInput(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                            AIController.instance.GetInput(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                            GridController.instance.ActivateBlocks(GridController.instance.directionCurrent, GridController.instance.directionPrevious);
                            SoundController.PlayerAudio(SoundInfo.PlayerMovement);
                            GridController.instance.MoveCount += 1;
                            return true;
                        }
                    }
                    break;
                }
            }
            return false;
        }

    }
}