using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Hair Particle Struct
public class HairParticle
{
    //particle index
    public int index;
    //hair renddering
    public GameObject hair_particle;
    //parent transform
    public Transform parent_transform;
    //parent rotation
    public Quaternion localRotation;
    //previous frame/current current frame pos
    public Vector3 prePos, curPos;
    //length between two particles
    public float length;
    //particle collider radius
    public float radius;
    public void Update_rendering()
    {
        hair_particle.transform.position = curPos;
    }
}

public class Hair : MonoBehaviour
{
    [SerializeField] GameObject root;
    [SerializeField] float angle_offset;
    private Vector3 root_normal;
    [SerializeField] GameObject hairParticle;//hair prefab
    public int size;//list length
    [SerializeField] List<HairParticle> particles = new List<HairParticle>();
    public Transform head;
    public float head_radius = 0.5f;
    public int iterations = 3;//iterations
    [SerializeField] float spacing = 0.5f;//spacing between each two particles
    [SerializeField] [Range(0, 1)] float damping=0.1f;//damping
    public float gravity=9.8f;//gravity
    [SerializeField] float pr=0.05f;//particle radius

    
    // Start is called before the first frame update
    void Start()
    {
        root_normal = Vector3.forward;
        //Debug.Log(root_normal.ToString("f3"));
        Vector3 axis = Vector3.up;
        root_normal = Quaternion.AngleAxis(root.transform.localEulerAngles.y + angle_offset, axis) * root_normal;
        //Debug.Log(root_normal.ToString("f3"));
        for (int i = 0; i < size; i++) {
            Vector3 pos = root.transform.position + root_normal * i * spacing;
            //Debug.Log(pos.ToString("f3"));
            HairParticle tmp_particle = new HairParticle();
           
            if (i == 0)
            {
                tmp_particle.parent_transform = root.transform;
                tmp_particle.localRotation = root.transform.rotation;
                tmp_particle.length = spacing;
            }
            else
            {
                tmp_particle.parent_transform = particles[particles.Count - 1].hair_particle.transform;
                tmp_particle.localRotation = particles[particles.Count - 1].hair_particle.transform.rotation;
                tmp_particle.length = spacing;
            }
            tmp_particle.hair_particle = Instantiate(hairParticle, pos, tmp_particle.localRotation);
            tmp_particle.hair_particle.transform.parent = this.transform;

            tmp_particle.prePos = pos;
            tmp_particle.curPos = pos;
            tmp_particle.radius = pr;
            tmp_particle.index = i;
            particles.Add(tmp_particle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.deltaTime);

        for (int i = 0; i < size; i++)
        {
            Verlet(particles[i]);
        }

        for (int j = 0; j < iterations; j++)
        {
            for (int i = 0; i < size; i++)
            {
                CheckCollision(particles[i]);
                Constrain(particles[i]);

                //fix the root
                particles[0].curPos = root.transform.position;
            } 
        }
        

        //rendering
        for (int i = 0; i < size; i++)
        {
            particles[i].Update_rendering();
        }
    }

    //caculate next time pos of current hair particle with the pos of current pos and previous pos
    void Verlet(HairParticle curr_particle)
    {
        //verlet
        Vector3 g = Vector3.down * gravity;
        Vector3 new_pos;
        float sqrt_t = Time.deltaTime * Time.deltaTime;
        new_pos = curr_particle.curPos + damping * (curr_particle.curPos - curr_particle.prePos) + g * sqrt_t;
        curr_particle.prePos = curr_particle.curPos;
        curr_particle.curPos = new_pos;

        return;
    }

    void CheckCollision(HairParticle curr_particle)
    {
        if (curr_particle.index == 0) return;
        //collision
        //Head
        if (Vector3.Distance(curr_particle.curPos, head.position) <= (head_radius + curr_particle.radius))
        {
           /* Debug.Log(Vector3.Distance(curr_particle.curPos, head.position));
            Debug.Log(head_radius + curr_particle.radius);
            Debug.Log(curr_particle.index.ToString() + " collison with head");*/
            Vector3 normal = (curr_particle.curPos - head.position).normalized;
            curr_particle.curPos = head.position + (normal * (head_radius + curr_particle.radius));
        }
        //other particle
        for (int i = 0; i < size; i++)
        {
            if (i == curr_particle.index) continue;
            if (Vector3.Distance(curr_particle.curPos, particles[i].curPos) <= (particles[i].radius + curr_particle.radius))
            {
                /*Debug.Log(Vector3.Distance(curr_particle.curPos, particles[i].curPos));
                Debug.Log(particles[i].radius + curr_particle.radius);
                Debug.Log(curr_particle.index.ToString() + " collison with " + i.ToString());*/
                Vector3 normal = (curr_particle.curPos - particles[i].curPos).normalized;
                curr_particle.curPos = particles[i].curPos + (normal * (particles[i].radius + curr_particle.radius));
            }
        }
    }

    void Constrain(HairParticle curr_particle)
    {
        //constrain
        //method1
        /*Vector3 distance = curr_particle.curPos - curr_particle.parent_transform.position;
        float length = Vector3.Distance(curr_particle.curPos, curr_particle.parent_transform.position);
        Vector3 delta = distance.normalized * (length - curr_particle.length) / 2;

        if (curr_particle.index > 1)
        {
            curr_particle.curPos -= delta;
            particles[curr_particle.index - 1].curPos = particles[curr_particle.index - 1].curPos + delta;
        }
        else if (curr_particle.index == 1)
        {
            curr_particle.curPos -= delta;
        }*/
        //method2
        if (curr_particle.index == size - 1) return;
        Vector3 x1 = curr_particle.curPos;
        Vector3 x2 = particles[curr_particle.index + 1].curPos;
        Vector3 delta = (x2 - x1).normalized * (Vector3.Distance(x1, x2) - curr_particle.length) / 2;
        curr_particle.curPos += delta;
        particles[curr_particle.index + 1].curPos -= delta;
    }

    public void EditLength(int value)
    {
        if (value == size) return;
        else if (value > size)
        {
            for (int i = size; i < value; i++)
            {
                Vector3 pos = particles[i-1].hair_particle.transform.position + root_normal * (i-1) * spacing;
                //Debug.Log(pos.ToString("f3"));
                HairParticle tmp_particle = new HairParticle();

                if (i == 0)
                {
                    tmp_particle.parent_transform = root.transform;
                    tmp_particle.localRotation = root.transform.rotation;
                    tmp_particle.length = spacing;
                }
                else
                {
                    tmp_particle.parent_transform = particles[particles.Count - 1].hair_particle.transform;
                    tmp_particle.localRotation = particles[particles.Count - 1].hair_particle.transform.rotation;
                    tmp_particle.length = spacing;
                }
                tmp_particle.hair_particle = Instantiate(hairParticle, pos, tmp_particle.localRotation);
                tmp_particle.hair_particle.transform.parent = this.transform;

                tmp_particle.prePos = pos;
                tmp_particle.curPos = pos;
                tmp_particle.radius = pr;
                tmp_particle.index = i;
                particles.Add(tmp_particle);
            }
        }
        else if (value < size)
        {
            for (int i = size; i > value; i--)
            {
                if (i == 0) return;
                Destroy(particles[i - 1].hair_particle);
                particles.Remove(particles[i - 1]);
            }
        }
        size = value;
    }

    public void SetDamping(float value)
    {
        damping = value;
    }
}
