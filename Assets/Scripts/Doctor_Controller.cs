using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.Demos;
using UnityEngine;

public class Doctor_Controller : MonoBehaviour
{
    public GameObject rightHand;
    public Vector3 rightHandDefaultPos;
    private Quaternion rightHandDefaultRot;
    public Transform rightHandTargetPos;
    public GameObject rightHandCollider;
    public Transform rightHandHitPos;
    public bool rightHandCycle = false;

    public GameObject leftHand;
    public Vector3 leftHandDefaultPos;
    private Quaternion leftHandDefaultRot;
    public Transform LeftHandTargetPos;
    public GameObject leftHandCollider;
    public Transform leftHandHitPos;
    public bool leftHandCycle = false;

    public bool handsHitCycle = false;

    public GameObject head;
    public Vector3 headDefaultPos;
    public Quaternion headDefaultRot;
    public Transform headDefault;
    public Transform headTargetPos;

    public GameObject patientHeadTarget;

    public GameObject PatientHeadTargetSurgery;

    public HeadFollowController headFollowController;

    public bool doctorIsFighting = false;
    public int speed = 5;
    public Transform doctorFightPosition;
    public Transform doctorSurgeryPositon;
    public SkinnedMeshRenderer playaHead;
    public bool punchedOnce = false;
    public ParticleSystem bloodParticles;
    
    
    public GameLogicController gameLogicController;
    private void Start()
    {
        InitDefaultPost();
    }

    void InitDefaultPost()
    {
        rightHandDefaultPos = rightHand.transform.localPosition;
        rightHandDefaultRot = rightHand.transform.localRotation;

        leftHandDefaultPos = leftHand.transform.localPosition;
        leftHandDefaultRot = leftHand.transform.localRotation;

        headDefaultPos = head.transform.localPosition;
        headDefaultRot = head.transform.localRotation;
    }

    public void CallFallbackCoroutine(bool forward)
    {
        StartCoroutine(StartMovingHands(false));
    }

    public IEnumerator StartMovingHands(bool forward)
    {
        var time = 0f;
        while (time < 1)
        {
            time += Time.deltaTime;
            if (forward)
            {
                headFollowController.targetObj = PatientHeadTargetSurgery;
                HandsMovementController(time);
                //HeadState = 1;
                HeadMovementController(time);
            }
            else
            {
                HandsReturnController(time);
                //HeadState = 0;
                HeadReturnController(time);
                headFollowController.targetObj = patientHeadTarget;
            }

            yield return null;
        }
    }

    void HeadMovementController(float time)
    {
        head.transform.localPosition = Vector3.Lerp(headDefaultPos, headTargetPos.localPosition, time);
        head.transform.localRotation = Quaternion.Lerp(headDefaultRot, headTargetPos.localRotation, time);
    }

    void HeadReturnController(float time)
    {
        head.transform.localPosition = Vector3.Lerp(headTargetPos.localPosition, headDefaultPos, time);
        head.transform.localRotation = Quaternion.Lerp(headTargetPos.localRotation, headDefaultRot, time);
    }

    void HandsMovementController(float time)
    {
        rightHand.transform.localPosition = Vector3.Lerp(rightHandDefaultPos, rightHandTargetPos.localPosition, time);
        rightHand.transform.localRotation =
            Quaternion.Lerp(rightHandDefaultRot, rightHandTargetPos.localRotation, time);

        leftHand.transform.localPosition = Vector3.Lerp(leftHandDefaultPos, LeftHandTargetPos.localPosition, time);
        leftHand.transform.localRotation = Quaternion.Lerp(leftHandDefaultRot, LeftHandTargetPos.localRotation, time);
    }

    void HandsReturnController(float time)
    {
        rightHand.transform.localPosition = Vector3.Lerp(rightHandTargetPos.localPosition, rightHandDefaultPos, time);
        rightHand.transform.localRotation =
            Quaternion.Lerp(rightHandTargetPos.localRotation, rightHandDefaultRot, time);

        leftHand.transform.localPosition = Vector3.Lerp(LeftHandTargetPos.localPosition, leftHandDefaultPos, time);
        leftHand.transform.localRotation = Quaternion.Lerp(LeftHandTargetPos.localRotation, leftHandDefaultRot, time);
    }

