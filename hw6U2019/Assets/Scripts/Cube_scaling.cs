using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_scaling : MonoBehaviour
{
    public GameManager gm;
    private float existTime = 3f;
    private float currTime = 0f;
    private float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        if (currTime > existTime)
            Destroy(gameObject);

        Vector3 scale =Vector3.one;
        scale *= Mathf.Sin(Time.time * speed);
        transform.localScale = Vector3.one + 0.5f * scale;
    }
    public void CubeClick()
    {
        gm.GetScore(2);
        Destroy(gameObject);
    }
}
