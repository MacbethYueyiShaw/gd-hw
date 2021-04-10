using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    Animator animator;
    AudioSource audio;
    bool musicNotPlayed = true;
    public Score score;
    private void Start()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }
    void PickupOver()
    {
        score.score += 500;
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        //Debug.Log(hitInfo.name);
        Player player = hitInfo.GetComponent<Player>();
        if (player != null)
        {
            animator.SetBool("Pickup", true);
            if (musicNotPlayed)
            {
                audio.Play();
                musicNotPlayed = false;
            }
        }
    }
}
