using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections.Generic;

public class GoogleAdMobController : Singleton<GoogleAdMobController>
{

    public string BannerViewID = "ca-app-pub-3940256099942544/6300978111";
    public string InterstitialID = "ca-app-pub-3940256099942544/1033173712";
    public string RewardedAdID = "ca-app-pub-3940256099942544/5224354917";

    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    //Put the width and height where banner has to show
    private AdSize BannerSize = new AdSize ( 260, 50 );

    public Action OnAdLoadedEvent;
    public Action OnAdFailedToLoadEvent;
    public Action OnAdOpeningEvent;
    public Action OnAdFailedToShowEvent;
    public Action OnUserEarnedRewardEvent;
    public Action OnAdClosedEvent;
    public Action OnAdLeavingApplicationEvent;
    // public Text statusText;
    // public Text rewardedText;

    #region UNITY MONOBEHAVIOR METHODS

    public void Start()
    {
        MobileAds.SetiOSAppPauseOnBackground(true);

        List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };

        // Add some test device IDs (replace with your own device IDs).
// #if UNITY_IPHONE
//         deviceIds.Add("2077ef9a63d2b398840261c8221a0c9b");
// #elif UNITY_ANDROID
//         Debug.Log("==== Android");
//         deviceIds.Add("2077ef9a63d2b398840261c8221a0c9b");
// #endif

        // Configure TagForChildDirectedTreatment and test device IDs.
        RequestConfiguration requestConfiguration =
            new RequestConfiguration.Builder()
            .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
            // .SetTestDeviceIds(deviceIds)
            .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(HandleInitCompleteAction);

