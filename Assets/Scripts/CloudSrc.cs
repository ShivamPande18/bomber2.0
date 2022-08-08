using UnityEngine;

public class CloudSrc : MonoBehaviour
{
    public SpriteRenderer back;

    static int isRight;

    private void Start()
    {
        if (Random.Range(0, 11) % 2 == 0)
        {
            isRight = -1;
        }
        else
        {
            isRight = 1;
        }

        Invoke("SetColor",0.01f);
    }
    void Update()
    {
        transform.Translate(Vector2.right * Random.Range(0.1f,1f) * isRight * Time.deltaTime);
    }

    void SetColor()
    {
        GetComponent<SpriteRenderer>().color = back.color;
    }
}