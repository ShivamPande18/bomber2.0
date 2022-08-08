using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdVisible : MonoBehaviour
{
    public Image adImg;
    public Text adText;
    public Button adBtn;
    public bool BubbleMovement = true;
    RectTransform selfRect;
    public float horizontalSpeed = 1;
    public float verticalSpeed = 1;
    public int offset = 50;
    float screenWidth;
    float screenHeight;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        selfRect = GetComponent<RectTransform>();
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    void OnEnable()
    {
        if (LoadAds.Instance.adRoot.Count > 0 ) {
            if(LoadAds.Instance.AdScroller) {
                adImg.sprite = LoadAds.Instance.MoreGameImg;
                adBtn.onClick.AddListener( () => { 
                    LoadAds.Instance.OpenMoreGameScroll();
                    // gameObject.SetActive (false);
                } );
            } else {
                int no = Random.Range(0 , LoadAds.Instance.adRoot.Count );
                adImg.sprite = LoadAds.Instance.adRoot[no].GameImg;
                if(adText) adText.text = LoadAds.Instance.adRoot[no].GameName;
                adBtn.onClick.AddListener( () => OnClickAds (LoadAds.Instance.adRoot[no].GameUrl) );
            }
        } else {
            adImg.sprite = LoadAds.Instance.defaultImg;
            if(adText) adText.text = LoadAds.Instance.defaultGameName;
            adBtn.onClick.AddListener( () => OnClickAds (LoadAds.Instance.defaultUrl) );
        }
    }

    void OnDisable() {
        adBtn.onClick.RemoveAllListeners(  );
    }

    public void OnClickAds (string url){
        Debug.Log("url " + url);
        // gameObject.SetActive(false);
        // LoadAds.Instance.AdStartAfterFewSecond ( );
        Application.OpenURL(url);
    }
}
