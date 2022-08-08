using UnityEngine;

public class TruckSrc : MonoBehaviour
{
    public float truckSpeed;
    bool isRight;
    
    private void Start()
    {
        if (transform.rotation.y == 0)
        {
            isRight = true;
        }
        else
        {
            isRight = false;  
        }
        truckSpeed = Random.Range(truckSpeed, truckSpeed+3);
    }
    void Update()
    {
        transform.Translate(Vector2.right * truckSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("side"))
        {
            if (isRight)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                isRight = false;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                isRight = true;
            }
        }
    }

    private void OnDisable()
    {
        Debug.Log("truck dead");
        try
        {
            FindObjectOfType<TruckSpawn>().trucksOnScreen--;
        }
        catch(System.Exception)
        {
            Debug.Log("error");
        }
    }
}