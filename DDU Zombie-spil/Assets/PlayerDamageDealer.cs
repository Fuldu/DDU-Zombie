using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageDealer : MonoBehaviour
{


    public int Damage;
    public bool DestroyOnHit;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            collision.GetComponent<Enemy>().TakeDamage(Damage);
            if (DestroyOnHit) { Destroy(gameObject); }
        }
    }




}
