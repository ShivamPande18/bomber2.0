using UnityEngine;

public class PowerUpSrc : MonoBehaviour
{
    public GameObject health;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("messile"))
        {
            Instantiate(health, transform.position, Quaternion.identity);
        }
    }
}