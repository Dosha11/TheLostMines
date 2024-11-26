using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        CaveManager.Instance.CollisionStone(gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        Manager.Instance.CloseIteractionButton();
    }
}
