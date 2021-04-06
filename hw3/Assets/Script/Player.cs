using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Animator animator;
    Rigidbody2D rb;

    public float maxHealth = 100f;
	public float currentHealth;
    public float maxMana = 100f;
    public float currentMana;
    public float manaRecovery = 1f;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 force;
            force.x = 0f;
            force.y = 0f;
            TakeDamage(20f, force);
        }
        if (Input.GetKeyDown(KeyCode.Q))
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
    void UseSkill(float manaCost)
    {
        if (currentMana < manaCost)
        {
            Debug.Log("Mana not enough!");
            return;
        }
        currentMana -= manaCost;

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
