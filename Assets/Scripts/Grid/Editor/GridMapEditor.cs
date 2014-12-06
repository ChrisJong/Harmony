namespace Grid {

    using System;
    using UnityEditor;
    using UnityEngine;

    using GameInfo;
    using Helpers;
    using Blocks;

    /// <summary>
    /// Provides a editor for the <see cref="GridMap"/> component.
    /// </summary>
    [CustomEditor(typeof(GridMap))]
    public class GridMapEditor : Editor {

        private GridMap _target;
        GridMapEditorWindow blockWindow;

        /// <summary>
        /// Holds the location of the mouse hit position.
        /// </summary>
        private Vector3 _mouseHitPosition;

        /// <summary>
        /// The String format for the block name display. (Name of block)(x coordinate)(z coordinate).
        /// </summary>
        private string _blockName = "Block-{0}_{1}";

        /// <summary>
        /// Variables for placing spawn points on the grid map.
        /// </summary>
        private bool _placeSpawnPoints = false;
        private bool _humanSapwnSet = false;
        private bool _aiSpawnSet = false;
        private Vector2 _tempHumanPoint = Vector2.zero;
        private Vector2 _tempAIPoint = Vector2.zero;

        private bool _switchPointSet = false;
        private int _switchBlockCount = 0;
        private GameObject _switchParentNode;
        private GameObject _warpParentNode;

        /// <summary>
        /// GUI Input.
        /// </summary>
        private bool _spawnFoldOut = false;

        /// <summary>
        /// Lets the Editor handle an event in the scene view.
        /// </summary>
        private void OnSceneGUI() {

            // if UpdateHitPosition returns True we should update the scene views so that the marker will update in real time.
            if(this.UpdateHitPosition())
                SceneView.RepaintAll();

            // calculates the location of the marker based on the location of the mouse.
            this.RecalculateMarkerPosition();

            // gets a reference to the currentDirection event (used for keyboard presses and mouse presses, mainly input related for now).
            Event currentEvent = Event.current;

            // if the mouse is positioned over the grid map allow drawing actions to occur.
            if(this.IsMouseOnLayer()) {

                // if mouse down or the mouse draged event occurs.
                if(currentEvent.type == EventType.MouseDown || currentEvent.type == EventType.MouseDrag) {

                    if(this._placeSpawnPoints) {
                        if(currentEvent.button == 0) {
                            this.PlaceSpawnPoints();
                        }
                    } else {
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
            }

            Handles.BeginGUI();
            
            if(this._placeSpawnPoints) {
                if(!this._humanSapwnSet) {
                    GUI.Label(new Rect(10, Screen.height - 65, 300, 100), "LEFT MOUSE BUTTON: Select Human Spawn Point");
                } else {
                    GUI.Label(new Rect(10, Screen.height - 65, 300, 100), "LEFT MOUSE BUTTON: Select AI Spawn Point");
                }
            }else if(this._switchPointSet){
                GUI.Label(new Rect(10, Screen.height - 80, 230, 100), "SWITCH BLOCK COUNT: " + this._switchBlockCount.ToString() + " / " + this._target.switchBlockCount.ToString());
                GUI.Label(new Rect(10, Screen.height - 65, 200, 100), "LEFT MOUSE BUTTON: Place Switch Empty Block");
            } else {
                GUI.Label(new Rect(10, Screen.height - 65, 200, 100), "LEFT MOUSE BUTTON: Place Block");
                GUI.Label(new Rect(10, Screen.height - 80, 230, 100), "RIGHT MOUSE BUTTON: Remove Block");
                GUI.Label(new Rect(10, Screen.height - 95, 300, 100), "RED = Human Spawn / YELLOW = AI Spawn");
            }
            Handles.EndGUI();
        }

        public override void OnInspectorGUI() {

            base.OnInspectorGUI();

            GUILayout.Space(20);

            if(GUILayout.Button("Place Blocks")) {
                this.blockWindow = (GridMapEditorWindow)EditorWindow.GetWindow(typeof(GridMapEditorWindow));
                this.blockWindow.Init();
                this.blockWindow.Show();
            }

            EditorGUILayout.BeginHorizontal();
            if(!this._placeSpawnPoints) {
                if(GUILayout.Button("Place Spawn Points")) {
                    this._placeSpawnPoints = true;
                    this._humanSapwnSet = false;
                    this._aiSpawnSet = false;
                }
            } else{
                EditorGUILayout.LabelField("Please Select Spawn Points", EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical();
            this._spawnFoldOut = EditorGUILayout.Foldout(this._spawnFoldOut, "Manual Spawn Entry - Use With Caution!");
                if(this._spawnFoldOut) {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.BeginHorizontal();
                    this._tempHumanPoint = EditorGUILayout.Vector2Field("Player Spawn Point:", new Vector2(this._target.humanSpawnPoint.x, this._target.humanSpawnPoint.z));
                    this._target.humanSpawnPoint = new Vector3(this._tempHumanPoint.x, this._target.humanSpawnPoint.y, this._tempHumanPoint.y);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    this._tempAIPoint = EditorGUILayout.Vector2Field("AI Spawn Point:", new Vector2(this._target.aiSpawnPoint.x, this._target.aiSpawnPoint.z));
                    this._target.aiSpawnPoint = new Vector3(this._tempAIPoint.x, this._target.aiSpawnPoint.y, this._tempAIPoint.y);
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel--;
                }
            EditorGUILayout.EndVertical();

            GUILayout.Space(20);

            SceneView.RepaintAll();
            EditorUtility.SetDirty(target);
        }

        /// <summary>
        /// when the <see cref="GameObject"/> is selected set the currentDirection tool to the view tool.
        /// </summary>
        private void OnEnable() {
            Tools.current = Tool.View;
            Tools.viewTool = ViewTool.Orbit;
            this._target = (GridMap)this.target;
        }

        /// <summary>
        /// when the <see cref="GameObject"/> is not selected we close any window that it might have opened.
        /// </summary>
        private void OnDisable() {
            if(this.blockWindow != null)
                this.blockWindow.CloseWindow();
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
            var block = GameObject.Find(string.Format(_blockName, gridPosition.x, gridPosition.z));

            if(map.blockToPlace == BlockInfo.BlockTypes.NONE)
                return;

            // if there is already a block present at the location and is a child of the grid map component we can just leave it and exit this function.
            if(block != null && block.transform.parent == map.transform) {
                var tempSwitch = block.GetComponent<SwitchBlock>();
                if(tempSwitch != null) {
                    tempSwitch.RemoveAllBlocks(ref map.blockList);
                }

                var tempSwitchBlock = block.GetComponent<SwitchEmptyBlock>();
                if(tempSwitchBlock != null) {
                    tempSwitchBlock.RemoveFromParent();
                }

                var tempWarpBlock = block.GetComponent<WarpBlock>();
                if(tempWarpBlock != null) {
                    tempWarpBlock.RemoveWarpNode(ref map.blockList);
                }

                map.blockList.Remove(block);
                UnityEngine.Object.DestroyImmediate(block);
                block = null;
            }
                
            // if no game object was found we will create one.
            if(block == null) {
                if(map.blockToPlace == BlockInfo.BlockTypes.NORMAL) {
                    block = AssetProcessor.InstantiatePrefab<GameObject>(AssetPaths.PathPrefabBlocks, AssetPaths.NormalBlockName);
                    block.GetComponent<NormalBlock>().blockRenderer = block.GetComponent<MeshRenderer>() as MeshRenderer;
                    block.GetComponent<NormalBlock>().SetupBlock(map.blockToPlace, map.blockOneDirection, map.blockState);
                    map.blockList.Add(block);
                } else if(map.blockToPlace == BlockInfo.BlockTypes.MULTI) {
                    block = AssetProcessor.InstantiatePrefab<GameObject>(AssetPaths.PathPrefabBlocks, AssetPaths.MultiBlockName);
                    block.GetComponent<MultiBlock>().blockRenderer = block.GetComponent<MeshRenderer>();
                    block.GetComponent<MultiBlock>().SetupBlock(map.blockToPlace, map.blockOneDirection, map.blockTwoDirection, map.blockState);
                    map.blockList.Add(block);
                } else if(map.blockToPlace == BlockInfo.BlockTypes.NUMBER) {
                    block = AssetProcessor.InstantiatePrefab<GameObject>(AssetPaths.PathPrefabBlocks, AssetPaths.NumberBlockName);
                    block.GetComponent<NumberBlock>().blockRenderer = block.GetComponent<MeshRenderer>();
                    block.GetComponent<NumberBlock>().SetupBlock(map.blockToPlace, map.blockState, map.blockNumber);
                    map.blockList.Add(block);
                } else if(map.blockToPlace == BlockInfo.BlockTypes.STUN) {
                    block = AssetProcessor.InstantiatePrefab<GameObject>(AssetPaths.PathPrefabBlocks, AssetPaths.StunBlockName);
                    block.GetComponent<StunBlock>().blockRenderer = block.GetComponent<MeshRenderer>();
                    block.GetComponent<StunBlock>().SetupBlock(map.blockToPlace, map.blockState, map.blockNumber);
                    map.blockList.Add(block);
                } else if(map.blockToPlace == BlockInfo.BlockTypes.SWITCH){
                    block = AssetProcessor.InstantiatePrefab<GameObject>(AssetPaths.PathPrefabBlocks, AssetPaths.SwitchBlockName);
                    block.GetComponent<SwitchBlock>().blockRenderer = block.GetComponent<MeshRenderer>();
                    block.GetComponent<SwitchBlock>().SetupBlock(map.blockToPlace, map.blockState);
                    map.blockList.Add(block);
                    this._switchParentNode = block;
                    this._switchPointSet = true;
                    this._switchBlockCount = 0;
                    this._target.blockToPlace = BlockInfo.BlockTypes.SWTICH_EMPTY;
                    if(this.blockWindow != null)
                        this.blockWindow.Focus();
                } else if(map.blockToPlace == BlockInfo.BlockTypes.SWTICH_EMPTY) {
                    this._switchBlockCount++;

                    block = AssetProcessor.InstantiatePrefab<GameObject>(AssetPaths.PathPrefabBlocks, AssetPaths.SwitchEmptyBlockName);
                    block.GetComponent<SwitchEmptyBlock>().blockRenderer = block.GetComponent<MeshRenderer>();
                    block.GetComponent<SwitchEmptyBlock>().SetupBlock(map.blockToPlace, map.blockState);
                    block.GetComponent<SwitchEmptyBlock>().SetParentNode(this._switchParentNode);
                    map.blockList.Add(block);
                    this._switchParentNode.GetComponent<SwitchBlock>().AddEmptyBlock(block);

                    if(this._switchBlockCount == map.switchBlockCount) {
                        this._switchParentNode = null;
                        this._switchPointSet = false;
                        this._switchBlockCount = 0;
                        map.blockToPlace = BlockInfo.BlockTypes.SWITCH;
                        if(this.blockWindow != null)
                            this.blockWindow.Focus();
                    }
                } else if(map.blockToPlace == BlockInfo.BlockTypes.EMPTY) {
                    block = AssetProcessor.InstantiatePrefab<GameObject>(AssetPaths.PathPrefabBlocks, AssetPaths.EmptyBlockName);
                    block.GetComponent<EmptyBlock>().blockRenderer = block.GetComponent<MeshRenderer>();
                    block.GetComponent<EmptyBlock>().SetupBlock(map.blockToPlace, map.blockState);
                    map.blockList.Add(block);
                } else if(map.blockToPlace == BlockInfo.BlockTypes.EMPTY_TALL){
                    block = AssetProcessor.InstantiatePrefab<GameObject>(AssetPaths.PathPrefabBlocks, AssetPaths.EmptyTallBlockName);
                    block.GetComponent<EmptyBlock>().blockRenderer = block.GetComponent<MeshRenderer>();
                    block.GetComponent<EmptyBlock>().SetupBlock(map.blockToPlace);
                    map.blockList.Add(block);
                } else if(map.blockToPlace == BlockInfo.BlockTypes.INVISIBLE){
                    block = AssetProcessor.InstantiatePrefab<GameObject>(AssetPaths.PathPrefabBlocks, AssetPaths.InvisibleBlock);
                    map.blockList.Add(block);
                } else if(map.blockToPlace == BlockInfo.BlockTypes.WARP) {
                    block = AssetProcessor.InstantiatePrefab<GameObject>(AssetPaths.PathPrefabBlocks, AssetPaths.WarpBlockName);
                    block.GetComponent<WarpBlock>().blockRenderer = block.GetComponent<MeshRenderer>();
                    block.GetComponent<WarpBlock>().SetupBlock(map.blockToPlace, map.blockOneDirection, map.blockState);
                    map.blockList.Add(block);
                    this._warpParentNode = block;
                    map.blockToPlace = BlockInfo.BlockTypes.WARP_NODE;
                    if(this.blockWindow != null)
                        this.blockWindow.Focus();
                } else if(map.blockToPlace == BlockInfo.BlockTypes.WARP_NODE){
                    block = AssetProcessor.InstantiatePrefab<GameObject>(AssetPaths.PathPrefabBlocks, AssetPaths.WarpBlockName);
                    block.GetComponent<WarpBlock>().blockRenderer = block.GetComponent<MeshRenderer>();
                    block.GetComponent<WarpBlock>().SetupBlock(BlockInfo.BlockTypes.WARP, map.blockOneDirection, map.blockState);
                    block.GetComponent<WarpBlock>().AddWarpNode(this._warpParentNode);
                    this._warpParentNode.GetComponent<WarpBlock>().AddWarpNode(block);
                    map.blockList.Add(block);

                    this._warpParentNode = null;
                    map.blockToPlace = BlockInfo.BlockTypes.WARP;
                    if(this.blockWindow != null)
                        this.blockWindow.Focus();
                } else {
                    Debug.LogError("NOTHING TO CREATE.");
                }
            }

            Vector3 blockPositionInLocalSpace = new Vector3();
            // set the block position on the grid map.
            if(map.blockToPlace == BlockInfo.BlockTypes.EMPTY_TALL)
                blockPositionInLocalSpace = new Vector3((gridPosition.x * map.BlockWidth) + (map.BlockWidth * 0.5f), 1.0f, (gridPosition.z * map.BlockBreadth));
            else if(map.blockToPlace == BlockInfo.BlockTypes.INVISIBLE)
                blockPositionInLocalSpace = new Vector3((gridPosition.x * map.BlockWidth) + (map.BlockWidth * 0.5f), 1.5f, (gridPosition.z * map.BlockBreadth));
            else
                blockPositionInLocalSpace = new Vector3((gridPosition.x * map.BlockWidth) + (map.BlockWidth * 0.5f), 0.0f, (gridPosition.z * map.BlockBreadth));

            block.transform.position = map.transform.position + blockPositionInLocalSpace;

            // we scale the block to the grid sizes defined by the BlockWidth and BlockHeight fields.
            if(map.blockToPlace == BlockInfo.BlockTypes.EMPTY_TALL) // NEED TO GET RID OF LATER.
                block.transform.localScale = new Vector3(map.BlockWidth, 3.0f, map.BlockBreadth); // NEED TO GET RID OF LATER.
            else // NEED TO GET RID OF LATER.
                block.transform.localScale = new Vector3(map.BlockWidth, map.BlockHeight, map.BlockBreadth);

            // we set the block as a child to the parent.
            block.transform.parent = map.transform;

            // give the block a name that will represent its location on the grid.
            block.name = string.Format(_blockName, gridPosition.x, gridPosition.z);
        }

        /// <summary>
        /// Enables Placing of spawn points in the scene.
        /// </summary>
        private void PlaceSpawnPoints() {
            if(!this._humanSapwnSet) {
                this._target.humanSpawnPoint = this.GetGridPositionFromMouseLocation();
                this._tempHumanPoint = new Vector2(this._target.humanSpawnPoint.x + 1.0f, this._target.humanSpawnPoint.z + 1.0f);
                this._humanSapwnSet = true;
            } else if(!this._aiSpawnSet) {
                this._target.aiSpawnPoint = this.GetGridPositionFromMouseLocation();
                this._tempAIPoint = new Vector2(this._target.aiSpawnPoint.x + 1.0f, this._target.aiSpawnPoint.z + 1.0f);
                this._aiSpawnSet = true;
                this._placeSpawnPoints = false;
            } else {
                this._placeSpawnPoints = false;
            }
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
            var block = GameObject.Find(string.Format(_blockName, gridPosition.x, gridPosition.z));
            
            // if a game object was found with the same name and it is a child of the map we just destroy it.
            if(block != null && block.transform.parent == map.transform) {
                var tempSwitch = block.GetComponent<SwitchBlock>();
                if(tempSwitch != null) {
                    tempSwitch.RemoveAllBlocks(ref map.blockList);
                }

                var tempSwitchBlock = block.GetComponent<SwitchEmptyBlock>();
                if(tempSwitchBlock != null) {
                    tempSwitchBlock.RemoveFromParent();
                }

                var tempWarpBlock = block.GetComponent<WarpBlock>();
                if(tempWarpBlock != null) {
                    tempWarpBlock.RemoveWarpNode(ref map.blockList);
                }

                map.blockList.Remove(block);
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
            var pos = new Vector3(this._mouseHitPosition.x / map.BlockWidth, map.transform.position.y, this._mouseHitPosition.z / map.BlockBreadth);

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
            return this._mouseHitPosition.x > 0 && this._mouseHitPosition.x < (map.columns * map.BlockWidth) &&
                   this._mouseHitPosition.z > 0 && this._mouseHitPosition.z < (map.rows * map.BlockBreadth);
        }

        /// <summary>
        /// Recalculates the position of the marker based on the location of the mouse position.
        /// </summary>
        private void RecalculateMarkerPosition() {

            // gets the reference to the grid map component/gameobject the script is connected to (GridController).
            var map = (GridMap)this.target;

            // stores the grid location (Vector3(Column/0/Row)) based on the currentDirection location of the mouse pointer.
            var gridPosition = this.GetGridPositionFromMouseLocation();

            // stores the grid position in world space.
            var pos = new Vector3(gridPosition.x * map.BlockWidth, 0, gridPosition.z * map.BlockBreadth);

            // sets the GridMap.MarkerPsotion to a new value.
            map.markerPosition = map.transform.position + new Vector3(pos.x + (map.BlockWidth / 2), 0, pos.z + (map.BlockBreadth / 2));
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

            // builds a ray type from the currentDirection mouse position, will help us get information of the coorindates and if the ray hit an object.
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

            // if the value is differernt than the currentDirection mouse hit location.
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