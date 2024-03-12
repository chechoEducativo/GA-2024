using System;
using System.Linq;
using UnityEngine;

public class HumanoidFootIkSolver : MonoBehaviour
{
    public struct SnapTargetData
    {
        public RaycastHit hit;
        public float offsetThreshold; //0 = no offset, 1 = offset

        public SnapTargetData(RaycastHit hit, float offsetThreshold)
        {
            this.hit = hit;
            this.offsetThreshold = offsetThreshold;
        }
    }
    
    [SerializeField] private AvatarIKGoal ikGoal;
    [SerializeField] private AvatarIKHint ikHint;
    [SerializeField] private Transform detectionReference;
    [SerializeField] private Transform foot;
    [SerializeField] private Transform knee;
    [SerializeField][Range(0,1)] private float detectionStart;
    [SerializeField] private float maxDetectionDistance;
    [SerializeField] private float surfaceOffset;
    [SerializeField] [Range(0, 180)] private float snapSlope;
    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private float hintRotationOffset;

    [SerializeField] private float hipsOffset;
    [SerializeField] private Transform hipBone;

    private bool hasSnapTarget;
    private SnapTargetData snapTarget;
    private Vector3 smoothSnapPosition;
    private Vector3 smoothSnapNormal;
    private Animator anim;
    

    public Vector3 GetDetectionStartPoint()
    {
        Vector3 referenceSpacePosition = detectionReference.InverseTransformPoint(foot.position);
        Vector3 startPoint = new Vector3(referenceSpacePosition.x, Mathf.Lerp(0, referenceSpacePosition.y, detectionStart), referenceSpacePosition.z);
        return detectionReference.TransformPoint(startPoint);
    }

    private RaycastHit[] GetSuitableSurfaces()
    {
        return Physics.RaycastAll(GetDetectionStartPoint(), -detectionReference.up * maxDetectionDistance);
    }

    private bool GetNearestSurfacePoint(RaycastHit[] hits, out SnapTargetData point)
    {
        try
        {
            Vector3 detectionStartPoint = GetDetectionStartPoint();
            RaycastHit hit = hits.OrderBy(hit => Vector3.Distance(hit.point, detectionReference.position)).First();
            float snapThreshold = (Vector3.Distance(hit.point, detectionStartPoint) /
                                  Vector3.Distance(detectionStartPoint, foot.position));
            point = new SnapTargetData(hit, snapThreshold);
            return true;
        }
        catch
        {
            point = new SnapTargetData(new RaycastHit
            {
                point = foot.position,
                normal = detectionReference.up
            }, 0);
            return false;
        }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        hasSnapTarget = GetNearestSurfacePoint(GetSuitableSurfaces(), out snapTarget);
        smoothSnapPosition = Vector3.Lerp(smoothSnapPosition, snapTarget.hit.point, Time.deltaTime * 30);
        smoothSnapNormal = Vector3.Slerp(smoothSnapNormal, snapTarget.hit.normal, Time.deltaTime * 30);
        anim.SetIKPositionWeight(ikGoal,1);
        anim.SetIKRotationWeight(ikGoal,1);
        anim.SetIKPosition(ikGoal, smoothSnapPosition + detectionReference.up * surfaceOffset);
        Quaternion targetRotation = Quaternion.LookRotation(smoothSnapNormal) * Quaternion.Euler(rotationOffset);
        anim.SetIKRotation(ikGoal, targetRotation);

        Vector3 rotatedHintPosition = Quaternion.AngleAxis(Mathf.Lerp(0, hintRotationOffset, snapTarget.offsetThreshold),
            detectionReference.position - foot.position) * knee.position;
        Vector3 targetHintPosition = Vector3.Lerp(knee.position,rotatedHintPosition, snapTarget.offsetThreshold);
    }

    private void OnAnimatorMove()
    {
    }

    private void LateUpdate()
    {
        if (hipBone == null) return;
        hipBone.position += detectionReference.up * hipsOffset;
    }

    public Transform DetectionReference => detectionReference;
    public float MaxDetectionDistance => maxDetectionDistance;

    public bool HasSnapTarget => hasSnapTarget;

    public SnapTargetData SnapTarget => snapTarget;

    public float SurfaceOffset => surfaceOffset;

    public Transform Foot => foot;
}
