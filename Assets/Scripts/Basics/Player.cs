using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Unit))]
public class Player : MonoBehaviour {

    public static Unit Hero { private set; get; }

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
            GetComponent<Unit>().InstanceMoveTo(Vector2.right * Hero.MovementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(Options.Left))
        {
            GetComponent<Unit>().InstanceMoveTo(Vector2.left * Hero.MovementSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(Options.Jump))
        {
            GetComponent<Unit>().Jump();
        }        
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
