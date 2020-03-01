using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon_Fang : MonoBehaviour
{
    public PlayerControll pl;
    public bool direction = true;
    GameObject player;
    float xPos;
    public Vector2 vForward = new Vector2(1, 0),
        vBack = new Vector2(-1, 0);
    public float DeathTime = 5;

    private void Start()
    {
        Destroy(gameObject, DeathTime);
        xPos = player.transform.position.x;
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (direction)
            transform.Translate(vForward * Time.deltaTime);
        else transform.Translate(vBack * Time.deltaTime);
    }
}
