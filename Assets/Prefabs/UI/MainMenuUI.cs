using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] UISwitcher menuSwitcher;
    [SerializeField] Transform mainMenu;
    [SerializeField] Transform howToPlayMenu;
    [SerializeField] Transform creditsMenu;

    
   public void StartGame()
    {
        GameplayStatics.GetGameMode().LoadFirstLevel();
    }
   
   public void BackToMainMenu()
   {
       menuSwitcher.SetActiveUI(mainMenu);
   }
   
   public void HowToPlayMenu()
   {
       menuSwitcher.SetActiveUI(howToPlayMenu);
   }
   
   public void CreditsMenu()
   {
       menuSwitcher.SetActiveUI(creditsMenu);
   }

   public void QuitGame()
   {
       GameplayStatics.GetGameMode().QuitGame();
       Debug.Log("Made a quit");
   }
   
   
}
