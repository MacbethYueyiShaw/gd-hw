using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    public float speed;
    public float checkRadius;
    public LayerMask platformMask;
    public GameObject groundCheck;
    public bool isOnGround;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isOnGround = Physics2D.OverlapCircle(groundCheck.transform.position, checkRadius, platformMask);
        animator.SetBool("isOnGround", isOnGround);
        Move();
    }

    void Move()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(xInput * speed, rb.velocity.y);
        if (xInput != 0)
            transform.localScale = new Vector3(xInput, 1, 1);
        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));
    }

    public void Die()
    {
        Debug.Log("Die");
        animator.SetBool("isdead", true);
        GameManager.GameOver(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spike"))
        {
            Die();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.transform.position, checkRadius);
    }
}
