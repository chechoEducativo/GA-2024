using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

[BurstCompile]
public struct IKFootSolverJob : IWeightedAnimationJob
{
    public ReadOnlyTransformHandle foot;
    public ReadWriteTransformHandle footIkTarget;

    public NativeArray<Vector3> raycastHitsBuffer;
    public NativeArray<float> animatedData;

    public void ProcessAnimation(AnimationStream stream)
    {
        float w = jobWeight.Get(stream);
        if (w <= 0f) return;
        Vector3 originalFootPos = foot.GetPosition(stream);
        Vector3 solvedFootPos = originalFootPos +
                                raycastHitsBuffer[2] + Vector3.up * (raycastHitsBuffer[0].y-originalFootPos.y);
        Quaternion footRot = footIkTarget.GetRotation(stream);

        footIkTarget.SetPosition(stream, Vector3.Lerp(foot.GetPosition(stream),solvedFootPos, animatedData[0] * animatedData[1]));

        Vector3 rotAxis = Vector3.Cross(Vector3.up, raycastHitsBuffer[1]);

        float angle = Vector3.Angle(Vector3.up, raycastHitsBuffer[1]);
        Quaternion rot = Quaternion.AngleAxis(angle, rotAxis);

        footIkTarget.SetRotation(stream,
            Quaternion.Slerp(foot.GetRotation(stream), footRot * rot, animatedData[0] * animatedData[1]));
    }

    public void ProcessRootMotion(AnimationStream stream)
    {
    }

    public FloatProperty jobWeight { get; set; }
}
