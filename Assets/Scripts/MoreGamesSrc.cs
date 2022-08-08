using UnityEngine;
using UnityEngine.UI;

public class MoreGamesSrc : MonoBehaviour
{
    public Image img;
    public Button btn;

    [SerializeField]
    bool canShow = false;


    private void OnEnable()
    {
        if(!canShow)
        {
            img.enabled = false;
            btn.enabled = false;
        }
    }

    public void Show()
    {
        canShow = true;
        img.enabled = true;
        btn.enabled = true;
    }

    public void Hide()
    {
        canShow = false;
        img.enabled = false;
        btn.enabled = false;
    }
}