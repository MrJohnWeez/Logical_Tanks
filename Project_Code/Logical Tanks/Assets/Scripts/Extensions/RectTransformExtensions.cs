using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformExtensions
{
    /// <summary>
    /// Debug draw the outline of a given RectTransform
    /// </summary>
    /// <param name="rect">RectTransform to draw</param>
    public static void DebugDrawRect(this RectTransform rect)
    {
        Vector3[] v = new Vector3[4];
        rect.GetWorldCorners(v);
        Debug.DrawLine(v[0], v[1], Color.green);
        Debug.DrawLine(v[1], v[2], Color.blue);
        Debug.DrawLine(v[2], v[3], Color.red);
        Debug.DrawLine(v[3], v[0], Color.yellow);
    }

    /// <summary>
    /// Determine if RectTransform contains another rect within it
    /// </summary>
    /// <param name="thisRect">RectTransform to define bounds</param>
    /// <param name="other">RectTransform that will be checked if within bounds</param>
    /// <returns>True if other RectTransform is in RectTransform</returns>
    public static bool Contains(this RectTransform thisRect, RectTransform other)
    {
        Vector3[] currRect = new Vector3[4];
        thisRect.GetWorldCorners(currRect);
        Vector3[] otherRect = new Vector3[4];
        other.GetWorldCorners(otherRect);
        return otherRect[0].x >= currRect[0].x && otherRect[0].y >= currRect[0].y && otherRect[2].x <= currRect[2].x && otherRect[2].y <= currRect[2].y;
    }

    /// <summary>
    /// Convert RectTransform to worldspace rect
    /// </summary>
    /// <param name="thisRect">RectTransform to transform</param>
    /// <returns>Worldspace Rect</returns>
    public static Rect GetWorldSapceRect(this RectTransform thisRect)
    {
        var r = thisRect.rect;
        r.center = thisRect.TransformPoint(r.center);
        r.size = thisRect.TransformVector(r.size);
        return r;
    }
}
