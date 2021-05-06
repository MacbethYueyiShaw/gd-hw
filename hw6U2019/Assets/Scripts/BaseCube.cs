using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCube : MonoBehaviour
{
    public GameManager gm;
    private float existTime = 2f;
    private float currTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        if(currTime>existTime)
            Destroy(gameObject);
    }
    public void CubeClick()
    {
        gm.GetScore(1);
        Destroy(gameObject);
    }
}
