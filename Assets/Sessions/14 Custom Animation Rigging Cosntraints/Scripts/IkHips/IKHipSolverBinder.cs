using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKHipSolverBinder : AnimationJobBinder<IKHipSolverJob, IKHipSolverData>
{
    public override IKHipSolverJob Create(Animator animator, ref IKHipSolverData data, Component component)
    {
        IKHipSolverJob job = new IKHipSolverJob();
        job.hip = ReadOnlyTransformHandle.Bind(animator, data.hip);
        job.root = ReadWriteTransformHandle.Bind(animator, data.root);
        job.raycastHitbuffer = new NativeArray<Vector3>(data.feetRaycasters.Length, Allocator.Persistent,
            NativeArrayOptions.UninitializedMemory);
        job.floatVariableBuffer = new NativeArray<float>(data.feetRaycasters.Length, Allocator.Persistent,
            NativeArrayOptions.UninitializedMemory);
        job.minReadjustmentDistance = data.minReadjustmentDistance;
        job.reAdjustmentCondition = (int)data.reAdjustmentCondition;
        return job;
    }

    public override void Destroy(IKHipSolverJob job)
    {
        job.raycastHitbuffer.Dispose();
    }

    public override void Update(IKHipSolverJob job, ref IKHipSolverData data)
    {
        base.Update(job, ref data);
        for (int i = 0; i < data.feetRaycasters.Length; i++)
        {
            job.raycastHitbuffer[i] = data.hip.InverseTransformPoint(data.feetRaycasters[i].Hit.point);
            job.floatVariableBuffer[i] = data.feetRaycasters[i].HasTarget ? 1 : 0;
        }
    }
}
