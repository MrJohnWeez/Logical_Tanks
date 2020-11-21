using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPopupMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] _menus = null;
    private GameManager _gameManager = null;

    void Awake()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void SpawnMenu(GameObject prefabToSpawn)
    {
        Instantiate(prefabToSpawn, transform, false);
    }
    
    private void Start()
    {
        switch(_gameManager.LevelNumber)
        {
            case 1:
                SpawnMenu(_menus[17]);
                break;
            case 2:
                SpawnMenu(_menus[11]);
                break;
            case 4:
                SpawnMenu(_menus[15]);
                SpawnMenu(_menus[2]);
                break;
            case 5:
                SpawnMenu(_menus[13]);
                SpawnMenu(_menus[12]);
                break;
            case 6:
                SpawnMenu(_menus[5]);
                SpawnMenu(_menus[14]);
                break;
            case 7:
                SpawnMenu(_menus[4]);
                break;
            case 9:
                SpawnMenu(_menus[10]);
                break;
            case 10:
                SpawnMenu(_menus[9]);
                SpawnMenu(_menus[3]);
                break;
            case 12:
                SpawnMenu(_menus[1]);
                break;
            case 13:
                SpawnMenu(_menus[0]);
                break;
            case 14:
                SpawnMenu(_menus[8]);
                break;
            case 15:
                SpawnMenu(_menus[7]);
                break;
        }
    }
}
