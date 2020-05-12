using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Item
{
    public enum ItemType { ACTIVE, PASSIVE }
    public enum ItemCharacteristic { PERMANENT, EXHAUSTIBLE }
    public ItemCharacteristic Characteristic { set; get; } = ItemCharacteristic.PERMANENT;
    public string Name { set; get; }
    public string Description { set; get; }
    public byte Charges { set; get; }
    public ItemType Type { set; get; }
    public float Cooldown { set; get; }
    public float Probability { set; get; }
    public SerializedSprite Icon;

    public Item(string _name)
    {
        Name = _name;
    }
    public void Use()
    {
        
    }
}
