using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreMarker : MonoBehaviour
{
    public ItemType Type;
    public GameObject PrefabItem;
    public Dot MyDot;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            OreManager.Instance.CollisionOre(Type, gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Manager.Instance.CloseIteractionButton();
    }

    public void Touch() 
    {
        OreManager.Instance.CollisionOreo(Type, gameObject);
    }

    public IEnumerator Timer()
    {
        int time = 0;
        switch (Type)
        {
            case ItemType.gold:
                time = 160;
                break;
            case ItemType.silver:
                time = 120;
                break;
            case ItemType.cooper:
                time = 80;
                break;
            case ItemType.ferrum:
                time = 40;
                break;
        }
        yield return new WaitForSeconds(time);
        transform.GetChild(0).gameObject.SetActive(true);
        gameObject.GetComponent<Collider>().enabled = true;
    }
}
