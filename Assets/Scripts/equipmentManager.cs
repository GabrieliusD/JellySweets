using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equipmentManager : MonoBehaviour
{
    public Transform equipmentCanvas;
    Equipment[] currentEquipment;
    Inventory inventory;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;
    private void Start()
    {
        inventory = Inventory.instance;
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
    }
    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;
        Equipment oldItem = null;
        if(currentEquipment[slotIndex]!=null)
        {
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);
        }

        if(onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
        currentEquipment[slotIndex] = newItem;
        UpdateEquipmentSlots();
    }
    public void Unequip(int slotIndex)
    {
        if(currentEquipment[slotIndex]!=null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);
            currentEquipment[slotIndex] = null;

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }
        }
    }

    public void UpdateEquipmentSlots()
    {
         equipmentSlot[] slots = equipmentCanvas.GetComponentsInChildren<equipmentSlot>();
        for (int i = 0; i < slots.Length; i++)
        {
            if(currentEquipment[i] != null)
            {
                 if(slots[i].equipSlot == currentEquipment[i].equipSlot)
                 {
                    slots[i].AddItem(currentEquipment[i]);
                 }
            }

        }
    }
}
