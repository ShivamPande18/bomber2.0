using UnityEngine;
using System.Collections;

public class BossSrc : MonoBehaviour
{
    public GameObject messile;
    public Transform firePnt;

    public float bossSpeed;
    public float timeBwShoot;
    public int bossHealth;

    float cnt;

    private void Start()
    {
        StartCoroutine(ChangeDir());
    }
    void Update()
    {
        cnt+=Time.deltaTime;

        if (cnt >= timeBwShoot)
        {
            Shoot();
            cnt = 0;
        }

        transform.Translate(Vector3.right*bossSpeed*Time.deltaTime);

    }


    void Shoot()
    {
        Instantiate(messile,firePnt.transform.position,firePnt.transform.rotation);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("messile"))
        {
            Destroy(collision.gameObject);
            bossHealth--;
            FindObjectOfType<ValuesSrc>().OnKill();

            if (bossHealth <= 0)
            {
                FindObjectOfType<ValuesSrc>().OnKill();
                Destroy(gameObject);
            }
        }
    }

    IEnumerator ChangeDir()
    {
        bossSpeed = -bossSpeed;
        yield return new WaitForSecondsRealtime(7);
        StartCoroutine(ChangeDir());
    }
}