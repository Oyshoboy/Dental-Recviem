using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockSoundProcessor : MonoBehaviour
{
    public AudioClip tik;
    public AudioSource source;
    private float nextActionTime = 0.0f;
    public float period = 0.1f;
    
    void Update () {
        if (Time.time > nextActionTime ) {
            nextActionTime += period;
            source.pitch = Random.Range(0.95f, 1.05f);
            source.PlayOneShot(tik);
        }
    }
}
