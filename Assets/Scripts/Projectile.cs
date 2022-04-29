using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private float speed;
    [SerializeField] private float ttl=5f;

    void Start()
    {
        Destroy(gameObject, ttl);
    }

    public static Projectile Shoot(Projectile projectile, Vector3 dir,float speed)
    {
        Projectile p = Instantiate(projectile);
        p.transform.forward = dir;
        p.GetComponent<Rigidbody>().velocity =dir.normalized*speed;
        return p;
    }
}
