using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterSlot : MonoBehaviour
{
    public CharacterObject charobj;
    public Image icon;
    public void AddItem(CharacterObject newItem)
    {
        charobj = newItem;
        icon.sprite = newItem.icon;
        icon.enabled = true;
        //removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        charobj = null;
        icon.sprite = null;
        icon.enabled = false;

        // removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        //Inventory.instance.Remove(item);
    }

    public void UseItem()
    {
        if (charobj != null)
        {
            //item.Use();
        }
    }
}
