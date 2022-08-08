using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK.Setup;

public class ValuesSrc : MonoBehaviour
{
    public GameManager gameManager;
    public TMP_Text curKillTxt;

    public static int currentKill;
    public GameObject boss;
    public Transform[] spawnPnts;

    public GameObject pausePanel;
    public Animator transition;

    bool paused = false;

    private void Start()
    {
        currentKill = 0;
    }

    private void Update()
    {
        int bestKill = PlayerPrefs.GetInt("bestKill");

        if (bestKill < currentKill)
        {
            PlayerPrefs.SetInt("bestKill", currentKill);
        }
        curKillTxt.text = currentKill.ToString();
    }

    public void OnKill()
    {
        currentKill++;
        PlayerPrefs.SetInt("totalKill", PlayerPrefs.GetInt("totalKill")+1);
        if (currentKill >= 50 && currentKill % 50  == 0)
        {
            int r = Random.Range(0, spawnPnts.Length);
            Instantiate(boss,spawnPnts[r].position,Quaternion.identity);
        }
    }

    public void OnPause()
    {
        if (!paused)
        {
            paused = true;
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void OnPlay()
    {
        paused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnHome()
    {
        gameManager.OnEnd();
        Time.timeScale = 1;
        StartCoroutine(LoadLevel("MainMenu"));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}