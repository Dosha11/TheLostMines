using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public static TreeManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void CollisionOre(ItemType type, GameObject tree)
    {
        ItemType tool = Inventory.Instance.CheckTools(ItemType.axe);
        if (tool != ItemType.none)
        {
            Manager.Instance.OnIteractionButton(tool);
            Manager.Instance.IteractionButton.onClick.AddListener(() =>
            {
                for (int i = 0; i < 8; i++)
                {
                    GameObject clone = Instantiate(tree.GetComponent<Tree>().PrefabItem, tree.transform.position, Quaternion.identity);
                }
                tree.transform.GetChild(0).gameObject.SetActive(false);
                tree.GetComponent<Collider>().enabled = false;
                tree.GetComponent<Tree>().StartCoroutine(tree.GetComponent<Tree>().Timer());
                Inventory.Instance.RemoveTools(tool);
                Manager.Instance.CloseIteractionButton();
            });
        }
    }
}
