using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    public float speedX = 0.02f;
    public float speedY = 5;
    public float speedXIA = 0.005f;
    public int jumpCount;
    public bool isGrounded;
    public bool lookForward = true;
    public float dodg = 5f;
    public float distance = 50f;
    BoxCollider2D coll;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D)) {
            if (isGrounded)
                transform.Translate(new Vector2(speedX, 0));
            else transform.Translate(new Vector2(speedXIA, 0));
            lookForward = true;
        }
        if (Input.GetKey(KeyCode.A)) {
            if (isGrounded)
                transform.Translate(new Vector2(-speedX, 0));
            else transform.Translate(new Vector2(-speedXIA, 0));
            lookForward = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || (jumpCount < 2)))
        {
            rb.AddForce(new Vector2(0, speedY), ForceMode2D.Impulse);
            isGrounded = false;
            jumpCount++;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (lookForward)
                //transform.Translate(new Vector2(1, 0));
                transform.position = Vector3.Lerp(transform.position, new Vector2(transform.position.x + distance, transform.position.y), dodg * Time.deltaTime);
            //else transform.Translate(new Vector2(-1, 0));
            else transform.position = Vector3.Lerp(transform.position, new Vector2(transform.position.x - distance, transform.position.y), dodg * Time.deltaTime);

        }
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            jumpCount = 0;
        }

        if (collision.gameObject.tag == "Wall")
            rb.gravityScale = 0;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
            rb.gravityScale = 1;
    }
}
