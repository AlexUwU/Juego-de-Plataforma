using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{
    public enum pickupType { coin,gem,health}

    public pickupType pt;
    [SerializeField] public GameObject PickupEffect;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(pt == pickupType.coin)
        {
            if(collision.gameObject.tag == "Player")
            {
                GameManager.instance.IncrementCoinCount();
           
                Instantiate(PickupEffect, transform.position, Quaternion.identity);

                Destroy(this.gameObject);
                
            }
            
        }

        if (pt == pickupType.gem)
        {
            if (collision.gameObject.tag == "Player")
            {
                GameManager.instance.IncrementGemCount();
            
                Instantiate(PickupEffect, transform.position, Quaternion.identity);

                Destroy(this.gameObject, 0.2f);

            }

        }
    }
}
