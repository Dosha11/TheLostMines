using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FossilsManager : MonoBehaviour
{
    public static FossilsManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void CollisionOre(ItemType type, GameObject tree)
    {
        ItemType tool = Inventory.Instance.CheckTools(ItemType.kirka);
        if (tool != ItemType.none)
        {
            Manager.Instance.OnIteractionButton(tool);
            Manager.Instance.IteractionButton.onClick.AddListener(() =>
            {
                for (int i = 0; i < 8; i++)
                {
                    GameObject clone = Instantiate(tree.GetComponent<Fossils>().PrefabItem, tree.transform.position, Quaternion.identity);
                }
                tree.transform.GetChild(0).gameObject.SetActive(false);
                tree.GetComponent<Collider>().enabled = false;
                tree.GetComponent<Fossils>().StartCoroutine(tree.GetComponent<Fossils>().Timer());
                Inventory.Instance.RemoveTools(tool);
                Manager.Instance.CloseIteractionButton();
            });
        }
    }
}
