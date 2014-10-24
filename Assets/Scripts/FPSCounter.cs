using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUIText))]
public class FPSCounter : MonoBehaviour {

    public float updateInterval = 0.5f;

    private float accumlatedOverTime = 0.0f;
    private int frames = 0;
    private float timeLeft;

	void Start () {
        if(!guiText) {
            Debug.Log("framesPerSecond needs a GUIText component!");
            enabled = false;
            return;
        }

        timeLeft = updateInterval;
	}
	
	void Update () {
        timeLeft -= Time.deltaTime;
        accumlatedOverTime += Time.timeScale / Time.deltaTime;
        ++frames;

        if(timeLeft <= 0.0f) {
            guiText.text = "" + (accumlatedOverTime / frames).ToString("f2");
            timeLeft = updateInterval;
            accumlatedOverTime = 0.0f;
            frames = 0;
        }
	}
}