using GameAnalyticsSDK.Setup;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaneSrc : MonoBehaviour
{
    [Header("Analytics")]
    public GameManager gameManager;

    [Header("RewardRespawn")]
    public PositionGenericSrc pos;

    [Header("TimeAttack")]
    public GameObject dirPanel;

    [Header("Plane Extra")]
    public Sprite[] planeSprites;
    public Collider2D planeColl;
    public GameObject firepoint;
    public GameObject messile;
    public float planeSpeed;
    public bool isChallenge;

        [Header("private")]
        Rigidbody2D rb;
        bool isRight;

    [Header("Plane Health")]
    public Gradient healthColor;
    public Animator damageAnim;
    public Slider healthBar;
    public Image fill;
    public int[] life;

        [Header("private")]
        float curHealth;
        float maxHealth;

    [Header("End Panel")]
    public GameObject EndPanel;
    public GameObject blastPart;
    public GameObject savePanel;
    [SerializeField]
    bool canSave = true;
   
        [Header("private")]
        bool ended = false;


    public void Start()
    {
        if (isChallenge)
        {
            maxHealth = curHealth = 10;
            StartCoroutine(ShowDir());
        }
        else
        { 
            maxHealth = life[PlayerPrefs.GetInt("currentPlane")] + 1;
            curHealth = maxHealth;
        
        }
        GetComponent<SpriteRenderer>().sprite = planeSprites[PlayerPrefs.GetInt("currentPlane")];
        rb = GetComponent<Rigidbody2D>();
        isRight = false;
    }
    void Update()
    {
        if(isChallenge)
        {
            curHealth -= Time.deltaTime;
            if (curHealth < 0)
            {
                rb.gravityScale = 1;
                ended = true;
            }
        }

        transform.Translate(Vector2.left * planeSpeed * Time.deltaTime);
        healthBar.value = curHealth / maxHealth;
        fill.color = healthColor.Evaluate(healthBar.value);

        if (Input.GetKeyDown(KeyCode.A))
        {
            OnShootClick();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SaveMe();
        }
    }

    IEnumerator ShowDir()
    {
        yield return new WaitForSeconds(1);
        dirPanel.SetActive(true);
        maxHealth = curHealth = 10;
        Time.timeScale = 0;
    }

    public void OnDirPlay()
    {
        dirPanel.SetActive(false);
        Time.timeScale = 1;
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

    public void Revive()
    {
        curHealth = maxHealth;
    }

    public void TakeDamage(int n)
    {
        damageAnim.Play("DamageBlink");
        curHealth-=n;
        if (curHealth <= 0)
        {
            rb.gravityScale = 1;
            ended = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("messileBack"))
        {
            curHealth++;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("truckMessile"))
        {
            Destroy(collision.gameObject);
            if (isChallenge)
            {
                TakeDamage(100);
            }
            else
            {
                TakeDamage(1);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("land") || collision.gameObject.CompareTag("truck") || collision.gameObject.CompareTag("boss"))
        {
            RohitShetty();
            FindObjectOfType<CameraSrc>().PlaySound();
            Destroy(Instantiate(blastPart,transform.position,Quaternion.identity) as GameObject , 2f);
            EndPanel.SetActive(true);

            if (canSave)
            {
                canSave = false;
            }
            else
            {
                if (savePanel != null)
                {
                    savePanel.SetActive(false);
                }
            }
            planeColl.enabled = false;
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

    public void SaveMe()
    {
        rb.velocity = Vector2.zero;
        pos.SetPostion();
        EndPanel.SetActive(false);
        curHealth = maxHealth;
        ended = false;
        rb.gravityScale = 0;
        planeColl.enabled = true;
    }
}