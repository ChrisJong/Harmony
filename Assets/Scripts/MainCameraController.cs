﻿using UnityEngine;
using System.Collections;
using Constants;
using Grid;

public class MainCameraController : MonoBehaviour {

    public GridMap gridMapScript;

    private Camera _mainCamera;
    private Transform _targetToLookAt;
    private Vector3 _position;

    void Awake() {
        if(GameController.instance.gameState == GlobalValues.GameState.INGAME)
            gridMapScript = GameObject.FindGameObjectWithTag("GridMap").GetComponent<GridMap>();

        this._mainCamera = this.transform.camera;
        this._mainCamera.fieldOfView = CameraValues.MaxFOV;
    }

	void Start() {
        UpdateCamera();
        StartCoroutine(StartCamera());
	}

    /// <summary>
    /// Updates The Camera Position At The Start To Fit The Maze.
    /// </summary>
    private void UpdateCamera() {

        var totalWidth = BlockValues.BlockWidth * gridMapScript.columns;
        var totalHeight = BlockValues.BlockBreadth * gridMapScript.rows;

        var posX = (float)(totalWidth * 0.5f);
        var posY = (float)Mathf.Round((totalHeight + (totalHeight / 3.0f)) + 0.5f);
        var posZ = (float)(totalHeight * 0.5f);
        var fov = (float)CameraValues.MaxFOV;

        if(gridMapScript.columns > (gridMapScript.rows * 2))
            posY = gridMapScript.rows + 2.5f + (gridMapScript.columns - (gridMapScript.rows * 2));
        else
            posY = gridMapScript.rows + 2.5f;

        _position = new Vector3(posX, posY, posZ);
        transform.position = _position;
        transform.rotation = CameraValues.CameraRotation;
        camera.fieldOfView = fov;
        camera.backgroundColor = CameraValues.BackgroundColor;
    }

    private IEnumerator StartCamera(){
        while(true) {
            this._mainCamera.fieldOfView = Mathf.Lerp(this._mainCamera.fieldOfView, CameraValues.MinFOV, CameraValues.FOVSpeed * Time.deltaTime);
            if((CameraValues.MinFOV + 0.1f) < this._mainCamera.fieldOfView) {
                yield return null;
            } else {
                this._mainCamera.fieldOfView = CameraValues.MinFOV;
                yield break;
            }
        }
    }

    /// <summary>
    /// Finds Or Creates The Main Camera On the Scene.
    /// </summary>
    public static void FindOrCreate() {

        GameObject tempCamera;

        if(Camera.main != null) {
            tempCamera = Camera.main.gameObject;
        } else {
            tempCamera = new GameObject("Main Camera");
            tempCamera.AddComponent<Camera>();
            tempCamera.AddComponent<GUILayer>();
            tempCamera.AddComponent<AudioListener>();
            tempCamera.tag = "MainCamera";
        }

        if(GameController.instance.gameState == GlobalValues.GameState.INGAME) {
            GameObject tempLight = new GameObject("Directional Light");
            tempLight.transform.rotation = Quaternion.Euler(50.0f, -30.0f, 0.0f);
            tempLight.AddComponent<Light>();
            Light lightDetails = tempLight.GetComponent<Light>();
            lightDetails.type = LightType.Directional;
            lightDetails.intensity = 0.50f;
            lightDetails.shadows = LightShadows.Soft;

            tempCamera.AddComponent<MainCameraController>();
        }
    }
}
