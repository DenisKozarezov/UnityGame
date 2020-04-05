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
            GetComponent<Unit>().MoveTo(Vector2.right);
        }

        if (Input.GetKey(Options.Left))
        {
            GetComponent<Unit>().MoveTo(Vector2.left);
        }

        if (Input.GetKeyDown(Options.Jump))
        {
            GetComponent<Unit>().Jump();
        }        
    }
}
