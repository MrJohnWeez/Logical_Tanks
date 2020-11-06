using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseMenu : MonoBehaviour
{
    [Header("BaseMenu")]
    [SerializeField] protected Button backButton = null;

    protected virtual void Awake()
    {
        backButton?.onClick.AddListener(CloseMenu);
    }

    protected virtual void SpawnMenu(GameObject prefabToSpawn)
    {
        Instantiate(prefabToSpawn, transform.parent, false);
    }

    protected virtual void CloseMenu()
    {
        Destroy(gameObject);
    }
}
