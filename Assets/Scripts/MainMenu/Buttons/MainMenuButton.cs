namespace MainMenu {

    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    using Input;
    using Helpers;
    using GameInfo;
    using Resource;

#if UNITY_IPHONE || UNITY_ANDROID
    public class MainMenuButton : MonoBehaviour, ITouchable {
#else
    public class MainMenuButton : MonoBehaviour {
#endif
        public Material tileEnter;
        public Material tileExit;
        public Material iconEnter;
        public Material iconExit;

        public MainMenuInfo.MenuTypes buttonType;

        private int _materialID;
        private MeshRenderer _renderer;
        private Material[] _blockMaterials;
        private List<EmptyMenuBlock> _emptyBlockContainer;

        void Awake() {
            this._renderer = this.transform.GetComponent<MeshRenderer>() as MeshRenderer;
            this._blockMaterials = this._renderer.materials;

            this._emptyBlockContainer = new List<EmptyMenuBlock>(this.gameObject.GetComponentsInChildren<EmptyMenuBlock>());
        }

        void Start() {
            this.SetupSkin();
        }

        public void SetupSkin() {
            this._materialID = TileManager.instance.ChangeMaterialID();
            this.tileEnter = TileManager.instance.GetCurrentSkinMaterial("up", this._materialID);
            this.tileExit = TileManager.instance.GetCurrentSkinMaterial("down", this._materialID);

            this._blockMaterials[0] = this.tileExit;
            this._blockMaterials[1] = this.iconExit;
            this._renderer.materials = this._blockMaterials;
        }

#if UNITY_IPHONE || UNITY_ANDROID
        public void OnTouchBegan() {
            this._blockMaterials[0] = this.tileEnter;
            this._blockMaterials[1] = this.iconEnter;
            this._renderer.materials = this._blockMaterials;
            if(this._emptyBlockContainer.Count > 0) {
                for(int i = 0; i < this._emptyBlockContainer.Count; i++)
                    this._emptyBlockContainer[i].OnInputEnter();
            }
        }

        public void OnTouchEnded() {
            switch(buttonType) {
                case MainMenuInfo.MenuTypes.NEWGAME:
                this._blockMaterials[0] = this.tileExit;
                this._blockMaterials[1] = this.iconExit;
                this._renderer.materials = this._blockMaterials;
                if(this._emptyBlockContainer.Count > 0) {
                    for(int i = 0; i < this._emptyBlockContainer.Count; i++)
                        this._emptyBlockContainer[i].OnInputExit();
                }

                MainMenuController.instance.currentMenuScene = buttonType;
                break;

                case MainMenuInfo.MenuTypes.INSTRUCTIONS:
                this._blockMaterials[0] = this.tileExit;
                this._blockMaterials[1] = this.iconExit;
                this._renderer.materials = this._blockMaterials;
                if(this._emptyBlockContainer.Count > 0) {
                    for(int i = 0; i < this._emptyBlockContainer.Count; i++)
                        this._emptyBlockContainer[i].OnInputExit();
                }
                
                MainMenuController.instance.currentMenuScene = buttonType;
                break;

                case MainMenuInfo.MenuTypes.LEVELSELECT:
                this._blockMaterials[0] = this.tileExit;
                this._blockMaterials[1] = this.iconExit;
                this._renderer.materials = this._blockMaterials;
                if(this._emptyBlockContainer.Count > 0) {
                    for(int i = 0; i < this._emptyBlockContainer.Count; i++)
                        this._emptyBlockContainer[i].OnInputExit();
                }
                
                MainMenuController.instance.currentMenuScene = buttonType;
                break;

                case MainMenuInfo.MenuTypes.CREDITS:
                this._blockMaterials[0] = this.tileExit;
                this._blockMaterials[1] = this.iconExit;
                this._renderer.materials = this._blockMaterials;
                if(this._emptyBlockContainer.Count > 0) {
                    for(int i = 0; i < this._emptyBlockContainer.Count; i++)
                        this._emptyBlockContainer[i].OnInputExit();
                }

                MainMenuController.instance.currentMenuScene = buttonType;
                break;

                case MainMenuInfo.MenuTypes.MAINMENU:
                this._blockMaterials[0] = this.tileExit;
                this._blockMaterials[1] = this.iconExit;
                this._renderer.materials = this._blockMaterials;
                MainMenuController.instance.currentMenuScene = buttonType;
                break;

                case MainMenuInfo.MenuTypes.PREV:
                this._blockMaterials[0] = this.tileExit;
                this._blockMaterials[1] = this.iconExit;
                this._renderer.materials = this._blockMaterials;
                
                if(LevelSelectController.instance.currentPage <= 0) {
                    LevelSelectController.instance.currentPage = 0;
                } else {
                    LevelSelectController.instance.currentPage -= 1;
                }
                break;

                case MainMenuInfo.MenuTypes.NEXT:
                this._blockMaterials[0] = this.tileExit;
                this._blockMaterials[1] = this.iconExit;
                this._renderer.materials = this._blockMaterials;
                
                if(LevelSelectController.instance.currentPage >= LevelSelectController.instance.totalPages) {
                    LevelSelectController.instance.currentPage += 1;
                } else {
                    LevelSelectController.instance.currentPage = 1;
                }
                break;

                case MainMenuInfo.MenuTypes.EXIT:
                MazeDataHelper.SaveGameData();
                Application.Quit();
                break;
            }
        }

        public void OnTouchMoved() {
            this._blockMaterials[0] = this.tileExit;
            this._blockMaterials[1] = this.iconExit;
            this._renderer.materials = this._blockMaterials;
            if(this._emptyBlockContainer.Count > 0) {
                for(int i = 0; i < this._emptyBlockContainer.Count; i++)
                    this._emptyBlockContainer[i].OnInputExit();
            }
        }

        public void OnTouchStayed() {
            this._blockMaterials[0] = this.tileEnter;
            this._blockMaterials[1] = this.iconEnter;
            this._renderer.materials = this._blockMaterials;
            if(this._emptyBlockContainer.Count > 0) {
                for(int i = 0; i < this._emptyBlockContainer.Count; i++)
                    this._emptyBlockContainer[i].OnInputExit();
            }
        }

        public void OnTouchCanceled() {
            this._blockMaterials[0] = this.tileExit;
            this._blockMaterials[1] = this.iconExit;
            this._renderer.materials = this._blockMaterials;
            if(this._emptyBlockContainer.Count > 0) {
                for(int i = 0; i < this._emptyBlockContainer.Count; i++)
                    this._emptyBlockContainer[i].OnInputExit();
            }
        }

        public void OnTouchEndedGlobal() {
            this._blockMaterials[0] = this.tileExit;
            this._blockMaterials[1] = this.iconExit;
            this._renderer.materials = this._blockMaterials;
            if(this._emptyBlockContainer.Count > 0) {
                for(int i = 0; i < this._emptyBlockContainer.Count; i++)
                    this._emptyBlockContainer[i].OnInputExit();
            }
        }

        public void OnTouchMovedGlobal() {
            this._blockMaterials[0] = this.tileExit;
            this._blockMaterials[1] = this.iconExit;
            this._renderer.materials = this._blockMaterials;
            if(this._emptyBlockContainer.Count > 0) {
                for(int i = 0; i < this._emptyBlockContainer.Count; i++)
                    this._emptyBlockContainer[i].OnInputExit();
            }
        }

        public void OnTouchStayedGlobal() {
            this._blockMaterials[0] = this.tileEnter;
            this._blockMaterials[1] = this.iconEnter;
            this._renderer.materials = this._blockMaterials;
            if(this._emptyBlockContainer.Count > 0) {
                for(int i = 0; i < this._emptyBlockContainer.Count; i++)
                    this._emptyBlockContainer[i].OnInputEnter();
            }
        }

        public void OnTouchCanceledGlobal() {
            this._blockMaterials[0] = this.tileExit;
            this._blockMaterials[1] = this.iconExit;
            this._renderer.materials = this._blockMaterials;
            if(this._emptyBlockContainer.Count > 0) {
                for(int i = 0; i < this._emptyBlockContainer.Count; i++)
                    this._emptyBlockContainer[i].OnInputExit();
            }
        }
#else
        private void OnMouseEnter() {
            this._blockMaterials[0] = this.tileEnter;
            this._blockMaterials[1] = this.iconEnter;
            this._renderer.materials = this._blockMaterials;

            if(this._emptyBlockContainer.Count > 0) {
                for(int i = 0; i < this._emptyBlockContainer.Count; i++)
                    this._emptyBlockContainer[i].OnInputEnter();
            }
        }

        private void OnMouseExit() {
            this._blockMaterials[0] = this.tileExit;
            this._blockMaterials[1] = this.iconExit;
            this._renderer.materials = this._blockMaterials;

            if(this._emptyBlockContainer.Count > 0) {
                for(int i = 0; i < this._emptyBlockContainer.Count; i++)
                    this._emptyBlockContainer[i].OnInputExit();
            }
        }

        private void OnMouseUp() {
            switch(buttonType) {
                case MainMenuInfo.MenuTypes.NEWGAME:
                    this._blockMaterials[0] = this.tileExit;
                    this._blockMaterials[1] = this.iconExit;
                    this._renderer.materials = this._blockMaterials;
                    if(this._emptyBlockContainer.Count > 0) {
                        for(int i = 0; i < this._emptyBlockContainer.Count; i++)
                            this._emptyBlockContainer[i].OnInputExit();
                    }

                    MainMenuController.instance.currentMenuScene = buttonType;
                    break;

                case MainMenuInfo.MenuTypes.INSTRUCTIONS:
                    this._blockMaterials[0] = this.tileExit;
                    this._blockMaterials[1] = this.iconExit;
                    this._renderer.materials = this._blockMaterials;
                    if(this._emptyBlockContainer.Count > 0) {
                        for(int i = 0; i < this._emptyBlockContainer.Count; i++)
                            this._emptyBlockContainer[i].OnInputExit();
                    }

                    MainMenuController.instance.currentMenuScene = buttonType;
                    break;

                case MainMenuInfo.MenuTypes.LEVELSELECT:
                    this._blockMaterials[0] = this.tileExit;
                    this._blockMaterials[1] = this.iconExit;
                    this._renderer.materials = this._blockMaterials;
                    if(this._emptyBlockContainer.Count > 0) {
                        for(int i = 0; i < this._emptyBlockContainer.Count; i++)
                            this._emptyBlockContainer[i].OnInputExit();
                    }

                    MainMenuController.instance.currentMenuScene = buttonType;
                    break;

                case MainMenuInfo.MenuTypes.CREDITS:
                    this._blockMaterials[0] = this.tileExit;
                    this._blockMaterials[1] = this.iconExit;
                    this._renderer.materials = this._blockMaterials;
                    if(this._emptyBlockContainer.Count > 0) {
                        for(int i = 0; i < this._emptyBlockContainer.Count; i++)
                            this._emptyBlockContainer[i].OnInputExit();
                    }

                    MainMenuController.instance.currentMenuScene = buttonType;
                    break;

                case MainMenuInfo.MenuTypes.MAINMENU:
                    this._blockMaterials[0] = this.tileExit;
                    this._blockMaterials[1] = this.iconExit;
                    this._renderer.materials = this._blockMaterials;

                    MainMenuController.instance.currentMenuScene = buttonType;
                    break;

                case MainMenuInfo.MenuTypes.PREV:
                    this._blockMaterials[0] = this.tileExit;
                    this._blockMaterials[1] = this.iconExit;
                    this._renderer.materials = this._blockMaterials;

                    if(LevelSelectController.instance.currentPage <= 0) {
                        LevelSelectController.instance.currentPage = 0;
                    } else {
                        LevelSelectController.instance.currentPage -= 1;
                    }
                    break;

                case MainMenuInfo.MenuTypes.NEXT:
                    this._blockMaterials[0] = this.tileExit;
                    this._blockMaterials[1] = this.iconExit;
                    this._renderer.materials = this._blockMaterials;

                    if(LevelSelectController.instance.currentPage >= LevelSelectController.instance.totalPages) {
                        LevelSelectController.instance.currentPage += 1;
                    } else {
                        LevelSelectController.instance.currentPage = 1;
                    }
                    break;

                case MainMenuInfo.MenuTypes.EXIT:
                    MazeDataHelper.SaveGameData();
                    Application.Quit();
                    break;
            }
        }
#endif
    }
}
