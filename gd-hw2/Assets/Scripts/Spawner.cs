using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> platforms;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float spawnTime;
    private float countTime;
    // Update is called once per frame
    void Update()
    {
        countTime += Time.deltaTime;
        if (countTime >= spawnTime)
        {
            SpawnPlatform();
            countTime = 0;
        }
    }

    public void SpawnPlatform()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.x = Random.Range(-3.5f, 3.5f);

        int index = Random.Range(0, platforms.Count);
        GameObject go = Instantiate(platforms[index], spawnPosition, Quaternion.identity);
    }
}
