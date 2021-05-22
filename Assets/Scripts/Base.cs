using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
public class Base : MonoBehaviour
{
    public GameObject enemyspawner;
    public static Base instance;
    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    public delegate void BaseUpgraded();
    public BaseUpgraded onBaseUpgradedCallback;

    public GameObject uiPrefab;
    public Transform parent;
    float currentHealth;
    public GameObject gameOverUI;
    int numOfCharactersInBase = 0;
    int capacity = 1;
    float defense;
    float healthRegen;
    public float sugarPercent;
    float xpPercent;
    float maxHealth;
    public Text baseText;

    List<BaseUpgrade> baseUpgrades = new List<BaseUpgrade>();

    BaseCapacity baseCapacity = new BaseCapacity("Base Capacity", "increases the amount of characters in the base", 1, 100);
    HealthUpgrade healthUpgrade = new HealthUpgrade("Health", "increases health", 1, 100);
    BaseDefense baseDefense = new BaseDefense("Defense", "reduces the damage base takes", 1, 100);
    HealthReg healthReg = new HealthReg("Health Regeneration", "Increases the amount of health you get after completing a night", 1, 100);
    SugarCollector sugarCollector = new SugarCollector("Sugar Collector", "Increases the amount of sugar you get from enemies", 1, 100);
    XPCollector xpCollector = new XPCollector("XP Collector", "Increases the amount of xp enemies drop", 1, 100);
    GameObject[] upgradePanel;
    void Start()
    {
        currentHealth = 100;
        TakeDamage(0);
        baseUpgrades.Add(baseCapacity);
        baseUpgrades.Add(healthUpgrade);
        baseUpgrades.Add(baseDefense);
        baseUpgrades.Add(healthReg);
        baseUpgrades.Add(sugarCollector);
        baseUpgrades.Add(xpCollector);
        upgradePanel = new GameObject[baseUpgrades.Count];
        for (int i = 0; i < baseUpgrades.Count; i++)
        {
            BaseUpgradeContainer bus = uiPrefab.GetComponent<BaseUpgradeContainer>();
            bus.Description.text = baseUpgrades[i].Description;
            bus.CostText.text = "Cost: " + baseUpgrades[i].UpgradeCost.ToString();
            bus.icon.sprite = null;
            upgradePanel[i] = Instantiate(uiPrefab, parent);
            upgradePanel[i].gameObject.GetComponent<Button>().onClick.AddListener(baseUpgrades[i].levelUp);
            upgradePanel[i].gameObject.GetComponent<Button>().onClick.AddListener(delegate { AudioManager.instance.Play("ButtonClick"); });

        }
        onBaseUpgradedCallback += updatePanelText;
    }
    public bool AddCharacter()
    {
        if (capacity > numOfCharactersInBase)
        {
            numOfCharactersInBase++;
            return true;
        }
        else return false;
    }
    public void removeCharacter()
    {
        numOfCharactersInBase--;
    }
    void updatePanelText()
    {
        int i = 0;
        foreach(GameObject p in upgradePanel)
        { 
            BaseUpgradeContainer bus = p.gameObject.GetComponent<BaseUpgradeContainer>();
            bus.Description.text = baseUpgrades[i].Description;
            bus.CostText.text = "Cost " + baseUpgrades[i].UpgradeCost.ToString();
            i++;
        }
        ApplyUpgrades();
        TakeDamage(0);
    }
    private void Update()
    {
        
    }

    public int GetNumCharacters()
    {
        return numOfCharactersInBase;
    }
    public void ApplyUpgrades()
    {
        currentHealth = healthUpgrade.upgradeEffect();
        maxHealth = currentHealth;
        capacity = (int)baseCapacity.upgradeEffect();
        healthRegen = healthReg.upgradeEffect();
        defense = baseDefense.upgradeEffect();
        sugarPercent = sugarCollector.upgradeEffect();
        xpPercent = xpCollector.upgradeEffect();
        TakeDamage(0);
    }
    public void RegenerateHealth()
    {
        //after each round regenarates some health;
        currentHealth += Mathf.Clamp(healthReg.upgradeEffect(), 0, maxHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= Mathf.Clamp(amount-defense, 0, 100);
        baseText.text = ((int)currentHealth).ToString();
        BaseDestroyed();

    }
    public void BaseDestroyed()
    {
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            //game over screen 
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (var e in enemies)
            {
                Destroy(e.gameObject);
            }
            gameOverUI.SetActive(true);
            ValueManager.instance.gameoverScreen();
            enemyspawner.SetActive(false);
            Debug.Log("Base Destroyed");
        }
    }

}

