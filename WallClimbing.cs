using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimbing : MonoBehaviour
{
    public bool wallCl = false;
    public float distance = 0.5f;
    Vector2 direction;
    Rigidbody2D rb;
    public GameObject player;
    public RaycastHit2D hit;
    PlayerControll pl;
    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector2(2f, transform.position.y);
        rb = GetComponent<Rigidbody2D>();
        Physics2D.queriesStartInColliders = false;
        pl = GetComponent<PlayerControll>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pl.lookForward)
            hit = Physics2D.Raycast(transform.position, Vector3.right * transform.localScale.x, distance);
        else hit = Physics2D.Raycast(transform.position, Vector3.left * transform.localScale.x, distance);

        if (hit.collider != null)
        {
            wallCl = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -0.05f);
            if (pl.jumpCount > 0)
                pl.jumpCount--;
        }
        else wallCl = false;
    }
}
