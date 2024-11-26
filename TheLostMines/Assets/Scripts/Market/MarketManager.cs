using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MarketManager : MonoBehaviour
{
    public static MarketManager Instance;

    [SerializeField] private GameObject _window;
    [SerializeField] private List<GameObject> _windows;
    [SerializeField] private List<ItemForSell> _itemForSell;
    [SerializeField] private List<ItemForBy> _itemForBy;
    [SerializeField] private GameObject _prefabSlotBy;

    private bool _click = false;
    private GameObject _selectedItem;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void CollisionMarket()
    {
        Manager.Instance.OnIteractionButton(ItemType.kolodec);
        Manager.Instance.IteractionButton.onClick.AddListener(() =>
        {
            _window.SetActive(true);
            Manager.Instance.CloseIteractionButton();
        });
    }

    public void OpenBy()
    {
        CloseBy();
        CloseSell();
        CloseKraft();
        _windows[0].SetActive(true);
        _windows[1].SetActive(false);
        _windows[2].SetActive(false);
        for (int i = 0; i < Manager.Instance.GetLevel(); i++)
        {
            for (int q = 0; q < _itemForBy[i].Items.Count; q++)
            {
                GameObject clone = Instantiate(_prefabSlotBy, _windows[0].transform.GetChild(0).transform.position, Quaternion.identity, _windows[0].transform.GetChild(0).transform);
                clone.transform.GetChild(0).GetComponent<Items>().ChangeType(_itemForBy[i].Items[q].Type);
                clone.transform.GetChild(0).GetComponent<Items>().ChengePrice(_itemForBy[i].Items[q].Price);
                clone.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (Manager.Instance.GetCoin() >= clone.transform.GetChild(0).GetComponent<Items>().Price)
                    {
                        if (Inventory.Instance.CollectedItem(clone.transform.GetChild(0).GetComponent<Items>().Type, 1))
                        {
                            Manager.Instance.ChangeCoin(clone.transform.GetChild(0).GetComponent<Items>().Price * -1);
                        }
                        else
                        {
                            Debug.Log("»нвентарь заполнен");
                        }
                    }
                    else
                    {
                        Debug.Log("ƒен€г нема");
                    }
                    clone.GetComponent<Button>().onClick.RemoveAllListeners();
                });
            }
        }
    }

    public void CloseBy()
    {
        for (int i = 0; i < _windows[0].transform.GetChild(0).childCount; i++)
        {
            Destroy(_windows[0].transform.GetChild(0).GetChild(i).gameObject);
        }
        _windows[0].SetActive(false);
    }

    public void OpenSell()
    {
        CloseBy();
        CloseSell();
        CloseKraft();
        _windows[0].SetActive(false);
        _windows[1].gameObject.SetActive(true);
        _windows[2].SetActive(false);
        for (int i = 0; i < Inventory.Instance.GetContent().transform.childCount; i++)
        {
            if(Inventory.Instance.GetContent().transform.GetChild(i).childCount > 0)
            {
                GameObject clone = Instantiate(Inventory.Instance.GetContent().transform.GetChild(i).gameObject, _windows[1].transform.GetChild(0).transform.position, Quaternion.identity, _windows[1].transform.GetChild(0).transform);
                clone.GetComponent<Button>().onClick.AddListener(() =>
                {
                    for (int i = 0; i < _itemForSell.Count; i++)
                    {
                        if (_itemForSell[i].Type == clone.transform.GetChild(0).GetComponent<Items>().Type)
                        {
                            Inventory.Instance.RemoveItem(clone.transform.GetChild(0).GetComponent<Items>().Key);
                            Manager.Instance.ChangeCoin(_itemForSell[i].Price * clone.transform.GetChild(0).GetComponent<Items>().Count);
                            Destroy(clone);
                            break;
                        }
                    }
                });
            }
        }
    }

    public void CloseSell()
    {
        for (int i = 0; i < _windows[1].transform.GetChild(0).childCount; i++)
        {
            Destroy(_windows[1].transform.GetChild(0).GetChild(i).gameObject);
        }
        _windows[1].SetActive(false);
    }

    public void OpenKraft()
    {
        CloseBy();
        CloseSell();
        CloseKraft();
        _windows[0].SetActive(false);
        _windows[1].SetActive(false);
        _windows[2].gameObject.SetActive(true);
        for (int i = 0; i < Inventory.Instance.GetContent().transform.childCount; i++)
        {
            if (Inventory.Instance.GetContent().transform.GetChild(i).childCount > 0)
            {
                GameObject clone = Instantiate(Inventory.Instance.GetContent().transform.GetChild(i).gameObject, _windows[2].transform.GetChild(0).transform.position, Quaternion.identity, _windows[2].transform.GetChild(0).transform);
                clone.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (!_click)
                    {
                        if (clone.transform.childCount > 0)
                        {
                            _selectedItem = clone.transform.GetChild(0).gameObject;
                            _click = true;
                        }
                    }
                });
            }
        }
    }

    public void ChangeSlotCraft(GameObject slot)
    {
        if(_click)
        {
            if (slot.transform.childCount == 0)
            {
                GameObject cloneItem = Instantiate(_selectedItem, slot.transform.position, Quaternion.identity, slot.transform);
                Inventory.Instance.RemoveItem(_selectedItem.GetComponent<Items>().Key);
                Destroy(_selectedItem.gameObject);
            }
            slot.GetComponent<Button>().enabled = false;
            _click = false;
        }
    }

    public void CloseKraft()
    {
        for (int i = 0; i < _windows[2].transform.GetChild(0).childCount; i++)
        {
            Destroy(_windows[2].transform.GetChild(0).GetChild(i).gameObject);
        }
        _windows[2].SetActive(false);
    }
}

[Serializable]
public class ItemForSell
{
    public ItemType Type;
    public int Price;
}

[Serializable]
public class ItemForBy
{
    public List<ItemForSell> Items;
}
