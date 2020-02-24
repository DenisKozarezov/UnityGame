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

    }

    // Update is called once per frame
    void Update()
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
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, JumpScale * 100));
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
