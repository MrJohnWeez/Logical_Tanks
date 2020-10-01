using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
public class AutoDisableLayoutElement : MonoBehaviour
{
    LayoutElement test = null;

    private void Start()
    {
        test = GetComponent<LayoutElement>();
    }
}
