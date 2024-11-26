using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sikret : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        CaveManager.Instance.CollisionSikres(gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        Manager.Instance.CloseIteractionButton();
    }
}
