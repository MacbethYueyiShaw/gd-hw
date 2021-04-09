using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    public Camera cam;
    public Rigidbody2D follow;
    public Rigidbody2D rb;
    Vector2 mousePos;

    // Update is called once per frame
    void Update()
    {
        Vector2 offset;
        offset.x = 0f;
        offset.y = -0.1f;
        rb.position = follow.position+offset;
        //Debug.Log(Input.mousePosition);
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(mousePos);
    }

    void FixedUpdate()
    {
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        rb.rotation = angle;
    }
}
