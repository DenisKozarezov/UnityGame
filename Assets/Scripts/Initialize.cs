using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Initialize : MonoBehaviour
{
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
                Sprite background = Resources.Load<Sprite>("Sprites/Background2");
                Material lightMaterial = Resources.Load<Material>("Materials/LightMaterial");

                if (background != null)
                {
                    GameObject obj = new GameObject();
                    obj.AddComponent<SpriteRenderer>().sprite = background;
                    obj.GetComponent<SpriteRenderer>().material = lightMaterial;

                    obj.transform.position = new Vector2(i * background.bounds.size.x, j * background.bounds.size.y);
                    obj.gameObject.layer = 0;
                    obj.gameObject.transform.parent = wall.gameObject.transform;
                    obj.name = "Background";
                }
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
