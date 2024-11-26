using System.Collections;
using UnityEngine;


public class MarkerParent : MonoBehaviour
{
    [SerializeField] ItemType instrument;
    [SerializeField] GameObject PrefabItem;
    [SerializeField] int hp;

    public void CollisionOre()
    {
        Debug.Log("FFFFFFFFFFFF");
        ItemType tool = Inventory.Instance.CheckTools(instrument);
        if (tool != ItemType.none)
        {
            Debug.Log("kjk");
            hp--;
            Inventory.Instance.RemoveTools(tool);
            if (hp == 0) 
            {
                for (int i = 0; i < 8; i++)
                {
                    GameObject clone = Instantiate(PrefabItem,transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }
        }
    }

}
