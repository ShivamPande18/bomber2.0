using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class AdsInfo
{
    public string GameName = string.Empty;
    public int BubbleWidth = 128;
    public int BubbleHight = 128;
    public string BubbleUrl = string.Empty;
    public int ImgWidth = 128;
    public int ImgHight = 128;
    public string ImgUrl = string.Empty;
    public string GameUrl = string.Empty;
}

[System.Serializable]
public class AdInfoRoot
{
    public AdsInfo[] AdsInfo;
}

[System.Serializable]
public class AdsData
{
    public string GameName = string.Empty;
    public int BubbleWidth = 128;
    public int BubbleHight = 128;
    public Sprite BubbleImg = null;
    public int ImgWidth = 128;
    public int ImgHight = 128;
    public Sprite GameImg = null;
    public string GameUrl = string.Empty;

    public AdsData(String GameName, int BubbleWidth, int BubbleHight, Sprite BubbleImg, int ImgWidth, int ImgHight, Sprite GameImg, string GameUrl)
    {
        this.GameName = GameName;
        this.BubbleWidth = BubbleWidth;
        this.BubbleHight = BubbleHight;
        this.BubbleImg = BubbleImg;
        this.ImgWidth = ImgWidth;
        this.ImgHight = ImgHight;
        this.GameImg = GameImg;
        this.GameUrl = GameUrl;
    }
}


public class LoadAds : Singleton<LoadAds>
{

    public AdInfoRoot adInfoRoot;//= new AdRoot();
    public List<AdsData> adRoot = new List<AdsData>();

    public int downloadCount = 0;
    public string AdJsonUrl;

    [Header("More Game Ads Scroll")]
    public bool AdScroller = false;
    public Sprite MoreGameImg;
    public AdScroller AdScrollObj;

    [Header("Default Bubble")]
    public Sprite defaultImg;
    public string defaultGameName = "";
    public string defaultUrl = "";

    public GameObject MoreGame;
    bool loaded = false;
    void Start()
    {
        StartCoroutine(Utils.DownLoadJson(AdJsonUrl, adInfoRoot, (response, success) =>
        {
            if (success)
            {
                adInfoRoot = response;
                DownloadImages();
            }
            else
                Debug.LogError("----------- Error Occurred ----------");
        }));
        // for (int i = 0; i < adRoots.AdsData.Length; i++)
        // {
        //     StartCoroutine ( GetTextureRequest ( adRoots.AdsData[i].GameImg, (response) => {
        //         targetSprite = response;
        //         spriteRenderer.sprite = targetSprite;
        //     }));
        // }         
    }

    void DownloadImages()
    {
        // Debug.LogError("----------- DownloadImages ----------");
        StartCoroutine(Utils.GetTextureRequest(adInfoRoot.AdsInfo[downloadCount].BubbleUrl, adInfoRoot.AdsInfo[downloadCount].BubbleWidth, adInfoRoot.AdsInfo[downloadCount].BubbleHight, (response1) =>
        {
            Sprite bubbleImg = response1;
            StartCoroutine(Utils.GetTextureRequest(adInfoRoot.AdsInfo[downloadCount].ImgUrl, adInfoRoot.AdsInfo[downloadCount].ImgWidth, adInfoRoot.AdsInfo[downloadCount].ImgHight, (response2) =>
            {
                adRoot.Add(
                                new AdsData(adInfoRoot.AdsInfo[downloadCount].GameName,
                                    adInfoRoot.AdsInfo[downloadCount].BubbleWidth,
                                    adInfoRoot.AdsInfo[downloadCount].BubbleHight,
                                    bubbleImg,
                                    adInfoRoot.AdsInfo[downloadCount].ImgWidth,
                                    adInfoRoot.AdsInfo[downloadCount].ImgHight,
                                    response2,
                                    adInfoRoot.AdsInfo[downloadCount].GameUrl)
                            );
                downloadCount++;
                // Debug.LogError("----------- DownloadImages ---------- " + downloadCount);
                if (downloadCount < adInfoRoot.AdsInfo.Length)
                {
                    // Debug.LogError("----------- Called Again DownloadImages ---------- " + downloadCount);
                    DownloadImages();
                }
                else
                {
                    MoreGame.SetActive(true);
                }
            }));

        }));
        // Debug.LogError("----------- After Start DownloadImages ---------- " + downloadCount);
    }

    public void AdStartAfterFewSecond()
    {
        StartCoroutine(VisibleAdSpot());
    }

    IEnumerator VisibleAdSpot()
    {
        // Debug.Log("Loading after 10 seconds");
        yield return new WaitForSeconds(2f);
        // Debug.Log("Loaded after 10 seconds");
        MoreGame.SetActive(true);
    }

    public void OpenMoreGameScroll()
    {
        if (!loaded)
        {
            AdScrollObj.CreateAdSpot(adRoot.Count, () =>
            {
                loaded = true;
                ImgAndUrlLink();

            });
        }
        AdScrollObj.gameObject.SetActive(true);
    }

    void ImgAndUrlLink()
    {
        int count = 0;
        foreach (var item in AdScrollObj.ItemList)
        {
            // Debug.Log("i " + item);
            AdSpot adSpot = item.GetComponent<AdSpot>();
            adSpot.AdImg.sprite = adRoot[count].GameImg;
            adSpot.AdText.text = adRoot[count].GameName;
            string urlStr = adRoot[count].GameUrl;
            adSpot.AdBttn.onClick.AddListener(() =>
            {
                // Debug.LogError(urlStr);
                // AdStartAfterFewSecond (); 
                Application.OpenURL(urlStr);
            });
            count++;
        }
    }
}
