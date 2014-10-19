namespace Grid {

    using UnityEngine;
    using System.Collections.Generic;

    using GameInfo;
    using Helpers;
    using Blocks;

    /// <summary>
    /// Provides a component for grid mapping.
    /// </summary>
    [DisallowMultipleComponent]
    //[System.Serializable]
    public class GridMap : MonoBehaviour {

        public static GridMap instance;

        /// <summary>
        /// gets or sets the number of rows of blocks.
        /// </summary>
        [Range(2, 20)]
        [SerializeField]
        public int rows = 5;
        /// <summary>
        /// gets or sets the number of columns of blocks.
        /// </summary>
        [Range(2, 20)]
        [SerializeField]
        public int columns = 5;

        /// <summary>
        /// gets or sets the value of the block width.
        /// </summary>
        private float _blockWidth;

        /// <summary>
        /// gets or sets the value of the block height.
        /// </summary>
        private float _blockBreadth;

        /// <summary>
        /// gets or sets the actual block height (Y-Coordinates).
        /// </summary>
        private float _blockHeight;

        /// <summary>
        /// gets or sets the type of block to place down onto the grid. (Every block placed will be in the down position, even if it says UP).
        /// </summary>
        [HideInInspector]
        public BlockInfo.BlockTypes blockToPlace;
        [HideInInspector]
        public BlockInfo.BlockState blockState;
        [HideInInspector]
        public BlockInfo.BlockDirection blockOneDirection;
        [HideInInspector]
        public BlockInfo.BlockDirection blockTwoDirection;
        [HideInInspector]
        public int blockNumber;

        /// <summary>
        /// Used by the GridMap Editor Script to indicate a grid location.
        /// </summary>
        [HideInInspector]
        public Vector3 markerPosition;

        /// <summary>
        /// Spawn Locations For Player And AI.
        /// </summary>
        [HideInInspector]
        public Vector3 humanSpawnPoint = Vector3.zero;
        [HideInInspector]
        public Vector3 aiSpawnPoint = Vector3.zero;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="GridMap"/> class.
        /// </summary>
        public GridMap() {
            this.rows = 5;
            this.columns = 5;
            this._blockWidth = BlockInfo.BlockWidth;
            this._blockHeight = BlockInfo.BlockHeight;
            this._blockBreadth = BlockInfo.BlockBreadth;
            this.blockToPlace = BlockInfo.BlockTypes.NONE;
        }

        public void Awake() {
            instance = this;
            Instantiate(Resources.Load("ResourceController") as GameObject);
            GenerateWalls();
            //GeneratePlayers();
            GridController.FindOrCreate();
        }

        /// <summary>
        /// Generates the outer walls for the maze.
        /// </summary>
        private void GenerateWalls() {
            float xPos = columns * 0.5f;
            float yPos = transform.position.y + _blockHeight;
            float zPos = rows * 0.5f;

            GameObject _wallOrigin = new GameObject("WallContainer");
            _wallOrigin.transform.position = new Vector3(xPos, yPos, zPos);
            for(int i = 0; i < 4; i++) {
                if(i == 0) {
                    GameObject wall = new GameObject("Wall");
                    wall.AddComponent("BoxCollider");
                    wall.transform.position = Vector3.zero;
                    wall.transform.parent = _wallOrigin.transform;
                    wall.transform.localPosition = new Vector3(0, _blockHeight * 0.5f, -zPos);
                    wall.GetComponent<BoxCollider>().size = new Vector3(columns, _blockHeight * 2.0f, 0);
                    wall.tag = "Wall";
                }

                if(i == 1) {
                    GameObject wall = new GameObject("Wall");
                    wall.AddComponent("BoxCollider");
                    wall.transform.position = Vector3.zero;
                    wall.transform.parent = _wallOrigin.transform;
                    wall.transform.localPosition = new Vector3(0, _blockHeight * 0.5f, zPos);
                    wall.GetComponent<BoxCollider>().size = new Vector3(columns, _blockHeight * 2.0f, 0);
                    wall.tag = "Wall";
                }

                if(i == 2) {
                    GameObject wall = new GameObject("Wall");
                    wall.AddComponent("BoxCollider");
                    wall.transform.position = Vector3.zero;
                    wall.transform.parent = _wallOrigin.transform;
                    wall.transform.localPosition = new Vector3(xPos, _blockHeight * 0.5f, 0);
                    wall.GetComponent<BoxCollider>().size = new Vector3(0, _blockHeight * 2.0f, rows);
                    wall.tag = "Wall";
                }

                if(i == 3) {
                    GameObject wall = new GameObject("Wall");
                    wall.AddComponent("BoxCollider");
                    wall.transform.position = Vector3.zero;
                    wall.transform.parent = _wallOrigin.transform;
                    wall.transform.localPosition = new Vector3(-xPos, _blockHeight * 0.5f, 0);
                    wall.GetComponent<BoxCollider>().size = new Vector3(0, _blockHeight * 2.0f, rows);
                    wall.tag = "Wall";
                }
            }
        }

        /// <summary>
        /// Finds the Spawn Blocks To Genereate The Player And AI Objects.
        /// </summary>
        public void GeneratePlayers() {
            var humanSpawnPoint = this.humanSpawnPoint;
            var aiSpawnPoint = this.aiSpawnPoint;

            if(humanSpawnPoint == aiSpawnPoint) {
                humanSpawnPoint = new Vector3(0 + BlockInfo.BlockWidth * 0.5f, 2.5f, 0 + BlockInfo.BlockBreadth * 0.5f);
                aiSpawnPoint = new Vector3((this.columns * BlockInfo.BlockWidth) - (BlockInfo.BlockWidth * 0.5f), 2.5f, (this.rows * BlockInfo.BlockBreadth) - (BlockInfo.BlockBreadth * 0.5f));
            } else {
                humanSpawnPoint = new Vector3(humanSpawnPoint.x + (BlockInfo.BlockWidth * 0.5f), 2.5f, humanSpawnPoint.z + (BlockInfo.BlockBreadth * 0.5f));
                aiSpawnPoint = new Vector3(aiSpawnPoint.x + (BlockInfo.BlockWidth * 0.5f), 2.5f, aiSpawnPoint.z + (BlockInfo.BlockBreadth * 0.5f));
            }
#if UNITY_EDITOR
            Instantiate((GameObject)AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabPlayer, AssetPaths.PlayerName), humanSpawnPoint, Quaternion.identity);
            Instantiate((GameObject)AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabPlayer, AssetPaths.AIName), aiSpawnPoint, Quaternion.identity);
#else
            Instantiate((GameObject)Controller.instance.playerObject, humanSpawnPoint, Quaternion.identity);
            Instantiate((GameObject)Controller.instance.aiObject, aiSpawnPoint, Quaternion.identity);
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        /// When the game object is selected this will draw the grid.
        /// </summary>
        private void OnDrawGizmosSelected() {
            // stores map width, height and position.
            var mapHeight = this.rows * this._blockBreadth;
            var mapWidth = this.columns * this._blockWidth;
            var position = this.transform.position;

            var sceneCamera = UnityEditor.SceneView.lastActiveSceneView;

            if(sceneCamera != null)
                sceneCamera.LookAt(new Vector3(this.transform.position.x + (this.columns * this._blockWidth) * 0.5f, this.transform.position.y, this.transform.position.z + (this.rows * this._blockBreadth) * 0.5f), Quaternion.Euler(90, 0, 0));
            
            // draws the grid borders.
            Gizmos.color = Color.white;
            Gizmos.DrawLine(position, position + new Vector3(mapWidth, 0, 0));
            Gizmos.DrawLine(position, position + new Vector3(0, 0, mapHeight));
            Gizmos.DrawLine(position + new Vector3(mapWidth, 0, 0), position + new Vector3(mapWidth, 0, mapHeight));
            Gizmos.DrawLine(position + new Vector3(0, 0, mapHeight), position + new Vector3(mapWidth, 0, mapHeight));

            // draw the grid cells.
            Gizmos.color = Color.white;
            for(int i = 1; i < this.columns; i++)
                Gizmos.DrawLine(position + new Vector3(i * this._blockWidth, 0, 0), position + new Vector3(i * this._blockWidth, 0, mapHeight));

            for(int i = 1; i < this.rows; i++)
                Gizmos.DrawLine(position + new Vector3(0, 0, i * this._blockBreadth), position + new Vector3(mapWidth, 0, i * this._blockBreadth));

            // draws the marker position. (in this case a red wireframe block).
            Gizmos.color = Color.cyan;
            //Gizmos.DrawWireCube(this.markerPosition, new Vector3(this._blockWidth, this._blockHeight, this._blockBreadth));
            Gizmos.DrawCube(this.markerPosition, new Vector3(this._blockWidth, this._blockHeight + 0.1f, this._blockBreadth));

            // draws the marker position for player and ai spawn points.
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector3(this.humanSpawnPoint.x + (BlockInfo.BlockWidth * 0.5f), 0.0f, this.humanSpawnPoint.z + (BlockInfo.BlockBreadth * 0.5f)), new Vector3(this._blockWidth, this._blockHeight + 0.5f, this._blockBreadth));
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(new Vector3(this.aiSpawnPoint.x + (BlockInfo.BlockWidth * 0.5f), 0.0f, this.aiSpawnPoint.z + (BlockInfo.BlockBreadth * 0.5f)), new Vector3(this._blockWidth, this._blockHeight + 0.5f, this._blockBreadth));
        }

#endif
        #region
        public float BlockWidth {
            get { return this._blockWidth; }
        }

        public float BlockBreadth {
            get { return this._blockBreadth; }
        }

        public float BlockHeight {
            get { return this._blockHeight; }
        }
        #endregion
    }
}