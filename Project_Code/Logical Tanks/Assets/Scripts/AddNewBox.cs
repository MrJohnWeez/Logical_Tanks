using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddNewBox : MonoBehaviour
{
    [SerializeField] private GameObject _nodePrefab = null;
    [SerializeField] private RectTransform _spawnPoint;
    [SerializeField] private GameObject _content;
    [SerializeField] private NodeConnectionPoint[] _nodeConnectionPoints;

    public void AddNewBoxToLayout()
    {
        GameObject newNode = Instantiate(_nodePrefab);
        RectTransform rt = newNode.GetComponent<RectTransform>();
        rt.localScale = _content.GetComponent<RectTransform>().localScale;
        rt.position = _spawnPoint.position;
        newNode.transform.SetParent(_content.transform, true);
    }
}
