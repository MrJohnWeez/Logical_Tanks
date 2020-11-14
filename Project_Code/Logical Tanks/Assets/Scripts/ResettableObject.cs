using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResettableObject : MonoBehaviour
{
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Vector3 _startScale;

    protected virtual void Start()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        _startScale = transform.localScale;
    }

    public virtual void ResetObject()
    {
        transform.position = _startPosition;
        transform.rotation = _startRotation;
        transform.localScale = _startScale;
    }
}
