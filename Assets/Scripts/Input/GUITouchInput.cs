namespace Input {

    using System.Collections;

    using UnityEngine;

    public class GUITouchInput : MonoBehaviour {
        public static int curTouchID = 0;
        public int touchID = 64;

        public virtual void Update() {
            if(Input.touchCount <= 0) {
                this.OnNoTouch();
            } else {
                foreach(Touch touch in Input.touches) {
                    curTouchID = touch.fingerId;

                    if(this.GetComponent<GUITexture>() != null && (this.GetComponent<GUITexture>().HitTest(touch.position))) {
                        if(touch.phase == TouchPhase.Began) {
                            this.OnTouchBegan();
                            this.touchID = curTouchID;
                        }
                        if(touch.phase == TouchPhase.Ended) {
                            this.OnTouchEnded();
                        }
                        if(touch.phase == TouchPhase.Moved) {
                            this.OnTouchMoved();
                        }
                        if(touch.phase == TouchPhase.Stationary) {
                            this.OnTouchStayed();
                        }
                    } else {
                        this.OnTouchCanceled();
                    }
                }
            }
        }

        public virtual void OnNoTouch() {
        }
        public virtual void OnTouchBegan() {
        }
        public virtual void OnTouchEnded() {
        }
        public virtual void OnTouchMoved() {
        }
        public virtual void OnTouchStayed() {
        }
        public virtual void OnTouchCanceled() {
        }
    }
}