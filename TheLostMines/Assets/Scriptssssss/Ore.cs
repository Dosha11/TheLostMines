using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    [SerializeField] float hp = 0;
    [SerializeField] GameObject chunk;
    public Dot myDot;
   
    public void TakeDamage(float damage)
    {
      
        if (hp <= damage)
        {
            GameObject clone= Instantiate(chunk,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
           // myDot.Collapse((int)transform.position.y / 10);
            hp -=damage;
        }
    }

    [ContextMenu("TakeDamage")]
    public void Dead()
    {
    //    myDot.Collapse(Mathf.Abs( (int)(transform.position.y / 10)) );
        
        GameObject clone = Instantiate(chunk, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }



    private void OnDestroy()
    {
      //  GameObject clone = Instantiate(chunk, transform.position, Quaternion.identity);
    }
}
