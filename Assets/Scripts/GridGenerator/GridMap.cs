namespace GridGenerator {

    using UnityEngine;
    using System.Collections.Generic;

    using Constants;
    using Helpers;
    using Blocks;

    /// <summary>
    /// Provides a component for grid mapping.
    /// </summary>
    [DisallowMultipleComponent]
    //[System.Serializable]
    public class GridMap : MonoBehaviour {

        /// <summary>
        /// gets or sets the number of rows of blocks.
        /// </summary>
        [Range(2, 20)]
        public int rows;

        /// <summary>
        /// gets or sets the number of columns of blocks.
        /// </summary>
        [Range(2, 20)]
        public int columns;

        /// <summary>
        /// gets or sets the value of the block width.
        /// </summary>
        [Range(1.0f, 20.0f)]
        public float blockWidth = 1.0f;

        /// <summary>
        /// gets or sets the value of the block height.
        /// </summary>
        [Range(1.0f, 20.0f)]
        public float blockBreadth = 1.0f;

        /// <summary>
        /// gets or sets the actual block height (Y-Coordinates).
        /// </summary>
        [Range(1.0f, 20.0f)]
        public float blockHeight = 1.0f;

        [HideInInspector]
        public List<GameObject> spawnBlocks;

        /// <summary>
        /// Used by the GridMap Editor Script to indicate a grid location.
        /// </summary>
        [HideInInspector]
        public Vector3 markerPosition;

        /// <summary>
        /// gets or sets the type of block to place down onto the grid. (Every block placed will be in the down position, even if it says UP).
        /// </summary>
        public BlockValues.BlockType blockToPlace;

        /// <summary>
        /// used to see if the grid map is complete. needs 2 spawn point blocks for the player and ai to be complete. else if will log a warning.
        /// </summary>
        [HideInInspector]
        public bool humanReady = false;
        [HideInInspector]
        public bool aiReady = false;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="GridMap"/> class.
        /// </summary>
        public GridMap() {
            this.rows = 5;
            this.columns = 5;
            this.blockBreadth = 1.0f;
            this.blockWidth = 1.0f;
            this.blockToPlace = BlockValues.BlockType.EMPTYDOWN;
            this.spawnBlocks = new List<GameObject>();
        }

        public void Awake() {

            GenerateWalls();
            GeneratePlayers();

            if(!humanReady || !aiReady) {
                Debug.LogError("No Spawn Blocks Are In The Grid");
                UnityEditor.EditorApplication.isPlaying = false;
                Application.Quit();
                return;
            }
        }

        /// <summary>
        /// Generates the outer walls for the maze.
        /// </summary>
        private void GenerateWalls() {
            float xPos = columns * 0.5f;
            float yPos = transform.position.y + blockHeight;
            float zPos = rows * 0.5f;

            GameObject _wallOrigin = new GameObject("WallContainer");
            _wallOrigin.transform.position = new Vector3(xPos, yPos, zPos);
            for(int i = 0; i < 4; i++) {
                if(i == 0) {
                    GameObject wall = new GameObject("Wall");
                    wall.AddComponent("BoxCollider");
                    wall.transform.position = Vector3.zero;
                    wall.transform.parent = _wallOrigin.transform;
                    wall.transform.localPosition = new Vector3(0, blockHeight * 0.5f, -zPos);
                    wall.GetComponent<BoxCollider>().size = new Vector3(columns, blockHeight * 2.0f, 0);
                    wall.tag = "Wall";
                }

                if(i == 1) {
                    GameObject wall = new GameObject("Wall");
                    wall.AddComponent("BoxCollider");
                    wall.transform.position = Vector3.zero;
                    wall.transform.parent = _wallOrigin.transform;
                    wall.transform.localPosition = new Vector3(0, blockHeight * 0.5f, zPos);
                    wall.GetComponent<BoxCollider>().size = new Vector3(columns, blockHeight * 2.0f, 0);
                    wall.tag = "Wall";
                }

                if(i == 2) {
                    GameObject wall = new GameObject("Wall");
                    wall.AddComponent("BoxCollider");
                    wall.transform.position = Vector3.zero;
                    wall.transform.parent = _wallOrigin.transform;
                    wall.transform.localPosition = new Vector3(xPos, blockHeight * 0.5f, 0);
                    wall.GetComponent<BoxCollider>().size = new Vector3(0, blockHeight * 2.0f, rows);
                    wall.tag = "Wall";
                }

                if(i == 3) {
                    GameObject wall = new GameObject("Wall");
                    wall.AddComponent("BoxCollider");
                    wall.transform.position = Vector3.zero;
                    wall.transform.parent = _wallOrigin.transform;
                    wall.transform.localPosition = new Vector3(-xPos, blockHeight * 0.5f, 0);
                    wall.GetComponent<BoxCollider>().size = new Vector3(0, blockHeight * 2.0f, rows);
                    wall.tag = "Wall";
                }
            }
        }

        /// <summary>
        /// Finds the Spawn Blocks To Genereate The Player And AI Objects.
        /// </summary>
        private void GeneratePlayers() {
            if(spawnBlocks.Count == 2) {
                foreach(GameObject obj in spawnBlocks) {
                    var type = obj.GetComponent<Block>().blockType;
                    var spawnPoint = obj.transform.FindChild("SpawnPoint").gameObject;
                    if(type == BlockValues.BlockType.HUMAN) {
                        Instantiate((GameObject)AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabPlayer, "Player"), spawnPoint.transform.position, Quaternion.identity);
                        humanReady = true;
                    }

                    if(type == BlockValues.BlockType.AI) {
                        Instantiate((GameObject)AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabPlayer, "AI"), spawnPoint.transform.position, Quaternion.identity);
                        aiReady = true;
                    }
                }
            } else {
                Debug.LogError("Missing Spawn Blocks For Either Human/AI");
                UnityEditor.EditorApplication.isPlaying = false;
                Application.Quit();
                return;
            }
        }

        /// <summary>
        /// When the game object is selected this will draw the grid.
        /// </summary>
        private void OnDrawGizmosSelected() {
            // stores map width, height and position.
            var mapHeight = this.rows * this.blockBreadth;
            var mapWidth = this.columns * this.blockWidth;
            var position = this.transform.position;

            // draws the grid borders.
            Gizmos.color = Color.white;
            Gizmos.DrawLine(position, position + new Vector3(mapWidth, 0, 0));
            Gizmos.DrawLine(position, position + new Vector3(0, 0, mapHeight));
            Gizmos.DrawLine(position + new Vector3(mapWidth, 0, 0), position + new Vector3(mapWidth, 0, mapHeight));
            Gizmos.DrawLine(position + new Vector3(0, 0, mapHeight), position + new Vector3(mapWidth, 0, mapHeight));

            // draw the grid cells.
            Gizmos.color = Color.white;
            for(int i = 1; i < this.columns; i++)
                Gizmos.DrawLine(position + new Vector3(i * this.blockWidth, 0, 0), position + new Vector3(i * this.blockWidth, 0, mapHeight));

            for(int i = 1; i < this.rows; i++)
                Gizmos.DrawLine(position + new Vector3(0, 0, i * this.blockBreadth), position + new Vector3(mapWidth, 0, i * this.blockBreadth));

            // draws the marker position. (in this case a red wireframe block).
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(this.markerPosition, new Vector3(this.blockWidth, this.blockHeight, this.blockBreadth));
        }
    }
}