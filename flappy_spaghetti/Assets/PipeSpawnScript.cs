using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{
    public GameObject pipe;
    public float spawnRate = 2.8f;
    private float timer = 0;
    public float heightOffset = 13;
    public LogicScript logic;

    void Start()
    {
        
        spawnPipe();
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    
    void Update()
    {
        if (timer < spawnRate - (getHarmonic(logic.playerScore)/6.0f))
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            spawnPipe();
            timer = 0;
        }

    }
    float getHarmonic(int i)
    {
        float r = 0;
        for (int j = 1; j <= i; j++)
        {
            r += 1.0f / j;
        }
        return r;
    }
    void spawnPipe()
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;

        Instantiate(pipe, new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0), transform.rotation);
    }
}