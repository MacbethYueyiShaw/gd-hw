using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBorn : MonoBehaviour
{
    bool hasBorned = false;
    public EnemyAI monster;
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
                EnemyAI mons = Instantiate(monster, transform.position, Quaternion.identity);
                mons.target = player.transform;
                mons.score = score;

                hasBorned = true;
                Destroy(gameObject);
            } 
        }
    }
}
