using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class FootIkGizmoDrawer
{
    [DrawGizmo(GizmoType.Active | GizmoType.NonSelected, typeof(FootIk_Aguapanela))]
    public static void DrawGizmos(Component component, GizmoType gizmoType)
    {
        FootIk_Aguapanela target = component as FootIk_Aguapanela;
        if (target == null) return;
        Gizmos.color = target.HasTarget ? Color.green : Color.red; // Elegir un color dependiendo si el script detecta o no una superficie
        Vector3 detectionStartPosition = target.GetDetectionStartPosition();
        Gizmos.DrawSphere(detectionStartPosition, 0.05f); //Esfera en punto inicial del rayo de deteccion
        Handles.Label(detectionStartPosition, "Punto de deteccion"); 
        Gizmos.DrawLine(detectionStartPosition, detectionStartPosition - target.DetectionReference.up * target.MaxDetectionDistance); //Linea que representa el rayo de deteccion de superficies
    }
}