        InvokeRepeating("CacheInterstitial", 5f, 5f);
    }

    void CacheInterstitial()
    {
        if(!interstitialAd.IsLoaded())
        {
            AdRequest request = new AdRequest.Builder().Build();
            interstitialAd.LoadAd(request);
        }
    }

    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // main thread.
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.
        MobileAdsEventExecutor.ExecuteInUpdate(() => {
            Debug.Log ("==== Initialization complete");
            // statusText.text = "Initialization complete";
            // RequestBannerAd();
            RequestAndLoadInterstitialAd();
            RequestAndLoadRewardedAd();
        });
    }

    #endregion

    #region HELPER METHODS

    private AdRequest CreateAdRequest()
    {
        Debug.Log("==== CreateAdRequest");

        return new AdRequest.Builder()
            // .AddTestDevice(AdRequest.TestDeviceSimulator)
            // .AddTestDevice("2077ef9a63d2b398840261c8221a0c9b")
            // .AddKeyword("unity-admob-sample")
            // .TagForChildDirectedTreatment(false)
            // .AddExtra("color_bg", "9B30FF")
            .Build();
    }

    #endregion

    #region BANNER ADS

    public void RequestBannerAd()
    {
        // statusText.text = "Requesting Banner Ad.";
        // These ad units are configured to always serve test ads.
// #if UNITY_EDITOR
//         string adUnitId = "unused";
// #elif UNITY_ANDROID
//         string adUnitId = "ca-app-pub-3940256099942544/6300978111";
// #elif UNITY_IPHONE
//         string adUnitId = "ca-app-pub-3940256099942544/2934735716";
// #else
//         string adUnitId = "unexpected_platform";
// #endif
        // Clean up banner before reusing
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(BannerViewID, AdSize.SmartBanner, AdPosition.Top);

        // Add Event Handlers
        bannerView.OnAdLoaded += (sender, args) => { if( OnAdLoadedEvent != null ) OnAdLoadedEvent.Invoke(); };
        bannerView.OnAdFailedToLoad += (sender, args) => { if( OnAdFailedToLoadEvent != null ) OnAdFailedToLoadEvent.Invoke(); };
        bannerView.OnAdOpening += (sender, args) => { if( OnAdOpeningEvent != null ) OnAdOpeningEvent.Invoke(); };
        bannerView.OnAdClosed += (sender, args) => { if( OnAdClosedEvent != null ) OnAdClosedEvent.Invoke(); };
        bannerView.OnAdLeavingApplication += (sender, args) => { if( OnAdLeavingApplicationEvent != null ) OnAdLeavingApplicationEvent.Invoke(); };

        // Load a banner ad
        bannerView.LoadAd(CreateAdRequest());
    }

    public void ShowBanner () {
        Debug.Log("==== ShowBanner");
        if (bannerView == null)
        {
            // statusText.text = "Banner ad is not ready yet";
            return;
        }
        if (bannerView != null)
            bannerView.Show( );
    }

    public void ShowBanner( AdPosition adPos )
    {
        try
        {
            if (bannerView == null)
            {
                // statusText.text = "Banner ad is not ready yet";
                return;
            }    
            bannerView.SetPosition( adPos );
            bannerView.Show( );
        }
        catch
        {
            Debug.LogError( "ShowBanner failed " );
        }
    }

    public void ShowBanner ( GameObject BannerHolder ){
        int x, y;
        if( GetBannerOrigin( BannerHolder, out x, out y ) ) 
            ShowBanner( x, y );
        else
            CloseBanner( );
    }

    bool GetBannerOrigin( GameObject BannerHolder, out int originX, out int originY )
    {
        originX = originY = 0;

        if( BannerHolder )
        {
            Vector3[ ] corners = new Vector3[ 4 ];
            BannerHolder.GetComponent< RectTransform >( ).GetWorldCorners( corners );

            Vector3 o = Vector3.zero;
            foreach( var p in corners ) o += p;

            o /= corners.Length;

            originX = (int) o.x;
            originY = (int) o.y;

            print( "X, Y : " + originX + " , " + originY );

            return true;
        }
        else
            return false;
    }

    void ShowBanner( int x, int y )
    {
        try
        {
            if (bannerView == null)
            {
                // statusText.text = "Banner ad is not ready yet";
                return;
            }    
            y = Screen.height - y;
    
            float w = (int) bannerView.GetWidthInPixels( ) ;
            float h = (int) bannerView.GetHeightInPixels( );
    
            x = (int) ( (float) x / w * (float) BannerSize.Width ) - BannerSize.Width / 2;
            y = (int) ( (float) y / h * (float) BannerSize.Height ) - BannerSize.Height / 2;
    
            bannerView.SetPosition( x, y );
            bannerView.Show( );
        }
        catch
        {
            Debug.LogError( "ShowBanner failed " );
        }
    }

    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }
    public void CloseBanner( )
    {
        if( bannerView != null ) bannerView.Hide( );
    }

    #endregion

    #region INTERSTITIAL ADS

    public void RequestAndLoadInterstitialAd()
    {
        // statusText.text = "Requesting Interstitial Ad.";
// #if UNITY_EDITOR
//         string adUnitId = "unused";
// #elif UNITY_ANDROID
//         string adUnitId = "ca-app-pub-3940256099942544/1033173712";
// #elif UNITY_IPHONE
//         string adUnitId = "ca-app-pub-3940256099942544/4411468910";
// #else
//         string adUnitId = "unexpected_platform";
// #endif

        // Clean up interstitial before using it
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }

        interstitialAd = new InterstitialAd(InterstitialID);

        // Add Event Handlers
        interstitialAd.OnAdLoaded += (sender, args) => { if( OnAdLoadedEvent != null ) OnAdLoadedEvent.Invoke(); };
        interstitialAd.OnAdFailedToLoad += (sender, args) => { if( OnAdFailedToLoadEvent != null ) OnAdFailedToLoadEvent.Invoke(); };
        interstitialAd.OnAdOpening += (sender, args) => { if( OnAdOpeningEvent != null ) OnAdOpeningEvent.Invoke(); };
        interstitialAd.OnAdClosed += (sender, args) => { if( OnAdClosedEvent != null ) OnAdClosedEvent.Invoke(); };
        interstitialAd.OnAdLeavingApplication += (sender, args) => { if( OnAdLeavingApplicationEvent != null ) OnAdLeavingApplicationEvent.Invoke(); };

        // Load an interstitial ad
        interstitialAd.LoadAd(CreateAdRequest());
    }

    public void ShowInterstitialAd()
    {
        if ( interstitialAd == null ) 
        {
            // statusText.text = "Interstitial ad is not ready yet";
            return;
        }

        if (interstitialAd.IsLoaded())
        {
            Debug.Log("==== ShowInterstitialAd");
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial ad is not ready yet");
            // statusText.text = "Interstitial ad is not ready yet";
        }
    }

    public void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }
    #endregion

    #region REWARDED ADS

    public void RequestAndLoadRewardedAd()
    {
        // statusText.text = "Requesting Rewarded Ad.";
// #if UNITY_EDITOR
//         string adUnitId = "unused";
// #elif UNITY_ANDROID
//         string adUnitId = "ca-app-pub-3940256099942544/5224354917";
// #elif UNITY_IPHONE
//         string adUnitId = "ca-app-pub-3940256099942544/1712485313";
// #else
//         string adUnitId = "unexpected_platform";
// #endif

        // create new rewarded ad instance
        rewardedAd = new RewardedAd(RewardedAdID);

        // Add Event Handlers
        rewardedAd.OnAdLoaded += (sender, args) => { if(OnAdLoadedEvent != null ) OnAdLoadedEvent.Invoke(); };
        rewardedAd.OnAdFailedToLoad += (sender, args) => { if(OnAdFailedToLoadEvent != null )  OnAdFailedToLoadEvent.Invoke(); };
        rewardedAd.OnAdOpening += (sender, args) =>  { if(OnAdOpeningEvent != null ) OnAdOpeningEvent.Invoke(); };
        rewardedAd.OnAdFailedToShow += (sender, args) => { if(OnAdFailedToShowEvent != null ) OnAdFailedToShowEvent.Invoke( ); };
        rewardedAd.OnAdClosed += (sender, args) =>  { if(OnAdClosedEvent != null )  OnAdClosedEvent.Invoke(); };
        rewardedAd.OnUserEarnedReward += (sender, args) => { 
            if(OnUserEarnedRewardEvent != null ) OnUserEarnedRewardEvent.Invoke();
            OnUserEarnedRewardEvent = null;
        };

        // Create empty ad request
        rewardedAd.LoadAd(CreateAdRequest());
    }

    public void ShowRewardedAd( Action callback, Action closedcallback)
    {
        if(rewardedAd == null)
        {
            // statusText.text = "Rewarded ad is not ready yet.";
            return;
        }
        if ( rewardedAd.IsLoaded() )
        {
            Debug.Log("==== ShowRewardedAd");
            OnUserEarnedRewardEvent = callback;
            OnAdClosedEvent = closedcallback;
            rewardedAd.Show();
        }
        else
        {
            Debug.Log("Rewarded ad is not ready yet.");
            // statusText.text = "Rewarded ad is not ready yet.";
        }
    }

    #endregion
}
