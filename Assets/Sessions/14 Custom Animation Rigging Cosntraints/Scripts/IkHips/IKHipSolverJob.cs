using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

[BurstCompile]
public struct IKHipSolverJob : IWeightedAnimationJob
{
    public ReadOnlyTransformHandle hip;
    public ReadWriteTransformHandle root;
    public NativeArray<Vector3> raycastHitbuffer;
    public NativeArray<float> floatVariableBuffer;
    public float minReadjustmentDistance;
    public int reAdjustmentCondition; 

    public void ProcessAnimation(AnimationStream stream)
    {
    }

    public void ProcessRootMotion(AnimationStream stream)
    {
        float yOffset = 0;
        for (int i = 0; i < raycastHitbuffer.Length; i++)
        {
            float absoluteOffset = Mathf.Abs(raycastHitbuffer[i].y);
            if(floatVariableBuffer[i] <= 0 || absoluteOffset > minReadjustmentDistance)
            {
                if(reAdjustmentCondition > 1)
                    break;
                Debug.Log("shouldn't break execution");
            }
            if (yOffset <= 0 || yOffset > 0 && absoluteOffset < yOffset)
            {
                yOffset = absoluteOffset;
            }
        }

        stream.velocity += Vector3.up * yOffset * 3;
    }

    public FloatProperty jobWeight { get; set; }
}
