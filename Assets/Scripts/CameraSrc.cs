using UnityEngine;

public class CameraSrc : MonoBehaviour
{
    public AudioSource blastSound; 
    public Animator anim;
    
    public void Shake()
    {
        anim.Play("CameraShake");
    }

    public void PlaySound()
    {
        blastSound.Play();
    }
}