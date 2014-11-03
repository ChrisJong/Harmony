namespace Input {

    using System.Collections;

    using UnityEngine;

    using Input;
    using Player;
    using AI;
    using Grid;
    using GameInfo;
    using Sound;
    using MainMenu;

    public class SwipeMenuInput : MonoBehaviour {
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
                            if(MainMenuController.instance.currentMenuScene == MainMenuInfo.MenuTypes.MAINMENU)
                                MainMenuController.instance.currentMenuScene = MainMenuInfo.MenuTypes.NEWGAME;
                            else if(MainMenuController.instance.currentMenuScene == MainMenuInfo.MenuTypes.LEVELSELECT)
                                MainMenuController.instance.currentMenuScene = MainMenuInfo.MenuTypes.MAINMENU;
                            return true;
                        } else if(swipeValue < 0.0f) {
                            if(MainMenuController.instance.currentMenuScene == MainMenuInfo.MenuTypes.MAINMENU)
                                MainMenuController.instance.currentMenuScene = MainMenuInfo.MenuTypes.LEVELSELECT;
                            return true;
                        }
                    }

                    float swipeDistHorizontal = (new Vector3(touch.position.x, 0.0f, 0.0f) - new Vector3(this._startPosition.x, 0.0f, 0.0f)).magnitude;

                    if(swipeDistHorizontal > minSwipeDistX) {
                        float swipeValue = Mathf.Sign(touch.position.x - this._startPosition.x);

                        if(swipeValue > 0.0f) {
                            if(MainMenuController.instance.currentMenuScene == MainMenuInfo.MenuTypes.MAINMENU)
                                MainMenuController.instance.currentMenuScene = MainMenuInfo.MenuTypes.INSTRUCTIONS;
                            else if(MainMenuController.instance.currentMenuScene == MainMenuInfo.MenuTypes.CREDITS)
                                MainMenuController.instance.currentMenuScene = MainMenuInfo.MenuTypes.MAINMENU;
                            return true;
                        } else if(swipeValue < 0.0f) {
                            if(MainMenuController.instance.currentMenuScene == MainMenuInfo.MenuTypes.MAINMENU)
                                MainMenuController.instance.currentMenuScene = MainMenuInfo.MenuTypes.CREDITS;
                            else if(MainMenuController.instance.currentMenuScene == MainMenuInfo.MenuTypes.INSTRUCTIONS)
                                MainMenuController.instance.currentMenuScene = MainMenuInfo.MenuTypes.MAINMENU;
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