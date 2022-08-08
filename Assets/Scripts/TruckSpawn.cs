using UnityEngine;

public class TruckSpawn : MonoBehaviour
{
	public GameObject[] truck;
	public GameObject powerUp;
	public Transform[] spawnPoints;

	public float timeBwSpawns;
	public int powerUpTime;
	public int trucksOnScreen;
	public int truckLimit;
	public bool isChallenge;

	int spawnCnt = 1;
	int progression = 1;
	float cnt;
	int spawnPntNo;

	void Start()
	{
		cnt = 0;
		spawnPntNo = Random.Range(0,2);
	}

	void Update()
	{
		cnt += Time.deltaTime;

        if (cnt >= timeBwSpawns)
        {
            cnt = 0;
            spawnCnt++;
            Inst();
        }
    }

	void Inst()
	{
		spawnPntNo = (spawnPntNo == 0) ? 1 : 0;
		if (spawnCnt >= powerUpTime && !isChallenge)
		{
			Instantiate(powerUp, spawnPoints[spawnPntNo].position, spawnPoints[spawnPntNo].rotation);
			spawnCnt = 0;
			trucksOnScreen++;
		}
		else if (trucksOnScreen <= truckLimit)
		{
			trucksOnScreen++;
			if (isChallenge)
			{
				Instantiate(truck[Random.Range(0, truck.Length)], spawnPoints[spawnPntNo].position, spawnPoints[spawnPntNo].rotation);
			}
			else
			{
				Instantiate(truck[Random.Range(0, progression)], spawnPoints[spawnPntNo].position, spawnPoints[spawnPntNo].rotation);
			}
		}
	}

	public void Progress()
	{
		if (progression < truck.Length)
		{
			progression++;
			truckLimit+=3;
		}
	}
}