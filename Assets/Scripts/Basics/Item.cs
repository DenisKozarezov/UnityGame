using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

public class Item : MonoBehaviour
{
    public enum ItemType { HasCharges, Disposable, Usable }
    public string Name { set; get; }
    public string Description { set; get; }
    public int Charges { set; get; }
    ItemType Type { set; get; }
    Unit Owner { set; get; }

    public bool ShowName;
    private GameObject TextName;

    public delegate void ChargesAction();
    public delegate void UsableAction();

    ChargesAction CAction;
    UsableAction UAction;

    private void Update()
    {
        Show();
    }

    public void Show()
    {
        if (ShowName)
        {
            Vector3 position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, GetComponent<Collider2D>().bounds.size.y / 2));
            if (TextName == null)
            {           
                GameObject name = new GameObject();
                TextName = name;
                name.AddComponent<Text>();
                name.GetComponent<RectTransform>().position = position;
                name.GetComponent<Text>().text = gameObject.name;
                name.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                name.GetComponent<Text>().font = Resources.Load<Font>("UI/Fonts/CONSOLA");
                name.transform.parent = GameObject.Find("UI Text").transform;
            }
            else
            {
                TextName.GetComponent<RectTransform>().position = position;
            }
        }
        else
        {
            Destroy(TextName);
        }
    }
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
