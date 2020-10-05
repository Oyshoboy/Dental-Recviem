using System;
using System.Collections;
using System.Collections.Generic;
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


    public GameObject creditCard;
    public FullBodyBipedIK playaFullBodyIk;
    public Doctor_Controller doctorController;

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


    public IEnumerator StartMovingHand(bool forward)
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
            StartCoroutine(StartMovingHandBack());
        }
    }

    public IEnumerator StartMovingHandBack()
    {
        yield return new WaitForSeconds(1f);
        creditCard.SetActive(false);

        var time = 0f;
        while (time < 1)
        {
            time += Time.deltaTime;
            HandController(1f - time);
            yield return null;
        }
        
        StopAllCoroutines();
        StartCoroutine(CardDeclindeMessage());
    }


    IEnumerator CardDeclindeMessage()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("CARD DECLINED");
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
            t += Time.deltaTime / timeToMove;
            LerpController(state, t);
            yield return null;
        }

        state = Random.Range(0, 1) - 1;
        if (!isTalking) state = -1;
        StartCoroutine(MouthMovements(state, mouthMoveSpeed));
    }

    void LerpController(int state, float time)
    {
        var moutOpenedBlendShapeValue = clientHeadMesh.GetBlendShapeWeight(36);
        if (state == -1)
        {
            clientHeadMesh.SetBlendShapeWeight(36, Mathf.Lerp(moutOpenedBlendShapeValue, 0, speed * Time.deltaTime));
            return;
        }

        clientHeadMesh.SetBlendShapeWeight(36,
            Mathf.Lerp(moutOpenedBlendShapeValue, mouthOpenGoal, speed * Time.deltaTime));
    }
}