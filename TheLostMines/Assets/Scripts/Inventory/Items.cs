using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    public ItemType Type;
    public int Count;
    public int XP;
    public int Price;
    public int Key;

    public void ChangeData(ItemType type, int count, int xp, int key)
    {
        Type = type;
        transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(Type.ToString());
        Count = count;
        transform.GetChild(1).GetComponent<Text>().text = Count.ToString();
        XP = xp;
        Key = key;
    }

    public void ChangeType(ItemType type)
    {
        Type = type;
        transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(Type.ToString());
    }

    public void ChangeCount(int count)
    {
        Count += count;
        transform.GetChild(1).GetComponent<Text>().text = Count.ToString();
    }

    public void ChengePrice(int price)
    {
        Price = price;
    }
}
