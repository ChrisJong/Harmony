using System.Collections.Generic;

using UnityEngine;

public class ResourceController : MonoBehaviour {

    public static ResourceController instance;

    public GameObject gameMenuController;
    public GameObject gameController;
    public GameObject gridController;
    public GameObject soundController;

    public GameObject playerObject;
    public GameObject aiObject;

    public GameObject levelNumberBlock;

    public Material skyBox;

    void Awake() {
        instance = this;
    }
}