using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
public class ValueManager : MonoBehaviour
{
    public static ValueManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }
    public Text gameplaySugar;
    public Text scoreScreenSugar;
    public Text gameOverScreenSugar;
    public Transform scoreScreen;
    public Text whiteSugarText;
    public Text brownSugarText;
    public Text NightText;
    public Text WaveText;
    public int whiteSugarEarned = 0;
    public int whiteSugar = 0;
    public int brownSugar = 0;
    public int Night = 1;
    int NumOfEnemiesInArena = 0;

    // Start is called before the first frame update
    void Start()
    {
        updateText();
    }

    // Update is called once per frame
    void Update()
    {
        updateGameplaySugarEarned();
    }

    public void updateText()
    {
        whiteSugarText.text = whiteSugar.ToString();
        brownSugarText.text = brownSugar.ToString();
        NightText.text = "Night: " + Night.ToString();
    }
    public void enemyDeath()
    {
        NumOfEnemiesInArena--;
    }
    public void enemyInBattle()
    {
        NumOfEnemiesInArena++;
    }
    
    public void addToEarned(int amount)
    {
        whiteSugarEarned += amount;
        whiteSugar += amount;
    }
    public void checkIfAllEnemiesDefeated()
    {
        if(NumOfEnemiesInArena<=0)
        {
            scoreScreen.gameObject.SetActive(true);
            scoreScreenSugar.text = "Sugar Earned: " + whiteSugarEarned.ToString();
            Debug.Log("all enemies Dead");
        }
    }

    public void gameoverScreen()
    {
        gameOverScreenSugar.text = "Sugar Earned: " + whiteSugarEarned.ToString();
        NumOfEnemiesInArena = 0;
    }
    public void resetScoreScreen()
    {
        Dictionary<string, object> dNight = new Dictionary<string, object>();
        dNight.Add("Night", Night);
        AnalyticsResult result = Analytics.CustomEvent("Night Completed", dNight);
        Debug.Log(result);
        scoreScreen.gameObject.SetActive(false);
        whiteSugarEarned = 0;
        Night++;
        updateText();
    }

    public void resetToTryAgain()
    {
        Base.instance.gameOverUI.SetActive(false);
        whiteSugarEarned = 0;
        updateText();
    }

    public void GameOverScreen()
    {
        
    }
    void updateGameplaySugarEarned()
    {
        gameplaySugar.text = whiteSugarEarned.ToString();
    }
    public void WaveTextUpdate(int currentWave)
    {
        WaveText.text = "Wave " + (currentWave) + "/5";
    }

    public bool Purchase(int cost)
    {
        if (cost <= whiteSugar)
        {
            whiteSugar -= cost;
            return true;
        }
        else return false;
    }
}
