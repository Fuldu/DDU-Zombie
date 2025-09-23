using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{

    public GameObject NoiseColliderPre;



    public void MakeNoise(float radius)
    {

        GameObject newNoiseObj = Instantiate(NoiseColliderPre, transform.position, Quaternion.identity);

        newNoiseObj.GetComponent<CircleCollider2D>().radius = radius;
        



    }
 

}
