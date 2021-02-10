using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Easing_move : MonoBehaviour
{
    [SerializeField] Transform origin;
    [SerializeField] Transform destination;
    [SerializeField] float time;
    [SerializeField] AnimationCurve curve;
    [SerializeField] Rigidbody rb;

    float timespeed = 1f;
    float currentTime;

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTime += Time.deltaTime * timespeed;
        if (currentTime > time)
        {
            timespeed = -1f;
        }
        else if(currentTime < 0)
        {
            timespeed = 1;  
        }

        float progress = currentTime / time;
        float curvedProgress = curve.Evaluate(progress);
        Vector3 desiredPosition = Vector3.Lerp(origin.position, destination.position, curvedProgress);

        rb.velocity = (desiredPosition - rb.position) / Time.fixedDeltaTime;
    }
}
