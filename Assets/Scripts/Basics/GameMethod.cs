using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class Action
{
    public string Name;
    public static Action Last;
    public static int Count = 0;
    public enum ActionTier1Type { HEALING, DESTRUCTION, BUFF, DEBUFF, CREATION, PROTECTION }
    public ActionTier1Type ActionType;

    public System.Action<object[]> Method;
    public Action(string name) {
        Name = name;
        if (Last != null)
        {
            Debug.Log("DELETING LAST = " + this);
            Count--;
            Last = null;
        }
        Debug.Log("COUNT =" + Count);
    }
}

[Serializable]
public class HealingAction : Action
{
    public enum Healing { INSTANCE, PERIODIC }
    public Healing HealingType;

    public HealingAction(string name, int index) : base(name)
    {
        HealingType = (Healing)index;
        ActionType = ActionTier1Type.HEALING;
        Count++;
        Last = this;
        Debug.Log("CREATING LAST = " + this);
    }
}

[Serializable]
public class DestructionAction : Action
{
    public enum Destruction { INSTANCE, PERIODIC }
    public Destruction DestructionType;

    public DestructionAction(string name, int index) : base(name)
    {
        DestructionType = (Destruction)index;
        ActionType = ActionTier1Type.DESTRUCTION;
        Count++;
        Last = this;
        Debug.Log("CREATING LAST = " + this);
    }
}

[Serializable]
public class BuffAction : Action
{
    public enum Buff { HEALTH, MANA, DAMAGE, ATTACKRANGE, SPEED }
    public Buff BuffType;

    public BuffAction(string name, int index) : base(name)
    {
        BuffType = (Buff)index;
        ActionType = ActionTier1Type.BUFF;
        Count++;
        Last = this;
        Debug.Log("CREATING LAST = " + this);
    }
}

[Serializable]
public class DebuffAction : Action
{
    public enum Destruction { INSTANCE, PERIODIC }
    public Destruction HealingType;

    public DebuffAction(string name, int index) : base(name)
    {
        HealingType = (Destruction)index;
        ActionType = ActionTier1Type.DESTRUCTION;
        Count++;
        Last = this;
        Debug.Log("CREATING LAST = " + this);
    }
}

[Serializable]
public class CreationAction : Action
{
    public enum Destruction { INSTANCE, PERIODIC }
    public Destruction HealingType;

    public CreationAction(string name, int index) : base(name)
    {
        HealingType = (Destruction)index;
        ActionType = ActionTier1Type.DESTRUCTION;
        Count++;
        Last = this;
        Debug.Log("CREATING LAST = " + this);
    }
}

[Serializable]
public class ProtectionAction : Action
{
    public enum Destruction { INSTANCE, PERIODIC }
    public Destruction HealingType;

    public ProtectionAction(string name, int index) : base(name)
    {
        HealingType = (Destruction)index;
        ActionType = ActionTier1Type.DESTRUCTION;
        Count++;
        Last = this;
        Debug.Log("CREATING LAST = " + this);
    }
}



