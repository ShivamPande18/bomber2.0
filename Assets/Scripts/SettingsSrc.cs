using UnityEngine;

public class SettingsSrc : MonoBehaviour
{
    public GameObject unmute, mute, fx, nofx;
    public AudioSource source;

    private void Start()
    {
        if (PlayerPrefs.GetInt("sound") == 0)
        {
            mute.SetActive(false);
            unmute.SetActive(true);
            source.mute = false;
        }
        else
        {
            mute.SetActive(true);
            unmute.SetActive(false);
            source.mute = true;
        }

        if (PlayerPrefs.GetInt("fx") == 0)
        {
            fx.SetActive(true);
            nofx.SetActive(false);
        }
        else
        {
            fx.SetActive(false);
            nofx.SetActive(true);
        }
    }

    public void OnSound()
    {
        CheckSound();
    }

    public void OnFx()
    {
        CheckFx();
    }

    void CheckSound()
    {
        if (PlayerPrefs.GetInt("sound") == 0)
        {
            PlayerPrefs.SetInt("sound", 1);
            mute.SetActive(true);
            unmute.SetActive(false);
            source.mute = true;
        }
        else
        {
            PlayerPrefs.SetInt("sound", 0);
            mute.SetActive(false);
            unmute.SetActive(true);
            source.mute = false;
        }
    }

    void CheckFx()
    {
        if (PlayerPrefs.GetInt("fx") == 0)
        {
            PlayerPrefs.SetInt("fx", 1);
            fx.SetActive(false);
            nofx.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("fx", 0);
            fx.SetActive(true);
            nofx.SetActive(false);
        }
    }
}