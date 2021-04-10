using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public GameObject impactEffect;
    public float atk = 10.0f;
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        //Debug.Log(hitInfo.name);
        Player player = hitInfo.GetComponent<Player>();
        if (player != null)
        {
            Vector2 direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
            Vector2 dashForce = direction * atk * 50f;
            Vector3 offset;
            offset.x = 0f;
            offset.y = 0.5f;
            offset.z = 0f;
            if (player.TakeDamage(atk, dashForce))
                Instantiate(impactEffect, hitInfo.transform.position - offset, hitInfo.transform.rotation);
        }
        //Destroy(gameObject);
    }
    private void OnTriggerStay2D(Collider2D hitInfo)
    {
        //Debug.Log(hitInfo.name);
        Player player = hitInfo.GetComponent<Player>();
        if (player != null)
        {
            Vector2 direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
            Vector2 dashForce = direction * atk * 50f;
            Vector3 offset;
            offset.x = 0f;
            offset.y = 0.5f;
            offset.z = 0f;
            if (player.TakeDamage(atk, dashForce))
                Instantiate(impactEffect, hitInfo.transform.position - offset, hitInfo.transform.rotation);
        }
        //Destroy(gameObject);
    }
}
