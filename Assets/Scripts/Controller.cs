using System.Collections.Generic;

using UnityEngine;

public class Controller : MonoBehaviour {

    public static Controller instance;

    public GameObject gameMenuController;
    public GameObject gameController;
    public GameObject gridController;
    public GameObject soundController;

    public GameObject playerObject;
    public GameObject aiObject;

    public GameObject levelNumberBlock;

    void Awake() {
        instance = this;
    }
}