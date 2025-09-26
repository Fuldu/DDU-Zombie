using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public int CurrentLevel;

    public GameObject ActiveLevelParent;
    public GameObject Player;


    public List<GameObject> LevelPrefabs;



    [Header("UI")]

    public GameObject GameOverScreen;


    // Start is called before the first frame update
    void Start()
    {
        MakeNewLevel();
    }



    public void GameOver()
    {
        GameOverScreen.SetActive(true);
    }


    public void CompleteLevel()
    {
        if (CurrentLevel + 1 > LevelPrefabs.Count)
        {
            WinGame();
        }
        else
        {
            CurrentLevel++;
            MakeNewLevel();

        }


    }


    public void MakeNewLevel()
    {
        Destroy(ActiveLevelParent);



        GameObject newLevel = Instantiate(LevelPrefabs[CurrentLevel - 1], Vector2.zero, Quaternion.identity);
        ActiveLevelParent = newLevel;


        ResetState();
    }


    public void ResetState()
    {
        Player.transform.position = Vector2.zero;
    }


    private void WinGame()
    {
        Debug.Log("Spiller vandt");
    }


}
