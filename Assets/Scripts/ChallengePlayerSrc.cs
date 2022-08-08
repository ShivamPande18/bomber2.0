using UnityEngine;
using UnityEngine.SceneManagement;

public class ChallengePlayerSrc : MonoBehaviour
{
    public Sprite[] planeSprites;

    public ChallengeSrc challenge;
    public Animator damageAnim;

    public GameObject blastPart;
    public GameObject EndPanel;
    public GameObject firepoint;
    public GameObject messile;

    public float planeSpeed;

    Rigidbody2D rb;
    bool ended = false;
    bool isRight;

    public void Start()
    {
        GetComponent<SpriteRenderer>().sprite = planeSprites[PlayerPrefs.GetInt("currentPlane")];
        rb = GetComponent<Rigidbody2D>();
        isRight = false;
    }
    void Update()
    {
        transform.Translate(Vector2.left * planeSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.A))
        {
            OnShootClick();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void OnDirClick()
    {
        if (!ended)
        {
            if (isRight)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                isRight = false;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                isRight = true;
            }
        }
    }

    public void OnShootClick()
    {
        if (!ended)
        {
            Instantiate(messile, firepoint.transform.position, firepoint.transform.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("messileBack"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("truckMessile"))
        {
            Destroy(collision.gameObject);
            damageAnim.Play("DamageBlink");
            EndGame();
        }
    }

    public void EndGame()
    {
        rb.gravityScale = 1;
        ended = true;
        damageAnim.Play("DamageBlink");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("land") || collision.gameObject.CompareTag("truck") || collision.gameObject.CompareTag("boss"))
        {
            RohitShetty();
            FindObjectOfType<CameraSrc>().PlaySound();
            Destroy(Instantiate(blastPart, transform.position, Quaternion.identity) as GameObject, 2f);
            GetComponent<Collider2D>().enabled = false;
            challenge.OnChallengeEnd(false);
            FindObjectOfType<AdManager>().ShowAd();
        }
    }

    void RohitShetty()
    {
        Collider2D[] trucks = Physics2D.OverlapCircleAll(transform.position, 10f);
        foreach (var truck in trucks)
        {
            if (truck.CompareTag("truck"))
            {
                Destroy(Instantiate(blastPart, truck.transform.position, Quaternion.identity) as GameObject, 2f);
                Destroy(truck.gameObject);
            }
        }
    }
}