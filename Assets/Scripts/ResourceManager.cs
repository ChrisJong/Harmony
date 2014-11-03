using System.Collections.Generic;

using UnityEngine;

public class ResourceManager : MonoBehaviour {

    public static ResourceManager instance;

    public GameObject gameMenuController;
    public GameObject gameController;
    public GameObject gridController;
    public GameObject soundController;

    public GameObject playerObject;
    public GameObject aiObject;

    public GameObject levelNumberBlock;

    public Material skyBox;
    public Material bottomPlane;

    public GameObject fireworkParticle;
    public GameObject endAnimation;
    public GameObject warningTexture;

    void Awake() {
        instance = this;
    }
}