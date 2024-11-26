using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CaveManager : MonoBehaviour
{
    public static CaveManager Instance;

    [SerializeField] private List<Cave> _caves;
    [SerializeField] private List<CaveView> _views;
    [SerializeField] private GameObject _stonePrefab;

    private bool _inCave = false;
    public Cave SelectedCave;
    private Cave _readyCave;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        StartCoroutine(Obval());
        CheckView();
    }

    public void CheckView()
    {
        _views.Clear();
        CaveView[] views = FindObjectsOfType<CaveView>();
        for (int i = 0; i < views.Length; i++)
        {
            _views.Add(views[i]);
        }
        CheckStatus();
    }

    public void ChangeCaves(List<Cave> caves)
    {
        _caves.Clear();
        for (int i = 0; i < caves.Count; i++)
        {
            _caves.Add(caves[i]);
        }
        CheckStatus();
    }

    public void CheckStatus()
    {
        for (int i = 0; i < _caves.Count; i++)
        {
            if (_caves[i].Status == Status.die)
            {
                if (_caves[i].Level <= Manager.Instance.GetLevel())
                {
                    _caves[i].Status = Status.standart;
                }
            }
            if(_caves[i].Status == Status.obval)
            {
                for (int q = 0; q < _views.Count; q++)
                {
                    if (_views[q].Key == i)
                    {
                        _views[q].ChangeObval(true);
                    }
                }
            }
            else
            {
                for (int q = 0; q < _views.Count; q++)
                {
                    if (_views[q].Key == i)
                    {
                        _views[q].ChangeObval(false);
                    }
                }
            }
        }
        for (int i = 0; i < _views.Count; i++)
        {
            _views[i].ChangeStatus(_caves[_views[i].Key].Status);
        }
    }

    public IEnumerator Obval()
    {
        while (true)
        {
            yield return new WaitForSeconds(20);
            if(_inCave)
            {
                int chanse = Random.Range(1, 102);
                if(SelectedCave.chanceObval >= chanse)
                {
                    GameObject clone = Instantiate(_stonePrefab, new Vector3(8, 0, Player.Instance.GetPosition().z + 10), Quaternion.identity);
                    SelectedCave.Status = Status.obval;
                    OreMarker[] ore = FindObjectsOfType<OreMarker>();
                }
            }
        }
    }


    public void lol(int key)
    {
        if (!_inCave)
        {
            if (_caves[key].Status != Status.die && _caves[key].Status != Status.ready)
            {
                if (_caves[key].Status == Status.obval)
                {
                    ItemType tools = Inventory.Instance.CheckTools(ItemType.lopata);
                    if (tools != ItemType.none)
                    {
                        SelectedCave = _caves[key];
                        for (int i = 0; i < _views.Count; i++)
                        {
                            if (_views[i].Key == key)
                            {
                                _views[i].ChangeObval(false);
                            }
                        }
                        SelectedCave.Status = Status.standart;
                        CheckStatus();
                        Inventory.Instance.RemoveTools(tools);
                        Manager.Instance.CloseIteractionButton();
                    }
                }
                else
                {
                    Manager.Instance.CloseIteractionButton();
                    SelectedCave = _caves[key];
                    GameManager.Instance.SceneLoader(2);
                    _inCave = true;
                }
            }
        }
        else
        {
            Manager.Instance.CloseIteractionButton();
            Save.Instance.SoSaves();
            GameManager.Instance.SceneLoader(1);
            Save.Instance.Load();
            _inCave = false;

        }
    }



    public void CollisionCave(int key)
    {
        if (!_inCave)
        {
            if (_caves[key].Status != Status.die && _caves[key].Status != Status.ready)
            {
                if (_caves[key].Status == Status.obval)
                {
                    ItemType tools = Inventory.Instance.CheckTools(ItemType.lopata);
                    if(tools != ItemType.none)
                    {
                        Manager.Instance.OnIteractionButton(tools);
                        Manager.Instance.IteractionButton.onClick.AddListener(() =>
                        {
                            SelectedCave = _caves[key];
                            for (int i = 0; i < _views.Count; i++)
                            {
                                if (_views[i].Key == key)
                                {
                                    _views[i].ChangeObval(false);
                                }
                            }
                            SelectedCave.Status = Status.standart;
                            CheckStatus();
                            Inventory.Instance.RemoveTools(tools);
                            Manager.Instance.CloseIteractionButton();
                        });
                    }
                }
                else
                {
                    Manager.Instance.OnIteractionButton(ItemType.cave);
                    Manager.Instance.IteractionButton.onClick.AddListener(() =>
                    {
                        Manager.Instance.CloseIteractionButton();
                        SelectedCave = _caves[key];
                        GameManager.Instance.SceneLoader(2);
                        _inCave = true;
                    });
                }
            }
        }
        else
        {
            Manager.Instance.OnIteractionButton(ItemType.cave);
            Manager.Instance.IteractionButton.onClick.AddListener(() =>
            {
                Manager.Instance.CloseIteractionButton();
                Save.Instance.SoSaves();
                GameManager.Instance.SceneLoader(1);
                Save.Instance.Load();
                _inCave = false;
            });
        }
    }

    public void CollisionStone(GameObject stone)
    {
        ItemType tool = Inventory.Instance.CheckTools(ItemType.lopata);
        if (tool != ItemType.none)
        {
            Manager.Instance.OnIteractionButton(tool);
            Manager.Instance.IteractionButton.onClick.AddListener(() =>
            {
                Destroy(stone);
                SelectedCave.Status = Status.ready;
                Inventory.Instance.RemoveTools(tool);
                Manager.Instance.CloseIteractionButton();
            });
        }
    }

    public void CollisionSikres(GameObject sikret)
    {
        ItemType tool = Inventory.Instance.CheckTools(ItemType.molot);
        if (tool != ItemType.none)
        {
            Manager.Instance.OnIteractionButton(tool);
            Manager.Instance.IteractionButton.onClick.AddListener(() =>
            {
                Destroy(sikret);
                Inventory.Instance.RemoveTools(tool);
                Manager.Instance.CloseIteractionButton();
            });
        }
    }

    public void AddOre()
    {
        if (_inCave)
        {
            SelectedCave.CountOre++;
            SelectedCave.Status = Status.soReady;
            if (SelectedCave.CountOre >= SelectedCave.MaxCountOre)
            {
                SelectedCave.Status = Status.ready;
                OreMarker[] ore = FindObjectsOfType<OreMarker>();
                for (int i = 0; i < ore.Length; i++)
                {
                    ore[i].StopAllCoroutines();
                }
                _readyCave = SelectedCave;
                Invoke("ChengeStatusReady", 30);
                Save.Instance.SoSaves();
                GameManager.Instance.SceneLoader(1);
                Save.Instance.Load();
                _inCave = false;
            }
        }
    }

    public void ChengeStatusReady()
    {
        _readyCave.Status = Status.standart;
        _readyCave.CountOre = 0;
        CheckStatus();
        OreMarker[] ore = FindObjectsOfType<OreMarker>();
        for (int i = 0; i < ore.Length; i++)
        {
            ore[i].StartCoroutine(ore[i].Timer());
        }
    }

    public int GetLevelSelectCave()
    {
        return SelectedCave.Level;
    }

    public bool GetReady()
    {
        if(SelectedCave.Status == Status.ready)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool GetObval()
    {
        if (SelectedCave.Status == Status.obval)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public List<Cave> GetCave()
    {
        return _caves;
    }
}

[Serializable] 
public class Cave
{
    public Status Status;
    public int Level;
    public int chanceObval;
    public int MaxCountOre;
    public int CountOre;
}

public enum Status
{
    standart,
    die,
    obval,
    soReady,
    ready
}
