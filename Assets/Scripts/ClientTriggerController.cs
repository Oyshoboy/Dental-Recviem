using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Random = System.Random;

public class ClientTriggerController : MonoBehaviour
{
    [Header("Playa Refferences")]
    public GameObject playa;
    public Animator playaAnimator;
    public SkinnedMeshRenderer playaHead;
    public FBBIKHeadEffector playaHeadEffector;
    
    [Header("Playa Sit Config")]
    public float timeToTurn = 3f;
    public float idleTime = 3f;
    
    [Header("Playa Directions")]
    public Transform playaTurnDirection;
    public Transform playaSitDirection;

    [Header("Doctor Config")]
    public Doctor_Controller doctorController;

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
            time += Time.deltaTime;
            TurnLeft(time);
            
            yield return null;
        }

        PlayerSit();
        time = 0f;
        while (time < 1)
        {
            time += Time.deltaTime;
            MoveToSitPosition(time);
            
            yield return null;
        }
        
        time = 0f;
        while (time < 1)
        {
            time += Time.deltaTime;
            OpenMouthAndMoveHead(time);
            
            yield return null;
        }

        doctorController.StartCoroutine(nameof(Doctor_Controller.StartMovingHands), true);
    }

    private void OpenMouthAndMoveHead(float value)
    {
        playaHead.SetBlendShapeWeight(34, value * 100); // MOUTH WITHD
        playaHead.SetBlendShapeWeight(36, value * 100); // MOUTH OPEN

        playaHeadEffector.positionWeight = value;
        playaHeadEffector.rotationWeight = value;
    }

    private void PlayerSit()
    {
        playaAnimator.SetTrigger("sit");
    }

    void TurnLeft(float time)
    {
        playa.transform.rotation = Quaternion.Lerp(playa.transform.rotation, playaTurnDirection.transform.rotation, time);
    }
    
    void MoveToSitPosition(float time)
    {
        playa.transform.position = Vector3.Lerp(playa.transform.position, playaSitDirection.transform.position, time);
    }
    
}// 34 36
