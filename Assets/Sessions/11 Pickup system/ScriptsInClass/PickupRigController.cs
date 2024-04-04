using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickupRigController : MonoBehaviour
{
    public struct QuadrantData
    {
        public Vector3 localDirection;
    }
    
    [SerializeField] private float maxReachingDistance;
    [SerializeField] private float perQuadrantAngle;
    [SerializeField] private Transform targetReference;

    public List<Transform> availableItems = new List<Transform>();
    
    private List<QuadrantData> quadrants = new List<QuadrantData>();

    private void UpdateQuadrantData()
    {
        quadrants = new List<QuadrantData>()
        {
            new QuadrantData
            {
                //UpperLeft
                localDirection = Quaternion.Euler(-perQuadrantAngle * 0.5f, -perQuadrantAngle * 0.5f, 0) * Vector3.forward * maxReachingDistance
            },
            new QuadrantData
            {
                //UpperRight
                localDirection = Quaternion.Euler(-perQuadrantAngle * 0.5f, perQuadrantAngle * 0.5f, 0) * Vector3.forward * maxReachingDistance
            },
            new QuadrantData
            {
                //LowerRight
                localDirection = Quaternion.Euler(perQuadrantAngle * 0.5f, perQuadrantAngle * 0.5f, 0) * Vector3.forward * maxReachingDistance
            },
            new QuadrantData
            {
                //LowerLeft
                localDirection = Quaternion.Euler(perQuadrantAngle * 0.5f, -perQuadrantAngle * 0.5f, 0) * Vector3.forward * maxReachingDistance
            }
        };
    }

    public void PickUpNearestObject()
    {
        Transform nearestItem = availableItems.OrderBy(item =>
        {
            Vector3 itemDir = targetReference.position - item.position;
            float sqrMagnitude = Vector3.SqrMagnitude(itemDir);
            float dot = Mathf.Abs(Vector3.Dot(itemDir.normalized, TargetReference.forward));
            return sqrMagnitude * dot;
        }).FirstOrDefault();

        if (nearestItem == default) return;
        
        // 
    }
    
    private void OnValidate()
    {
        UpdateQuadrantData();
    }


    public Transform TargetReference => targetReference == null ? transform : targetReference;
    public float MaxReachingDistance => maxReachingDistance;

    public List<QuadrantData> Quadrants => quadrants;
}
