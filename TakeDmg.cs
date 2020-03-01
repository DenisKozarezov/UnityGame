using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Timers;

public class TakeDmg : MonoBehaviour
{
    public float XForce = 3f;
    public float YForce = 3f;
    public float invincbleTime = 2f;
    bool notInvincble = true;
    Rigidbody2D rb;
    int timer = 0;
    Vector2 Force;
    // Start is called before the first frame update
    void Start()
    {
        Force = new Vector2(XForce, YForce);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void makeHurt(bool lookForword)
    {
        if(lookForword)
        Force = new Vector2(XForce, YForce);
        else Force = new Vector2(-XForce, YForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DBox" && notInvincble)
        {
            if (collision.transform.position.x >= transform.position.x)
                Force = new Vector2(-XForce, YForce);
            else Force = new Vector2(XForce, YForce);
            rb.AddForce(Force, ForceMode2D.Impulse);
            GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            notInvincble = false;
            StartCoroutine(MakeNormal());
        }
    }

    IEnumerator MakeNormal()
    {
        yield return new WaitForSeconds(1.5f);
        notInvincble = true;
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }
}
