using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hair : MonoBehaviour
{
    [SerializeField] GameObject root;
    [SerializeField] GameObject hairParticle;
    [SerializeField] int size = 10;
    [SerializeField] float constrain_length = 10;
    [SerializeField] List<GameObject> particles;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < size; i++) { 

            Vector3 pos = root.transform.position;
            pos.x = i*0.1f;
            Quaternion rotation = Quaternion.identity;
            GameObject tmp = Instantiate(hairParticle,pos, rotation); 
          
            particles.Add(tmp);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    Vector3 verlet()
    {
        return Vector3.zero;
    }
}
