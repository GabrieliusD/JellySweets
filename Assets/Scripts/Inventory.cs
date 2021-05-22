using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item[] randomItems;
    public static Inventory instance;
    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
    }

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public List<Item> items = new List<Item>();
    public void Add(Item item)
    {
        if (!item.isDefaultItem)
        {
            items.Add(item);
        }
        if(onItemChangedCallback != null)
             onItemChangedCallback.Invoke();
    }

    public void AddTest()
    {
        int randomItem = Random.Range(0, randomItems.Length);
        if (!randomItems[randomItem].isDefaultItem)
        {
            items.Add(randomItems[randomItem]);
        }
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
    public void Remove(Item item)
    {
        items.Remove(item);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
