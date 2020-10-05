﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsSoundController : MonoBehaviour
{
    public AudioClip[] stepSounds;

    public AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WalkinEventProcessor()
    {
        source.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)]);
    }
}
