using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothPlateSpawner : MonoBehaviour
{
    public float radius = 1;
    public Vector2 regionSize = Vector2.one;
    public int rejectionSamples = 30;
    public float displayRadius = 1f; 
    // 0 FOR UNLIMITED
    
    private List<Vector2> points;

    public GameObject doctorsPlate;
    public GameObject[] toothesPool;

    public void SpawnToothOnPlate(int samplesAmount)
    {
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples, samplesAmount);

        int foreachIndex = 0;

        for (int i = 0; i < toothesPool.Length; i++)
        {
            toothesPool[i].SetActive(false);
        }
        
        if (points != null)
        {
            foreach (Vector2 point in points)
            {
                toothesPool[foreachIndex].transform.localPosition = new Vector3(doctorsPlate.transform.localPosition.x + point.x * 10, doctorsPlate.transform.localPosition.y + point.y * 10, -0.054f);
                toothesPool[foreachIndex].SetActive(true);
                foreachIndex++;
            }
        }
    }
}
