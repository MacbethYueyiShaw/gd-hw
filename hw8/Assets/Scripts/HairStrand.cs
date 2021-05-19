using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairStrand : MonoBehaviour
{
    [SerializeField] GameObject hair;
    [SerializeField] int strand_number = 33;
    [SerializeField] Transform head;
    [SerializeField] float head_radius = 0.5f;
    [SerializeField] List<GameObject> hairs = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        float alpha = 90f;
        float beta = 0f;
        float delta = 15f;
        float d2r =1/ 180f * Mathf.PI;
        int loop_times = 0;
        int iterations = 12;

        int tmp_size = strand_number;
        int counter = 0;
        int remain = 0;
        while (tmp_size>=0)
        {
            //Debug.Log(counter);
            remain = tmp_size;
            tmp_size -= (iterations - counter);
            loop_times++;
            counter++;
        }

       /* Debug.Log(remain);
        Debug.Log(loop_times);*/

        for (int j = 0; j < loop_times; j++)
        {
            if (j == loop_times-1)
            {
                iterations = remain;
            }

            //Debug.Log(iterations);

            for (int i = 0; i < iterations; i++)
            {
                float x = head_radius * Mathf.Sin(alpha * d2r) * Mathf.Cos((beta + i * delta) * d2r);
                float z = head_radius * Mathf.Sin(alpha * d2r) * Mathf.Sin((beta + i * delta) * d2r);
                float y = head_radius * Mathf.Cos(alpha * d2r);
                Vector3 pos;
                pos.x = x;
                pos.y = y;
                pos.z = z;
                //Debug.Log(i);
                //Debug.Log(pos.ToString("f3"));
                GameObject tmp = Instantiate(hair, pos, head.rotation);
                tmp.transform.parent = this.transform;
                Hair tmp_hair = tmp.GetComponent<Hair>();
                tmp_hair.head = head;
                tmp_hair.head_radius = head_radius;
                hairs.Add(tmp);
            }

            alpha -= 20f;
            beta += 7.5f;
            iterations--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
