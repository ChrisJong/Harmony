﻿using UnityEngine;
using System.Collections;
using GameInfo;
using Grid;
using Resource;

public class SkyboxCameraController : MonoBehaviour {

    public GridMap gridMapScript;

    private Camera _skyboxCamera;
    private Transform _targetToLookAt;

    void Awake() {
        if(GameController.instance.gameState == GlobalInfo.GameState.INGAME)
            gridMapScript = GameObject.FindGameObjectWithTag("GridMap").GetComponent<GridMap>();

        this._skyboxCamera = this.transform.GetComponent<Camera>();
    }

    void Start() {
        if(GameController.instance.gameState == GlobalInfo.GameState.INGAME) {
            this._skyboxCamera.fieldOfView = CameraInfo.MaxFOV;
            UpdateCamera();
            StartCoroutine(StartCamera());
        }

        this.transform.rotation = Quaternion.Euler(Random.Range(-180.0f, 180.0f), Random.Range(-180.0f, 180.0f), Random.Range(-180.0f, 180.0f));
    }

    void LateUpdate() {
        //this.transform.Rotate(this.transform.rotation.eulerAngles, CameraInfo.SkyboxRotationSpeed * Time.deltaTime);
        this.transform.Rotate(CameraInfo.SkyboxRotationSpeed * Time.deltaTime, CameraInfo.SkyboxRotationSpeed * Time.deltaTime, CameraInfo.SkyboxRotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Updates The Camera Position At The Start To Fit The Maze.
    /// </summary>
    private void UpdateCamera() {

        var totalWidth = BlockInfo.BlockWidth * gridMapScript.columns;
        var totalHeight = BlockInfo.BlockBreadth * gridMapScript.rows;

        var posX = (float)(totalWidth * 0.5f);
        var posY = (float)Mathf.Round((totalHeight + (totalHeight / 3.0f)) + 0.5f);
        var posZ = (float)(totalHeight * 0.5f);
        var fov = (float)CameraInfo.MaxFOV;

        if(gridMapScript.columns > (gridMapScript.rows * 2))
            posY = gridMapScript.rows + 2.5f + (gridMapScript.columns - (gridMapScript.rows * 2));
        else
            posY = gridMapScript.rows + 2.5f;

        transform.position = new Vector3(posX, posY, posZ);;
        transform.rotation = CameraInfo.CameraRotation;
        GetComponent<Camera>().fieldOfView = fov;
        GetComponent<Camera>().backgroundColor = CameraInfo.BackgroundColor;
    }

    private IEnumerator StartCamera() {
        while(true) {
            this._skyboxCamera.fieldOfView = Mathf.Lerp(this._skyboxCamera.fieldOfView, CameraInfo.MinFOV, CameraInfo.FOVSpeed * Time.deltaTime);
            if((CameraInfo.MinFOV + 0.1f) < this._skyboxCamera.fieldOfView) {
                yield return null;
            } else {
                this._skyboxCamera.fieldOfView = CameraInfo.MinFOV;
                yield break;
            }
        }
    }

    public static void CreateSkyboxCamera() {
        GameObject tempSkyboxCamera;

        tempSkyboxCamera = new GameObject("Skybox Camera");
        tempSkyboxCamera.AddComponent<Camera>();
        tempSkyboxCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
        tempSkyboxCamera.GetComponent<Camera>().cullingMask = 0;
        tempSkyboxCamera.GetComponent<Camera>().depth = Camera.main.depth - 1;
        tempSkyboxCamera.GetComponent<Camera>().fieldOfView = 60.0f;
        tempSkyboxCamera.tag = "SkyboxCamera";
        tempSkyboxCamera.AddComponent<Skybox>();
        tempSkyboxCamera.GetComponent<Skybox>().material = ResourceManager.instance.skyBox;
        tempSkyboxCamera.AddComponent<SkyboxCameraController>();
    }
}
