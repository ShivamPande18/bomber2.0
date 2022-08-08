using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSrc : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayerPrefs.SetInt("totalKill", PlayerPrefs.GetInt("totalKill")+10);
        }
    }
    public void OnPlaneSelect()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<PlaneBtnSrc>().Clicked();
    }
}