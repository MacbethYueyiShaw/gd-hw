using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGFX : MonoBehaviour
{
    Animator animator;
    public EnemyAI enemyAI;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void TakeDamageOverSlot()
    {
        enemyAI.TakeDamageOver();
    }

    void DeathAnimationOverSlot()
    {
        enemyAI.DeathAnimationOver();
    }
}
