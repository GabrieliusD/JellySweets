using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Characters/Character")]
public class CharacterObject : ScriptableObject
{
    public string Name;
    public Sprite icon;
    public int Level;
    public float Health;
    public bool Purchased;
    public bool Equiped;
    public int Cost;
    public float currentXP;
    public float xpNeeded;
    public int strength;
    public int speed;
    public int intelligence;
    public int vitality;
}
