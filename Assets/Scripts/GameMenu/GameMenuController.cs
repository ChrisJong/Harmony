namespace GameMenu {

    using System.Collections;
    using UnityEngine;

    using GameInfo;
    using Helpers;

    public class GameMenuController : MonoBehaviour {

        public static GameMenuController instance;

        public GameObject menu;

        void Awake() {
            instance = this;
            if(this.transform.GetChild(0).gameObject.name == "Menu")
                this.menu = this.transform.GetChild(0).gameObject;

            this.menu.SetActive(false);
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
