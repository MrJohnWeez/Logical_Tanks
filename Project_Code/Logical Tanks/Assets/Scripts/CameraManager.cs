using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private const float WALL_OFFSET = 1.5f;

    [SerializeField] private Camera _mapCamera = null;

    private Transform _mapCameraTransform = null;
    private Transform[] _cameraWalls = new Transform[4];
    private Vector2 _xLimits = new Vector3();
    private Vector2 _zLimits = new Vector3();

    private void Awake()
    {
        _mapCameraTransform = _mapCamera.transform;
        GameObject[] camWallGOs = GameObject.FindGameObjectsWithTag("CameraWall");
        if(camWallGOs.Length == 4)
        {
            for(int i = 0; i < camWallGOs.Length; i++) { _cameraWalls[i] = camWallGOs[i].transform; }
            float tmpXPos = _cameraWalls[0].position.x;
            _xLimits = new Vector2(tmpXPos, tmpXPos);
            float tmpZPos = _cameraWalls[0].position.z;
            _zLimits = new Vector2(tmpZPos, tmpZPos);
            for(int i = 0; i < camWallGOs.Length; i++)
            {
                Vector3 currWall = _cameraWalls[i].position;
                _xLimits.x = Mathf.Max(_xLimits.x, currWall.x);
                _xLimits.y = Mathf.Min(_xLimits.y, currWall.x);
                _zLimits.x = Mathf.Max(_zLimits.x, currWall.z);
                _zLimits.y = Mathf.Min(_zLimits.y, currWall.z);
            }
        }
        else { Debug.LogError("Invalid number of CameraWall(s) in scene"); }
    }

    private void LateUpdate()
    {
        Vector3 camPos = _mapCameraTransform.position;
        float offset = _mapCamera.orthographicSize + WALL_OFFSET;
        camPos.x = Mathf.Clamp(camPos.x, _xLimits.y + offset, _xLimits.x - offset);
        camPos.z = Mathf.Clamp(camPos.z, _zLimits.y + offset, _zLimits.x - offset);
        _mapCameraTransform.position = camPos;
    }
}
