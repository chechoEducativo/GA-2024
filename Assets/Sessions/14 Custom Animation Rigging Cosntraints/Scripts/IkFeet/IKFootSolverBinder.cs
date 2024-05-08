using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKFootSolverBinder : AnimationJobBinder<IKFootSolverJob, IKFootSolverData>
{
    public override IKFootSolverJob Create(Animator animator, ref IKFootSolverData data, Component component)
    {
        IKFootSolverJob job = new IKFootSolverJob();
        
        job.foot = ReadOnlyTransformHandle.Bind(animator, data.foot);
        job.footIkTarget = ReadWriteTransformHandle.Bind(animator, data.footIkTarget);
        job.raycastHitsBuffer = new NativeArray<Vector3>(6, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        job.animatedData = new NativeArray<float>(2, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        return job;
    }

    public override void Destroy(IKFootSolverJob job)
    {
        job.raycastHitsBuffer.Dispose();
        job.animatedData.Dispose();
    }

    public override void Update(IKFootSolverJob job, ref IKFootSolverData data)
    {
        base.Update(job, ref data);
        job.raycastHitsBuffer[0] = data.raycaster.Hit.point;
        job.raycastHitsBuffer[1] = data.raycaster.Hit.normal;
        job.raycastHitsBuffer[2] = data.footOffset;
        job.animatedData[0] = data.anim.GetFloat(data.footGroundedParameter);
        job.animatedData[1] = data.raycaster.HasTarget ? 1 : 0;
    }
}
