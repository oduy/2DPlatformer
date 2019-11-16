using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerProjectile : MonoBehaviour
{
    public GameObject projectile;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(projectile, transform.position, Quaternion.identity);

    }
}
