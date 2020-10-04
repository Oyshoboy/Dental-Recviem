using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Random = System.Random;

public class ClientTriggerController : MonoBehaviour
{
    public GameObject playa;
    public float timeToTurn = 3f;
    public float idleTime = 3f;
    public GameObject directionPoint;
    public Animator playaAnimator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayaCollider")
        {
            Debug.Log("COllided");
            StartCoroutine(nameof(TurnAroundAfterWhile), timeToTurn);
        }
    }


    public IEnumerator TurnAroundAfterWhile(float timeToTurn)
    {
        yield return new WaitForSeconds(idleTime);
        Debug.Log("EXECUTING");
        playa.GetComponent<SplineFollower>().enabled = false;
        var time = 0f;
        while (time < 1)
        {
            time += Time.deltaTime / timeToTurn;
            TurnLeft(time);
            yield return null;
        }

        PlayerSit();
    }

    private void PlayerSit()
    {
        playaAnimator.applyRootMotion = true;
        playaAnimator.SetTrigger("sit");
    }

    void TurnLeft(float time)
    {
        playa.transform.rotation = Quaternion.Lerp(playa.transform.rotation, directionPoint.transform.rotation, time);
    }
    
}
