using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShootBullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firepoint;
    public Transform arrow;

    public float cd = 1f;
    float cdCounter = 0f;
    public float bulletForce = 20f;

    // Update is called once per frame
    void FixedUpdate()
    {
        cdCounter += Time.deltaTime;
        if (cdCounter >= cd)
        {
            GameObject bullet = Instantiate(bulletPrefab, arrow.position, firepoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firepoint.up * bulletForce, ForceMode2D.Impulse);
            cdCounter =0f;
        }
    }
}
