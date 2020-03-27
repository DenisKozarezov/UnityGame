using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class Unit : MonoBehaviour
{
    [Header("Статус")]
    public bool Commandable = true;
    public bool Movable = true;
    public bool Invulnerable = false;
    public bool IsDead = false;
    public bool CanDoubleJump;
    public bool OnGround;

    [Header("Характеристики")]
    public float Health;
    public float MaxHealth;
    public float Mana;
    public float MaxMana;

    public float MovementSpeed = 7f;
    public float JumpScale = 4f;

    private float DoubleJumpsCount { set; get; } = 0;
    public int DoubleJumpsMax = 1;

    [Header("Аниматор")]
    public Animator Animator;

    public void MoveTo(Vector2 _direction)
    {
        if (Movable && Commandable && !IsDead) transform.Translate(_direction * MovementSpeed * Time.deltaTime);
    }
    public void Jump()
    {
        if (Movable)
        {
            if (OnGround)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.up * JumpScale;
                OnGround = false;
            }
            else
            {
                if (CanDoubleJump && DoubleJumpsCount < DoubleJumpsMax)
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.up * JumpScale * 1.2f;
                    DoubleJumpsCount++;
                }
            }
        }
    }
    public void Attack()
    {

    }
    public void Parry()
    {

    }
    public void Deflect()
    {

    }
    public void Damage(Unit _target, float _value)
    {
        if (_target.Health - _value > 0) _target.Health -= _value;
        else
        {
            _target.Health = 0;
            IsDead = true;
            Commandable = false;
            Movable = false;
        }

        if (Player.Hero.IsDead) Game.Defeat();
    }
    public void Kill(Unit _target)
    {
        if (this == Player.Hero) Interface.UpdateBar(Interface.UnitBarType.HEALTH, Player.Hero.Health, 0, 1f);
        
        _target.Damage(_target, _target.MaxHealth);
    }

    public void Remove()
    {
        Destroy(GetComponent<Player>());
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            OnGround = true;
            GetComponent<Rigidbody2D>().inertia = 0;
            DoubleJumpsCount = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Fall Collider")
        {                     
            Kill(this);
        }
    }
}
