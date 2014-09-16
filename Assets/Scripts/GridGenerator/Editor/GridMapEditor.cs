namespace GridGenerator {

    using System;
    using UnityEditor;
    using UnityEngine;

    using Constants;
    using Helpers;
    using Blocks;

    /// <summary>
    /// Provides a editor for the <see cref="GridMap"/> component.
    /// </summary>
    [CustomEditor(typeof(GridMap))]
    public class GridMapEditor : Editor {

        /// <summary>
        /// Holds the location of the mouse hit position.
        /// </summary>
        private Vector3 _mouseHitPosition;

        /// <summary>
        /// The String format for the block name display. (Name of block)(x coordinate)(z coordinate).
        /// </summary>
        private string sBlockName = "Block-{0}_{1}";

        /// <summary>
        /// Lets the Editor handle an event in the scene view.
        /// </summary>
        private void OnSceneGUI() {

            // if UpdateHitPosition returns True we should update the scene views so that the marker will update in real time.
            if(this.UpdateHitPosition())
                SceneView.RepaintAll();

            // calculates the location of the marker based on the location of the mouse.
            this.RecalculateMarkerPosition();

            // gets a reference to the current event (used to keyboard presses and mouse presses, mainly input related for now).
            Event currentEvent = Event.current;

            // if the mouse is positioned over the grid map allow drawing actions to occur.
            if(this.IsMouseOnLayer()) {

                // if mouse down or the mouse draged event occurs.
                if(currentEvent.type == EventType.MouseDown || currentEvent.type == EventType.MouseDrag) {

                    if(currentEvent.button == 1) {

                        // if the right mouse button was pressed we erase blocks.
                        this.Erase();
                        EditorUtility.SetDirty(target);
                        currentEvent.Use();
                    } else if(currentEvent.button == 0) {

                        // if the left mouse button was pressed we create blocks.
                        this.Draw();
                        EditorUtility.SetDirty(target);
                        currentEvent.Use();
                    }
                }
            }

            Handles.BeginGUI();
            GUI.Label(new Rect(10, Screen.height - 90, 100, 100), "LMB: Draw");
            GUI.Label(new Rect(10, Screen.height - 105, 100, 100), "RMB: Erase");
            Handles.EndGUI();
        }

        /// <summary>
        /// when the <see cref="GameObject"/> is selected set the current tool to the view tool.
        /// </summary>
        private void OnEnable() {
            Tools.current = Tool.View;
            Tools.viewTool = ViewTool.Orbit;
        }

        /// <summary>
        /// Draw a block at the pre-calculated mouse hit position.
        /// </summary>
        private void Draw() {

            // gets the reference to the grid map component/gameobject the script is connected to (GridController).
            var map = (GridMap)this.target;

            // calculates the position of the mouse over the grid layer.
            var gridPosition = this.GetGridPositionFromMouseLocation();

            // given the grid position check to see if a block has already been created at that location.
            var block = GameObject.Find(string.Format(sBlockName, gridPosition.x, gridPosition.z));

            // if there is already a block present at the location and is a child of the grid map component we can just leave it and exit this function.
            if(block != null && block.transform.parent == map.transform) {
                UnityEngine.Object.DestroyImmediate(block);
                block = null;
            }
                

            // if no game object was found we will create one.
            if(block == null) {
                switch(map.blockToPlace) {
                    case BlockValues.BlockType.EMPTYUP:
                    block = (GameObject)Instantiate(AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabBlocks, AssetPaths.EmptyBlockName));
                    block.GetComponent<Block>().SetType(BlockValues.BlockType.EMPTYUP);
                    //sBlockName = "EmptyBlock_{0}_{1}";
                    break;

                    case BlockValues.BlockType.EMPTYDOWN:
                    block = (GameObject)Instantiate(AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabBlocks, AssetPaths.EmptyBlockName));
                    block.GetComponent<Block>().SetType(BlockValues.BlockType.EMPTYDOWN);
                    //sBlockName = "EmptyBlock_{0}_{1}";
                    break;

                    case BlockValues.BlockType.UP:
                    block = (GameObject)Instantiate(AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabBlocks, AssetPaths.UpBlockName));
                    block.GetComponent<Block>().SetType(BlockValues.BlockType.UP);
                    //sBlockName = "UpBlock_{0}_{1}";
                    break;

                    case BlockValues.BlockType.RIGHT:
                    block = (GameObject)Instantiate(AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabBlocks, AssetPaths.RightBlockName));
                    block.GetComponent<Block>().SetType(BlockValues.BlockType.RIGHT);
                    //sBlockName = "RightBlock{0}_{1}";
                    break;

                    case BlockValues.BlockType.DOWN:
                    block = (GameObject)Instantiate(AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabBlocks, AssetPaths.DownBlockName));
                    block.GetComponent<Block>().SetType(BlockValues.BlockType.DOWN);
                    //sBlockName = "DownBlock{0}_{1}";
                    break;

                    case BlockValues.BlockType.LEFT:
                    block = (GameObject)Instantiate(AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabBlocks, AssetPaths.LeftBlockName));
                    block.GetComponent<Block>().SetType(BlockValues.BlockType.LEFT);
                    //sBlockName = "LeftBlock{0}_{1}";
                    break;

                    case BlockValues.BlockType.HUMAN:
                    if(!map.humanReady) {
                        block = (GameObject)Instantiate(AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabBlocks, AssetPaths.SpawnBlockHumanName));
                        block.GetComponent<Block>().SetType(BlockValues.BlockType.HUMAN);
                        map.humanReady = true;
                        map.spawnBlocks.Add(block);
                    } else
                        return;
                    //sBlockName = "SpawnBlock{0}_{1}";
                    break;

                    case BlockValues.BlockType.AI:
                    if(!map.aiReady) {
                        block = (GameObject)Instantiate(AssetProcessor.FindAsset<GameObject>(AssetPaths.PathPrefabBlocks, AssetPaths.SpawnBlockAIName));
                        block.GetComponent<Block>().SetType(BlockValues.BlockType.AI);
                        map.aiReady = true;
                        map.spawnBlocks.Add(block);
                    } else
                        return;
                    //sBlockName = "AIBlock{0}_{1}";
                    break;
                }
            }
                
            // set the block position on the grid map.
            var blockPositionInLocalSpace = new Vector3((gridPosition.x * map.blockWidth) + (map.blockWidth * 0.5f), 0, (gridPosition.z * map.blockBreadth));
            block.transform.position = map.transform.position + blockPositionInLocalSpace;

            // we scale the block to the grid sizes defined by the BlockWidth and BlockHeight fields.
            block.transform.localScale = new Vector3(map.blockWidth, map.blockHeight, map.blockBreadth);

            // we set the block as a child to the parent.
            block.transform.parent = map.transform;

            // give the block a name that will represent its location on the grid.
            block.name = string.Format(sBlockName, gridPosition.x, gridPosition.z);
        }

        /// <summary>
        /// Erases a block at the pre-caulcated mouse hit position.
        /// </summary>
        private void Erase() {

            // gets the reference to the grid map component/gameobject the script is connected to (GridController).
            var map = (GridMap)this.target;

            // calculate the position of the mouse over the grid layer.
            var gridPosition = this.GetGridPositionFromMouseLocation();

            // given the grid position check to see if a tile has already been created at that location.
            var block = GameObject.Find(string.Format(sBlockName, gridPosition.x, gridPosition.z));
            
            // if a game object was found with the same name and it is a child of the map we just destroy it.
            if(block != null && block.transform.parent == map.transform) {
                var blockScript = (Block)block.GetComponent<Block>();
                if(blockScript.blockType == BlockValues.BlockType.HUMAN) {
                    map.humanReady = false;
                    map.spawnBlocks.Remove(block).Equals(block);
                }

                if(blockScript.blockType == BlockValues.BlockType.AI) {
                    map.aiReady = false;
                    map.spawnBlocks.Remove(block).Equals(block);
                }

                UnityEngine.Object.DestroyImmediate(block);
            }
        }

        /// <summary>
        /// Calculates the location in grid coordinates (Column/Row) of the mouse position.
        /// </summary>
        /// <returns>Returns a <see cref="Vector3"/> type representing the Column and Row where the mouse is positioned over.</returns>
        private Vector3 GetGridPositionFromMouseLocation() {

            // gets the reference to the grid map component/gameobject the script is connected to (GridController).
            var map = (GridMap)this.target;

            // calculates the coloumn and row location from the mouse hit location.
            var pos = new Vector3(this._mouseHitPosition.x / map.blockWidth, map.transform.position.y, this._mouseHitPosition.z / map.blockBreadth);

            // round the numbers to the nearest whole number using 5 decimal place precision.
            pos = new Vector3((int)Math.Round(pos.x, 5, MidpointRounding.ToEven), 0, (int)Math.Round(pos.z, 5, MidpointRounding.ToEven));

            // do a check to see if the row and column are within the bounds of the grid map.
            var col = (int)pos.x;
            var row = (int)pos.z;
            if(row < 0)
                row = 0;

            if(row > map.rows - 1)
                row = map.rows - 1;

            if(col < 0)
                col = 0;

            if(col > map.columns - 1)
                col = map.columns - 1;

            // returns the column and row in vector3 format.
            return new Vector3(col, 0, row);
        }

        /// <summary>
        /// Checks to see whether the mouse pointer is over the Grid Map of not.
        /// </summary>
        /// <returns>Will return True Or False if the mouse is positioned over the Grid Map or not.</returns>
        private bool IsMouseOnLayer() {

            // gets the reference to the grid map component/gameobject the script is connected to (GridController).
            var map = (GridMap)this.target;

            // returns True or False depending if the mouse is positioned over the Grid Map or not.
            return this._mouseHitPosition.x > 0 && this._mouseHitPosition.x < (map.columns * map.blockWidth) &&
                   this._mouseHitPosition.z > 0 && this._mouseHitPosition.z < (map.rows * map.blockBreadth);
        }

        /// <summary>
        /// Recalculates the position of the marker based on the location of the mouse position.
        /// </summary>
        private void RecalculateMarkerPosition() {

            // gets the reference to the grid map component/gameobject the script is connected to (GridController).
            var map = (GridMap)this.target;

            // stores the grid location (Vector3(Column/0/Row)) based on the current location of the mouse pointer.
            var gridPosition = this.GetGridPositionFromMouseLocation();

            // stores the grid position in world space.
            var pos = new Vector3(gridPosition.x * map.blockWidth, 0, gridPosition.z * map.blockBreadth);

            // sets the GridMap.MarkerPsotion to a new value.
            map.markerPosition = map.transform.position + new Vector3(pos.x + (map.blockWidth / 2), 0, pos.z + (map.blockBreadth / 2));
        }

        /// <summary>
        /// Calculates the position of the mouse over the grid map in the local space coordinates.
        /// </summary>
        /// <returns>Returns True if the mouse is over a grid point in the grid map.</returns>
        private bool UpdateHitPosition() {

            // gets the reference to the grid map component/gameobject the script is connected to (GridController).
            var map = (GridMap)this.target;

            // builds a plane object that will help us get the coordinates for the mouse input.
            var p = new Plane(map.transform.TransformDirection(Vector3.up), map.transform.position);

            // builds a ray type from the current mouse position, will help us get information of the coorindates and if the ray hit an object.
            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            // stores the hit information/location of the ray cast.
            var hit = new Vector3();

            // stores the distance to the hit location.
            float distance;

            // we case a ray outwards to determine what location it intersects with the plane.
            if(p.Raycast(ray, out distance))
                hit = ray.origin + (ray.direction.normalized * distance); // the ray hits the plane so we calculate the hit location in world space.

            // converts the hit location from world space to local space.
            var value = map.transform.InverseTransformPoint(hit);

            // if the value is differernt than the current mouse hit location.
            // we set the new mouse location with the new loction and return true that it has hit something.
            if(value != this._mouseHitPosition) {
                this._mouseHitPosition = value;
                return true;
            }

            // return false if the hit test failed to hit the plane. meaning something is blocking it (e.g. a block).
            return false;
        }
    }
}