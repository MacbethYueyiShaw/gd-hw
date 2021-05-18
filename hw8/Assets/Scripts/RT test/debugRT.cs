using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugRT : MonoBehaviour
{
    public float counter=0f;
    public float timer=3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter >= timer)
        {
            Debug.Log(transform.localRotation.ToString("f3"));
            counter = 0;
        }
    }
}
