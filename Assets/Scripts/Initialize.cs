using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialize : MonoBehaviour
{
    public Sprite Background;
    public Material LightMaterial;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i <= 25; i++)
        {
            for (int j = 1; j <= 5; j++)
            {
                GameObject obj = new GameObject();
                obj.AddComponent<SpriteRenderer>().sprite = Background;
                obj.GetComponent<SpriteRenderer>().material = LightMaterial;
                obj.transform.position = new Vector2(i * Background.bounds.size.x, j * Background.bounds.size.y);
                obj.gameObject.layer = 0;
                obj.gameObject.transform.parent = GameObject.Find("Wall").gameObject.transform;
                obj.name = "Background";
            }
        }
    }
}
