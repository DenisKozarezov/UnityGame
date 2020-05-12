using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[ExecuteInEditMode]
public class Lootbox : MonoBehaviour
{
    public List<Item> Items = new List<Item>();

    public bool ItemsFoldout;
    public float FoldoutHeight = 100;

    public Animator Animator;
}
