using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    void Update() { transform.LookAt(transform.position + Vector3.down); }
}
