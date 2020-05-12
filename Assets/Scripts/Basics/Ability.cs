using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Ability
{
    public enum AbilityType { TARGET, NONTARGET, POINT, PASSIVE }
    public enum AbilityTarget { HERO, UNIT, CASTER, HEROANDUNIT }
    public AbilityType Type { set; get; } = AbilityType.NONTARGET;
    public AbilityTarget TargetType { set; get; }
    public string Name { set; get; }
    public string Description { set; get; }
    public float Cooldown { set; get; } = 0;
    public byte Range { set; get; } = 0;
    public SerializedSprite Icon { set; get; }

    public readonly Action<object[]> Action;

    public Ability(string _name)
    {
        Name = _name;
    }

    public void SetType(AbilityType _type)
    {
        Type = _type;
    }
    public void SetCooldown(float _value)
    {
        Cooldown = _value;
    }
    public void Cast(object[] arguments)
    {
           
    }
}