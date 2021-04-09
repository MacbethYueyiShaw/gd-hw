using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Animator animator;
    public shooting shoot;
    Rigidbody2D rb;

    public float maxHealth = 100f;
	public float currentHealth;
    public float maxMana = 100f;
    public float currentMana;
    public float manaRecovery = 5f;
    public float invincibleTime = 0.5f;
    bool isInvincible = false;

    public HealthBar healthBar;
    public ManaBar manaBar;

    // Start is called before the first frame update
    void Start()
    {
		currentHealth = maxHealth;
        currentMana = maxMana;
		healthBar.SetMaxHealth(maxHealth);
        manaBar.SetMaxMana(maxMana);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0f)
        {
            death();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 force;
            force.x = 0f;
            force.y = 0f;
            TakeDamage(20f, force);
        }
        if (Input.GetButtonDown("Fire1"))
        {
            UseSkill(20f);
        }
        if (currentMana<=maxMana)
        {
            currentMana += manaRecovery * Time.deltaTime;
            if(currentMana > maxMana)
            {
                currentMana = maxMana;
            }
            manaBar.SetMana(currentMana);
        }
    }
    void death()
    {
        //Debug.Log("Death");
        animator.SetBool("Death", true);
    }

    public void DeathAnimationOver()
    {
        //Debug.Log("destroy");
        //Destroy(gameObject);
    }

    public bool TakeDamage(float damage,Vector2 force)
	{
        if (isInvincible) 
            return false;
        //Debug.Log(force);
        currentHealth -= damage;
		healthBar.SetHealth(currentHealth);
        animator.SetBool("TakeDMG", true);
        rb.AddForce(force);
        isInvincible = true;
        Invoke("InvincibleOver", invincibleTime);
        return true;
    }

    public void ResetPosition()
    {
        rb.position *= 0f;
    }

    void UseSkill(float manaCost)
    {
        if (currentMana < manaCost)
        {
            shoot.ableToShoot = false;
            Debug.Log("Mana not enough!");
            return;
        }
        shoot.ableToShoot = true;
        currentMana -= manaCost;
        shoot.Shoot();
        manaBar.SetMana(currentMana);
    }
    void TakeDamageOver()
    {
        //Debug.Log("TakeDamageOver!");
        animator.SetBool("TakeDMG", false);
    }

    void InvincibleOver()
    {
        isInvincible = false;
    }
}
