using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using Dreamteck.Splines.Primitives;
using RootMotion.FinalIK;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClientBehaviourHandler : MonoBehaviour
{
    public SkinnedMeshRenderer clientHeadMesh;
    private float mouthOpenGoal;
    public bool isTalking = false;
    private bool cycle = false;
    public float speed = 5;
    public float mouthMoveSpeed = 1f;

    public AudioClip[] audioClips;
    public AudioClip cardPickup;
    public AudioSource cardPickupSource;
    public AudioSource audioSource;
    public AudioSource transactionDeclinedSource;
    public AudioClip transactionDeclined;
    private int audioIndex = 0;
    
    public GameObject creditCard;
    public FullBodyBipedIK playaFullBodyIk;
    public Doctor_Controller doctorController;
    public ClientTriggerController clientTriggerController;
    
    public void GiveCardToDoctor()
    {
        creditCard.SetActive(true);
        StartCoroutine(StartMovingHand(true));
    }

    public void TakeCardBack()
    {
        StartCoroutine(StartMovingHand(false));
    }

    public void StartTalking()
    {
        StartCoroutine(MouthMovements(1, mouthMoveSpeed));
    }


    public IEnumerator StartMovingHand(bool forward, bool cardDisplay = false)
    {
        var time = 0f;
        while (time < 1)
        {
            time += Time.deltaTime;
            if (forward)
            {
                HandController(time);
            }
            else
            {
                HandController(1 - time);
            }

            yield return null;
        }

        if (forward)
        {
            Debug.Log("FHINISHED");
            StopAllCoroutines();
            StartCoroutine(StartMovingHandBack(cardDisplay));
        }
    }

    public IEnumerator StartMovingHandBack(bool cardDisplay)
    {
        yield return new WaitForSeconds(.3f);
        creditCard.SetActive(cardDisplay);
        cardPickupSource.PlayOneShot(cardPickup);
        var time = 0f;
        while (time < 1)
        {
            time += Time.deltaTime;
            HandController(1f - time);
            yield return null;
        }
        
        StopAllCoroutines();

        if (!cardDisplay)
        {
            StartCoroutine(CardDeclindeMessage());
        }
        else
        {
            StartCoroutine(MouthMovements(1, 10));
            
            audioSource.PlayOneShot(audioClips[audioIndex]);
            audioIndex++;

            if (audioIndex + 1 == audioClips.Length)
            {
                audioIndex = 0;
            }
            
            StartCoroutine(StopTalking());
        }
    }

    private IEnumerator StopTalking()
    {
        yield return new WaitForSeconds(1.5f);
        StopAllCoroutines();
        isTalking = false;
        clientTriggerController.CloseMouthBaebe(1);
        StartCoroutine(StopCoroutinesWithDelay(.2f));
    }

    void ClientLeaving()
    {
        var splineFolower = clientTriggerController.playa.GetComponent<SplineFollower>();
        splineFolower.enabled = true;
    }
    
    private IEnumerator StopCoroutinesWithDelay(float idle)
    {
        yield return new WaitForSeconds(idle);
        StopAllCoroutines();
        ClientLeaving();
        Debug.Log("STOPPED TALKING");
    }


    IEnumerator CardDeclindeMessage()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("CARD DECLINED");
        transactionDeclinedSource.PlayOneShot(transactionDeclined);
        yield return null;
        StopAllCoroutines();
        StartCoroutine(DoctorReadyToFight());
    }

    IEnumerator DoctorReadyToFight()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("DOCTOR IS READY TO FIGHT");
        doctorController.doctorIsFighting = true;
        yield return null;
        StopAllCoroutines();
    }
    

    void HandController(float time)
    {
        playaFullBodyIk.solver.rightHandEffector.positionWeight = time;
        playaFullBodyIk.solver.rightHandEffector.rotationWeight = time;
    }

    public IEnumerator MouthMovements(int state, float timeToMove)
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime * timeToMove;
            LerpController(state, t);
            yield return null;
        }

        state = Random.Range(0, 2) - 1;
        if (!isTalking) state = -1;
        StartCoroutine(MouthMovements(state, Random.Range(5, 10)));
    }

    void LerpController(int state, float time)
    {
        var moutOpenedBlendShapeValue = clientHeadMesh.GetBlendShapeWeight(36);
        if (state == -1)
        {
            clientHeadMesh.SetBlendShapeWeight(36, 100 - time * 100);
            return;
        }

        clientHeadMesh.SetBlendShapeWeight(36,time * 100);
    }
}