using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Range(0, 1)]
    public float ProjectileSpeed;

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other);
        if (other.tag == "Ground") Destroy(gameObject);
    }
}