    void OpenMouthWhenPunched()
    {
        playaHead.SetBlendShapeWeight(34, 100); // MOUTH WITHD
        playaHead.SetBlendShapeWeight(36, 100);
        playaHead.SetBlendShapeWeight(43, 100);
        playaHead.SetBlendShapeWeight(44, 100);
        playaHead.SetBlendShapeWeight(46, 100);
    }

    public void Update()
    {
        if (doctorIsFighting)
        {
            PositionHandler(doctorFightPosition);
            
            if (Input.GetKeyDown("space"))
            {
                if (!handsHitCycle && !rightHandCycle)
                {
                    if (!punchedOnce)
                    {
                        punchedOnce = true;
                        OpenMouthWhenPunched();
                    }
                    StopAllCoroutines();
                    handsHitCycle = true;
                    rightHandCycle = true;
                    
                    leftHandCycle = false;
                    gameLogicController.RemoveOneTooth();
                   // Debug.Log("Right Hit");
                    rightHandCollider.SetActive(true);
                    StartCoroutine(RightHook(12));
                } else if (handsHitCycle && !leftHandCycle)
                {
                    StopAllCoroutines();
                    handsHitCycle = false;
                    leftHandCycle = true;

                    rightHandCycle = false;
                    gameLogicController.RemoveOneTooth();
                  //  Debug.Log("Left Hit");
                    leftHandCollider.SetActive(true);
                    StartCoroutine(LeftHook(12));
                }
            }
        }
        else
        {
            PositionHandler(doctorSurgeryPositon);
        }
        
        //HeadPositionHandler();
    }
    
    public IEnumerator RightHook(float strikeSpeed)
    {
        var time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * strikeSpeed;
            RightHandHookControl(time, rightHandHitPos.localPosition, rightHandHitPos.localRotation);

            yield return null;
        }
        
        StopAllCoroutines();
        bloodParticles.Play();
        StartCoroutine(RightHookReturn(12));
    }

    public IEnumerator RightHookReturn( float strikeSpeed)
    {
        var time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * strikeSpeed;
            RightHandHookControl(time, rightHandDefaultPos, rightHandDefaultRot);

            yield return null;
        }
        
        StopAllCoroutines();
        rightHandCollider.SetActive(false);
    }
    
    
    
    public IEnumerator LeftHook(float strikeSpeed)
    {
        var time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * strikeSpeed;
            LeftHandHookControl(time, leftHandHitPos.localPosition, leftHandHitPos.localRotation);

            yield return null;
        }
        bloodParticles.Play();
        StopAllCoroutines();
        StartCoroutine(LeftHookReturn(12));
    }
    
    public IEnumerator LeftHookReturn( float strikeSpeed)
    {
        var time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * strikeSpeed;
            LeftHandHookControl(time, leftHandDefaultPos, leftHandDefaultRot);

            yield return null;
        }
        
        StopAllCoroutines();
        leftHandCollider.SetActive(false);
        //Debug.Log("LEFT HOOK COMPLETE!");
    }
    
    void LeftHandHookControl(float time, Vector3 goalPos, Quaternion goalRot)
    {
        leftHand.transform.localPosition = Vector3.Lerp(leftHandDefaultPos, goalPos, time);
        leftHand.transform.localRotation = Quaternion.Lerp(leftHandDefaultRot, goalRot, time);
    }

    void RightHandHookControl(float time, Vector3 goalPos, Quaternion goalRot)
    {
        rightHand.transform.localPosition = Vector3.Lerp(rightHandDefaultPos, goalPos, time);
        rightHand.transform.localRotation = Quaternion.Lerp(rightHandDefaultRot, goalRot, time);
    }

    private void PositionHandler(Transform target)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, speed * Time.deltaTime);
        transform.position = Vector3.Slerp(transform.position, target.position, speed * Time.deltaTime);
    }

    /*
    private void HeadPositionHandler()
    {
        var target = headDefault;
        if (HeadState == 1)
        {
            target = headTargetPos;
        }
        head.transform.localPosition = Vector3.Lerp(head.transform.localPosition, target.localPosition, speed * Time.deltaTime);
        head.transform.localRotation = Quaternion.Lerp(head.transform.localRotation, target.localRotation, speed * Time.deltaTime);
    }
    */
}
