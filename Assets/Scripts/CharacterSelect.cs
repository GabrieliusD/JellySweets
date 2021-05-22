using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterSelect : MonoBehaviour
{
    public static CharacterSelect instance;
  
    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }
    public List<CharacterObject> characterObjects = new List<CharacterObject>();
    public GameObject CharIconTemplate;
    public Transform playerSelectionPanel;
    PlayerSlot playerSlot = null;
    public Transform playerSlotParent;
    public GameObject mainMenuCanvas;
    public GameObject menuPanel;
    public GameObject CharacterDetailPanel;
    public Sprite defaultHolder;
    private void Start()
    {
        for (int i = 0; i < characterObjects.Count; i++)
        {
            GameObject selectionIcon = Instantiate(CharIconTemplate, playerSelectionPanel);
            CharacterSlot cs = selectionIcon.GetComponent<CharacterSlot>();
            characterObjects[i].Equiped = false;
            cs.AddItem(characterObjects[i]);
            selectionIcon.GetComponent<Button>().onClick.AddListener(delegate { displayStats(cs); });
        }
    }

    public void SelectPlayerSlot(PlayerSlot slot)
    {
        if (slot.charobj == null)
        {
            playerSlot = slot;
            playerSelectionPanel.gameObject.SetActive(true);
            menuPanel.gameObject.SetActive(false);
        }
        else
        {
            //open character window
            CharacterObject charobj = slot.charobj;
            CharacterDetailPanel.SetActive(true);
            characterDetails cd = CharacterDetailPanel.GetComponent<characterDetails>();
            cd.Name.text = "Name: " + charobj.Name;
            cd.icon.sprite = charobj.icon;
            cd.Cost.text = "Cost: " + (100).ToString();
            cd.BaseHealth.text = "Health: " + (100).ToString();
            cd.Vit.text = "Vitality: " + charobj.vitality.ToString();
            cd.Strength.text = "Strength: " + charobj.strength.ToString();
            cd.Speed.text = "Speed: " + charobj.speed.ToString();
            cd.Intelligence.text = "Intelligence: " + charobj.intelligence.ToString();
            CharacterDetailPanel.SetActive(true);
            cd.button.onClick.RemoveAllListeners();
            cd.button.gameObject.transform.Find("Text").GetComponent<Text>().text = "Unequip Character";
            cd.Cost.gameObject.SetActive(false);
            cd.button.onClick.AddListener(delegate { UnequepCharacter(slot); });
        }

    }
    void UnequepCharacter(PlayerSlot slot)
    {
        slot.icon.sprite = defaultHolder;
        slot.charobj.Equiped = false;
        slot.charobj = null;
        Base.instance.removeCharacter();
        CharacterDetailPanel.SetActive(false);
    }
    public void displayStats(CharacterSlot characterslot)
    {
        CharacterObject charobj = characterslot.charobj;
        characterDetails cd = CharacterDetailPanel.GetComponent<characterDetails>();
        cd.Name.text = "Name: " + charobj.Name;
        cd.icon.sprite = charobj.icon;
        cd.Cost.text = "Cost: " + charobj.Cost.ToString();
        cd.BaseHealth.text = "Health: " + (charobj.vitality * 2).ToString();
        cd.Vit.text = "Vitality: " + charobj.vitality.ToString();
        cd.Strength.text = "Strength: " + charobj.strength.ToString();
        cd.Speed.text = "Speed: " + charobj.speed.ToString();
        cd.Intelligence.text = "Intelligence: " + charobj.intelligence.ToString();
        CharacterDetailPanel.SetActive(true);
        cd.button.onClick.RemoveAllListeners();
        if (charobj.Purchased)
        {
            if (!charobj.Equiped)
            {
                cd.button.onClick.AddListener(delegate { chooseCharacter(characterslot); });
               // charobj.Equiped = true;
            }
            else
            {
                cd.button.onClick.AddListener(delegate { ShowError(charobj.name); });

                Debug.Log("Character is already Equiped");
            }
            cd.button.gameObject.transform.Find("Text").GetComponent<Text>().text = "Equip Character";
            cd.Cost.gameObject.SetActive(false);

        }
        else
        {

                cd.button.onClick.AddListener(delegate { purchaseCharacter(charobj); });
                cd.Cost.gameObject.SetActive(true);
                cd.button.transform.Find("Text").GetComponent<Text>().text = "Purchase";

        }

    }
    public void ShowError(string name)
    {
        ErrorManager.instance.SetMessage("Character " + name + " is Already Equiped");
        ErrorManager.instance.ShowMessage();
    }
    public void purchaseCharacter(CharacterObject charobj)
    {
        if(ValueManager.instance.Purchase(charobj.Cost))
        {
            ValueManager.instance.updateText();
            CharacterDetailPanel.SetActive(false);
            charobj.Purchased = true;
        }
        else
        {
            ErrorManager.instance.SetMessage("Not Enough Sugar to Purchase this Character");
            ErrorManager.instance.ShowMessage();
            Debug.Log("Not Enough Sugar");
        }
    }
    public void BackToMainMenu()
    {
        playerSelectionPanel.gameObject.SetActive(false);
        menuPanel.gameObject.SetActive(true);
    }
    public void chooseCharacter(CharacterSlot characterslot)
    {
        if (Base.instance.AddCharacter())
        {
            CharacterObject character = characterslot.charobj;
            playerSlot.charobj = character;
            playerSlot.icon.sprite = character.icon;
            character.Equiped = true;
            //characterObjects.Remove(character);
            //characterslot.gameObject.SetActive(false);
            playerSelectionPanel.gameObject.SetActive(false);
            menuPanel.gameObject.SetActive(true);
            CharacterDetailPanel.SetActive(false);
        }

    }

    public void StartBattle()
    {
        PlayerSlot[] slots = playerSlotParent.gameObject.GetComponentsInChildren<PlayerSlot>();
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].charobj != null)
            {
                slots[i].player.GetComponent<Player>().charObj = slots[i].charobj;
                Instantiate(slots[i].player, slots[i].spawnLocation.transform.position, Quaternion.identity);
            }
        }
        mainMenuCanvas.SetActive(false);
    }

    public void unSpawnCharacters()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject p in players)
        Destroy(p);
    }
    
    public void closeCharacterDetails()
    {
        CharacterDetailPanel.SetActive(false);
    }
}
