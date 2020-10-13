using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public ColorID GetColorID  => _colorID;
    
    [SerializeField] private ColorID _colorID = ColorID.Green;

    void Start()
    {
        
    }

    public void Explode()
    {
        Debug.Log("Tank Exploded with color: " + _colorID);
        Destroy(gameObject);
    }
}
