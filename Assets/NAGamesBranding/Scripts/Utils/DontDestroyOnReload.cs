using UnityEngine;

public class DontDestroyOnReload : MonoBehaviour 
{
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
}
