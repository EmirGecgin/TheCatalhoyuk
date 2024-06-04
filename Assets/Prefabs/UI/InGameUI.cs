using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI meterText;
    [SerializeField] private UISwitcher menuSwitcher;
    [SerializeField] private Transform inGameUI;
    [SerializeField] private Transform pauseUI;
    [SerializeField] private Transform gameOverUI;
    public int currentMeter = 0;


    private void Start()
    {
        ScoreKeeper scoreKeeper = FindObjectOfType<ScoreKeeper>();
        if (scoreKeeper != null)
        {
            scoreKeeper.onScoreChanged += UpdateScoreText;
        }

        // Oyun ba�lad���nda metreyi g�ncelleyin
        StartCoroutine(UpdateMeterText());

        GameplayStatics.GetGameMode().onGameOver += OnGameOver;
    }

    private void OnGameOver()
    {
        menuSwitcher.SetActiveUI(gameOverUI);
    }

    private void UpdateScoreText(int newVal)
    {
        scoreText.SetText($"{newVal}");
    }

    private IEnumerator UpdateMeterText()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.12f); // Her saniye �al��acak

            // Metreyi art�r ve g�ncelle
            currentMeter += 1;
            meterText.SetText(currentMeter.ToString("D5"));
        }
    }

    

    internal void SignalPause(bool isGamePaused)
    {
        if (isGamePaused)
        {
            menuSwitcher.SetActiveUI(pauseUI); 
        }
        else {             
            menuSwitcher.SetActiveUI(inGameUI);
        }
    }
    public void ResumeGame()
    {
        GameplayStatics.GetGameMode().SetGamePaused(false);
        menuSwitcher.SetActiveUI(inGameUI); 
    }
    public void BackToMainMenu()
    {
        GameplayStatics.GetGameMode().BackToMainMenu();
    }
    public void RestartCurrentLevel()
    {
        GameplayStatics.GetGameMode().RestartCurrentLevel();
    }
}
