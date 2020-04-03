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
    
    // УПРАВЛЕНИЕ ПЕРСОНАЖЕМ
    private void FixedUpdate()
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

        if (Input.GetKeyDown(Options.GameMenu))
        {         
            if (!GameObject.Find("Canvas").GetComponent<Interface>().GameMenuPanel.activeInHierarchy)
            {
                GameMenu.Open();
            }
            else if (GameObject.Find("Canvas").GetComponent<Interface>().GameMenuPanel.activeInHierarchy && GameObject.Find("Canvas").GetComponent<Interface>().GameMenuPanel.transform.GetSiblingIndex() + 1 == GameObject.Find("Canvas").transform.childCount)
            {
                GameMenu.Close();
            }
        }
    }
}
