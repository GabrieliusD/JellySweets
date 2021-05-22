
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;

    public virtual void Use()
    {
        Debug.Log("Using ITEM");
    }
    public void RemoveFromInvetory()
    {
        Inventory.instance.Remove(this);
    }
}
