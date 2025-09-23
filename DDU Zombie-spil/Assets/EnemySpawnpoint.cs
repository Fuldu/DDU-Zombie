using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnpoint : MonoBehaviour
{

    public GameObject EnemyPre;


    // Start is called before the first frame update
    void Start()
    {
        Instantiate(EnemyPre, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }


}
