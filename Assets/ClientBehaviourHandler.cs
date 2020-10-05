using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines.Primitives;
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

    private void Start()
    {
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
        
        clientHeadMesh.SetBlendShapeWeight(36, Mathf.Lerp(moutOpenedBlendShapeValue, mouthOpenGoal, speed * Time.deltaTime));
    }
}
