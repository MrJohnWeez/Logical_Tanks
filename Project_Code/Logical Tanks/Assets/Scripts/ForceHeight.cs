using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
public class ForceHeight : MonoBehaviour
{
    LayoutElement test = null;
    float test2 = 0;

    private void Start()
    {
        test = GetComponent<LayoutElement>();
    }

    private void Update() {
        int test3 = gameObject.transform.childCount;
        int ending = 38 + 80 * test3 + 38;
        test.preferredHeight = ending;
    }
}
