using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawnerScript : MonoBehaviour
{
    public GameObject obstacle;
    public float spawnRate = 3f;
    private float timer = 0;
    public float offset = 2;
    //public LogicScript logic;

    void Start()
    {

        spawnObstacle();
        //logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }
    void Update()
    {
        if (timer < spawnRate)
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            spawnObstacle();
            timer = 0;
        }

    }

    /*void Update()
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
    }*/
    void spawnObstacle()
    {
        float lowestPos = transform.position.z - offset;
        float highestPos = transform.position.z + offset;

        Instantiate(obstacle, new Vector3(transform.position.x, transform.position.y, Random.Range(lowestPos, highestPos)), transform.rotation);
    }
}
