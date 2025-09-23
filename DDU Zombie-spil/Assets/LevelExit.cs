using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{


    GameController gc;


    // Start is called before the first frame update
    void Start()
    {
        gc = FindObjectOfType<GameController>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            gc.CompleteLevel();

            // To be sure
            Destroy(gameObject);
        }
    }



}
