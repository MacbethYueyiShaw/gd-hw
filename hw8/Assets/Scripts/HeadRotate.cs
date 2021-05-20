using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotate : MonoBehaviour
{
    [SerializeField] float speed = 200;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.right * -0.01f * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * 0.01f * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * 0.01f * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.up * -0.01f * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, 0.5f * speed * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, -0.5f * speed * Time.fixedDeltaTime);
        }
    }
}
