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
    public ClientBehaviourHandler playaHandler;
    
    
    [Header("Playa Sit Config")]
    public float timeToTurn = 3f;
    public float idleTime = 3f;
    private Vector3 playaDefaultPos;
    
    [Header("Playa Directions")]
    public Transform playaTurnDirection;
    public Transform playaSitDirection;

    [Header("Doctor Config")]
    public Doctor_Controller doctorController;

    public GameLogicController gameLogicController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayaCollider")
        {
            var splineFolower = playa.GetComponent<SplineFollower>();
            splineFolower.wrapMode = SplineFollower.Wrap.Default;
            gameLogicController.InitToothAndStuff();
            StartCoroutine(nameof(TurnAroundAfterWhile), timeToTurn);
        }
    }


    public IEnumerator TurnAroundAfterWhile(float timeToTurn)
    {
        yield return new WaitForSeconds(idleTime);
        var splineFolower = playa.GetComponent<SplineFollower>();
        splineFolower.enabled = false;
        splineFolower.wrapMode = SplineFollower.Wrap.PingPong;
        
        var time = 0f;
        while (time < 1)
        {
            time += Time.deltaTime;
            TurnLeft(time);
            
            yield return null;
        }

        PlayerSit("sit");
        time = 0f;
        while (time < 1)
        {
            time += Time.deltaTime;
            MoveToSitPosition(time, playaSitDirection.position);
            
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

    public void PlayerCloseMouthAndStandUp()
    {
        StartCoroutine(nameof(StandUpAndCloseMouth), timeToTurn);
    }
    
    public IEnumerator StandUpAndCloseMouth(float timeToTurn)
    {
        yield return new WaitForSeconds(idleTime);
       // playa.GetComponent<SplineFollower>().enabled = false;

       var time = 0f;
       
       while (time < 1)
       {
           time += Time.deltaTime;
           CloseMouthAndDetachHead(time);
            
           yield return null;
       }
       
        PlayerSit("stand");
        time = 0f;
        playaDefaultPos = playa.transform.position;
        while (time < 1)
        {
            time += Time.deltaTime;
            MoveToSitPosition(time, playaDefaultPos);
            
            yield return null;
        }
        
//        playa.GetComponent<SplineFollower>().enabled = true;
        playaHandler.GiveCardToDoctor();
    }
    
    private void CloseMouthAndDetachHead(float value)
    {
        playaHead.SetBlendShapeWeight(34, 100 - (value * 100)); // MOUTH WITHD
        playaHead.SetBlendShapeWeight(36, 100 - (value * 100)); // MOUTH OPEN

        playaHeadEffector.positionWeight = 1 - value;
        playaHeadEffector.rotationWeight = 1 - value;
    }

    private void OpenMouthAndMoveHead(float value)
    {
        playaHead.SetBlendShapeWeight(34, value * 100); // MOUTH WITHD
        playaHead.SetBlendShapeWeight(36, value * 100); // MOUTH OPEN

        playaHeadEffector.positionWeight = value;
        playaHeadEffector.rotationWeight = value;
    }

    private void PlayerSit(string name)
    {
        playaAnimator.SetTrigger(name);
    }

    void TurnLeft(float time)
    {
        playa.transform.rotation = Quaternion.Lerp(playa.transform.rotation, playaTurnDirection.transform.rotation, time);
    }
    
    void MoveToSitPosition(float time, Vector3 position)
    {
        playa.transform.position = Vector3.Lerp(playa.transform.position, position, time);
    }
    
}// 34 36
