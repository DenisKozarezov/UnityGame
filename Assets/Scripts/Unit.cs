using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class Unit : MonoBehaviour
{
    /* Статусы юнита */
    public bool Commandable = true;
    public bool Movable = true;
    public bool Invulnerable = false;
    public bool IsDead = false;
    public bool OnGround;

    public float Health;
    public float MaxHealth;

    public float MovementSpeed = 7f;
    private float JumpScale = 4f;

    public Animator Animator;

    private void Awake()
    {
        GetComponent<Rigidbody2D>().gravityScale = JumpScale / 2;
    }
    
    public void MoveTo(Vector2 _direction)
    {
        if (Movable) transform.Translate(_direction * MovementSpeed * Time.deltaTime);
    }
    public void Jump()
    {
        if (OnGround && Movable)
        {
            this.GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpScale * 100);
            OnGround = false;
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
    }
    public void Kill(Unit _target, float _value)
    {
        _target.Damage(_target, _target.MaxHealth);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            OnGround = true;
            this.GetComponent<Rigidbody2D>().inertia = 0;
        }
    }
}
