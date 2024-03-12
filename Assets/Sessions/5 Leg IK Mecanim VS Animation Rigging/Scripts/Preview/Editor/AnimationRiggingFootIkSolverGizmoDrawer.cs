using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class AnimationRiggingFootIkSolverGizmoDrawer
{
    [DrawGizmo(GizmoType.Active | GizmoType.NonSelected,typeof(HumanoidFootIkSolver))]
    public static void DrawGizmos(Component component, GizmoType gizmoType)
    {
        HumanoidFootIkSolver solver = component as HumanoidFootIkSolver;
        if (solver == null) return;
        Vector3 detectionStartPoint = solver.GetDetectionStartPoint();
        Vector3 detectionEndPoint = detectionStartPoint - solver.DetectionReference.up * solver.MaxDetectionDistance;
        Gizmos.color = solver.HasSnapTarget ? Color.green : Color.red;
        Handles.color = Gizmos.color;
        Handles.DrawWireDisc(detectionStartPoint,solver.DetectionReference.up, 0.05f);
        Handles.DrawWireDisc(detectionEndPoint,solver.DetectionReference.up, 0.05f);
        Gizmos.DrawLine(detectionStartPoint, detectionEndPoint);
        Gizmos.DrawSphere(solver.SnapTarget.hit.point, 0.05f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(solver.Foot.position, solver.Foot.position - solver.DetectionReference.up * solver.SurfaceOffset);
    }
}
