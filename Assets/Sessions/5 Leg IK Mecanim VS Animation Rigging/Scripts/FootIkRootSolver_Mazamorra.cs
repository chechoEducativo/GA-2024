using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FootIkRootSolver_Mazamorra : MonoBehaviour
{
    [SerializeField] private Transform characterRoot;
    [SerializeField] private float readjustmentThreshold;
    [SerializeField] private float readjustmentSpeed = 15.0f;
    [SerializeField] private Rigidbody rigidBody;

    private List<float> heightOffsets = new List<float>();

    private Vector3 rootTarget;
    private Vector3 currentRootPosition;
    private void OnAnimatorMove()
    {
        if (heightOffsets.Count >= 2)
        {
            float minimumOffset = Mathf.Min(heightOffsets[0], heightOffsets[1]);
            if (minimumOffset > readjustmentThreshold)
            {
                rootTarget = characterRoot.TransformPoint(new Vector3(0, minimumOffset, 0));
                rigidBody.isKinematic = true;
            }
            else
            {
                rigidBody.isKinematic = false;
                rootTarget = characterRoot.position;
            }
        }
        else
        {
            rigidBody.isKinematic = false;
            rootTarget = characterRoot.position;
        }

        currentRootPosition = Vector3.Lerp(currentRootPosition, rootTarget, Time.deltaTime * readjustmentSpeed);
        characterRoot.position = currentRootPosition;
        heightOffsets.Clear();
    }

    public void UpdateTargetOffset(float heightValue)
    {
        heightOffsets.Add(heightValue);
    }

    public Vector3 RootTarget => rootTarget;
}
