using UnityEngine;

public class PositionGenericSrc : MonoBehaviour
{
    [Header("0d,1u,2l,3r")]
    public int dir;
    public float margin;
    void Start()
    {
        SetPostion();
    }
    public void SetPostion()
    {
        if (dir == 0)
        {
            transform.position = new Vector3(0, -ScreenBounds.screenBounds.y + margin, 0);
        }
        else if (dir == 1)
        {
            transform.position = new Vector3(0, ScreenBounds.screenBounds.y + margin, 0);
        }
        else if (dir == 2)
        {
            transform.position = new Vector3(-ScreenBounds.screenBounds.x + margin, 0, 0);
        }
        else
        {
            transform.position = new Vector3(ScreenBounds.screenBounds.x + margin, 0, 0);
        }
    }
}