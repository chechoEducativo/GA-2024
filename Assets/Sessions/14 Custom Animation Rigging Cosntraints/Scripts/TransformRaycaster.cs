using UnityEditor;
using UnityEngine;

public class TransformRaycaster : MonoBehaviour
{
    [SerializeField] private Transform originReference;
    [SerializeField] private Vector3 originOffset;
    [SerializeField] private Vector3 rayDir;
    [SerializeField] private LayerMask detectionMask;

    private bool hasTarget;
    private RaycastHit hit;

    public RaycastHit Hit => hit;

    public bool HasTarget => hasTarget;

    private Vector3 GetRayOrigin()
    {
        Vector3 referenceSpacePosition = originReference.InverseTransformPoint(transform.position);
        return originReference.TransformPoint(Vector3.Scale(referenceSpacePosition, new Vector3(1,0,1))) + originOffset;
    }

    private Vector3 GetRayDirection()
    {
        return originReference.TransformDirection(rayDir);
    }

    private void FixedUpdate()
    {
        Ray r = new Ray(GetRayOrigin(), GetRayDirection().normalized);
        hasTarget = Physics.Raycast(r, out hit, rayDir.magnitude, detectionMask);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Color col = hasTarget ? Color.green : Color.red;
        Gizmos.color = col;
        Handles.color = col;
        Vector3 rayOrigin = GetRayOrigin();
        Handles.DrawSolidDisc(rayOrigin, -GetRayDirection(), 0.1f);
        Gizmos.DrawLine(rayOrigin, rayOrigin + GetRayDirection());
        Gizmos.DrawWireSphere(hit.point, 0.05f);
        Handles.Label(hit.point, $"Pos: {hit.point}");
    }
#endif
}
