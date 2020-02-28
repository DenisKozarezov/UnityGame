using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemsBank
{
    public static Item[] Items = new Item[10];

    public static Item GetItemByName(string _name)
    {
        foreach (Item item in Items)
        {
            if (item != null & item.Name == _name) return item;
        }
        return null;
    }
}

[RequireComponent(typeof(Unit))]
public class Item : MonoBehaviour
{
    public enum ItemType { HasCharges, Disposable, Usable }
    public string Name { set; get; }
    public string Description { set; get; }
    public int Charges { set; get; }
    ItemType Type { set; get; }
    Unit Owner { set; get; }

    public delegate void ChargesAction();
    public delegate void UsableAction();

    ChargesAction CAction;
    UsableAction UAction;

    public void AddAction(ChargesAction _action)
    {
        CAction = _action;
    }
    public void AddAction(UsableAction _action)
    {
        UAction = _action;
    }
    public void Use()
    {
        switch (Type)
        {
            case ItemType.Disposable:
                break;
            case ItemType.HasCharges:
                break;
            case ItemType.Usable:
                break;
        }
    }
}
