using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HairStrand : MonoBehaviour
{
    [SerializeField] GameObject hair;
    [SerializeField] Slider sizeSlider;
    [SerializeField] Slider lengthSlider;
    [SerializeField] Slider iterationsSlider;
    [SerializeField] Slider gravitySlider;
    [SerializeField] Slider dampingSlider;
    [SerializeField] int strand_number = 33;
    [SerializeField] int length = 10;
    [SerializeField] [Range(0, 1)] float damping = 0.1f;
    [SerializeField] int p_iterations = 3;
    [SerializeField] float gravity = 9.8f;
     private bool isInit = false;
    [SerializeField] Transform head;
    [SerializeField] float head_radius = 0.5f;
    [SerializeField] List<GameObject> hairs = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        AddHair(strand_number);
        sizeSlider.value = strand_number;
        lengthSlider.value = length;
        iterationsSlider.value = p_iterations;
        gravitySlider.value = gravity;
        dampingSlider.value = damping;
        isInit = true;
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    void AddHair(int value)
    {
        float alpha = 90f;
        float beta = 0f;
        float delta = 15f;
        float d2r = 1 / 180f * Mathf.PI;
        int loop_times = 0;
        int iterations = 12;

        int tmp_size = value;
        int counter = 0;
        int remain = 0;
        while (tmp_size >= 0)
        {
            //Debug.Log(counter);
            remain = tmp_size;
            tmp_size -= (iterations - counter);
            loop_times++;
            counter++;
        }

        /* Debug.Log(remain);
         Debug.Log(loop_times);*/
        int index = 0;

        for (int j = 0; j < loop_times; j++)
        {
            if (j == loop_times - 1)
            {
                iterations = remain;
            }

            //Debug.Log(iterations);

            for (int i = 0; i < iterations; i++)
            {
                if (index++ < strand_number && isInit) continue;
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
                tmp_hair.size = length;
                tmp_hair.SetDamping(damping);
                tmp_hair.iterations = p_iterations;
                tmp_hair.gravity = gravity;
                hairs.Add(tmp);
            }

            alpha -= 20f;
            beta += 7.5f;
            iterations--;
        }
        strand_number = value;
    }

    public void EditSize()
    {
        int value = (int)sizeSlider.value;
        if (value == strand_number) return;
        else if (value > strand_number)
        {
            AddHair(value);
        }
        else if (value < strand_number)
        {
            for(int i = strand_number; i > value; i--)
            {
                if (i == 0) return;
                Destroy(hairs[i - 1]);
                hairs.Remove(hairs[i-1]);
            }
            strand_number = value;
        } 
    }

    public void EditLength()
    {
        int value = (int)lengthSlider.value;
        for (int i = 0; i < strand_number; i++)
        {
            Hair tmp_hair = hairs[i].GetComponent<Hair>();
            tmp_hair.EditLength(value);
        }
        length = value;
    }

    public void EditIterations()
    {
        int value = (int)iterationsSlider.value;
        for (int i = 0; i < strand_number; i++)
        {
            Hair tmp_hair = hairs[i].GetComponent<Hair>();
            tmp_hair.iterations = value;
        }
        p_iterations = value;
    }

    public void EditGravity()
    {
        float value = gravitySlider.value;
        for (int i = 0; i < strand_number; i++)
        {
            Hair tmp_hair = hairs[i].GetComponent<Hair>();
            tmp_hair.gravity = value;
        }
        gravity = value;
    }

    public void EditDamping()
    {
        float value = dampingSlider.value;
        for (int i = 0; i < strand_number; i++)
        {
            Hair tmp_hair = hairs[i].GetComponent<Hair>();
            tmp_hair.SetDamping(value);
        }
        damping = value;
    }
}
