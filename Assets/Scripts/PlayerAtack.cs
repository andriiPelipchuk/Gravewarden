using System;
using UnityEngine;

public class PlayerAtack : MonoBehaviour
{
    public float speed;
    public GameObject shootPoint, projectile; 
    public int damage = 10; 
    public float attackRange = 1.5f; 

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Atack();
        }
    }

    private void Atack()
    {
        Instantiate(projectile, shootPoint.transform.position, Quaternion.identity);
    }
}
