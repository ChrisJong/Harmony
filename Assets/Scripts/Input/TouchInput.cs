namespace Input {

    using System.Collections;

    using UnityEngine;

    public class TouchInput : MonoBehaviour {
        public static int currentTouchID = 0;

        private int touchID = 64;
        private Ray ray;
        private RaycastHit rayHitInfo;
        private ITouchable touchObject = null;

        void Update() {
            if(Input.touchCount > 0) {
                foreach(Touch touch in Input.touches) {
                    currentTouchID = touch.fingerId;
                    ray = Camera.main.ScreenPointToRay(touch.position);

                    if(Physics.Raycast(ray, out rayHitInfo)) {
                        touchObject = rayHitInfo.transform.GetComponent(typeof(ITouchable)) as ITouchable;

                        if(touchObject != null) {
                            switch(touch.phase) {
                                case TouchPhase.Began:
                                touchObject.OnTouchBegan();
                                touchID = currentTouchID;
                                break;

                                case TouchPhase.Ended:
                                touchObject.OnTouchEnded();
                                break;

                                case TouchPhase.Moved:
                                touchObject.OnTouchMoved();
                                break;

                                case TouchPhase.Stationary:
                                touchObject.OnTouchStayed();
                                break;

                                case TouchPhase.Canceled:
                                touchObject.OnTouchCanceled();
                                break;
                            }
                        }
                    } else if(touchObject != null && touchID == currentTouchID) {
                        switch(touch.phase) {
                            case TouchPhase.Ended:
                            touchObject.OnTouchEndedGlobal();
                            break;

                            case TouchPhase.Moved:
                            touchObject.OnTouchMovedGlobal();
                            break;

                            case TouchPhase.Stationary:
                            touchObject.OnTouchStayedGlobal();
                            break;

                            case TouchPhase.Canceled:
                            touchObject.OnTouchCanceledGlobal();
                            break;
                        }
                    }
                }
            }
        }
    }
}