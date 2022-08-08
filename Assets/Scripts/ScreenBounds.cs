using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    public static Vector2 screenBounds;

    void Awake()
    {
        screenBounds = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, transform.position.z));
        screenBounds = new Vector2(Mathf.Abs(screenBounds.x), Mathf.Abs(screenBounds.y));
    }
}