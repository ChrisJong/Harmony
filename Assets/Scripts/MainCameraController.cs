using UnityEngine;
using System.Collections;
using Constants;
using GridGenerator;

public class MainCameraController : MonoBehaviour {

    public GridMap gridMapScript;

    private Transform _targetToLookAt;
    private Vector3 _position;

    void Awake() {
        gridMapScript = GameObject.FindGameObjectWithTag("GridController").GetComponent<GridMap>();
    }

	void Start() {
        UpdateCamera();
	}
	
    /// <summary>
    /// Updates The Camera Position At The Start To Fit The Maze.
    /// </summary>
    private void UpdateCamera() {

        var totalWidth = gridMapScript.blockWidth * gridMapScript.rows;
        var totalHeight = gridMapScript.blockBreadth * gridMapScript.columns;

        var posX = (float)(totalWidth * 0.5f);
        var posY = (float)Mathf.Round((totalHeight + (totalHeight / 3.0f)) + 0.5f);
        var posZ = (float)(gridMapScript.blockBreadth * 0.5f);
        var fov = (float)CameraValues.minFOW;

        _position = new Vector3(posX, posY, posZ);
        transform.position = _position;
        transform.rotation = CameraValues.CameraRotation;
        camera.fieldOfView = fov;
        camera.backgroundColor = CameraValues.BackgroundColor;
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

        GameObject tempLight = new GameObject("Directional Light");
        tempLight.transform.rotation = Quaternion.Euler(50.0f, -30.0f, 0.0f);
        tempLight.AddComponent<Light>();
        Light lightDetails = tempLight.GetComponent<Light>();
        lightDetails.type = LightType.Directional;
        lightDetails.intensity = 0.85f;

        tempCamera.AddComponent<MainCameraController>();
    }
}
