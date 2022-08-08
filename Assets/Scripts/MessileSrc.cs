using UnityEngine;

public class MessileSrc : MonoBehaviour
{
	public GameObject burstPart;
	public int damage;
	public bool isTimeAttack;
	CameraSrc cam;

	private void Start()
	{
		cam = FindObjectOfType<CameraSrc>();
	}

	private void FixedUpdate()
	{
		transform.localEulerAngles = Vector3.Lerp(new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z), new Vector3(0, transform.localEulerAngles.y, 180),Time.fixedDeltaTime);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		cam.PlaySound();
		cam.Shake();
		Destroy(Instantiate(burstPart, transform.position, Quaternion.identity) as GameObject, 3f);

		if (collision.gameObject.CompareTag("truck"))
		{
			FindObjectOfType<ValuesSrc>().OnKill();
			
			Destroy(gameObject);
			Destroy(collision.gameObject);
			if (isTimeAttack)
			{
				FindObjectOfType<PlaneSrc>().Revive();
			}
			else
			{
				if (ValuesSrc.currentKill >= 10 && ValuesSrc.currentKill % 10 == 0)
				{
					FindObjectOfType<TruckSpawn>().Progress();
				}
			}
			return;
		}
		else if (collision.collider.CompareTag("land"))
		{
			FindObjectOfType<PlaneSrc>().TakeDamage(damage);
			Destroy(gameObject);
		}
	}
}