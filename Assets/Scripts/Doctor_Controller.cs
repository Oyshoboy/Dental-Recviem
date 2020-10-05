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

    public GameObject leftHand;
    public Vector3 leftHandDefaultPos;
    private Quaternion leftHandDefaultRot;
    public Transform LeftHandTargetPos;
    
    public GameObject head;
    public Vector3 headDefaultPos;
    public Quaternion headDefaultRot;
    public Transform headTargetPos;

    public GameObject patientHeadTarget;

    public GameObject PatientHeadTargetSurgery;

    public HeadFollowController headFollowController;

    public bool doctorIsFighting = false;
    public int speed = 5;
    public Transform doctorFightPosition;
    public Transform doctorSurgeryPositon;
    
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
                HeadMovementController(time);
            }
            else
            {
                HandsReturnController(time);
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
        rightHand.transform.localRotation = Quaternion.Lerp(rightHandDefaultRot, rightHandTargetPos.localRotation, time);
        
        leftHand.transform.localPosition = Vector3.Lerp(leftHandDefaultPos, LeftHandTargetPos.localPosition, time);
        leftHand.transform.localRotation = Quaternion.Lerp(leftHandDefaultRot, LeftHandTargetPos.localRotation, time);
    }

    void HandsReturnController(float time)
    {
        rightHand.transform.localPosition = Vector3.Lerp(rightHandTargetPos.localPosition, rightHandDefaultPos, time);
        rightHand.transform.localRotation = Quaternion.Lerp(rightHandTargetPos.localRotation, rightHandDefaultRot, time);
        
        leftHand.transform.localPosition = Vector3.Lerp(LeftHandTargetPos.localPosition, leftHandDefaultPos, time);
        leftHand.transform.localRotation = Quaternion.Lerp(LeftHandTargetPos.localRotation, leftHandDefaultRot, time);
    }

    public void Update()
    {
        if (doctorIsFighting)
        {
            PositionHandler(doctorFightPosition);
        }
        else
        {
            PositionHandler(doctorSurgeryPositon);
        }
    }

    private void PositionHandler(Transform target)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, speed * Time.deltaTime);
        transform.position = Vector3.Slerp(transform.position, target.position, speed * Time.deltaTime);
    }
}
