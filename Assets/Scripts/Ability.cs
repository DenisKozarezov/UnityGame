using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilitiesBank
{
    public static Ability [] Abilities = new Ability[10];

    public static Ability GetAbilityByName(string _name)
    {
        foreach (Ability ability in Abilities)
        {
            if (ability != null && ability.Name == _name) return ability;
        }
        return null;
    }
}
public class Ability
{    
    public enum AbilityType { TARGET, NONTARGET, POINT }
    public string Name { set; get; }
    public string Description { set; get; }
    public float Cooldown { set; get; }
    public Unit Target { set; get; }
    public Vector2 Point { set; get; }
    public AbilityType Type { set; get; } = AbilityType.NONTARGET;

    private delegate void TargetAction(Unit _target);
    private delegate void NonTargetAction();
    private delegate void PointAction(Vector2 _point);

    TargetAction TDelegate;
    NonTargetAction NTDelegate;
    PointAction PDelegate;

    private void AddAction(TargetAction _action)
    {
        TDelegate = _action;
    }
    private void AddAction(NonTargetAction _action)
    {
        NTDelegate = _action;
    }
    private void AddAction(PointAction _action)
    {
        PDelegate = _action;
    }
    public void Cast()
    {
        switch (Type)
        {
            case AbilityType.TARGET:
                TDelegate.Invoke(Target);
                break;
            case AbilityType.POINT:
                PDelegate.Invoke(Point);
                break;
            case AbilityType.NONTARGET:
                NTDelegate.Invoke();
                break;
        }
    }
}


/* Действия способностей */
static class Methods
{
    
}