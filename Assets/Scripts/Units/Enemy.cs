﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.Experimental.Networking.PlayerConnection;
using UnityEditor.Experimental.TerrainAPI;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(CircleCollider2D))]
[ExecuteInEditMode]
public class Enemy : MonoBehaviour
{        
    public bool Aggressive { private set; get; }
    [Header("Провокация")]
    [Range(0, 5)]
    public float AggressionRadius;
    [Range(0, 15)]
    public float AggressionLossRadius;

    const float PatrolMaxDistance = 25f;
    [Header("Патрулирование")]
    public bool OnPatrol = false;
    [Range(0, PatrolMaxDistance)]
    public float PatrolLeftDistance;
    [Range(0, PatrolMaxDistance)]
    public float PatrolRightDistance;

    private Vector3 LeftBound, RightBound;

    [Header("Круговой коллайдер для провокации")]
    public CircleCollider2D AggressionCollider;
    [Header("Круговой коллайдер для потери врага")]
    public CircleCollider2D AggressionLossCollider;

    private void LateUpdate()
    {
        if (AggressionCollider != null) AggressionCollider.radius = AggressionRadius;
        if (AggressionLossCollider != null) AggressionLossCollider.radius = AggressionLossRadius;

        if (!EditorApplication.isPlaying)
        {
            if (OnPatrol)
            {
                Debug.DrawRay(transform.position, Vector3.left * PatrolLeftDistance, Color.red);
                Debug.DrawRay(transform.position, Vector3.right * PatrolRightDistance, Color.red);
            }
        }
        else
        {
            if (OnPatrol)
            {
                float leftDistance = Vector3.Distance(transform.position, LeftBound);
                float rightDistance = Vector3.Distance(transform.position, RightBound);
                Debug.DrawRay(transform.position, Vector3.left * leftDistance, Color.red);
                Debug.DrawRay(transform.position, Vector3.right * rightDistance, Color.red);
            }
        }
    }

    // ПАТРУЛИРОВАНИЕ
    public void Patrol()
    {
        if (PatrolLeftDistance > 0 || PatrolRightDistance > 0)
        {
            LeftBound = new Vector2(transform.position.x - PatrolLeftDistance, transform.position.y);
            RightBound = new Vector2(transform.position.x + PatrolRightDistance, transform.position.y);
            OnPatrol = true;
            StartCoroutine(PatrolCoroutine());
        }
    }
    private IEnumerator PatrolCoroutine()
    {
        RandomNumberGenerator.Create();
        int random = UnityEngine.Random.Range(0, 2);
       
        if (random == 0) GetComponent<Unit>().MoveTo(LeftBound); else GetComponent<Unit>().MoveTo(RightBound);

        while (OnPatrol)
        {
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), LeftBound) == 0)
            {
                GetComponent<Unit>().MoveTo(RightBound);
            }

            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), RightBound) == 0)
            {
                GetComponent<Unit>().MoveTo(LeftBound);
            }            
            yield return null;
        }
        OnPatrol = false;
        StopCoroutine(PatrolCoroutine());
    }

    // ПРОВОКАЦИЯ
    public void Taunt(Unit _target)
    {
        Aggressive = true;
        OnPatrol = false;
        GetComponent<Unit>().Target = _target;
        Stop();
        GetComponent<Unit>().Attack(_target);
    }
    public void Lose()
    {
        Aggressive = false;
        GetComponent<Unit>().Target = null;
        Stop();
    }

    private void Stop()
    {
        StopAllCoroutines();
        GetComponent<Unit>().Stop();        
    }
}