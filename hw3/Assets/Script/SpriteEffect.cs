﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("EffectOver", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EffectOver()
    {
        Destroy(gameObject);
    }
}
