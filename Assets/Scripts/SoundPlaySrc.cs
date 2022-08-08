using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlaySrc : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Play());
    }
    IEnumerator Play()
    {
        yield return new WaitForSeconds(0.01f);
        GetComponent<AudioSource>().Play();
    }
}
