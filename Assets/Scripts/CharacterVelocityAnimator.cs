using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVelocityAnimator : MonoBehaviour
{

    #region Animator Properties
    private int _xMotionHash = Animator.StringToHash("x_motion");
    private int _yMotionHash   = Animator.StringToHash("y_motion");
    public Animator playerAnimator;
    #endregion
    
    // AVERAGE VELOCITY
    private Vector3 _velocity;
    public Transform velocityRefference;                            // object to track ( might be head if its VR, and Character body if it's PC)
    public GameObject characterBody;
    private Vector3 _lastVelocityRefferencePosition;                // prevous velocity refference position
    private readonly Vector3[] _oldPositions = new Vector3[30];
    private int _oldPositionsIteration;
    
    private void Update()
    {
        VelocityProviderByOldPosition();
    }


    void VelocityProviderByOldPosition()
    {
        _oldPositionsIteration++;                                                    // FILL ARRAY OF OLD POSITIONS
        _oldPositionsIteration %= _oldPositions.Length;
        _oldPositions[_oldPositionsIteration] = (velocityRefference.position - _lastVelocityRefferencePosition) / Time.deltaTime;

        Vector3 averageOldPosition = Vector3.zero;
        for (int i = 0; i < _oldPositions.Length; i++)
        {
            averageOldPosition += _oldPositions[i];
        }

        averageOldPosition /= _oldPositions.Length;
        _velocity = Vector3.Lerp(_velocity, averageOldPosition, 0.5f);

        AnimatorParamsSetuper();

        _lastVelocityRefferencePosition = velocityRefference.position;                        // SAVE OLD POSITION
    }

    void AnimatorParamsSetuper()
    {
        Vector3 relativeVelocity = _velocity;
        relativeVelocity = characterBody.transform.InverseTransformDirection(relativeVelocity);
        playerAnimator.SetFloat(_xMotionHash, relativeVelocity.x);
        playerAnimator.SetFloat(_yMotionHash, relativeVelocity.z);
    }
    
}
