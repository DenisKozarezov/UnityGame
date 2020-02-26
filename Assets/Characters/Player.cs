using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour {    

    // Start is called before the first frame update
    public float MovementSpeed = 7f;
    public float JumpScale = 2f;
    public bool ShowName = false;

    bool OnGround = true;

    void Start()
    {
        this.GetComponent<Rigidbody2D>().gravityScale = JumpScale / 2;

    }

    private void Update()
    {
        
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector2.right * MovementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector2.left * MovementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.W) && OnGround)
        {
            this.GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpScale * 100);
            OnGround = false;
        }
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
