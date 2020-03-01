using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class BattleAction : MonoBehaviour
{
    public GameObject Abox,
        BboxF,
        BboxB,
        player;
    PlayerControll pl;
    GameObject newBox;
    public float DDeley = 0.1f;
    public float ADistance = 2f;
    Moon_Fang cr;
    // Start is called before the first frame update
    void Start()
    {
        pl = GetComponent<PlayerControll>();
        cr = GetComponent<Moon_Fang>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            if (pl.lookForward)
            {
                newBox = Instantiate(Abox, new Vector2(player.transform.position.x + ADistance, transform.position.y), Quaternion.identity);
                Destroy(newBox,DDeley);
            }
            else
            {
                newBox = Instantiate(Abox, new Vector2(player.transform.position.x - ADistance, transform.position.y), Quaternion.identity);
                Destroy(newBox, DDeley);
            }
        if(Input.GetKeyDown(KeyCode.F))
            if (pl.lookForward)
            {
                Instantiate(BboxF, new Vector2(player.transform.position.x + ADistance, transform.position.y), Quaternion.identity);
            } else Instantiate(BboxB, new Vector2(player.transform.position.x - ADistance, transform.position.y), Quaternion.identity);
    }
}
