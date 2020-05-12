using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class SerializedSprite
{
    public int width, height;
    public IntPtr ptr = IntPtr.Zero;

    public SerializedSprite(int _width, int _height, IntPtr _ptr)
    {
        width = _width;
        height = _height;
        ptr = _ptr;
    }
}

[ExecuteAlways]
public class GameData : MonoBehaviour
{
    public static List<Ability> Abilities { set; get; } = new List<Ability>();
    public static List<Item> Items { set; get; } = new List<Item>();
}