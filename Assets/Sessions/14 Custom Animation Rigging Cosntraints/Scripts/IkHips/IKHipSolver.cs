using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Animations.Rigging;

[AddComponentMenu("Animation Rigging/Custom Constraints/IKHipSolver")]
public class IKHipSolver : RigConstraint<IKHipSolverJob, IKHipSolverData, IKHipSolverBinder>
{
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        try
        {
            Gizmos.color = Color.cyan;
            Handles.color = Color.cyan;
            Handles.DrawWireDisc(data.hip.position, data.hip.up, 0.3f);
            Gizmos.DrawLine(data.hip.position, data.hip.position - data.hip.up * data.minReadjustmentDistance);
        }
        catch
        {
            
        }
    }
    #endif
}