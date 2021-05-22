using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Equipment", menuName ="inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;
    public int armorModifier;
    public int damageModifier;
    public override void Use()
    {
        base.Use();
        equipmentManager em = GameObject.FindObjectOfType<equipmentManager>();
        em.Equip(this);
        RemoveFromInvetory();
    }
}

public enum EquipmentSlot { Helmet, Armor, Weapon, Boots }
