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
        CameraScript.AttachToUnit(Hero);
    }

    private void Update()
    {
        CameraScript.InstanceMoveTo(new Vector2(Hero.transform.position.x, CameraScript.attachedY));
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Unit>().MoveTo(Vector2.right);
        }

        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Unit>().MoveTo(Vector2.left);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            GetComponent<Unit>().Jump();
        }
    }
}
