using UnityEngine;

public class TruckShoot : MonoBehaviour
{
    public GameObject messile;
    public Transform firePnt;

    public bool isMovingShooter;
    public int fireRate;

    Transform plane;
    float cnt = 0;

    private void Start()
    {
        plane = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (isMovingShooter)
        {
            firePnt.transform.up = plane.position - firePnt.transform.position;
        }

        if (transform.position.x >= -ScreenBounds.screenBounds.x && transform.position.x <= ScreenBounds.screenBounds.x)
        {
            cnt+=Time.deltaTime;
            if (cnt >= fireRate)
            {
                Instantiate(messile, firePnt.transform.position, firePnt.transform.rotation);
                cnt = 0;
            }
        }
    }
}