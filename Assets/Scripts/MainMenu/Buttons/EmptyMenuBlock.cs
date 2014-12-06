namespace MainMenu {

    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;
    using Resource;

    public class EmptyMenuBlock : MonoBehaviour {

        public Material tileUpMaterial;
        public Material tileDownMaterial;

        private int _materialID;

        private Material _blockMaterial;
        private MeshRenderer _renderer;

        void Awake() {
            this._renderer = this.transform.GetComponent<MeshRenderer>() as MeshRenderer;
            this._blockMaterial = this._renderer.material;

        }

        void Start() {
            this.SetupSkin();
        }

        public void SetupSkin() {
            this._materialID = TileManager.instance.ChangeMaterialID();
            this.tileUpMaterial = TileManager.instance.GetCurrentSkinMaterial("up", this._materialID);
            this.tileDownMaterial = TileManager.instance.GetCurrentSkinMaterial("down", this._materialID);

            this._blockMaterial = this.tileDownMaterial;
            this._renderer.material = this._blockMaterial;
        }

        public void OnInputEnter() {
            this._blockMaterial = this.tileUpMaterial;
            this._renderer.material = this._blockMaterial;
        }

        public void OnInputExit() {
            this._blockMaterial = this.tileDownMaterial;
            this._renderer.material = this._blockMaterial;
        }
    }
}