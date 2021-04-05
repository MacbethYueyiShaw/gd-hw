using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	public float maxHealth = 100f;
	public float currentHealth;
    public float maxMana = 100f;
    public float currentMana;
    public float manaRecovery = 1f;

    public HealthBar healthBar;
    public ManaBar manaBar;

    // Start is called before the first frame update
    void Start()
    {
		currentHealth = maxHealth;
        currentMana = maxMana;
		healthBar.SetMaxHealth(maxHealth);
        manaBar.SetMaxMana(maxMana);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20f);
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

    void TakeDamage(float damage)
	{
		currentHealth -= damage;

		healthBar.SetHealth(currentHealth);
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
}
