using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBorn : MonoBehaviour
{
    bool hasBorned = false;
    public GameObject monster;
    public Score score;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasBorned) return;
        //Debug.Log(collision.name);
        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                GameObject mons = Instantiate(monster, transform.position, Quaternion.identity);
                EnemyAI enemyAI = mons.GetComponents<EnemyAI>()[0];
                enemyAI.target = player.transform;
                enemyAI.score = score;

                if (mons.GetComponentsInChildren<MonsterFirePoint>().Length > 0)
                {
                    MonsterFirePoint firePoint = mons.GetComponentsInChildren<MonsterFirePoint>()[0];
                    if (firePoint != null)
                    {
                        firePoint.target = player.transform;
                    }
                }
                hasBorned = true;
                Destroy(gameObject);
            } 
        }
    }
}
