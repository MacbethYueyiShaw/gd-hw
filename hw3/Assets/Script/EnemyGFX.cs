using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGFX : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void TakeDamageOver()
    {
        Debug.Log("EnemyTakeDamageOver!");
        animator.SetBool("TakeDMG", false);
    }
}
