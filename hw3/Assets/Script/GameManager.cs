using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public GameObject levelCompleteUI;
    public GameObject ingameMenu;
    public GameObject playerDeadthUI;
    public Player player;
    public void CompleteLevel()
    {
        //Debug.Log("LEVEL WON!");
        //levelCompleteUI.SetActive(true);
    }
    public void PlayerDeath()
    {
        Time.timeScale = 0;
        playerDeadthUI.SetActive(true);
    }
    public void GameStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void GameExit()
    {
        Application.Quit();
    }

    public void GameRestart()
    {
        Time.timeScale = 1f;
        playerDeadthUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GamePause()
    {
        Time.timeScale = 0;
        ingameMenu.SetActive(true);
    }

    public void GameResume()
    {
        Time.timeScale = 1f;
        ingameMenu.SetActive(false);
        player.gamePaused = false;
    }
}
