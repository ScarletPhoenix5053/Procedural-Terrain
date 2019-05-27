using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloMarchedCube : MonoBehaviour
{
#pragma warning disable 0414
    private const float minGroundLevel = -1;
    private const float maxGroundLevel = 1;
    [SerializeField]
    [Range(minGroundLevel, maxGroundLevel)]
    private float groundLevel = 0;

    private CubeField soloCubeField;
#pragma warning restore 0414

    private void OnValidate()
    {
        if (soloCubeField != null)
        {
            soloCubeField.GroundLevel = groundLevel;
        }
    }

    private void OnDrawGizmos()
    {
        if (soloCubeField == null) soloCubeField = new CubeField();
        GizmoRenderer.DrawCubeField(soloCubeField);
    }


}
