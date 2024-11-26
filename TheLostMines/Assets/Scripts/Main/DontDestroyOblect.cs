using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOblect : MonoBehaviour
{
    public static DontDestroyOblect Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
