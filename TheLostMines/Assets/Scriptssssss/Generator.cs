using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Generator : MonoBehaviour
{
    [Header("Блоки карты")]
    public GameObject DotHorizontal;
    public GameObject DotAngle;
    public GameObject DotFork;
    public GameObject deadlock;
    public GameObject descent;
    public GameObject stash;
    public GameObject collaps;


    public Dictionary<int, int> listMaxStep;
    public Dictionary<int, int> listMyStep;

    public List<Vector3Int> usedPoints = new List<Vector3Int>();
    public Vector3Int offset;

    [Header("Префабы руд")]
    public GameObject Gold;
    public GameObject Silver;
    public GameObject Iron;
    public GameObject Copper;
    [Header("Процент выпадения руд")]
    public int PercentageLossGold;
    public int PercentageLossSilver;
    public int PercentageLossIron;
    public int PercentageLossCopper;

    public float complexity;


    private void Awake()
    {
        listMaxStep = new Dictionary<int, int>
        {
            { 0, 100 },
            { -1, 200 },
            { -2, 500 },
            { -3, 1100 }
        };
        listMyStep = new Dictionary<int, int>
        {
            { 0, 0 },
            { -1, 0 },
            { -2, 0 },
            { -3, 0 }
        };
    }

    private void Start()
    {
        GameObject clone = Instantiate(DotHorizontal, new Vector3(0, 0, 0), Quaternion.identity);
        clone.GetComponent<Dot>().Registration(this, new Vector3Int(0, 0, 0), 10);

    }

    public bool Compare(Vector3Int point)
    {

        bool was = true;
        foreach (var item in usedPoints)
        {
            if (item == point)
            {
                was = false;
            }
        }
        return was;
    }
}
