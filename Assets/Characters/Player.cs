using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour {    

    // Start is called before the first frame update
    public float MovementSpeed = 7f;
    public float JumpScale = 2f;
    bool OnGround = true;
    bool OnWall = false;
    void Start()
    {
        this.GetComponent<Rigidbody2D>().gravityScale = JumpScale / 2;
    }

    // Update is called once per frame
    void Update()
    {
        float force = GetComponent<Rigidbody2D>().velocity.magnitude * GetComponent<Rigidbody2D>().mass;
        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * MovementSpeed - (Vector2.right * force));
        }

        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.left * MovementSpeed - (Vector2.left * force));
        }

        if (Input.GetKey(KeyCode.W) && OnGround)
        {
            this.GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpScale * 100);
            OnGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
            OnWall = true;
        }
        if (collision.transform.tag == "Ground")
        {
            OnGround = true;
            this.GetComponent<Rigidbody2D>().inertia = 0;
        }
    }
}
