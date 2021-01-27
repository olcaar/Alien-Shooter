using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Level : MonoBehaviour
{
    [SerializeField] float deathDelay = 3f;

    public void LoadGamerOver()
    {
        StartCoroutine(WaitAndLoad(deathDelay));
    }
    public void LoadGamerScene() { SceneManager.LoadScene("Game"); }
    public void LoadStartMenu()
    {
        FindObjectOfType<GameSession>().ResetGame();
        SceneManager.LoadScene("Start Menu");
    }
    public void QuitGame() { Application.Quit();}

    IEnumerator WaitAndLoad(float secsToDelay)
    {
        yield return new WaitForSeconds(secsToDelay);
        SceneManager.LoadScene("Game Over");
    }






}
