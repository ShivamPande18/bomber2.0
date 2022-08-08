using TMPro;
using UnityEngine;

public class PlaneBtnSrc : MonoBehaviour
{
    public GameManager gameManager;

    public int index;
    public int price;

    bool isBought = false;

    TMP_Text priceTxt;
    TMP_Text lifeTxt;

    private void Awake()
    {
        priceTxt = transform.GetChild(0).GetComponent<TMP_Text>();
        lifeTxt = transform.GetChild(2).GetComponent<TMP_Text>();
        priceTxt.text = price.ToString();
        if (index == PlayerPrefs.GetInt("currentPlane") || PlayerPrefs.GetInt(gameObject.name) == 1)
        {
            isBought = true;
        }
        else
        {
            lifeTxt.text = "LIFE : " + (index+1).ToString();
        }
    }

    private void Update()
    {
        if (isBought)
        {
            if (index == PlayerPrefs.GetInt("currentPlane"))
            {
                priceTxt.text = string.Empty;
                lifeTxt.text = "EQUIPPED";
            }
            else
            {
                priceTxt.text = string.Empty;
                lifeTxt.text = "NOT EQUIPPED";
            }
        }
    }
    public void Clicked()
    {
        if (isBought && index != PlayerPrefs.GetInt("currentPlane"))
        {
            PlayerPrefs.SetInt("currentPlane", index);
        }
        else if (PlayerPrefs.GetInt("totalKill") >= price)
        {
            PlayerPrefs.SetInt("totalKill", PlayerPrefs.GetInt("totalKill") - price);
            PlayerPrefs.SetInt("currentPlane",index);
            PlayerPrefs.SetInt(gameObject.name, 1);
            isBought = true;
            gameManager.OnBuy(price);
        }
    }
}