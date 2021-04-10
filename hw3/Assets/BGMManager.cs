using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public bool mute = false;
    public GameObject mute_on;
    public GameObject mute_off;
    AudioSource audio;

    void Start()
    {
        mute_on.SetActive(true);
        mute_off.SetActive(false);
        mute = false;
        audio = GetComponent<AudioSource>();
        UpdatePlayer(mute);
    }

    void UpdatePlayer(bool mute)
    {
        if (mute)
            audio.Pause();
        else audio.Play();
    }
    public void Mute()
    {
        mute_on.SetActive(false);
        mute_off.SetActive(true);
        mute = true;
        UpdatePlayer(mute);
    }
    public void Unmute()
    {
        mute_on.SetActive(true);
        mute_off.SetActive(false);
        mute = false;
        UpdatePlayer(mute);
    }
}
