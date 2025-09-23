using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    
    public int Damage;
    public bool DestroyOnHit;

    public float KnockbackMag;



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {

            if (KnockbackMag != 0) { collision.gameObject.GetComponent<Player>().TakeKnockback(transform.position, KnockbackMag); }


            collision.gameObject.GetComponent<Player>().TakeDamage(Damage);


            if (DestroyOnHit)
            {
                Destroy(gameObject);
            }
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(Damage);
            if (DestroyOnHit)
            {
                Destroy(gameObject);
            }
        }
    }



}
