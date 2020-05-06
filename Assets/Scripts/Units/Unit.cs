using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Unit : MonoBehaviour
{
    public static Unit[] Units = new Unit[100];

    [Header("Статус")]
    public bool Paused = false;
    public bool Commandable = true;
    public bool Movable = true;
    public bool Invulnerable = false;
    public bool IsDead = false;
    public bool CanJump;
    public bool OnGround;
    
    [Header("Характеристики")]
    public byte Health;
    public byte MaxHealth;
    public byte Mana;
    public byte MaxMana;

    public Unit Target { set; get; }
    public enum UnitType { MELEE, RANGE }
    [Header("Бой")]
    [SerializeField]
    UnitType Type = UnitType.MELEE;
    public byte AttackDamage;
    public float AttackRange;
    [Range(0, 10)]
    public float AttackCooldown;

    [Header("Передвижение")]
    [Range(0, 5)]
    public float MovementSpeed;
    public float JumpScale = 4f;

    private byte DoubleJumpsCount { set; get; } = 0;
    [Header("Кол-во дополнительных прыжков")] 
    public byte DoubleJumpsMax = 0;

    [Header("Аниматор для проигрывания анимаций")]
    public Animator Animator;

    [Header("Коллайдер для физического взаимодействия")]
    public Collider2D RigidbodyCollider;

    [HideInInspector]
    public UnitOrderType Order { set; get; } = UnitOrderType.NULL;
    public enum UnitOrderType { MOVING, STOPPED, ATTACKING, NULL }

    public Unit()
    {
        for (int i = 0; i < Units.Length; i++)
        {
            if (Units[i] == null)
            {
                Units[i] = this;
                break;
            }
        }
    }

    /*========= УМЕНИЯ =========*/

    // Передвижение
    public void InstanceMoveTo(Vector2 _direction)
    {       
        transform.Translate(new Vector3(_direction.x, _direction.y, transform.position.z));
    } // Мгновенное перемещение в указанную точку
    public void MoveTo(Vector2 _direction)
    {
        if (Movable && Commandable && !IsDead)
        {       
            if (_direction == Vector2.right)
            {
                GetComponent<SpriteRenderer>().flipX = true;          
            }
            else if (_direction == Vector2.left)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }

            Animator.SetTrigger("Walk");
            StopCoroutine(Move(_direction));
            Order = UnitOrderType.MOVING;
            StartCoroutine(Move(_direction));
        }
    } // Ходьба к указанной точке со скоростью MovementSpeed
    private IEnumerator Move(Vector2 _direction)
    {
        while (transform.position != new Vector3(_direction.x, _direction.y, transform.position.z))
        {
            if (Mathf.Abs(_direction.x - transform.position.x) >= (new Vector3(_direction.x - transform.position.x, 0, 0).normalized * MovementSpeed * Time.deltaTime).magnitude)
                transform.position += new Vector3(_direction.x - transform.position.x, 0, 0).normalized * MovementSpeed * Time.deltaTime;
            else
            {
                transform.position = _direction;
                break;
            }
            yield return null;
        }
        StopCoroutine(Move(_direction));
    }
    public void MoveTo(Unit _target)
    {
        if (Movable && Commandable && !IsDead)
        {
            StopCoroutine(Move(_target));
            Animator.SetTrigger("Walk");
            Order = UnitOrderType.MOVING;
            StartCoroutine(Move(_target));
        }
    } // Преследование указанной цели со скоростью MovementSpeed
    private IEnumerator Move(Unit _target)
    {
        while (Vector3.Distance(transform.position, _target.transform.position) >= GetPhysicRadius(this, _target))
        {
            transform.position += new Vector3(_target.transform.position.x - transform.position.x, 0, 0).normalized * MovementSpeed * Time.deltaTime;
            yield return null;
        }
        StopCoroutine(Move(_target));
    }

    /*========= ПРОЧИЕ =========*/
    public void Jump()
    {
        if (CanJump && Commandable)
        {
            if (OnGround)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.up * JumpScale;
                OnGround = false;
            }
            else
            {
                if (DoubleJumpsCount < DoubleJumpsMax)
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.up * JumpScale;
                    DoubleJumpsCount++;
                }
            }
        }       
    }
        
    public void Attack(Unit _target)
    {
        if (Target != null) StartCoroutine(AttackCoroutine());
    } // Атака цели
    private IEnumerator AttackCoroutine()
    {
        while (Target != null && !Target.IsDead)
        {
            if (Vector3.Distance(transform.position, Target.transform.position) <= GetAttackRadius(this, Target))
            {
                Order = UnitOrderType.ATTACKING;
                Damage(Target, AttackDamage);
                yield return new WaitForSeconds(AttackCooldown);
            }
            else
            {
                if (Order != UnitOrderType.MOVING)
                {
                    MoveTo(Target);
                }
                yield return null;
            }
        }

        Target = null;
    }
    
    public void Parry()
    {

    }
    public void Deflect()
    {

    }
    public void Damage(Unit _target, byte _value)
    {
        if (_target.Health - _value > 0)
        {
            _target.Health -= _value;

            if (_target == Player.Hero) PlayerBar.Update(PlayerBar.PlayerBarType.HEALTH, Player.Hero.Health, Player.Hero.Health, 2f);
        }
        else if (_target.Health - _value <= 0)
        {
            if (_target == Player.Hero) PlayerBar.Update(PlayerBar.PlayerBarType.HEALTH, Player.Hero.Health, 0, 2f);

            _target.Health = 0;
            _target.IsDead = true;
            _target.Commandable = false;
            _target.Movable = false;
        }

        if (Player.Hero.IsDead) Game.Defeat();
    }
    public void Kill(Unit _target)
    {        
        _target.Damage(_target, _target.MaxHealth);
        Remove();
    }
    
    private static float GetPhysicRadius(Unit _first, Unit _second)
    {
        float _firstRadius = Vector3.Distance(_first.RigidbodyCollider.bounds.center, _first.RigidbodyCollider.bounds.min);
        float _secondRadius = Vector3.Distance(_second.RigidbodyCollider.bounds.center, _second.RigidbodyCollider.bounds.min);
        return _firstRadius + _secondRadius;
    }
    private static float GetAttackRadius(Unit _first, Unit _second)
    {
        if (_first.Type == UnitType.MELEE && _second.Type == UnitType.MELEE)
        {
            return GetPhysicRadius(_first, _second);
        }
        else if (_first.Type == UnitType.MELEE && _second.Type == UnitType.RANGE)
        {
            float radius = GetPhysicRadius(_first, _second);
            return radius + _second.AttackRange;
        }
        else if (_first.Type == UnitType.RANGE && _second.Type == UnitType.MELEE)
        {
            float radius = GetPhysicRadius(_first, _second);
            return radius + _first.AttackRange;
        }
        return 0;
    }
    public void Stop()
    {
        StopAllCoroutines();
        GetComponent<Unit>().Order = UnitOrderType.STOPPED;
        Animator.SetTrigger("Idle");
    }
    public void Remove()
    {
        Destroy(gameObject);
    }
    public static void RemoveUnits()
    {
        foreach (Unit _unit in Units)
        {
            if (_unit != null) _unit.Remove();
        }
    }

    // КОЛЛИЗИЯ
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Касание с землёй
        if (collision.transform.tag == "Ground")
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal == Vector2.up) // Если нормаль хотя бы одной точки касания смотрит вверх - персонаж на земле
                {
                    OnGround = true;
                    DoubleJumpsCount = 0;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Fall Collider" && collision == RigidbodyCollider)
        {                     
            Kill(this);
        }
    }
}
