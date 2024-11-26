using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private GameObject _itemPrefab;

    private int _keyItem = 0;

    private bool _click = false;
    private GameObject _selectedItem;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            GameObject clone = Instantiate(_slotPrefab, _content.transform.position, Quaternion.identity, _content.transform);
            clone.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (clone.transform.childCount > 0)
                {
                    if (clone.transform.GetChild(0).GetComponent<Items>().Type == ItemType.fullBottle)
                    {
                        if (clone.transform.GetChild(0).GetComponent<Items>().XP > 0)
                        {
                            Manager.Instance.AddWhater();
                            clone.transform.GetChild(0).GetComponent<Items>().XP -= 4;
                        }
                    }
                }
                if (!_click)
                {
                    if (clone.transform.childCount > 0)
                    {
                        _selectedItem = clone.transform.GetChild(0).gameObject;
                        _click = true;
                    }
                }
                else
                {
                    if (clone.transform.childCount == 0)
                    {
                        GameObject cloneItem = Instantiate(_selectedItem, clone.transform.position, Quaternion.identity, clone.transform);
                        Destroy(_selectedItem);
                    }
                    _click = false;
                }
            });
        }
        CollectedItem(ItemType.kirkaFerrum, 1);
        CollectedItem(ItemType.ferrum, 5);
        CollectedItem(ItemType.lopataFerrum, 1);
        CollectedItem(ItemType.bottle, 1);
        CollectedItem(ItemType.bucketFerrum, 1);
        Manager.Instance.NullLevel();

    }

    public void Loading(List<SaveInventory> save, int key)
    {
        for (int i = 0; i < _content.transform.childCount; i++)
        {
            if(_content.transform.GetChild(i).childCount > 0)
            {
                Destroy(_content.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
        for (int i = 0; i < save.Count; i++)
        {
            GameObject clone = Instantiate(_itemPrefab, _content.transform.GetChild(i).transform.position, Quaternion.identity, _content.transform.GetChild(i).transform);
            clone.GetComponent<Items>().ChangeData(save[i].Type, save[i].Count, save[i].XP, save[i].Key);
        }
        _keyItem = key;
    }

    public bool CollectedItem(ItemType type, int count)
    {
        if (CheckStak(type))
        {
            int countItem = 0;
            for (int i = 0; i < _content.transform.childCount; i++)
            {
                if (_content.transform.GetChild(i).childCount > 0)
                {
                    if (_content.transform.GetChild(i).GetChild(0).GetComponent<Items>().Type == type)
                    {
                        if (_content.transform.GetChild(i).GetChild(0).GetComponent<Items>().Count + count < 64)
                        {
                            _content.transform.GetChild(i).GetChild(0).GetComponent<Items>().ChangeCount(count);
                            for (int j = 0; j < count; j++)
                            {
                                CheckLevel(type);
                            }
                            countItem++;
                            break;
                        }
                    }
                }
            }
            if (countItem > 0)
            {
                return true;
            }
            else
            {
                return AddItem(type, count);
            }
        }
        else
        {
            return AddItem(type, count);
        }
    }

    public bool AddItem(ItemType type, int count)
    {
        int countSlot = 0;
        for (int i = 0; i < _content.transform.childCount; i++)
        {
            if (_content.transform.GetChild(i).childCount == 0)
            {
                GameObject clone = Instantiate(_itemPrefab, _content.transform.GetChild(i).transform.position, Quaternion.identity, _content.transform.GetChild(i).transform);
                clone.GetComponent<Items>().ChangeData(type, count, CheckXP(type), _keyItem);
                _keyItem++;
                countSlot++;
                CheckLevel(type);
                break;
            }
        }
        if (countSlot > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveItem(int key)
    {
        for (int i = 0; i < _content.transform.childCount; i++)
        {
            if (_content.transform.GetChild(i).childCount > 0)
            {
                if (_content.transform.GetChild(i).GetChild(0).GetComponent<Items>().Key == key)
                {
                    Destroy(_content.transform.GetChild(i).GetChild(0).gameObject);
                    break;
                }
            }
        }
    }

    public void RemoveTools(ItemType type)
    {
        for (int i = 0; i < _content.transform.childCount; i++)
        {
            if(_content.transform.GetChild(i).childCount > 0)
            {
                if(_content.transform.GetChild(i).GetChild(0).GetComponent<Items>().Type == type)
                {
                    if (!Manager.Instance.Niht)
                    {
                        _content.transform.GetChild(i).GetChild(0).GetComponent<Items>().XP--;
                    }
                    else
                    {
                        _content.transform.GetChild(i).GetChild(0).GetComponent<Items>().XP-=2;
                    }
                    if(_content.transform.GetChild(i).GetChild(0).GetComponent<Items>().XP <= 0)
                    {
                        RemoveItem(_content.transform.GetChild(i).GetChild(0).GetComponent<Items>().Key);
                    }
                }
            }
        }
    }

    public ItemType CheckTools(ItemType type)
    {
        ItemType tools = ItemType.none;
        switch (type)
        {
            case ItemType.axe:
                for (int i = 0; i < _content.transform.childCount; i++)
                {
                    if(_content.transform.GetChild(i).childCount > 0)
                    {
                        ItemType typeItems = _content.transform.GetChild(i).GetChild(0).GetComponent<Items>().Type;
                        if (typeItems == ItemType.axeCooper || typeItems == ItemType.axeFerrum
                             || typeItems == ItemType.axeSilver || typeItems == ItemType.axeGold)
                        {
                            tools = _content.transform.GetChild(i).GetChild(0).GetComponent<Items>().Type;
                        }
                    }
                }
                break;
            case ItemType.kirka:
                for (int i = 0; i < _content.transform.childCount; i++)
                {
                    if (_content.transform.GetChild(i).childCount > 0)
                    {
                        ItemType typeItems = _content.transform.GetChild(i).GetChild(0).GetComponent<Items>().Type;
                        if (typeItems == ItemType.kirkaFerrum || typeItems == ItemType.kirkaCooper
                             || typeItems == ItemType.kirkaSilver || typeItems == ItemType.kirkaGold)
                        {
                            tools = _content.transform.GetChild(i).GetChild(0).GetComponent<Items>().Type;
                        }
                    }
                }
                break;
            case ItemType.lopata:
                for (int i = 0; i < _content.transform.childCount; i++)
                {
                    if (_content.transform.GetChild(i).childCount > 0)
                    {
                        ItemType typeItems = _content.transform.GetChild(i).GetChild(0).GetComponent<Items>().Type;
                        if (typeItems == ItemType.lopataFerrum || typeItems == ItemType.lopataCooper
                             || typeItems == ItemType.lopataSilver || typeItems == ItemType.lopataGold)
                        {
                            tools = _content.transform.GetChild(i).GetChild(0).GetComponent<Items>().Type;
                        }
                    }
                }
                break;
            case ItemType.molot:
                for (int i = 0; i < _content.transform.childCount; i++)
                {
                    if (_content.transform.GetChild(i).childCount > 0)
                    {
                        ItemType typeItems = _content.transform.GetChild(i).GetChild(0).GetComponent<Items>().Type;
                        if (typeItems == ItemType.molotFerrum || typeItems == ItemType.molotCooper
                             || typeItems == ItemType.molotSilver || typeItems == ItemType.molotGold)
                        {
                            tools = _content.transform.GetChild(i).GetChild(0).GetComponent<Items>().Type;
                        }
                    }
                }
                break;
            case ItemType.bucket:
                for (int i = 0; i < _content.transform.childCount; i++)
                {
                    if (_content.transform.GetChild(i).childCount > 0)
                    {
                        ItemType typeItems = _content.transform.GetChild(i).GetChild(0).GetComponent<Items>().Type;
                        if (typeItems == ItemType.bucketFerrum || typeItems == ItemType.bucketCooper
                             || typeItems == ItemType.bucketSilver || typeItems == ItemType.bucketGold)
                        {
                            tools = _content.transform.GetChild(i).GetChild(0).GetComponent<Items>().Type;
                        }
                    }
                }
                break;
            case ItemType.bottle:
                for (int i = 0; i < _content.transform.childCount; i++)
                {
                    if (_content.transform.GetChild(i).childCount > 0)
                    {
                        ItemType typeItems = _content.transform.GetChild(i).GetChild(0).GetComponent<Items>().Type;
                        if (typeItems == ItemType.bottle)
                        {
                            tools = _content.transform.GetChild(i).GetChild(0).GetComponent<Items>().Type;
                        }
                    }
                }
                break;
        }
        return tools;
    }

    public bool CheckStak(ItemType type)
    {
        if (type == ItemType.gold || type == ItemType.silver || type == ItemType.ferrum || type == ItemType.cooper
              || type == ItemType.bazalt || type == ItemType.slanec || type == ItemType.ygol || type == ItemType.mramor
               || type == ItemType.granit || type == ItemType.sosna || type == ItemType.bereza || type == ItemType.dub
                || type == ItemType.klen || type == ItemType.olxa || type == ItemType.neft)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckLevel(ItemType type)
    {
        float count = 0;
        switch (type)
        {
            case ItemType.gold:
                count = 100;
                break;
            case ItemType.silver:
                count = 33.3f;
                break;
            case ItemType.cooper:
                count = 15;
                break;
            case ItemType.ferrum:
                count = 10;
                break;
            default:
                count = 10;
                break;
        }
        Manager.Instance.AddLevel(count);
    }

    public int CheckXP(ItemType type)
    {
        int xp = 0;
        switch (type)
        {
            case ItemType.kirkaGold:
                xp = 40;
                break;
            case ItemType.kirkaSilver:
                xp = 30;
                break;
            case ItemType.kirkaCooper:
                xp = 20;
                break;
            case ItemType.kirkaFerrum:
                xp = 10;
                break;
            case ItemType.lopataGold:
                xp = 40;
                break;
            case ItemType.lopataSilver:
                xp = 30;
                break;
            case ItemType.lopataCooper:
                xp = 20;
                break;
            case ItemType.lopataFerrum:
                xp = 10;
                break;
            case ItemType.axeGold:
                xp = 40;
                break;
            case ItemType.axeSilver:
                xp = 30;
                break;
            case ItemType.axeCooper:
                xp = 20;
                break;
            case ItemType.axeFerrum:
                xp = 10;
                break;
            case ItemType.molotGold:
                xp = 40;
                break;
            case ItemType.molotSilver:
                xp = 30;
                break;
            case ItemType.molotCooper:
                xp = 20;
                break;
            case ItemType.molotFerrum:
                xp = 10;
                break;
            case ItemType.bucketGold:
                xp = 40;
                break;
            case ItemType.bucketSilver:
                xp = 30;
                break;
            case ItemType.bucketCooper:
                xp = 20;
                break;
            case ItemType.bucketFerrum:
                xp = 10;
                break;
            case ItemType.fullBottle:
                xp = 16;
                break;
        }
        return xp;
    }

    public GameObject GetContent()
    {
        return _content;
    }

    public int GetKeyItem()
    {
        return _keyItem;
    }
}
