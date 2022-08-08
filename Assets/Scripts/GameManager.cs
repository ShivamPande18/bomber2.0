using UnityEngine;
using GameAnalyticsSDK;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "play");
        Debug.Log("Play Started");
        FindObjectOfType<MoreGamesSrc>().Hide();
    }

    public void OnEnd()
    { 
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "play",ValuesSrc.currentKill);
        Debug.Log("Play completed");
    }

    public void OnBuy(int price)
    {
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "score ", price, "score", "score");
    }
}