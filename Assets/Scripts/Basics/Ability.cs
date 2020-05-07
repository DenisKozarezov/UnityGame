using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{    
    public enum AbilityType { TARGET, NONTARGET, POINT }
    public string Name { set; get; }
    public string Description { set; get; }
    public float Cooldown { set; get; }
    public Unit Target { set; get; }
    public Vector2 Point { set; get; }
    public AbilityType Type { set; get; } = AbilityType.NONTARGET;

    public bool IsReady = true;

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
    public virtual void Cast()
    {
        if (IsReady)
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
        IsReady = false;
    }
}

public enum ProjectileType { FIRE, POISION }
public class Shoot : Ability
{
    public void Cast(Unit _caster, Unit _target, ProjectileType _projectileType)
    {
        Vector3 targetPosition = _target.transform.position;
        
        switch (_projectileType)
        {
            case ProjectileType.FIRE:
                GameObject projectile = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Capsule), _caster.transform.position, Quaternion.identity);
                projectile.AddComponent<Projectile>().Shoot(projectile.transform.position, _target.transform.position);
                break;
        }
    }
}
