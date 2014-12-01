namespace Resource {

    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;
    using Helpers;

    public class TileManager : MonoBehaviour {

        public static TileManager instance;

        // Here Will Hold A List Of Tile Skins To Be Used In The Game. Add More Lists Undernearth It If Needed. Two Lists Needed For One Skin (Up/Down).
        public List<Material> standardUpTile;
        public List<Material> standardDownTile;

        private GlobalInfo.Skin _currentSkin = GlobalInfo.Skin.STANDARD;

        void Awake() {
            instance = this;
        }

        public void ChangeSkin(GlobalInfo.Skin skin) {
            this._currentSkin = skin;
        }

        public static void FindOrCreate() {
            GameObject temp = GameObject.FindGameObjectWithTag("TileManager");

            if(temp == null) {
#if UNITY_EDITOR
                temp = AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefab + "Resources/", "TileManager");
#else
                temp = ResourceManager.instance.tileManager;
#endif
                Instantiate(temp).name = "TileManager";
            }
        }

        #region Getter/Setter

        public GlobalInfo.Skin CurrentSkin {
            get { return this._currentSkin; }
        }

        #endregion
    }
}