using UnityEngine;

public class TeleportSrc : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.x > ScreenBounds.screenBounds.x)
        {
            transform.position = new Vector3(-ScreenBounds.screenBounds.x, transform.position.y, 0);
        }
        if (transform.position.x < -ScreenBounds.screenBounds.x)
        {
            transform.position = new Vector3(ScreenBounds.screenBounds.x, transform.position.y, 0);
        }
    }
}