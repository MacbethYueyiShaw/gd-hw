using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{
    public Transform firepoint;
    public Transform arrow;
    public GameObject bulletPrefab;
    public bool ableToShoot = true;

    public float bulletForce = 20f;


     public void Shoot()
    {
        if (!ableToShoot)
            return;
        GameObject bullet = Instantiate(bulletPrefab, arrow.position, firepoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firepoint.up * bulletForce, ForceMode2D.Impulse);
    }
}
