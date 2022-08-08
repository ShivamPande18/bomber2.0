using UnityEngine;

public class ChildPos : MonoBehaviour
{
    public GameObject parent;
    public Vector3 pos;
    void Update()
    {
        transform.position = parent.transform.position + pos;
    }
}
