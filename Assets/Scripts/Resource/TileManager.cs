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

        public List<Material> ballonUpTile;
        public List<Material> ballonDownTile;

        private bool _hasSkinChanged = false;
        public GlobalInfo.Skin _currentSkin = GlobalInfo.Skin.STANDARD;

        void Awake() {
            instance = this;
        }

        public void ChangeSkin(GlobalInfo.Skin skin) {
            this._hasSkinChanged = true;
            this._currentSkin = skin;
        }

        public Material GetCurrentSkinMaterial(string type, int id) {
            if(type.ToLower().Equals("down")) {
                if(this._currentSkin == GlobalInfo.Skin.STANDARD)
                    return this.standardDownTile[id];
                else if(this._currentSkin == GlobalInfo.Skin.BALLON)
                    return this.ballonDownTile[id];
            } else if(type.ToLower().Equals("up")) {
                if(this._currentSkin == GlobalInfo.Skin.STANDARD)
                    return this.standardUpTile[id];
                else if(this._currentSkin == GlobalInfo.Skin.BALLON)
                    return this.ballonUpTile[id];
            }
            return null;
        }

        public List<Material> GetCurrentSkin(string type) {
            if(type.ToLower().Equals("down")) {
                if(this._currentSkin == GlobalInfo.Skin.STANDARD)
                    return this.standardDownTile;
                else if(this._currentSkin == GlobalInfo.Skin.BALLON)
                    return this.ballonDownTile;
            } else if(type.ToLower().Equals("up")) {
                if(this._currentSkin == GlobalInfo.Skin.STANDARD)
                    return this.standardUpTile;
                else if(this._currentSkin == GlobalInfo.Skin.BALLON)
                    return this.ballonUpTile;
            }
            return null;
        }

        public int ChangeMaterialID() {
            int temp = 0;

            if(this._currentSkin == GlobalInfo.Skin.STANDARD) {
                if(this.standardDownTile.Count == this.standardUpTile.Count)
                    temp = Random.Range(0, this.standardUpTile.Count);
            } else if(this._currentSkin == GlobalInfo.Skin.BALLON) {
                if(this.ballonDownTile.Count == this.ballonUpTile.Count)
                    temp = Random.Range(0, this.ballonDownTile.Count);
            }

            return temp;
        }

        public static void FindOrCreate() {
            GameObject temp = GameObject.FindWithTag("TileManager");
            
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

        public bool HasSkinChanged {
            get { return this._hasSkinChanged; }
        }

        #endregion
    }
}