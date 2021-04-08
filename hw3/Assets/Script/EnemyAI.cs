using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public Transform GFX;
    public Animator animator;
    public GameObject impactEffect;

    public HealthBar healthBar;
    public float maxHealth = 100f;
    public float currentHealth;

    public float speed = 200f;
    public float atk = 10.0f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath",0f,0.5f);

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void TakeDamage(float damage)
    {
        Debug.Log("TakeDamageOver!");
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        animator.SetBool("TakeDMG", true);
    }

    void TakeDamageOver()
    {
        Debug.Log("TakeDamageOver!");
        animator.SetBool("TakeDMG", false);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        //Debug.Log(hitInfo.name);
        Player player = hitInfo.GetComponent<Player>();
        if (player != null)
        {
            Vector2 direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
            Vector2 dashForce = direction * atk * 200f;
            Vector3 offset;
            offset.x = 0f;
            offset.y = 0.5f;
            offset.z = 0f;
            if(player.TakeDamage(atk, dashForce))
                Instantiate(impactEffect, hitInfo.transform.position+offset, hitInfo.transform.rotation);
        }
        //Destroy(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20f);
        }

        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        //Debug.Log(force);
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (rb.velocity.x >= 0.01f)
        {
            GFX.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            GFX.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
