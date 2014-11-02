namespace MainMenu {

    using System.Collections;

    using UnityEngine;

    using Helpers;
    using GameInfo;

    public class LevelSelectController : MonoBehaviour {

        public static GameObject thisObject;
        public static LevelSelectController instance;
        public GUIText informationField;

        public GameObject previousButton;
        public GameObject nextButton;

        public int currentPage = 0;
        public int totalPages = 2;
        public float distanceBetweenPage = 20;

        private int _row = 4;
        private int _col = 6;
        private int _totalBlocks = 48;
        private int _blocksPerPage = 24;
        private int _count = 0;

        void Awake() {
            instance = this;
            thisObject = this.transform.gameObject;
            this._count = 0;
            this._totalBlocks = this._row * this._col;
            this._blocksPerPage = (this._row * this._col) / 2;
            this.totalPages = this._totalBlocks / this._blocksPerPage;
            this.currentPage = 0;
            SetupLevelBlocks();
        }

        void LateUpdate() {
            if(this.currentPage == 0) {
                this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(-2.5f, this.transform.position.y, this.transform.position.z), 6.0f * Time.deltaTime);
            } else if(this.currentPage == 1) {
                this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(-this.distanceBetweenPage + -2.5f, this.transform.position.y, this.transform.position.z), 6.0f * Time.deltaTime);
            }

            if(this.currentPage <= 0) {
                this.previousButton.SetActive(false);
                this.nextButton.SetActive(true);
            } else if(this.currentPage >= (totalPages - 1)) {
                this.previousButton.SetActive(true);
                this.nextButton.SetActive(false);
            } else {
                this.previousButton.SetActive(true);
                this.nextButton.SetActive(true);
            }
        }

        private void SetupLevelBlocks() {
            GameObject block = null;

            for(int i = 0; i < this.totalPages; i++) {
                for(int r = 0; r < this._row; r++) {
                    for(int c = 0; c < this._col; c++) {
#if UNITY_EDITOR
                        block = (GameObject)Instantiate(AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabMainMenu, AssetPaths.LevelNumberBlockName));
#else
                        block = (GameObject)Instantiate(ResourceManager.instance.levelNumberBlock);
#endif
                        block.GetComponent<LevelBlockButton>().SetId(this._count + 1);
                        if(i >= 1)
                            block.transform.position = new Vector3(thisObject.transform.position.x + (this.distanceBetweenPage * i) + c * 1, thisObject.gameObject.transform.position.y - r, thisObject.transform.position.z);
                        else
                            block.transform.position = new Vector3(thisObject.transform.position.x + c * 1, thisObject.gameObject.transform.position.y - r, thisObject.transform.position.z);

                        block.transform.parent = thisObject.transform;
                        this._count++;

                        if(this._count >= MazeInfo.MaxMazeLength)
                            break;
                    }
                }
            }
        }
    }
}