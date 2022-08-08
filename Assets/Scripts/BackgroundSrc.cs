using UnityEngine;

public class BackgroundSrc : MonoBehaviour
{
    public Color[] colors;
    public bool isBack;

    private void Start()
    {
        if (isBack)
        {
            transform.localScale = new Vector3(ScreenBounds.screenBounds.x, ScreenBounds.screenBounds.y, 0) / 2f;
        }
        GetComponent<SpriteRenderer>().color = colors[Random.Range(0, colors.Length)];
    }
}