public class BaseUpgrade
{
    public string Name;
    public string Description;
    public int Level;
    public int UpgradeCost;

    public BaseUpgrade(string name, string desc, int level, int upgradeCost)
    {
        Name = name;
        Description = desc;
        Level = level;
        UpgradeCost = 100;
    }
    public void levelUp()
    {
        
        if(ValueManager.instance.whiteSugar >= UpgradeCost)
        {
            ValueManager.instance.whiteSugar -= UpgradeCost;
            upgradeCostIncrease();
            Level += 1;

            Base.instance.onBaseUpgradedCallback.Invoke();
            ValueManager.instance.updateText();


            Dictionary<string, object> dUpgrade = new Dictionary<string, object>();
            dUpgrade.Add("Upgrade " + Name, Level);
            AnalyticsResult result = Analytics.CustomEvent("Upgrade", dUpgrade);
            Debug.Log(result);

        }
        else
        {
            ErrorManager.instance.SetMessage("Not Enough Sugar to Purchase " + Name);
            ErrorManager.instance.ShowMessage();
            Debug.Log("Not enaugh sugar for " + Name);
        }
    }

    public virtual float upgradeEffect()
    {
        return 0;
        //upgrade effects
    }
    public virtual void upgradeCostIncrease()
    {
        
        UpgradeCost = Mathf.RoundToInt(UpgradeCost * Mathf.Pow(1.07f, Level));
    }


}

public class HealthUpgrade : BaseUpgrade
{
    int health = 100;
    int healthPerLevel = 10;

    public HealthUpgrade(string name, string desc, int level, int upgrade) : base(name, desc, level, upgrade)
    {
        
    }
    public override float upgradeEffect()
    {
        base.upgradeEffect();
        return health + healthPerLevel * (Level-1);
    }
    public override void upgradeCostIncrease()
    {
        base.upgradeCostIncrease();
    }
}

public class BaseCapacity : BaseUpgrade
{
    public BaseCapacity(string name, string desc, int level, int upgrade) : base(name, desc, level, upgrade)
    {

    }
    public override float upgradeEffect()
    {
        return 1 * Level;

    }
    public override void upgradeCostIncrease()
    {
        base.upgradeCostIncrease();
        UpgradeCost = UpgradeCost * 10;
    }

}

public class BaseDefense : BaseUpgrade
{
    public BaseDefense(string name, string desc, int level, int upgrade) : base(name, desc, level, upgrade)
    {

    }

    public float defensePerLevel = 0.10f;
    public override float upgradeEffect()
    {
        return (Level-1) * defensePerLevel;
    }
    public override void upgradeCostIncrease()
    {
        base.upgradeCostIncrease();
    }
}

public class HealthReg : BaseUpgrade
{
    float healthRegenPerLevel = 5.0f;
    public HealthReg(string name, string desc, int level, int upgrade) : base(name, desc, level, upgrade)
    {

    }

    public override float upgradeEffect()
    {
        return (Level - 1) * (int)healthRegenPerLevel;
       
    }
    public override void upgradeCostIncrease()
    {
        base.upgradeCostIncrease();
    }
}

public class SugarCollector : BaseUpgrade
{
    float sugarIncreasePerLevel = 0.5f;
    public SugarCollector(string name, string desc, int level, int upgrade) : base(name, desc, level, upgrade)
    {

    }

    public override float upgradeEffect()
    {
        return (Level - 1) * sugarIncreasePerLevel;
    }
    public override void upgradeCostIncrease()
    {
        base.upgradeCostIncrease();
    }
}

public class XPCollector : BaseUpgrade
{
    float xpIncreasePerlevel = 0.5f;
    public XPCollector(string name, string desc, int level, int upgrade) : base(name, desc, level, upgrade)
    {

    }

    public override float upgradeEffect()
    {
        return (Level - 1) * xpIncreasePerlevel;
    }
    public override void upgradeCostIncrease()
    {
        base.upgradeCostIncrease();
    }
}




