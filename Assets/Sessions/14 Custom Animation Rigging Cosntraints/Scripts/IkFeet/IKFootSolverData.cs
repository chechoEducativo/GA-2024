using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[Serializable]
public struct IKFootSolverData : IAnimationJobData
{
    [SyncSceneToStream] public Transform foot;
    public Vector3 footOffset;
    [SyncSceneToStream] public Transform footIkTarget;
    public TransformRaycaster raycaster;
    public Animator anim;
    public string footGroundedParameter;

    public bool IsValid()
    {
        return foot != null || footIkTarget != null;
    }
    
    public void SetDefaultValues()
    {
        foot = foot = null;
    }
}
