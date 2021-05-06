using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_shaking : MonoBehaviour
{
    public GameManager gm;
    private float existTime = 2f;
    private float currTime = 0f;
    private float speed = 2f;
    private Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        if (currTime > existTime)
            Destroy(gameObject);

        Vector3 bias = Vector3.up;
        bias *= Mathf.Sin(Time.time * speed);
        transform.position = pos + bias;
    }
    public void CubeClick()
    {
        gm.GetScore(3);
        Destroy(gameObject);
    }
}
