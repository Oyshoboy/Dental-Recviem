using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DragAndDropBehaviour : MonoBehaviour
{
    public bool IsDragged { get; private set; }

    private float _risingStep;
    public Camera _mainCamera;
    private Rigidbody _rigidbody;

    public float yDraggingHeight = 2.1f;
    public float risingSpeed = 0.2f;

    public Transform defaultParent;
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    public GameLogicController gameLogicController;

    private void Awake()
    {
        _rigidbody = transform.GetComponent<Rigidbody>();
        defaultParent = gameObject.transform.parent;
        defaultPosition = transform.localPosition;
        defaultRotation = transform.localRotation;
    }

    private void OnMouseDown()
    {
        IsDragged = true;
    }

    private void OnMouseDrag()
    {
        gameObject.transform.parent = null;
        
        if (_rigidbody.isKinematic == false)
        {
            TurnOnKinematic();
        }
        
        var newPosition = GetPositionFromCamera();
        MoveInterpolate(newPosition);
    }

    private void OnMouseUp()
    {
        ToothFallBack();
    }

    void ToothFallBack()
    {
        ResetStep();

        IsDragged = false;
        transform.parent = defaultParent;
        transform.localRotation = defaultRotation;
        transform.localPosition = defaultPosition; 
    }

    private void TurnOnKinematic()
    {
        _rigidbody.isKinematic = true;
    }

    private void TurnOffKinematic()
    {
        _rigidbody.isKinematic = false;
    }

    private Vector3 GetPositionFromCamera()
    {
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.green);
        var newWorldPosition = ray.GetPoint(yDraggingHeight);
        return newWorldPosition;
    }

    private void MoveInterpolate(Vector3 position)
    {
        transform.position = Vector3.Lerp(transform.position, position, _risingStep);
        IncreaseStep();
    }

    private void OnTriggerEnter(Collider other)
    {
        ToothFallBack();
        gameObject.SetActive(false);
        gameLogicController.GrowOneTooth();
    }

    private void IncreaseStep()
    {
        _risingStep += risingSpeed * Time.deltaTime;
    }

    private void ResetStep()
    {
        _risingStep = 0;
    }
}