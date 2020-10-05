using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothSoundProcessor : MonoBehaviour
{
    public AudioSource Source;
    
    public AudioClip[] grunts;

    public AudioClip[] insert;

    public AudioClip[] stuck;
    // Start is called before the first frame update
    public AudioClip[] whooshes;

    public AudioClip[] punches;

    public AudioClip[] deepWhooshes;

    public AudioClip[] creaks;
    
    public void InsertTooth()
    {
        Source.PlayOneShot(stuck[Random.Range(0, stuck.Length)], 0.7f);
        StartCoroutine(insertSoundPlay(.1f));
    }
    
    public void ChairCreak()
    {
        Source.PlayOneShot(creaks[Random.Range(0, creaks.Length)], 0.45f);
    }


    IEnumerator insertSoundPlay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Source.PlayOneShot(insert[Random.Range(0, insert.Length)], 0.6f);
        StartCoroutine(gruntSoundPlay(.05f));
    }

    public void PlayDeepWhoosh()
    {
        Source.PlayOneShot(deepWhooshes[Random.Range(0, deepWhooshes.Length)], 0.25f);
    }
    
    IEnumerator gruntSoundPlay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Source.PlayOneShot(grunts[Random.Range(0, grunts.Length)], .6f);
    }

    public void HitPunch()
    {
        Source.PlayOneShot(whooshes[Random.Range(0, whooshes.Length)], 0.7f);
        StartCoroutine(punchLanded(.08f));
    }
    
    IEnumerator punchLanded(float delay)
    {
        yield return new WaitForSeconds(delay);
        Source.PlayOneShot(punches[Random.Range(0, punches.Length)], 0.9f);
        StartCoroutine(gruntSoundPlay(.05f));
    }
}
