using UnityEngine;

public class AdManager : MonoBehaviour
{
    public int cnt = 0;
    bool rewarded;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CacheRewardVideo();
    }
    public void ShowAd()
    {
        cnt++;
        if (cnt > 5)
        {
            GoogleAdMobController.Instance.ShowInterstitialAd();
            cnt = 0;
        }
    }

    void CacheRewardVideo()
    {
        GoogleAdMobController.Instance.RequestAndLoadRewardedAd();
    }

    public void ShowRewardedAd()
    {
        GoogleAdMobController.Instance.ShowRewardedAd(GiveReward, SkipButtonClicked);
    }

    void GiveReward()
    {
        Debug.Log("Reward Given");
        rewarded = true;
        CacheRewardVideo();
        FindObjectOfType<PlaneSrc>().SaveMe();
    }

    public void SkipButtonClicked()
    {
        if (!rewarded)
        {
            Debug.Log("reward not given");
            CacheRewardVideo();
        }
    }

}