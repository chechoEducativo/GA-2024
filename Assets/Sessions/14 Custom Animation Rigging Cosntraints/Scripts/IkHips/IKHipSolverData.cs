using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[Serializable]
public struct IKHipSolverData : IAnimationJobData
{
    public enum ReAdjustmentCondition
    {
        Single = 0,
        Many = 1,
        All = 2
    }
    
    public Animator anim;
    [SyncSceneToStream] public Transform hip;
    [SyncSceneToStream] public Transform root;
    public ReAdjustmentCondition reAdjustmentCondition;
    public TransformRaycaster[] feetRaycasters;
    public float minReadjustmentDistance;
    
    public bool IsValid()
    {
        return anim != null && hip != null && feetRaycasters != null && feetRaycasters.Length > 0 && root != null;
    }

    public void SetDefaultValues()
    {
        hip = null;
        feetRaycasters = null;
    }
}
