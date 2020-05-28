using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Unit>() && !collision.gameObject.GetComponent<Unit>().Invulnerable && !collision.gameObject.GetComponent<Unit>().IsDead)
        {
            collision.gameObject.GetComponent<Unit>().Kill(collision.gameObject.GetComponent<Unit>());
        }
    }
}
