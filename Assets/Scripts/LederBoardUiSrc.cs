using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;

public class LederBoardUiSrc : MonoBehaviour
{
    const string PRIVATE_CODE = "c3QCTPhojU2BlICBTQL7nwpMfYtTbwYEuZVePzOtd3kg";
    //const string PUBLIC_CODE = "5f1d6e44eb371809c48dfdcb";
    const string WEB_URL = "http://dreamlo.com/lb/";
    
    public List<string> names = new List<string>();
    public List<int> scores = new List<int>();

    public int numberOfplayers;
    
    public GameObject rankPrefab;
    public GameObject myPrefab;
    public Transform content;

    public TMP_Text plName;
    public TMP_Text plRank;
    public TMP_Text plLevel;
    
    bool inWork;

    private void Start()
    {
        inWork = false;
        //Debug.Log("Adding..");
        //StartCoroutine(Add());
    }

    IEnumerator Add()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(WEB_URL + PRIVATE_CODE + "/add/" + "hartda" + "/" + 4000);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log("Error " + webRequest.error);
            yield break;
        }
        else
        {
            Debug.Log("Done");
            StartCoroutine(Show());
        }
    }

    IEnumerator Show()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(WEB_URL + PRIVATE_CODE + "/json");
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log("Error " + webRequest.error);
            yield break;
        }
        JSONNode jInfo = JSON.Parse(webRequest.downloadHandler.text);
        JSONNode jvalues = jInfo["dreamlo"]["leaderboard"]["entry"];
        numberOfplayers = jvalues.Count;

        for (int i = 0; i < numberOfplayers; i++)
        {
            Debug.Log(jvalues[i]);
        }
        Debug.Log("Ayya??");
    }


    IEnumerator PostData()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(WEB_URL + PRIVATE_CODE + "/add/" + PlayerPrefs.GetString("name") + "/" + 1);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log("Error " + webRequest.error);
            SetData(true);
            yield break;
        }
        else
        {
            Debug.Log("Getting Data");
            StartCoroutine(GetData());
        }
    }

    IEnumerator GetData()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(WEB_URL + PRIVATE_CODE + "/json");
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log("Error " + webRequest.error);
            SetData(true);
            yield break;
        }


        JSONNode jInfo = JSON.Parse(webRequest.downloadHandler.text);
        JSONNode jvalues = jInfo["dreamlo"]["leaderboard"]["entry"];
        numberOfplayers = jvalues.Count;
        names.Clear();
        scores.Clear();

        for (int i = 0; i < numberOfplayers; i++)
        {
            scores.Add(jvalues[i]["score"]);
            names.Add(jvalues[i]["name"]);
        }

        SetData();
    }

    public void OnLederboard()
    {
        if (!inWork)
        {
            inWork = true;
            Debug.Log("posting Data");
            plName.text = "FETCHING...";
            StartCoroutine(PostData());
            //StartCoroutine(GetData());
        }
    }

    void SetData(bool error = false)
    {
        if (error)
        {
            plName.text = "NETWORK ERROR";
            inWork = false;
            return;
        }
        else
        {
            Debug.Log("Setting up stuffs");
            int loopLimit = 0;
            if (numberOfplayers > 100)
            {
                loopLimit = 10;
            }
            else
            {
                loopLimit = numberOfplayers;
            }

            for (int i = 0; i < loopLimit; i++)
            {
                string name = names[i];
                int score = scores[i];

                GameObject obj = Instantiate(rankPrefab) as GameObject;
                Transform rankTrans = obj.transform;
                rankTrans.SetParent(content);
                rankTrans.transform.localScale = new Vector3(1, 1, 1);
                string extra = "";

                if (i < 10)
                {
                    extra = "0";
                }
                rankTrans.Find("Rank").GetComponent<TMP_Text>().text = "#" + extra + (i + 1).ToString();
                Debug.Log("name" + name.Length);
                Debug.Log("name" + PlayerPrefs.GetString("username"));
                int spaces;
                spaces = 10 - name.Length;
                Debug.Log("spaces" + spaces);
                if (spaces < 0)
                {
                    spaces = 0;
                }
                extra = "";
                for (int j = 0; j < spaces; j++)
                {
                    extra += ".";
                }
                rankTrans.Find("Name").GetComponent<TMP_Text>().text = name + extra;

                extra = "";
                if (score < 10)
                {
                    extra = "0";
                }

                rankTrans.Find("Level").GetComponent<TMP_Text>().text = extra + score.ToString();

                if (names[i].ToLower() == PlayerPrefs.GetString("name").ToLower())
                {
                    plName.text = name;
                    plRank.text = "RANK : #" + (i+1).ToString();
                    plLevel.text = "BEST : " + scores[i].ToString();
                    obj.GetComponent<Image>().color = Color.blue;
                    obj.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.white;
                    obj.transform.GetChild(1).GetComponent<TMP_Text>().color = Color.white;
                    obj.transform.GetChild(2).GetComponent<TMP_Text>().color = Color.white;
                    myPrefab = obj;
                }
                Debug.Log("Done");
            }
        }
    }
}