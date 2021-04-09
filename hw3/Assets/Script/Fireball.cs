using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public GameObject effect;
    float existTime = 5f;
    float currentTime = 0f;

    void Update()
    {
        if (currentTime>=existTime)
        {
            Destroy(gameObject);
        }
        currentTime += Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.name != "player")
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
    }
}
