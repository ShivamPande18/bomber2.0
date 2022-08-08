using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndPanelSrc : MonoBehaviour
{
    public TMP_Text bestKill;
    public TMP_Text kills;
    public TMP_Text killHeading;

    public Animator transition;
    public bool isTimeAttack;
    public GameManager gameManager;

    private void OnEnable()
    {
        FindObjectOfType<MoreGamesSrc>().Show();
        UpdateScore();
    }

    private void OnDisable()
    {
        try
        {
            FindObjectOfType<MoreGamesSrc>().Hide();
        }
        catch (System.Exception)
        {
            Debug.Log("error");
        }
    }

    private void UpdateScore()
    {
        if (PlayerPrefs.GetInt("bestKill") == ValuesSrc.currentKill)
        {
            killHeading.text = "!! NEW BEST SCORE !!";
            kills.text = string.Empty;
        }
        else
        {
            kills.text = ValuesSrc.currentKill.ToString();
        }
        bestKill.text = PlayerPrefs.GetInt("bestKill").ToString();
    }

    public void OnHome()
    {
        StartCoroutine(LoadLevel("MainMenu"));
        gameManager.OnEnd();
        FindObjectOfType<MoreGamesSrc>().Hide();
    }
    public void OnRestart()
    {
        if (isTimeAttack)
        {
            StartCoroutine(LoadLevel("TimeAttack"));
        }
        else
        {
            StartCoroutine(LoadLevel("MainGame"));
        }
    }

    public void OnReward()
    {
        FindObjectOfType<AdManager>().ShowRewardedAd();
    }


    IEnumerator LoadLevel(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}