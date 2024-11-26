using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Save : MonoBehaviour
{
    public static Save Instance;

    public DataSave DataSave = new DataSave();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Saves()
    {
        List<SaveInventory> inventory = new List<SaveInventory>();
        for (int i = 0; i < Inventory.Instance.GetContent().transform.childCount; i++)
        {
            if(Inventory.Instance.GetContent().transform.GetChild(i).childCount > 0)
            {
                Items item = Inventory.Instance.GetContent().transform.GetChild(i).GetChild(0).GetComponent<Items>();
                inventory.Add(new SaveInventory { Type = item.Type, Count = item.Count, XP = item.XP, Key = item.Key });
            }
        }
        
        DataSave.PlayerPosition = Player.Instance.GetPosition();
        DataSave.Inventory = inventory;
        DataSave.KeyInventory = Inventory.Instance.GetKeyItem(); 
        DataSave.Caves = CaveManager.Instance.GetCave();
        DataSave.Level = Manager.Instance.GetLevel();
        DataSave.LevelProgress = Manager.Instance.GetLevelProgress();
        DataSave.TargetLevelProgress = Manager.Instance.GetTargetLevelProgress();
        DataSave.Thrist = Manager.Instance.GetThrist();
        DataSave.Coin = Manager.Instance.GetCoin();

        string data = JsonUtility.ToJson(DataSave);
        PlayerPrefs.SetString("Save", data);
    }

    public void SoSaves()
    {
        List<SaveInventory> inventory = new List<SaveInventory>();
        for (int i = 0; i < Inventory.Instance.GetContent().transform.childCount; i++)
        {
            if (Inventory.Instance.GetContent().transform.GetChild(i).childCount > 0)
            {
                Items item = Inventory.Instance.GetContent().transform.GetChild(i).GetChild(0).GetComponent<Items>();
                inventory.Add(new SaveInventory { Type = item.Type, Count = item.Count, XP = item.XP, Key = item.Key });
            }
        }

        DataSave.Inventory = inventory;
        DataSave.KeyInventory = Inventory.Instance.GetKeyItem();
        DataSave.Caves = CaveManager.Instance.GetCave();
        DataSave.Level = Manager.Instance.GetLevel();
        DataSave.LevelProgress = Manager.Instance.GetLevelProgress();
        DataSave.TargetLevelProgress = Manager.Instance.GetTargetLevelProgress();
        DataSave.Thrist = Manager.Instance.GetThrist();
        DataSave.Coin = Manager.Instance.GetCoin();

        string data = JsonUtility.ToJson(DataSave);
        PlayerPrefs.SetString("Save", data);
    }

    public void Load()
    {
        string data = PlayerPrefs.GetString("Save");
        DataSave = JsonUtility.FromJson<DataSave>(data);
        GameManager.Instance.SceneLoader(1);
        Invoke("Loading", 0.5f);
    }

    public void Loading()
    {
        Time.timeScale = 1;
        Player.Instance.ChangePosition(DataSave.PlayerPosition);
        Inventory.Instance.Loading(DataSave.Inventory, DataSave.KeyInventory);
        CaveManager.Instance.ChangeCaves(DataSave.Caves);
        Manager.Instance.Loading(DataSave.Level, DataSave.LevelProgress, DataSave.TargetLevelProgress, DataSave.Thrist, DataSave.Coin);
    }
}

[Serializable]
public class DataSave
{
    public Vector3 PlayerPosition;
    public List<SaveInventory> Inventory;
    public int KeyInventory;
    public List<Cave> Caves;
    public int Level;
    public float LevelProgress;
    public int TargetLevelProgress;
    public int Thrist;
    public int Coin;
}

[Serializable]
public class SaveInventory
{
    public ItemType Type;
    public int Count;
    public int XP;
    public int Key;
}
