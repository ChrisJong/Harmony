﻿namespace GameMenu {

    using System.Collections;
    using UnityEngine;

    using GameInfo;
    using Helpers;

    public class GameMenuController : MonoBehaviour {

        public static GameMenuController instance;

        public GameObject menu;

        public GUIText moveText;

        void Awake() {
            instance = this;
            if(this.transform.GetChild(0).gameObject.name == "Menu")
                this.menu = this.transform.GetChild(0).gameObject;

            this.menu.SetActive(false);
            this.moveText = this.transform.GetChild(2).gameObject.guiText as GUIText;
            this.moveText.pixelOffset = new Vector2(10.0f, GlobalInfo.ScreenHeight - 10.0f);
            //this.menu.transform.position = GameMenuInfo.MenuVector;
        }

        public void SetMenu(bool set) {
            this.menu.SetActive(set);
        }

        public static void FindOrCreate() {
            GameObject tempController = GameObject.FindGameObjectWithTag("GameMenuController");

            if(tempController == null) {
#if UNITY_EDITOR
                tempController = AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabGameMenu, AssetPaths.GameMenuControllerName);
#else
                tempController = Controller.instance.gameMenuController;
#endif
                Instantiate(tempController).name = AssetPaths.GameMenuControllerName;
                return;
            } else {
                return;
            }
        }
    }
}
