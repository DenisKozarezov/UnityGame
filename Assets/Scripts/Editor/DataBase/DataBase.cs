﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
class DataObject
{
    public List<Ability> Abilities { private set; get; } = new List<Ability>();
    public List<Item> Items { private set; get; } = new List<Item>();

    public DataObject(List<Ability> abilities, List<Item> items)
    {
        Abilities = abilities;
        Items = items;
    }
}

[Serializable]
public class DataBase : EditorWindow
{
    const short ButtonWidth = 80, ButtonHeight = 15;

    const short AreaWidth = 205, AreaHeight = 495;
    const short AreaPadding = 30;

    const short ArrowSize = 25;

    Vector2 ContentScrollPos;   
    const short ContentPadding = 80;
    const short ContentWidth = 200, ContentHeight = 200;
    const short ContentItemHeight = 15;

    object CurrentItem;
    int CurrentIndex = 0;
        
    string OptionName = "Способности";
    string Name;
    [TextArea(0, 5)]
    private string Description;
    float Cooldown;
    byte Range;
    Sprite Icon;
    SerializedSprite Sprite;

    enum OptionType { ABILITIES, ITEMS }
    OptionType ContentType = OptionType.ABILITIES;
    
    int AbilityType;
    int AbilityTarget;

    int ItemType;
    int ItemCharacteristic;
    byte ItemCharges;
    float ItemProbability;

    string Find = "";
    List<object> FindList = new List<object>();
    List<int> FindListIndexes = new List<int>();
    
    GUIStyle DefaultItemStyle = new GUIStyle();

    List<Ability> Abilities = new List<Ability>();
    List<Item> Items = new List<Item>();

    string dataBasePath = "Assets/DataBase.data";

