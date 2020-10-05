using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformExtensions
{
    public static void DebugDrawRect(this RectTransform rect)
    {
        Vector3[] v = new Vector3[4];
        rect.GetWorldCorners(v);
        Debug.DrawLine(v[0], v[1], Color.green);
        Debug.DrawLine(v[1], v[2], Color.blue);
        Debug.DrawLine(v[2], v[3], Color.red);
        Debug.DrawLine(v[3], v[0], Color.yellow);
    }
}
