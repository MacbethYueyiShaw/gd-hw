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
    [SerializeField] GameObject hairParticle;//hair prefab
    [SerializeField] int size = 10;//list length
    [SerializeField] List<HairParticle> particles = new List<HairParticle>();
    [SerializeField] Transform head;
    [SerializeField] float head_radius = 0.5f;

   
    [SerializeField] float spacing = 0.5f;//spacing between each two particles
    [SerializeField] [Range(0, 1)] float damping=0.1f;//damping
    [SerializeField] float gravity=9.8f;//gravity
    [SerializeField] int step = 5;//timestep
    [SerializeField] int counter = 0;//step couter
    [SerializeField] float pr=0.05f;//particle radius

    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < size; i++) {
            Vector3 pos = root.transform.position;
            pos.x = i * spacing;
            HairParticle tmp_particle = new HairParticle();
           
            if (i == 0)
            {
                tmp_particle.parent_transform = root.transform;
                tmp_particle.localRotation = root.transform.rotation;
                tmp_particle.length = 0;
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
        Debug.Log(Time.deltaTime);
        counter++;
        if (counter <= step)
        {
            return;
        }
        else
        {
            counter = 0;
        }

        for (int i = 0; i < size; i++)
        {
            Verlet(particles[i]);
        }
    }

    //caculate next time pos of current hair particle with the pos of current pos and previous pos
    void Verlet(HairParticle curr_particle)
    {
        //verlet
        Vector3 g = Vector3.down * gravity;
        Vector3 new_pos;
        float sqrt_t = Time.deltaTime * Time.deltaTime * step * step;
        new_pos = curr_particle.curPos + damping * (curr_particle.curPos - curr_particle.prePos) + g * sqrt_t;
        curr_particle.prePos = curr_particle.curPos;
        curr_particle.curPos = new_pos;

        //collision
        //Head
        if (Vector3.Distance(curr_particle.curPos,head.position)<=(head_radius+curr_particle.radius))
        {
            Vector3 normal = (curr_particle.curPos - head.position).normalized;
            curr_particle.curPos = head.position + (normal * (head_radius + curr_particle.radius));
        }
        //other particle
        for (int i = 0; i < size; i++)
        {
            if (i == curr_particle.index) continue;
            if (Vector3.Distance(curr_particle.curPos, particles[i].curPos) <= (particles[i].radius + curr_particle.radius))
            {
                Vector3 normal = (curr_particle.curPos - particles[i].curPos).normalized;
                curr_particle.curPos = particles[i].curPos + (normal * (particles[i].radius + curr_particle.radius));
            }
        }

        //constrain
        curr_particle.curPos = ((curr_particle.curPos - curr_particle.parent_transform.position).normalized * curr_particle.length) + curr_particle.parent_transform.position;



        //rendering
        curr_particle.Update_rendering();

        return;
    }
}
