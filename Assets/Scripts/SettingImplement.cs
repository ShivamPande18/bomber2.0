using UnityEngine;

public class SettingImplement : MonoBehaviour
{
    public GameObject planeTrail;
    public AudioSource backgroundLoop;
    public AudioSource blastSound;


    private void Awake()
    {
        if (PlayerPrefs.GetInt("fx") != 0)
        {
            planeTrail.SetActive(false);
        }

        if (PlayerPrefs.GetInt("sound") != 0)
        {
            blastSound.enabled = false;
            backgroundLoop.enabled = false;
        }
    }
}