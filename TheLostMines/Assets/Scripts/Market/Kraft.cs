using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kraft : MonoBehaviour
{
    public static Kraft Instance;

    [SerializeField] private GameObject _slotOne;
    [SerializeField] private GameObject _slotTwo;
    [SerializeField] private GameObject _listItemsResult;
    [SerializeField] private List<Recip> _recip;
    [SerializeField] private GameObject _prefabItem;

    private bool _one = false;
    private bool _two = false;
    private bool _oneCorrect = false;
    private bool _twoCorrect = false;
    private int _indexRecip;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void Krafting()
    {
        if(_slotOne.transform.childCount > 0 && _slotTwo.transform.childCount > 0)
        {
            for (int i = 0; i < _recip.Count; i++)
            {
                if (!_recip[i].OneSlot)
                {
                    if (_recip[i].Level <= Manager.Instance.GetLevel())
                    {
                        int count = 0;
                        for (int q = 0; q < _recip[i].ItemOne.Count; q++)
                        {
                            if (_slotOne.transform.GetChild(0).GetComponent<Items>().Type == _recip[i].ItemOne[q].Type)
                            {
                                _one = true;
                                _oneCorrect = true;
                                count++;
                                break;
                            }
                        }
                        if(count == 0)
                        {
                            if (_slotOne.transform.GetChild(0).GetComponent<Items>().Type == _recip[i].ItemTwo.Type)
                            {
                                _one = true;
                                break;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < _recip.Count; i++)
            {
                if (!_recip[i].OneSlot)
                {
                    if (_recip[i].Level <= Manager.Instance.GetLevel())
                    {
                        if (_oneCorrect)
                        {
                            if (_slotTwo.transform.GetChild(0).GetComponent<Items>().Type == _recip[i].ItemTwo.Type)
                            {
                                _two = true;
                                _indexRecip = i;
                                break;
                            }
                        }
                        else
                        {
                            for (int q = 0; q < _recip[i].ItemOne.Count; q++)
                            {
                                if (_slotTwo.transform.GetChild(0).GetComponent<Items>().Type == _recip[i].ItemOne[q].Type)
                                {
                                    _two = true;
                                    _indexRecip = i;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if(_one && _two)
            {
                for (int q = 0; q < _listItemsResult.transform.childCount; q++)
                {
                    Destroy(_listItemsResult.transform.GetChild(q).gameObject);
                }
                _listItemsResult.SetActive(true);
                for (int i = 0; i < _recip[_indexRecip].ItemsResult.Count; i++)
                {
                    GameObject clone = Instantiate(_prefabItem, _listItemsResult.transform.position, Quaternion.identity, _listItemsResult.transform);
                    clone.transform.GetChild(0).GetComponent<Items>().ChangeType(_recip[_indexRecip].ItemsResult[i].Type);
                    clone.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        int countOne = 0;
                        int countTwo = 0;
                        if (_oneCorrect)
                        {
                            countOne = _slotOne.transform.GetChild(0).GetComponent<Items>().Count - _recip[_indexRecip].ItemOne.Count;
                            countTwo = _slotTwo.transform.GetChild(0).GetComponent<Items>().Count - _recip[_indexRecip].ItemTwo.Count;
                        }
                        else
                        {
                            countOne = _slotOne.transform.GetChild(0).GetComponent<Items>().Count - _recip[_indexRecip].ItemTwo.Count;
                            countTwo = _slotTwo.transform.GetChild(0).GetComponent<Items>().Count - _recip[_indexRecip].ItemOne.Count;
                        }
                        if (countOne > 0)
                        {
                            Inventory.Instance.CollectedItem(_slotOne.transform.GetChild(0).GetComponent<Items>().Type, countOne);
                        }
                        if (countTwo > 0)
                        {
                            Inventory.Instance.CollectedItem(_slotTwo.transform.GetChild(0).GetComponent<Items>().Type, countTwo);
                        }
                        Inventory.Instance.CollectedItem(clone.transform.GetChild(0).GetComponent<Items>().Type, 1);
                        NormalEndKraft();
                    });
                }                
            }
            else
            {
                EndKraft();
            }
        }
        else
        {
            if(_slotOne.transform.childCount > 0)
            {
                for (int i = 0; i < _recip.Count; i++)
                {
                    if (_recip[i].OneSlot)
                    {
                        if (_recip[i].Level <= Manager.Instance.GetLevel())
                        {
                            int count = 0;
                            for (int q = 0; q < _recip[i].ItemOne.Count; q++)
                            {
                                if (_slotOne.transform.GetChild(0).GetComponent<Items>().Type == _recip[i].ItemOne[q].Type)
                                {
                                    _one = true;
                                    _oneCorrect = true;
                                    _indexRecip = i;
                                    count++;
                                    break;
                                }
                            }
                            if (count == 0)
                            {
                                if (_slotOne.transform.GetChild(0).GetComponent<Items>().Type == _recip[i].ItemTwo.Type)
                                {
                                    _one = true;
                                    _indexRecip = i;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (_one)
                {
                    for (int q = 0; q < _listItemsResult.transform.childCount; q++)
                    {
                        Destroy(_listItemsResult.transform.GetChild(q).gameObject);
                    }
                    _listItemsResult.SetActive(true);
                    for (int i = 0; i < _recip[_indexRecip].ItemsResult.Count; i++)
                    {
                        GameObject clone = Instantiate(_prefabItem, _listItemsResult.transform.position, Quaternion.identity, _listItemsResult.transform);
                        clone.transform.GetChild(0).GetComponent<Items>().ChangeType(_recip[_indexRecip].ItemsResult[i].Type);
                        clone.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            int countOne = 0;
                            if (_oneCorrect)
                            {
                                countOne = _slotOne.transform.GetChild(0).GetComponent<Items>().Count - _recip[_indexRecip].ItemOne.Count;
                            }
                            else
                            {
                                countOne = _slotOne.transform.GetChild(0).GetComponent<Items>().Count - _recip[_indexRecip].ItemTwo.Count;
                            }
                            if (countOne > 0)
                            {
                                Inventory.Instance.CollectedItem(_slotOne.transform.GetChild(0).GetComponent<Items>().Type, countOne);
                            }
                            Inventory.Instance.CollectedItem(clone.transform.GetChild(0).GetComponent<Items>().Type, 1);
                            NormalEndKraft();
                        });
                    }
                }
                else
                {
                    EndKraft();
                }
            }
            else if(_slotTwo.transform.childCount > 0)
            {
                for (int i = 0; i < _recip.Count; i++)
                {
                    if (_recip[i].OneSlot)
                    {
                        if (_recip[i].Level <= Manager.Instance.GetLevel())
                        {
                            if (_slotTwo.transform.GetChild(0).GetComponent<Items>().Type == _recip[i].ItemTwo.Type)
                            {
                                _two = true;
                                _indexRecip = i;
                                _twoCorrect = true;
                                break;
                            }
                            else
                            {
                                for (int q = 0; q < _recip[i].ItemOne.Count; q++)
                                {
                                    if (_slotTwo.transform.GetChild(0).GetComponent<Items>().Type == _recip[i].ItemOne[q].Type)
                                    {
                                        _two = true;
                                        _indexRecip = i;
                                        break;
                                    }

                                }
                            }
                        }
                    }
                    
                }
                if (_two)
                {
                    for (int q = 0; q < _listItemsResult.transform.childCount; q++)
                    {
                        Destroy(_listItemsResult.transform.GetChild(q).gameObject);
                    }
                    _listItemsResult.SetActive(true);
                    for (int i = 0; i < _recip[_indexRecip].ItemsResult.Count; i++)
                    {
                        GameObject clone = Instantiate(_prefabItem, _listItemsResult.transform.position, Quaternion.identity, _listItemsResult.transform);
                        clone.transform.GetChild(0).GetComponent<Items>().ChangeType(_recip[_indexRecip].ItemsResult[i].Type);
                        clone.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            int countTwo = 0;
                            if (_twoCorrect)
                            {
                                countTwo = _slotTwo.transform.GetChild(0).GetComponent<Items>().Count - _recip[_indexRecip].ItemTwo.Count;
                            }
                            else
                            {
                                countTwo = _slotTwo.transform.GetChild(0).GetComponent<Items>().Count - _recip[_indexRecip].ItemOne.Count;
                            }
                            if (countTwo > 0)
                            {
                                Inventory.Instance.CollectedItem(_slotTwo.transform.GetChild(0).GetComponent<Items>().Type, countTwo);
                            }
                            Inventory.Instance.CollectedItem(clone.transform.GetChild(0).GetComponent<Items>().Type, 1);
                            NormalEndKraft();
                        });
                    }
                }
                else
                {
                    EndKraft();
                }
            }
            else
            {
                EndKraft();
            }
        }
    }

    public void EndKraft()
    {
        if(_slotOne.transform.childCount > 0)
        {
            Inventory.Instance.CollectedItem(_slotOne.transform.GetChild(0).GetComponent<Items>().Type, _slotOne.transform.GetChild(0).GetComponent<Items>().Count);
        }
        if (_slotTwo.transform.childCount > 0)
        {
            Inventory.Instance.CollectedItem(_slotTwo.transform.GetChild(0).GetComponent<Items>().Type, _slotTwo.transform.GetChild(0).GetComponent<Items>().Count);
        }
        NormalEndKraft();
        MarketManager.Instance.CloseKraft();
        MarketManager.Instance.OpenKraft();
    }

    public void NormalEndKraft()
    {
        _one = false;
        _two = false;
        _oneCorrect = false;
        _twoCorrect = false;
        _indexRecip = 0;

        for (int i = 0; i < _listItemsResult.transform.childCount; i++)
        {
            Destroy(_listItemsResult.transform.GetChild(i).gameObject);
        }
        _listItemsResult.SetActive(false);

        if (_slotOne.transform.childCount > 0)
        {
            Destroy(_slotOne.transform.GetChild(0).gameObject);
        }
        if (_slotTwo.transform.childCount > 0)
        {
            Destroy(_slotTwo.transform.GetChild(0).gameObject); 
        }
        
        _slotOne.GetComponent<Button>().enabled = true;
        _slotTwo.GetComponent<Button>().enabled = true;
        MarketManager.Instance.CloseKraft();
        MarketManager.Instance.OpenKraft();
    }

}

[Serializable]
public class Recip
{
    public List<Item> ItemOne;
    public Item ItemTwo;
    public List<Item> ItemsResult;
    public bool OneSlot;
    public int Level;
}

[Serializable] 
public class Item
{
    public ItemType Type;
    public int Count;
}