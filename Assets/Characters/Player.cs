using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Unit))]
public class Player : MonoBehaviour {

    private Unit Hero;

    private void Awake()
    {
        Hero = GetComponent<Unit>();
        CameraScript.InstanceMoveTo(new Vector3(transform.position.x, Camera.main.gameObject.transform.position.y, Camera.main.gameObject.transform.position.z));
    }

    private void Update()
    {
        GameObject.Find("Health Bar").GetComponent<Slider>().value = ((Hero.Health * 100) / Hero.MaxHealth) / 100;
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Unit>().MoveTo(Vector2.right);
            CameraScript.InstanceMoveTo(new Vector2(transform.position.x, Camera.main.gameObject.transform.position.y));
        }

        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Unit>().MoveTo(Vector2.left);
            CameraScript.InstanceMoveTo(new Vector2(transform.position.x, Camera.main.gameObject.transform.position.y));
        }

        if (Input.GetKey(KeyCode.W))
        {
            GetComponent<Unit>().Jump();
        }
    }
}
