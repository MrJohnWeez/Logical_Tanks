using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraManager : MonoBehaviour
{
    public Action<float> OnZoomChanged;
    public const float MIN_ZOOM = 5;
    public const float MAX_ZOOM = 15;
    private const float WALL_OFFSET = 1.5f;
    [SerializeField] private InteractableRenderTexture _interactMapTexture = null;
    private float _zoomSensitivity = 1f;
    private Camera _mapCamera = null;
    private Transform _mapCameraTransform = null;
    private Transform[] _cameraWalls = new Transform[4];
    private Rect _camLimits = new Rect();

    private void Awake()
    {
        // Initialize camera
        _interactMapTexture.OnDragEvent += UpdateCameraPosition;
        _interactMapTexture.OnScrollEvent += ZoomCamera;
        _interactMapTexture.OnZoomEvent += SetZoom;
        _mapCamera = GameObject.FindGameObjectWithTag("MapCamera")?.GetComponent<Camera>();
        _mapCamera.orthographicSize = MAX_ZOOM - MIN_ZOOM;
        _mapCameraTransform = _mapCamera.transform;

        // Get camera bounds
        GameObject[] camWallGOs = GameObject.FindGameObjectsWithTag("CameraWall");
        if (camWallGOs.Length == 4)
        {
            for (int i = 0; i < camWallGOs.Length; i++) { _cameraWalls[i] = camWallGOs[i].transform; }
            float tmpXPos = _cameraWalls[0].position.x;
            _camLimits.xMin = tmpXPos;
            _camLimits.xMax = tmpXPos;
            float tmpZPos = _cameraWalls[0].position.z;
            _camLimits.yMin = tmpZPos;
            _camLimits.yMax = tmpZPos;
            for (int i = 0; i < camWallGOs.Length; i++)
            {
                Vector3 currWall = _cameraWalls[i].position;
                _camLimits.xMax = Mathf.Max(_camLimits.xMax, currWall.x);
                _camLimits.xMin = Mathf.Min(_camLimits.xMin, currWall.x);
                _camLimits.yMax = Mathf.Max(_camLimits.yMax, currWall.z);
                _camLimits.yMin = Mathf.Min(_camLimits.yMin, currWall.z);
            }
        }
        else { Debug.LogError("Invalid number of CameraWall(s) in scene"); }
    }

    private void OnDestroy()
    {
        _interactMapTexture.OnDragEvent -= UpdateCameraPosition;
        _interactMapTexture.OnScrollEvent -= ZoomCamera;
        _interactMapTexture.OnZoomEvent -= SetZoom;
    }

    private void UpdateCameraPosition(Vector2 delta)
    {
        if(delta != Vector2.zero)
        {
            Vector2 moveDelta = _interactMapTexture.ConvertToOrthoWorldSpace(delta, _mapCamera);
            Vector3 camPos = _mapCameraTransform.position;
            camPos.x -= moveDelta.x;
            camPos.z -= moveDelta.y;
            float offset = _mapCamera.orthographicSize + WALL_OFFSET;
            camPos.x = Mathf.Clamp(camPos.x, _camLimits.xMin + offset, _camLimits.xMax - offset);
            camPos.z = Mathf.Clamp(camPos.z, _camLimits.yMin + offset, _camLimits.yMax - offset);
            _mapCameraTransform.position = camPos;
        }
        else
        {
            Vector3 camPos = _mapCameraTransform.position;
            float offset = _mapCamera.orthographicSize + WALL_OFFSET;
            camPos.x = Mathf.Clamp(camPos.x, _camLimits.xMin + offset, _camLimits.xMax - offset);
            camPos.z = Mathf.Clamp(camPos.z, _camLimits.yMin + offset, _camLimits.yMax - offset);
            _mapCameraTransform.position = camPos;
        }
    }

    private void ZoomCamera(float delta)
    {
        _mapCamera.orthographicSize -= delta * _zoomSensitivity;
        SetZoom(_mapCamera.orthographicSize);
    }

    private void SetZoom(float newZoom)
    {
        _mapCamera.orthographicSize = Mathf.Clamp(newZoom, MIN_ZOOM, MAX_ZOOM);
        UpdateCameraPosition(Vector2.zero);
        OnZoomChanged?.Invoke(_mapCamera.orthographicSize);
    }
}
