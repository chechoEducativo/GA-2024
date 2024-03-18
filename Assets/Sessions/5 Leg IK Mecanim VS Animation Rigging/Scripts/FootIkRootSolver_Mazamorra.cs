using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FootIkRootSolver_Mazamorra : MonoBehaviour
{
    [SerializeField] private Transform characterRoot;
    [SerializeField] private float readjustmentThreshold;

    private List<float> heightOffsets = new List<float>();

    private Vector3 rootTarget;
    private void OnAnimatorMove()
    {
        float minimumOffset = heightOffsets.Min();
        if (minimumOffset > readjustmentThreshold)
        {
            rootTarget = characterRoot.position + characterRoot.up * minimumOffset;
        }
        else
        {
            rootTarget = characterRoot.position;
        }
    }

    public void UpdateTargetOffset(float heightValue)
    {
        heightOffsets.Add(heightValue);
    }

    public Vector3 RootTarget => rootTarget;
}
