using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketView : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        MarketManager.Instance.CollisionMarket();
    }

    private void OnCollisionExit(Collision collision)
    {
        Manager.Instance.CloseIteractionButton();
    }
}
