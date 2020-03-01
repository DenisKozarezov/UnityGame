using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Initialize : MonoBehaviour
{
    public Sprite Background;
    public Material LightMaterial;
    // Start is called before the first frame update
    void Start()
    {
        Clear();
        GameObject wall = new GameObject();
        wall.transform.parent = GameObject.Find("BACKGROUND LAYER").transform;
        wall.name = "Wall";
        for (int i = 1; i <= 25; i++)
        {
            for (int j = 1; j <= 5; j++)
            {
                GameObject obj = new GameObject();
                obj.AddComponent<SpriteRenderer>().sprite = Background;
                obj.GetComponent<SpriteRenderer>().material = LightMaterial;
                obj.transform.position = new Vector2(i * Background.bounds.size.x, j * Background.bounds.size.y);
                obj.gameObject.layer = 0;                
                obj.gameObject.transform.parent = wall.gameObject.transform;
                obj.name = "Background";
            }
        }
    }
    
    void Clear()
    {
        for (int i = 0; i < GameObject.Find("BACKGROUND LAYER").transform.childCount; i++)
        {
            DestroyImmediate(GameObject.Find("BACKGROUND LAYER").transform.GetChild(i).gameObject);
        }
    }
}
