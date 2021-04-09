using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public GameObject impactEffect;
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        //Debug.Log(hitInfo.name);
        Player player = hitInfo.GetComponent<Player>();
        if (player != null)
        {
            Vector2 direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
            Vector2 dashForce = direction * 0f;
            if (player.TakeDamage(20f, dashForce))
                Instantiate(impactEffect, hitInfo.transform.position, hitInfo.transform.rotation);
            player.ResetPosition();
        }
        //Destroy(gameObject);
    }
}
