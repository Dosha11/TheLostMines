using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CollectedItem : MonoBehaviour
{
    public ItemType Type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Inventory.Instance.CollectedItem(Type, 1))
            {
                Destroy(gameObject);
            }
        }
    }
}

public enum ItemType
{
    none,
    gold, 
    silver,
    cooper,
    ferrum,
    pureGold,
    pureSilver,
    bronz,
    steel,
    bazalt,
    slanec,
    ygol,
    mramor,
    granit,
    sosna,
    bereza,
    dub,
    klen,
    olxa,
    neft,
    plitaBazalt,
    plitaMramor,
    plitaGranit,
    axe,
    axeGold,
    axeSilver,
    axeCooper,
    axeFerrum,
    kirka,
    kirkaGold,
    kirkaSilver,
    kirkaCooper,
    kirkaFerrum,
    lopata,
    lopataGold,
    lopataSilver,
    lopataCooper,
    lopataFerrum,
    molot,
    molotGold,
    molotSilver,
    molotCooper,
    molotFerrum,
    bucket,
    bucketGold,
    bucketSilver,
    bucketCooper,
    bucketFerrum,
    bottle,
    fullBottle,
    fakel,
    fakelForever,
    zazigalka,
    carForOil,
    carForOre,
    carForRock,
    cave,
    kolodec,
    koster,
    pilorama,
    sklad,
    rudokop,
    kamnetes,
    neftanik,
    lesorub,
    ferrumCooper,
    silverFerrum,
    silverGold,
    plavilna,
    stone
}
