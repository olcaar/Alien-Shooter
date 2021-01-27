using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] int gameScore = 0;


    private void Awake()
    {
        SetUpSingleton();
    }
    void Update()
    {
        
    }
    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    public void AddToScore(int enemyScore)
    {
        gameScore += enemyScore;
    }
    public int GetGameScore()
    {
        return gameScore;
    }
    public void ResetGame()
    {
        gameScore = 0;
    }
}
