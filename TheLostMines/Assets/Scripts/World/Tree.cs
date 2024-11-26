using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public ItemType Type;
    public GameObject PrefabItem;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            TreeManager.Instance.CollisionOre(Type, gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Manager.Instance.CloseIteractionButton();
    }

    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(40);
        transform.GetChild(0).gameObject.SetActive(true);
        gameObject.GetComponent<Collider>().enabled = true;
    }
}
