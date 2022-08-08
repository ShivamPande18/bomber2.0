using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NASplashLoadNext : MonoBehaviour
{
    public string NextSceneName;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("LoadNext", 2);
    }

    // Update is called once per frame
    void LoadNext()
    {
        SceneManager.LoadSceneAsync(NextSceneName);
    }
}
