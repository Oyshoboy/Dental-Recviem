using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GlassTrigger : MonoBehaviour
{
   public AudioClip[] glassBrake;
   public AudioSource source;

   private void OnTriggerEnter(Collider other)
   {
      Debug.Log(other.name);
      source.PlayOneShot(glassBrake[Random.Range(0, glassBrake.Length)]);
   }
}
