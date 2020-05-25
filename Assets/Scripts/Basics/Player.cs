﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Unit))]
public class Player : MonoBehaviour {

    public static Unit Hero { private set; get; }
    public GameObject FireballPrefab;

    private void Awake()
    {
        Hero = GetComponent<Unit>();
        CameraScript.AttachToUnit(Hero, CameraScript.CameraAttachmentType.BOTH);
    }

    // УПРАВЛЕНИЕ ПЕРСОНАЖЕМ
    private void Update()
    {
        if (Input.GetKey(Options.Right))
        {
            if (Hero.Movable && Hero.Commandable && !Hero.IsDead)
            {
                Hero.Animator.ResetTrigger("Attack");
                Hero.GetComponent<SpriteRenderer>().flipX = false;
                Hero.Animator.SetTrigger("Walk");
                Hero.InstanceMoveTo(Vector2.right * Hero.MovementSpeed * Time.deltaTime);
            }

        }
        else if (Input.GetKeyUp(Options.Right)) Hero.Idle();

        if (Input.GetKey(Options.Left))
        {
            if (Hero.Movable && Hero.Commandable && !Hero.IsDead)
            {
                Hero.Animator.ResetTrigger("Attack");
                Hero.Animator.SetTrigger("Walk");
                Hero.GetComponent<SpriteRenderer>().flipX = true;
                Hero.InstanceMoveTo(Vector2.left * Hero.MovementSpeed * Time.deltaTime);
            }
        }
        else if (Input.GetKeyUp(Options.Left)) Hero.Idle();

        if (Input.GetKeyDown(Options.Jump))
        {
            Hero.Jump();
            if (Hero.OnWall)
            {
                Hero.OnWall = false;
                Hero.GetComponent<Rigidbody2D>().gravityScale = 1;
                Hero.Animator.ResetTrigger("Slide");
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Hero.IsReadyToAttack && Hero.OnGround && !Hero.Animator.GetBool("Run"))
            {
                StartCoroutine(AttackCooldown(Hero.AttackCooldown));
            }
        }

        if (Input.GetKeyDown(Options.Interaction))
        {
            Unit unit = (Unit)Hero.Abilities[0].Action.Target;
            Debug.Log(Hero.Abilities[0].Name + " " + unit.name);
            Hero.Abilities[0].Cast();
        }

        if (Input.GetKeyDown(Options.RangeAttack) && Hero.OnGround)
        {
            if (Hero.Animator != null)
            {
                Hero.Animator.ResetTrigger("Idle");
                Hero.Animator.SetTrigger("Range Attack");
            }
        }
    }

    private IEnumerator AttackCooldown(float _cooldown)
    {
        Hero.Animator.ResetTrigger("Idle");
        Hero.Animator.SetTrigger("Attack");
        Hero.IsReadyToAttack = false;
        yield return new WaitForSeconds(_cooldown);
        Hero.IsReadyToAttack = true;
        StopCoroutine(AttackCooldown(_cooldown));
    }

    public void Fireball()
    {
        Vector3 position = new Vector3(Hero.transform.position.x, Hero.transform.position.y + 1, Hero.transform.position.z);
        GameObject fireball = Instantiate(FireballPrefab, position, Quaternion.identity);
        Vector2 direction;
        if (Hero.GetComponent<SpriteRenderer>().flipX) direction = new Vector2(-20, 0);
        else direction = new Vector2(20, 0);
        StartCoroutine(Fireball(fireball, direction));
    }
    private IEnumerator Fireball(GameObject _fireball, Vector2 _direction)
    {
        Vector3 startPosition = _fireball.transform.position;
        Vector3 endPosition = startPosition + new Vector3(_direction.x, _direction.y, 0);
        float startTime = Time.time;
        while (_fireball.transform.position != endPosition && _fireball != null)
        {
            float elapsedTime = Time.time - startTime;
            _fireball.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime);
            yield return null;
        }
        if (_fireball != null) Destroy(_fireball);
        StopCoroutine(Fireball(_fireball, _direction));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision == collision.gameObject.GetComponent<Enemy>().AggressionCollider)
            {
                collision.gameObject.GetComponent<Enemy>().Taunt(Hero);                
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision == collision.gameObject.GetComponent<Enemy>().AggressionLossCollider && collision.gameObject.GetComponent<Unit>().Target == Hero)
            {
                collision.gameObject.GetComponent<Enemy>().Lose();
            }
        }
    }
}
