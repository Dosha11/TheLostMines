using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OreManager : MonoBehaviour
{
    public static OreManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void CollisionOreo(ItemType type, GameObject ore)
    {
        if (CaveManager.Instance.GetObval())
        {
            ItemType tool = Inventory.Instance.CheckTools(ItemType.kirka);
            if (tool != ItemType.none)
            {
                for (int i = 0; i < CheckOre(type); i++)
                {
                    GameObject clone = Instantiate(ore.GetComponent<OreMarker>().PrefabItem, ore.transform.position, Quaternion.identity);
                }
                ore.transform.GetChild(0).gameObject.SetActive(false);
                ore.GetComponent<Collider>().enabled = false;
                ore.GetComponent<OreMarker>().StartCoroutine(ore.GetComponent<OreMarker>().Timer());
                CaveManager.Instance.AddOre();
                Inventory.Instance.RemoveTools(tool);
                Manager.Instance.CloseIteractionButton();
            }
        }
    }


    public void CollisionOre(ItemType type, GameObject ore)
    {
        if (CaveManager.Instance.GetObval())
        {
            ItemType tool = Inventory.Instance.CheckTools(ItemType.kirka);
            if (tool != ItemType.none)
            {
                Manager.Instance.OnIteractionButton(tool);
                Manager.Instance.IteractionButton.onClick.AddListener(() =>
                {
                    for (int i = 0; i < CheckOre(type); i++)
                    {
                        GameObject clone = Instantiate(ore.GetComponent<OreMarker>().PrefabItem, ore.transform.position, Quaternion.identity);
                    }
                    ore.transform.GetChild(0).gameObject.SetActive(false);
                    ore.GetComponent<Collider>().enabled = false;
                    ore.GetComponent<OreMarker>().StartCoroutine(ore.GetComponent<OreMarker>().Timer());
                    CaveManager.Instance.AddOre();
                    Inventory.Instance.RemoveTools(tool);
                    Manager.Instance.CloseIteractionButton();
                });
            }
        }
    }

    public int CheckOre(ItemType type)
    {
        int count = 0;
        switch (type)
        {
            case ItemType.gold:
                count = 4;
                break;
            case ItemType.silver:
                count = 6;
                break;
            case ItemType.cooper:
                count = 8;
                break;
            case ItemType.ferrum:
                count = 8;
                break;
        }
        return count;
    }
}
