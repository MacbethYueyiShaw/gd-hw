using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public GameObject effect;
    float existTime = 5f;
    float currentTime = 0f;
    float DMG = 25f;
    bool makeDMG = false;

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
        //Debug.Log(collision.name);
        if (collision.name == "player"|| collision.tag == "Item")
        {
            return;
        }else if(collision.tag == "Enemy")
        {
            EnemyAI enemyai = collision.GetComponent<EnemyAI>();
            if (enemyai != null)
            {
                if (makeDMG) 
                    return;
                Vector2 direction = ((Vector2)enemyai.transform.position - (Vector2)transform.position).normalized;
                Vector2 dashForce = direction * DMG * 20f;
                enemyai.TakeDamage(DMG, dashForce);
                makeDMG = true;
            }
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
        
    }
}
