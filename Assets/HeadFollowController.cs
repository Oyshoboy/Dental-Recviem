using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadFollowController : MonoBehaviour
{
    public GameObject targetObj;
    public int speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetObj.transform.position - transform.position);
     
        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    }
}
