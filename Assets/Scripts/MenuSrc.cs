using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class MenuSrc : MonoBehaviour
{
    [Header("Review Panel")]
    public GameObject reviewPanel;

    [Header("Input Name")]
    public GameObject nameInputPanel;
    public TMP_InputField nameInputField;

    [Header("MAIN MENU")]
    public TMP_Text best;
    public GameObject modeSelector;
    public GameObject[] panels;
    public Animator transition;
    public Transform plane;
    public Sprite[] planeSprites;
    public SpriteRenderer sp;


    //public LeaderboardSrc leaderboard;

    /*
     0 = main Menu
     1 = shop
     2 = credits
     3 = leaderboard
     4 = settings
     */
    int states = 0;

    [Header("SHOP UI")]
   public TMP_Text totalKillTxt;

    private void Start()
    {
        best.text = "BEST : " + PlayerPrefs.GetInt("bestKill").ToString();

        if (PlayerPrefs.GetInt("isStart") == 0)
        {
            nameInputPanel.SetActive(true);
        }
    }

    private void Update()
    {
        plane.Translate(Vector3.left* 7f * Time.deltaTime);

        if (states == 1)
        {
            totalKillTxt.text = "X " + PlayerPrefs.GetInt("totalKill").ToString();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        sp.sprite = planeSprites[PlayerPrefs.GetInt("currentPlane")];
    }

    void AskForReview()
    {
        reviewPanel.SetActive(true);
    }

    public void OnReviewNo()
    { 
        reviewPanel.SetActive(false);
        modeSelector.SetActive(true);
    }

    public void OnReviewYes()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.TT.B");
        reviewPanel.SetActive(false);
        modeSelector.SetActive(true);
    }

    public void ToShop()
    {
        states = 1;
        ChangePanel();
    }

    public void ToCredits()
    {
        states = 2;
        ChangePanel();
    }

    public void ToMain()
    {
        states = 0;
        ChangePanel();
    }

    public void ToSettings()
    {
        states = 4;
        ChangePanel();
    }

    public void OnPlayClick()
    {
        if (PlayerPrefs.GetInt("isStart") == 0)
        {
            PlayerPrefs.SetInt("isStart", 10);
            StartCoroutine(LoadLevel("Tutorial"));
        }
        else
        {
            int reviewCnt = PlayerPrefs.GetInt("review");
            Debug.Log(reviewCnt + " review");
            if (reviewCnt < 5)
            {
                PlayerPrefs.SetInt("review", reviewCnt + 1);
                modeSelector.SetActive(true);
            }
            else if (reviewCnt == 5)
            {
                AskForReview();
                PlayerPrefs.SetInt("review", 10);
            }
            else
            {
                modeSelector.SetActive(true);
            }
        }
    }

    public void OnModeNormal()
    {
        StartCoroutine(LoadLevel("MainGame"));
    }

    public void OnModeChallenge()
    {
        StartCoroutine(LoadLevel("Challenges"));
    }

    public void OnModeTime()
    {
        StartCoroutine(LoadLevel("TimeAttack"));
    }

    public void OnNameSubmit()
    {
        string name = nameInputField.text;
        if (!string.IsNullOrEmpty(name))
        {
            PlayerPrefs.SetString("name", name);
            nameInputPanel.SetActive(false);
        }
    }

    public void OnLeaderBoard()
    { 
        states = 3;
        ChangePanel();
    }


    void ChangePanel()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (i == states)
            {
                panels[i].SetActive(true);
            }
            else
            {
                panels[i].SetActive(false);
            }
        }
    }

    IEnumerator LoadLevel(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }   
    
    public void OnShareBtn()
    {
        Debug.Log("share");
        StartCoroutine(Share());
    }

    public void OnYoutube()
    {
        Application.OpenURL("https://www.youtube.com/channel/UCv-skN1U7xAtx0NkYu-cDhQ");
    }

    IEnumerator Share()
    {
        yield return new WaitForEndOfFrame();
#if UNITY_ANDROID
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent");
        intent.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        intent.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "I challenge you to defeat me in this game https://play.google.com/store/apps/details?id=com.TT.B");
        intent.Call<AndroidJavaObject>("setType", "text/plain");
        AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intent, "Share Bomber2.0");
        currentActivity.Call("startActivity", chooser);
#endif
    }
}

//const string privateCode = "c3QCTPhojU2BlICBTQL7nwpMfYtTbwYEuZVePzOtd3kg";
//const string publicCode = "5ecc5b55377dce0a1439cdc7";