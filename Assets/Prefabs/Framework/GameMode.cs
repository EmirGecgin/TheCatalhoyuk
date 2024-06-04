using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    public delegate void OnGameOver();
    [SerializeField] int MainMenuSceneBuildIndex=0;
    [SerializeField] int firstLevelIndex=1;
    public event OnGameOver onGameOver;
    bool bIsGamePaused=false;
    bool bIsGameOver = false;
    public void GameOver()
    {   
        SetGamePaused(true);
        bIsGameOver = true;
        onGameOver?.Invoke();
    }
    
    public void SetGamePaused(bool bIsPaused)
    {
        if(bIsGameOver) { return; }
        bIsGamePaused = bIsPaused;
        if (bIsPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    internal void TogglePause()
    {
        if (IsGamePaused())
        {
            SetGamePaused(false);
        }
        else
        {
            SetGamePaused(true);
        }
    }
    public bool IsGamePaused()
    {
        return bIsGamePaused;
    }
    public void BackToMainMenu()
    {
        LoadScene(MainMenuSceneBuildIndex);
    }
    private void LoadScene(int sceneBuildIndex)
    {
        bIsGameOver = false;
        SetGamePaused(false);
        SceneManager.LoadScene(sceneBuildIndex);
    }

    internal void RestartCurrentLevel()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    internal bool isGameOver()
    {
        return bIsGameOver;
    }

    internal void LoadFirstLevel()
    {
        LoadScene(firstLevelIndex);
    }
    public void QuitGame()
    {
       Application.Quit();
    }
}
