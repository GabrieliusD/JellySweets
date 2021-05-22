using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
public class GameFlowManager : MonoBehaviour
{
    public Transform MainMenu;
    public Transform GameplayeMenu;
    public Transform BaseUpgradeMenu;
    public Transform playerSelect;
    public Transform MainMenuSelection;
    public GameObject enemySpawner;
    public Transform ControlUI;
    public Transform playableCharacter;
    public Transform settingUI;
    public Transform InstructionsUI;
    public Transform CreditUI;
    public SelectWithMouse mouse;
    public void BattleStart()
    {
        if (Base.instance.GetNumCharacters() > 0)
        {
            mouse.setCharSelected();
            CharacterSelect.instance.StartBattle();
            MainMenu.gameObject.SetActive(false);
            GameplayeMenu.gameObject.SetActive(true);
            enemySpawner.SetActive(true);
            Base.instance.ApplyUpgrades();
            AudioManager.instance.Stop("MainMenu");
            AudioManager.instance.Play("Gameplay");
        } else
        {
            ErrorManager.instance.SetMessage("Equip a Character Before Starting a Battle");
            ErrorManager.instance.ShowMessage();
        }
        
    }

    public void BackToMainMenu()
    {
        MainMenu.gameObject.SetActive(true);
        GameplayeMenu.gameObject.SetActive(false);
        enemySpawner.SetActive(false);
        Base.instance.gameOverUI.SetActive(false);
        ControlUI.gameObject.SetActive(false);
        ValueManager.instance.resetScoreScreen();
        AudioManager.instance.Play("MainMenu");
        AudioManager.instance.Stop("Gameplay");
        playableCharacter.position = new Vector2(-2, 0);
        playableCharacter.gameObject.SetActive(false);
    }

    public void BackToMainMenuGameOver()
    {
        MainMenu.gameObject.SetActive(true);
        GameplayeMenu.gameObject.SetActive(false);
        enemySpawner.SetActive(false);
        Base.instance.gameOverUI.SetActive(false);
        ControlUI.gameObject.SetActive(false);
        ValueManager.instance.whiteSugarEarned = 0;
        ValueManager.instance.updateText();
        AudioManager.instance.Play("MainMenu");
        AudioManager.instance.Stop("Gameplay");
        playableCharacter.position = new Vector2(-2, 0);
        playableCharacter.gameObject.SetActive(false);

        Dictionary<string, object> dNight = new Dictionary<string, object>();
        dNight.Add("Night", ValueManager.instance.Night);
        AnalyticsResult result = Analytics.CustomEvent("Night Failed", dNight);
        Debug.Log(result);
    }
    public void TryAgain()
    {
        ValueManager.instance.resetToTryAgain();
    }
    public void OpenBaseUpgradeMenu()
    {
        BaseUpgradeMenu.gameObject.SetActive(true);
        playerSelect.gameObject.SetActive(false);
        MainMenuSelection.gameObject.SetActive(false);
    }
    public void CloseBaseUpgradeMenu()
    {
        BaseUpgradeMenu.gameObject.SetActive(false);
        playerSelect.gameObject.SetActive(true);
        MainMenuSelection.gameObject.SetActive(true);

    }

    public void ShowSettingUI()
    {
        settingUI.gameObject.SetActive(true);
    }

    public void CloseSettingUI()
    {
        settingUI.gameObject.SetActive(false);
    }

    public void ShowInstructions()
    {
        InstructionsUI.gameObject.SetActive(true);
    }
    public void CloseInstructions()
    {
        InstructionsUI.gameObject.SetActive(false);
    }

    public void ShowCredits()
    {
        CreditUI.gameObject.SetActive(true);
    }

    public void CloseCredits()
    {
        CreditUI.gameObject.SetActive(false);
    }

    
}
