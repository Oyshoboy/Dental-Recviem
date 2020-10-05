using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchController : MonoBehaviour
{
    public GameObject hourArrow;

    public GameObject minutesArrow;

    public float timeModifier = 50;

    public float ourDelayFactor = 100f;
    
    void Update()
    {
        var timeFlow = Time.deltaTime * timeModifier;

        var hourArrowLocalEulers = hourArrow.transform.localEulerAngles;
        hourArrow.transform.localEulerAngles = new Vector3(hourArrowLocalEulers.x, hourArrowLocalEulers.y,hourArrowLocalEulers.z + (timeFlow / ourDelayFactor));

        var minutesArrowLocalEulers = minutesArrow.transform.localEulerAngles;
        minutesArrow.transform.localEulerAngles = new Vector3(minutesArrowLocalEulers.x, minutesArrowLocalEulers.y, minutesArrowLocalEulers.z + timeFlow);
    }
}