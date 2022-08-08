using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;
using UnityEngine;

public class GAManager : Singleton<GAManager> {

    void Start () {
        GameAnalytics.SetEnabledEventSubmission (true);
        GameAnalytics.Initialize ( );
    }

    public void PlayBtnClicked () {
        IDictionary<string, object> fields = new Dictionary<string,object>();
        fields.Add ("score", 99);
        fields.Add ("GameOver", true);
        GA_Design.NewEvent("PlayBtnClicked",1, fields);
    }
}