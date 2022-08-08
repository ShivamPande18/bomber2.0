using System.Collections;
using TMPro;
using UnityEngine;
using GameAnalyticsSDK;
using UnityEngine.SceneManagement;

public class ChallengeSrc : MonoBehaviour
{
    [Header("Challenge Panel")]
    public GameObject challengePanel;
    public TMP_Text challengeTxt;

    [Header("End Panel")]
    public TMP_Text resultTxt;
    public TMP_Text Earnings;
    public GameObject endPanel;
    public GameObject next,retry;

    [Header("Pause Panel")]
    public GameObject pausePanel;
    public Animator transition;

    [Header("General")]
    public Collider2D planeCollider;
    public TMP_Text curKillTxt;
    public int curKill;
    public int level;
    bool paused = false;

    private void Start()
    {
        StartCoroutine(ShowChallenge());
        level = PlayerPrefs.GetInt("level");
        level ++;
        Debug.Log("Level " + level);
        SetChallenge();
        curKill = 0;
        FindObjectOfType<MoreGamesSrc>().Hide();
    }

    void SetChallenge()
    {
        challengeTxt.text = "CHALLENGE : KILL " + level * 2 +  " ENEMIES";
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "challenge",level);
    }

    public void OnOk()
    {
        Time.timeScale = 1;
        challengePanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            OnChallengeEnd(true);
        }
    }

    public void OnKill()
    {
        curKill++;
        curKillTxt.text = curKill.ToString();
        if(curKill >= level * 2)
        {
            OnChallengeEnd(true);
        }
    }

    public void OnChallengeEnd(bool won)
    {
        if (won)
        {
            retry.SetActive(false);
            resultTxt.text = "CHALLENGE PASSED";
            Earnings.text = "+" + (level*5).ToString();
            PlayerPrefs.SetInt("level", level);
            PlayerPrefs.SetInt("totalKill", PlayerPrefs.GetInt("totalKill") + level*5);
            planeCollider.enabled = false;
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "challenge", level);
        }
        else
        { 
            resultTxt.text = "CHALLENGE FAILED";
            Earnings.text = "0";
            next.SetActive(false);
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "challenge", level);
        }
        endPanel.SetActive(true);
        FindObjectOfType<MoreGamesSrc>().Show();
    }

    public void OnNext()
    {
        StartCoroutine(LoadLevel("Challenges"));
    }

    IEnumerator ShowChallenge()
    {
        yield return new WaitForSeconds(1);
        challengePanel.SetActive(true);
        Time.timeScale = 0;
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
        Time.timeScale = 1;
        StartCoroutine(LoadLevel("MainMenu"));
        FindObjectOfType<MoreGamesSrc>().Hide();
    }

    IEnumerator LoadLevel(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}