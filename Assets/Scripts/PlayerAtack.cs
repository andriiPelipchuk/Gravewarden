using System;
using UnityEngine;

public class PlayerAtack : MonoBehaviour
{
    public float speed;
    public GameObject gun, projectile;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Atack();
        }
    }

    private void Atack()
    {
        Instantiate(projectile, gun.transform.position, Quaternion.identity);
    }
}
