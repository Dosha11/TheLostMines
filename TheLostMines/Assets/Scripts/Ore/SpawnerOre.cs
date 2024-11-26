using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerOre : MonoBehaviour
{
    [SerializeField] private List<GameObject> _prefabOre;

    private void Start()
    {
        for (int i = 0; i < CaveManager.Instance.SelectedCave.MaxCountOre; i++)
        {
            for (int q = 0; q < transform.childCount-1; q++)
            {
                if (transform.GetChild(i).childCount == 0)
                {
                    int random = Random.Range(0, CaveManager.Instance.GetLevelSelectCave());
                    GameObject clone = Instantiate(_prefabOre[random], transform.GetChild(i).transform.position, Quaternion.identity, transform.GetChild(i).transform);
                    break;
                }
            }
        }
        int randomSikret = Random.Range(0, CaveManager.Instance.GetLevelSelectCave());
        GameObject sikret = Instantiate(_prefabOre[randomSikret], transform.GetChild(transform.childCount-1).transform.position, Quaternion.identity, transform.GetChild(transform.childCount-1).transform);
    }
}
