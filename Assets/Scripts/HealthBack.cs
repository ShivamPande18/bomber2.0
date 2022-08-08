using UnityEngine;

public class HealthBack : MonoBehaviour
{
    public float messileBackSpeed;
    
    private void Update()
    {
        transform.Translate(Vector3.up * messileBackSpeed * Time.deltaTime);
    }
}