    [MenuItem("Tools/Game Data")]
    public static void ShowWindow()
    {
        GetWindow<DataBase>("Game Data");
    }
    public void OnEnable()
    {
        if (System.IO.File.Exists(dataBasePath) && System.IO.File.ReadAllText(dataBasePath).Length > 0)
        {
            DataObject load = Load();
            Abilities = load.Abilities;
            Items = load.Items;
        }        
    }
    public void OnDisable()
    {
        Save();
    }
    public void OnGUI()
    {
        #region ИНИЦИАЛИЗАЦИЯ СТИЛЕЙ
        GUIStyle contentBackground = new GUIStyle();
        contentBackground.normal.background = Texture2D.grayTexture;
        contentBackground.alignment = TextAnchor.MiddleCenter;
        contentBackground.fontStyle = FontStyle.Bold;
        contentBackground.fontSize = 14;
        contentBackground.normal.textColor = Color.white;

        GUIStyle propertyStyle = new GUIStyle();
        propertyStyle.alignment = TextAnchor.MiddleLeft;
        propertyStyle.fontStyle = FontStyle.Bold;
        propertyStyle.fontSize = 14;

        DefaultItemStyle.alignment = TextAnchor.MiddleCenter;
        DefaultItemStyle.normal.background = Texture2D.grayTexture;
        DefaultItemStyle.normal.textColor = Color.white;
        #endregion

        GUILayout.BeginHorizontal();

        /* ==== БАЗА ДАННЫХ ==== */
        GUILayout.BeginArea(new Rect(AreaPadding, AreaPadding, AreaWidth, AreaHeight));
        GUILayout.BeginVertical("box", GUILayout.Width(AreaWidth - ContentWidth));
        #region ОПЦИИ
        EditorGUILayout.BeginHorizontal();
        GUI.Label(new Rect(0, 0, AreaWidth, ArrowSize), OptionName, contentBackground);

        if (GUI.Button(new Rect(0, 0, ArrowSize, ArrowSize), "<"))
        {
            CurrentItem = null;
            Icon = null;
            switch (ContentType)
            {
                case OptionType.ABILITIES:
                    ContentType = OptionType.ITEMS;
                    if (Items.Count > 0)
                    {
                        CurrentItem = Items[0];
                        ShowItemInfo(Items[0]);                        
                        SelectItem(OptionType.ITEMS, 0);
                    }
                    OptionName = "Предметы";
                    break;
                case OptionType.ITEMS:
                    ContentType = OptionType.ABILITIES;
                    if (Abilities.Count > 0)
                    {
                        CurrentItem = Abilities[0];
                        ShowItemInfo(Abilities[0]);
                        SelectItem(OptionType.ABILITIES, 0);
                    }
                    OptionName = "Способности";
                    break;
            }
            CurrentIndex = 0;
        }
        if (GUI.Button(new Rect(AreaWidth - ArrowSize, 0, ArrowSize, ArrowSize), ">"))
        {
            CurrentItem = null;
            Icon = null;
            switch (ContentType)
            {
                case OptionType.ABILITIES:
                    ContentType = OptionType.ITEMS;
                    if (Items.Count > 0)
                    {
                        CurrentItem = Items[0];
                        ShowItemInfo(Items[0]);
                        SelectItem(OptionType.ITEMS, 0);
                    }
                    OptionName = "Предметы";
                    break;
                case OptionType.ITEMS:
                    ContentType = OptionType.ABILITIES;
                    if (Abilities.Count > 0)
                    {
                        CurrentItem = Abilities[0];
                        ShowItemInfo(Abilities[0]);
                        SelectItem(OptionType.ABILITIES, 0);
                    }
                    OptionName = "Способности";
                    break;
            }
            CurrentIndex = 0;
        }              
        EditorGUILayout.EndHorizontal();
        #endregion

        #region КНОПКИ МАНИПУЛИРОВАНИЯ
        EditorGUILayout.BeginVertical();
        if (GUI.Button(new Rect(0, ContentPadding - ButtonHeight - ContentItemHeight / 2, ButtonWidth, ButtonHeight), "Добавить"))
        {
            switch (ContentType)
            {
                case OptionType.ABILITIES:
                    Abilities.Add(new Ability("Новая способность"));
                    break;
                case OptionType.ITEMS:
                    Items.Add(new Item("Новый предмет"));
                    break;
            }
        }
        if (GUI.Button(new Rect(AreaWidth - ButtonWidth, ContentPadding - ButtonHeight - ContentItemHeight / 2, ButtonWidth, ButtonHeight), "Удалить"))
        {
            switch (ContentType)
            {
                case OptionType.ABILITIES:
                    if (Abilities.Count > 0)
                    {
                        if (CurrentItem != null)
                        {
                            Abilities.RemoveAt(CurrentIndex);
                        }
                        else
                        {
                            Abilities.RemoveAt(Abilities.Count - 1);
                        }

                        if (Abilities.Count > 0)
                        {
                            ShowItemInfo(Abilities[Abilities.Count - 1]);
                            SelectItem(OptionType.ABILITIES, Abilities.Count - 1);
                        }
                    }
                    break;
                case OptionType.ITEMS:
                    if (Items.Count > 0)
                    {
                        if (CurrentItem != null)
                        {
                            Items.RemoveAt(CurrentIndex);
                        }
                        else
                        {
                            Items.RemoveAt(Items.Count - 1);
                        }

                        if (Items.Count > 0)
                        {
                            ShowItemInfo(Items[Items.Count - 1]);
                            SelectItem(OptionType.ITEMS, Items.Count - 1);
                        }
                    }
                    break;
            }
        }        
        EditorGUILayout.Space(ContentPadding);
        #endregion
   
        #region КОНТЕНТ
        GUI.Label(new Rect(0, ContentPadding, AreaWidth, ContentHeight + ContentItemHeight / 2), "", contentBackground);
        ContentScrollPos = EditorGUILayout.BeginScrollView(ContentScrollPos, false, true, GUILayout.Width(ContentWidth), GUILayout.Height(ContentHeight));
        
        switch (ContentType)
        {
            case OptionType.ABILITIES:
                if (Abilities.Count > 0)
                {
                    for (int i = 0; i < Abilities.Count; i++)
                    {                        
                        if (GUI.Button(new Rect(0, i * (ContentItemHeight + 1), ContentWidth, ContentItemHeight), "   " + Abilities[i].Name))
                        {
                            CurrentIndex = i;
                            CurrentItem = Abilities[i];
                            ShowItemInfo(Abilities[i]);                            
                            SelectItem(OptionType.ABILITIES, CurrentIndex);
                            EditorGUIUtility.editingTextField = false;
                        }
                    }
                }
                break;
            case OptionType.ITEMS:
                if (Items.Count > 0)
                {
                    for (int i = 0; i < Items.Count; i++)
                    {
                        if (GUI.Button(new Rect(0, i * (ContentItemHeight + 1), ContentWidth, ContentItemHeight), "   " + Items[i].Name))
                        {
                            CurrentIndex = i;
                            CurrentItem = Items[i];
                            ShowItemInfo(Items[i]);
                            SelectItem(OptionType.ITEMS, CurrentIndex);
                            EditorGUIUtility.editingTextField = false;
                        }
                    }
                }
                break;
        }
        EditorGUILayout.EndScrollView();
        #endregion        

        #region ПОИСК
        GUIStyle findStyle = new GUIStyle();
        findStyle.alignment = TextAnchor.MiddleLeft;
        findStyle.normal.background = Texture2D.whiteTexture;
        EditorGUILayout.Space(5);
        Find = EditorGUILayout.TextField(Find, findStyle, GUILayout.Width(ContentWidth), GUILayout.Height(ContentItemHeight));        
        EditorGUILayout.EndVertical();

        if (Find != "" && Find.Length >= 1)
        {
            FindList.Clear();
            FindListIndexes.Clear();
            switch (ContentType)
            {
                case OptionType.ABILITIES:
                    if (Abilities.Count > 0)
                    {
                        for (int i = 0; i < Abilities.Count; i++)
                        {
                            if (Find.Length <= Abilities[i].Name.Length && Abilities[i].Name.Substring(0, Find.Length) == Find)
                            {
                                FindList.Add(Abilities[i]);
                                FindListIndexes.Add(i);
                            }
                        }
                    }
                    break;
                case OptionType.ITEMS:
                    if (Items.Count > 0)
                    {
                        for (int i = 0; i < Items.Count; i++)
                        {
                            if (Items[i].Name.Substring(0, Find.Length).Length <= Items[i].Name.Length && Items[i].Name.Substring(0, Find.Length) == Find)
                            {
                                FindList.Add(Items[i]);
                                FindListIndexes.Add(i);
                            }
                        }
                    }
                    break;
            }
        }
        #endregion
        GUILayout.EndVertical();
        GUILayout.EndArea();

        /* ==== СВОЙСТВА ОБЪЕКТА ==== */
        GUILayout.BeginArea(new Rect(AreaWidth + 50, AreaPadding, AreaWidth + 300, AreaHeight));     
        GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        #region СВОЙСТВА ОБЪЕКТА      
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        switch (ContentType)
        {
            case OptionType.ABILITIES:
                EditorGUILayout.LabelField("Тип способности", propertyStyle, GUILayout.Width(130));
                string[] ability = { "Цель", "Без цели", "Точка", "Пассивная"};
                AbilityType = EditorGUILayout.Popup(AbilityType, ability, GUILayout.Width(100));
                EditorGUILayout.Space(10);

                switch (AbilityType)
                {
                    case 0:
                        EditorGUILayout.BeginVertical();

                        EditorGUILayout.BeginHorizontal();
                        string[] target = { "Герой", "Юнит", "Заклинатель", "Юнит и Герой" };
                        EditorGUILayout.LabelField("Цель способности", propertyStyle, GUILayout.Width(130));
                        AbilityTarget = EditorGUILayout.Popup(AbilityTarget, target, GUILayout.Width(110));
                        EditorGUILayout.EndHorizontal();

                        if (AbilityTarget !=  (int)Ability.AbilityTarget.CASTER)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Радиус способности", propertyStyle, GUILayout.Width(150));
                            Range = (byte)EditorGUILayout.IntField(Range, GUILayout.Width(40));
                            EditorGUILayout.LabelField("единиц", propertyStyle);
                            EditorGUILayout.EndHorizontal();
                        }

                        EditorGUILayout.EndVertical();
                    break;
                    case 1:                        

                        break;
                }
                
                break;
            case OptionType.ITEMS:
                
                EditorGUILayout.LabelField("Тип предмета", propertyStyle, GUILayout.Width(100));
                string[] itemType = { "Активный", "Пассивный" };
                ItemType = EditorGUILayout.Popup(ItemType, itemType, GUILayout.Width(90));                
                EditorGUILayout.Space(10);

                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                string[] itemCharacter = { "Постоянный", "Расходуемый" };
                EditorGUILayout.LabelField("Характеристика", propertyStyle, GUILayout.Width(130));
                ItemCharacteristic = EditorGUILayout.Popup(ItemCharacteristic, itemCharacter, GUILayout.Width(110));
                EditorGUILayout.EndHorizontal();
                
                if (ItemCharacteristic == 1)
                { 
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Количество зарядов", propertyStyle, GUILayout.Width(150));
                    ItemCharges = (byte)EditorGUILayout.IntField(ItemCharges, GUILayout.Width(40));
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
                break;               
        }
        EditorGUILayout.EndHorizontal();

        switch (ContentType)
        {
            case OptionType.ABILITIES:
                if (AbilityType != (int)Ability.AbilityType.PASSIVE)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Перезарядка", propertyStyle, GUILayout.Width(95));
                    Cooldown = EditorGUILayout.FloatField(Cooldown, GUILayout.Width(40));
                    EditorGUILayout.LabelField("секунд", propertyStyle);
                    EditorGUILayout.EndHorizontal();
                    if (Cooldown < 0) Cooldown = 0;
                }
                break;
            case OptionType.ITEMS:
                if (ItemType != (int)Item.ItemType.PASSIVE)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Перезарядка", propertyStyle, GUILayout.Width(95));
                    Cooldown = EditorGUILayout.FloatField(Cooldown, GUILayout.Width(40));
                    EditorGUILayout.LabelField("секунд", propertyStyle, GUILayout.Width(40));
                    
                    EditorGUILayout.EndHorizontal();
                    if (Cooldown < 0) Cooldown = 0;
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Вероятность выпадения", propertyStyle, GUILayout.Width(170));
                ItemProbability = EditorGUILayout.FloatField(ItemProbability, GUILayout.Width(50));
                if (ItemProbability < 0 || ItemProbability > 1)
                {
                    if (Mathf.Min(Mathf.Abs(0 - ItemProbability), Mathf.Abs(1 - ItemProbability)) == Mathf.Abs(0 - ItemProbability)) ItemProbability = 0;
                    else if (Mathf.Min(Mathf.Abs(0 - ItemProbability), Mathf.Abs(1 - ItemProbability)) == Mathf.Abs(1 - ItemProbability)) ItemProbability = 1;
                }
                EditorGUILayout.EndHorizontal();
                break;
        }

