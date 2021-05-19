using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairStrand : MonoBehaviour
{
    [SerializeField] GameObject hair;
    [SerializeField] int size;
    [SerializeField] Transform head;
    [SerializeField] float head_radius = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        float alpha = 90f;
        float beta = 0f;
        float delta = 15f;
        float d2r =1/ 180f * Mathf.PI;
        for (int i = 0; i < 12; i++)
        {
            float x = head_radius * Mathf.Sin(alpha * d2r) * Mathf.Cos((beta + i * delta) * d2r);
            float z = head_radius * Mathf.Sin(alpha * d2r) * Mathf.Sin((beta + i * delta) * d2r);
            float y = head_radius * Mathf.Cos(alpha * d2r);
            Vector3 pos;
            pos.x = x;
            pos.y = y;
            pos.z = z;
            Debug.Log(pos.ToString("f3"));
            GameObject tmp = Instantiate(hair, pos, head.rotation);
            tmp.transform.parent = this.transform;
            Hair tmp_hair = tmp.GetComponent<Hair>();
            tmp_hair.head = head;
            tmp_hair.head_radius = head_radius;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
