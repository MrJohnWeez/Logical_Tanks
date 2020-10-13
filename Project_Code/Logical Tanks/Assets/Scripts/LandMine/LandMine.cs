using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SphereCollider))]
public class LandMine : MonoBehaviour
{
    public ColorID GetColorID => _colorID;

    [SerializeField] private ColorID _colorID = ColorID.Green;
    [SerializeField] private GameObject _mineCenter = null;
    [SerializeField] private GameObject _model = null;

    private BoxCollider _trigger = null;
    private SphereCollider _explosionRadius = null;
    private List<LandMine> _sameColoredMines = new List<LandMine>();

    private void Awake()
    {
        _trigger = GetComponent<BoxCollider>();
        _explosionRadius = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        LandMine[] landMines = GameObject.FindObjectsOfType<LandMine>();
        foreach(LandMine mine in landMines)
        {
            if(mine.GetColorID == _colorID)
            {
                _sameColoredMines.Add(mine);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        for(int i = _sameColoredMines.Count - 1; i >= 0; i--)
        {
            _sameColoredMines[i].Explode();
        }
    }

    public void Explode()
    {
        _model.SetActive(false);
        Collider[] allOverlappingColliders = Physics.OverlapSphere(_explosionRadius.center, _explosionRadius.radius);
        for(int i = allOverlappingColliders.Length - 1; i >= 0; i--)
        {
            TankController tankController = allOverlappingColliders[i].gameObject.GetComponent<TankController>();
            if(tankController && GetColorID == tankController.GetColorID)
            {
                tankController.Explode();
            }
        }
        // TODO: Show Explosion
        Debug.Log("Mine Exploded with color: " + _colorID);
        Destroy(gameObject);
    }
}
