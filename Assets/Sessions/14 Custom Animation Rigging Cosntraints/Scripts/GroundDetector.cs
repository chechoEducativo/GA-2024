using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private TransformRaycaster[] feetGroundCheckers;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;
    [SerializeField] [Range(0, 15)] private int airborneFrameCount;

    private bool grounded;
    private int airborneFrameCounter;
    
    private void FixedUpdate()
    {
        bool g = true;
        foreach (TransformRaycaster transformRaycaster in feetGroundCheckers)
        {
            if (!transformRaycaster.HasTarget)
            {
                g = false;
                break;
            }
        }
        if (grounded)
        {
            if (!g)
                airborneFrameCounter++;
            if (airborneFrameCounter > airborneFrameCount)
            {
                grounded = false;
                airborneFrameCounter = 0;
                rb.isKinematic = false;
                anim.SetBool("Airborne", true);
            }
        }
        else
        {
            if (g)
            {
                grounded = true;
                airborneFrameCounter = 0;
                rb.isKinematic = true;
                anim.SetBool("Airborne", false);
            }
        }
        Debug.Log(g);
    }
}
