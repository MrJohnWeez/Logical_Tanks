using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class ColoredObject : VariableCycledObject
{
    [Header("ColoredObject")]
    protected Rigidbody rigidBody = null;
    protected BoxCollider boxCollider = null;
    [SerializeField] private GameObject _coloredModel = null;
    private ColorID _colorID = ColorID.Green;

    public ColorID GetColorID => _colorID;

    protected override void Awake()
    {
        base.Awake();
        rigidBody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        if (_coloredModel)
        {
            Material[] materials = _coloredModel.GetComponent<Renderer>().materials;
            foreach (Material mat in materials)
            {
                _colorID = mat.GetMatchingColor();
                if (_colorID != ColorID.None)
                    break;
            }
        }
    }
}
