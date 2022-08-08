using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSMeter : MonoBehaviour
{
    public bool showFpsMeter = true;
    public Text fpsMeter;
    private float deltaTime;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (showFpsMeter)
        {
            fpsMeter.gameObject.SetActive(true);
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsMeter.text = string.Format("{0:0.} fps", fps);
        }
        else
        {
            fpsMeter.gameObject.SetActive(false);
        }
    }
}
