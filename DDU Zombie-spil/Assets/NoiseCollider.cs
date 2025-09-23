using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseCollider : MonoBehaviour
{

    public float Radius;
    public float Lifetime;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<CircleCollider2D>().radius = Radius;

        if (Lifetime != 0)
        {
            StartCoroutine(Lifespan());
        }
    }



    private IEnumerator Lifespan()
    {
        yield return new WaitForSeconds(Lifetime);
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            collision.GetComponent<Enemy>().Alert();
        }
    }



}
