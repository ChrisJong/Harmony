using UnityEngine;
using System.Collections;
using GameInfo;
using Grid;

public class SkyboxCameraController : MonoBehaviour {

    public GridMap gridMapScript;

    private Camera _skyboxCamera;
    private Transform _targetToLookAt;
    
    //private Vector3 _rotationTo;
    //private float _rotationX;

    void Awake() {
        if(GameController.instance.gameState == GlobalInfo.GameState.INGAME)
            gridMapScript = GameObject.FindGameObjectWithTag("GridMap").GetComponent<GridMap>();

        this._skyboxCamera = this.transform.camera;
    }

    void Start() {
        if(GameController.instance.gameState == GlobalInfo.GameState.INGAME) {
            this._skyboxCamera.fieldOfView = CameraInfo.MaxFOV;
            UpdateCamera();
            StartCoroutine(StartCamera());
        }
    }

    void LateUpdate() {
        /*this._rotationTo.y += CameraInfo.SkyboxRotationSpeed * Time.deltaTime;

        if(this._rotationTo.y > 360.0f){
            this._rotationTo.y -= 360.0f;
        } else if(this._rotationTo.y < 360.0f) {
            this._rotationTo.y += 360.0f;
        }

        this.transform.eulerAngles = this._rotationTo;*/

        this.transform.Rotate(0, CameraInfo.SkyboxRotationSpeed * Time.deltaTime, CameraInfo.SkyboxRotationSpeed * Time.deltaTime);
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
        camera.fieldOfView = fov;
        camera.backgroundColor = CameraInfo.BackgroundColor;
    }

    private IEnumerator StartCamera() {
        while(true) {
            this._skyboxCamera.fieldOfView = Mathf.Lerp(this._skyboxCamera.fieldOfView, CameraInfo.MinFOV, CameraInfo.FOVSpeed * Time.deltaTime);
            if((CameraInfo.MinFOV + 0.1f) < this._skyboxCamera.fieldOfView) {
                yield return null;
            } else {
                this._skyboxCamera.fieldOfView = CameraInfo.MinFOV;
                //this._rotationTo = this.transform.eulerAngles;
                yield break;
            }
        }
    }

    public static void CreateSkyboxCamera() {
        GameObject tempSkyboxCamera;

        tempSkyboxCamera = new GameObject("Skybox Camera");
        tempSkyboxCamera.AddComponent<Camera>();
        tempSkyboxCamera.camera.clearFlags = CameraClearFlags.Skybox;
        tempSkyboxCamera.camera.cullingMask = 0;
        tempSkyboxCamera.camera.depth = Camera.main.depth - 1;
        tempSkyboxCamera.camera.fieldOfView = 60.0f;
        tempSkyboxCamera.tag = "SkyboxCamera";
        tempSkyboxCamera.AddComponent<Skybox>();
        tempSkyboxCamera.GetComponent<Skybox>().material = ResourceManager.instance.skyBox;
        tempSkyboxCamera.AddComponent<SkyboxCameraController>();
    }
}
