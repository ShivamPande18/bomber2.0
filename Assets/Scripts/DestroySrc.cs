using UnityEngine;

public class DestroySrc : MonoBehaviour
{
    public float distTime;
    private void Start()
    {
        Destroy(gameObject,distTime);
    }
}