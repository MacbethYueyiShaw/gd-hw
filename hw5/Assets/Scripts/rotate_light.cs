using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate_light : MonoBehaviour
{
    public float speed = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.up * speed* Time.deltaTime);
    }
}
