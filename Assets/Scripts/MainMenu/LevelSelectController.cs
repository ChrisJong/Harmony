namespace MainMenu {

    using System.Collections;

    using UnityEngine;

    using Helpers;
    using GameInfo;

    public class LevelSelectController : MonoBehaviour {

        public static GameObject thisObject;
        public static LevelSelectController instance;
        public GUIText informationField;

        private int _page = 0;

        void Awake() {
            instance = this;
            thisObject = this.transform.gameObject;
            SetupLevelBlocks();
        }

        public static void SetupLevelBlocks() {
            int maxRows = MathHelper.RoundToWhole((MazeInfo.MaxMazeLength / 8.0f));
            int maxColums = 8;
            int count = 0;
            GameObject block = null;

            if(maxRows == 0) {
                for(int i = 0; i < maxColums; i++) {
                    if(count >= MazeInfo.MaxMazeLength)
                        break;
#if UNITY_EDITOR
                    block = (GameObject)Instantiate(AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabMainMenu, AssetPaths.LevelNumberBlockName));
#else
                    block = (GameObject)Instantiate(ResourceManager.instance.levelNumberBlock);
#endif
                    block.GetComponent<LevelBlockButton>().SetId(count + 1);
                    block.transform.position = new Vector3(thisObject.transform.position.x + i * 1, thisObject.gameObject.transform.position.y, thisObject.transform.position.z);
                    block.transform.parent = thisObject.transform;
                    count++;
                }
            } else {
                for(int i = 0; i < maxRows; i++) {
                    for(int j = 0; j < maxColums; j++) {
                        if(count >= MazeInfo.MaxMazeLength)
                            break;

#if UNITY_EDITOR
                        block = (GameObject)Instantiate(AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabMainMenu, AssetPaths.LevelNumberBlockName));
#else
                        block = (GameObject)Instantiate(ResourceManager.instance.levelNumberBlock);
#endif
                        block.GetComponent<LevelBlockButton>().SetId(count + 1);
                        block.transform.position = new Vector3(thisObject.transform.position.x + j * 1, thisObject.gameObject.transform.position.y - i * 1, thisObject.transform.position.z);
                        block.transform.parent = thisObject.transform;
                        count++;
                    }
                }
            }
        }
    }
}