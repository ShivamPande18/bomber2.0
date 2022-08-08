using UnityEngine;

public class ChallengeMessileSrc : MonoBehaviour
{
	public GameObject burstPart;
	CameraSrc cam;

	private void Start()
	{
		cam = FindObjectOfType<CameraSrc>();
	}

	private void FixedUpdate()
	{
		transform.localEulerAngles = Vector3.Lerp(new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z), new Vector3(0, transform.localEulerAngles.y, 180), Time.fixedDeltaTime);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Destroy(Instantiate(burstPart, transform.position, Quaternion.identity) as GameObject, 3f);
		cam.Shake();
		cam.PlaySound();

		if (collision.gameObject.CompareTag("truck"))
		{
			FindObjectOfType<ChallengeSrc>().OnKill();
			Destroy(gameObject);
			Destroy(collision.gameObject);
			return;
		}
		else if (collision.collider.CompareTag("land"))
		{
			FindObjectOfType<ChallengePlayerSrc>().EndGame();
			Destroy(gameObject);
		}
	}
}