        EditorGUILayout.LabelField("Наименование", propertyStyle);
        Name = EditorGUILayout.TextArea(Name, GUILayout.Width(350));

        EditorGUILayout.LabelField("Описание", propertyStyle);
        Description = EditorGUILayout.TextArea(Description, GUILayout.Width(350), GUILayout.Height(100));

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Иконка", propertyStyle, GUILayout.Width(60));
        Icon = (Sprite)EditorGUILayout.ObjectField(Icon, typeof(Sprite), GUILayout.Width(60), GUILayout.Height(60));
        if (Icon != null)
        {
            SerializedSprite sprite = new SerializedSprite(Icon.texture.width, Icon.texture.height, Icon.texture.GetNativeTexturePtr());

            if (CurrentItem != null)
            {
                switch (ContentType)
                {
                    case OptionType.ABILITIES:
                        Ability ability = (Ability)CurrentItem;
                        ability.Icon = sprite;
                        break;
                    case OptionType.ITEMS:
                        Item item = (Item)CurrentItem;
                        item.Icon = sprite;
                        break;
                }
            }            
        }
        else
        {
            if (CurrentItem != null)
            {
                switch (ContentType)
                {
                    case OptionType.ABILITIES:
                        Ability ability = (Ability)CurrentItem;
                        ability.Icon = new SerializedSprite(0, 0, IntPtr.Zero);
                        break;
                    case OptionType.ITEMS:
                        Item item = (Item)CurrentItem;
                        item.Icon = new SerializedSprite(0, 0, IntPtr.Zero);
                        break;
                }
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.LabelField("Действие", propertyStyle);        

        EditorGUILayout.EndVertical();
        #endregion
        GUILayout.EndVertical();
        GUILayout.EndArea();

        GUILayout.EndHorizontal();

        if (GUI.changed)
        {
            if (CurrentItem != null)
            {
                if (CurrentItem.GetType() == typeof(Ability))
                {
                    Ability ability = (Ability)CurrentItem;
                    ability.Name = Name;
                    ability.Description = Description;
                    ability.Cooldown = Cooldown;
                    ability.Type = (Ability.AbilityType)AbilityType;
                    ability.Range = Range;
                    ability.TargetType = (Ability.AbilityTarget)AbilityTarget;
                }
                else if (CurrentItem.GetType() == typeof(Item))
                {
                    Item item = (Item)CurrentItem;
                    item.Name = Name;
                    item.Description = Description;
                    item.Cooldown = Cooldown;
                    item.Type = (Item.ItemType)ItemType;
                    item.Charges = ItemCharges;
                    item.Characteristic = (Item.ItemCharacteristic)ItemCharacteristic;
                    item.Probability = ItemProbability;
                }
            }    
        }
    }

    private void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(dataBasePath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            DataObject data = new DataObject(Abilities, Items);
            formatter.Serialize(stream, data);
            stream.Close();
        }
        GameData.Abilities = Abilities;
        GameData.Items = Items;
    }
    private DataObject Load()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(dataBasePath, FileMode.Open, FileAccess.Read);
        DataObject data = formatter.Deserialize(stream) as DataObject;
        stream.Close();
        return data;
    }
    private void ShowItemInfo(object _item)
    {
        if (_item.GetType() == typeof(Ability))
        {
            Ability ability = (Ability)_item;

            AbilityType = (int)ability.Type;
            Name = ability.Name;
            Description = ability.Description;
            Cooldown = ability.Cooldown;
            Range = ability.Range;
            AbilityTarget = (int)ability.TargetType;
            if (ability.Icon != null)
            {
                if (ability.Icon.ptr != IntPtr.Zero)
                {
                    if (Texture2D.CreateExternalTexture(ability.Icon.width, ability.Icon.height, TextureFormat.ARGB32, false, false, ability.Icon.ptr))
                    {
                        Texture2D import = Texture2D.CreateExternalTexture(ability.Icon.width, ability.Icon.height, TextureFormat.ARGB32, false, false, ability.Icon.ptr);
                        Sprite sprite = UnityEngine.Sprite.Create(import, new Rect(0, 0, import.width, import.height), Vector2.one);
                        Icon = sprite;
                        Sprite = ability.Icon;
                    }
                }
                else Icon = null;
            }
            else Icon = null;
        }
        else if (_item.GetType() == typeof(Item))
        {
            Item item = (Item)_item;

            ItemType = (int)item.Type;
            Name = item.Name;
            Description = item.Description;
            Cooldown = item.Cooldown;
            ItemCharges = item.Charges;
            ItemCharacteristic = (int)item.Characteristic;
            ItemProbability = item.Probability;

            if (item.Icon != null)
            {
                if (item.Icon.ptr != IntPtr.Zero)
                {
                    if (Texture2D.CreateExternalTexture(item.Icon.width, item.Icon.height, TextureFormat.ARGB32, false, false, item.Icon.ptr))
                    {
                        Texture2D import = Texture2D.CreateExternalTexture(item.Icon.width, item.Icon.height, TextureFormat.ARGB32, false, false, item.Icon.ptr);
                        Sprite sprite = UnityEngine.Sprite.Create(import, new Rect(0, 0, import.width, import.height), Vector2.one);
                        Icon = sprite;
                        Sprite = item.Icon;
                    }
                }
                else Icon = null;
            }
            else Icon = null;
        }
    }
    private void SelectItem(OptionType type, int index)
    {
        CurrentIndex = index;
        switch (type)
        {
            case OptionType.ABILITIES:
                CurrentItem = Abilities[CurrentIndex];
                break;
            case OptionType.ITEMS:
                CurrentItem = Items[CurrentIndex];
                break;
        }
    }
}
