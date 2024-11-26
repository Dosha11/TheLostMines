using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whater : MonoBehaviour
{
    private bool _ok = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(_ok)
            {
                ItemType tool = Inventory.Instance.CheckTools(ItemType.bottle);
                if (tool != ItemType.none)
                {
                    Manager.Instance.OnIteractionButton(tool);
                    Manager.Instance.IteractionButton.onClick.AddListener(() =>
                    {
                        Inventory.Instance.RemoveTools(tool);
                        Inventory.Instance.CollectedItem(ItemType.fullBottle, 1);
                        _ok = false;
                        Manager.Instance.CloseIteractionButton();
                    });
                }
            }
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        Manager.Instance.CloseIteractionButton();
    }

    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(40);
        _ok = true;
    }
}
