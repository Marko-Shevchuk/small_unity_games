using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{
    public GameObject pipePrefab;
    public float spawnPeriod = 2.8f;
    private float timer = 0;
    public float heightOffset = 13;
    public LogicScript logic;

    private GameObject[] pipePool;
    private int currentPipeIndex = 0;

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        InitializePipePool();
        SpawnPipe();
    }

    void InitializePipePool()
    {
        pipePool = new GameObject[12];
        for (int i = 0; i < pipePool.Length; i++)
        {
            GameObject pipe = Instantiate(pipePrefab);
            pipe.SetActive(false);
            pipePool[i] = pipe;
        }
    }

    void Update()
    {
        if (timer < spawnPeriod - (GetHarmonic(logic.playerScore) / 4.5f))
        {
            timer += Time.deltaTime;
        }
        else
        {
            SpawnPipe();
            timer = 0;
        }
    }

    float GetHarmonic(int i)
    {
        float r = 0;
        for (int j = 1; j <= i; j++)
        {
            r += 1.0f / j;
        }
        return r;
    }

    void SpawnPipe()
    {
        GameObject pipe = pipePool[currentPipeIndex];
        pipe.SetActive(true);
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;
        pipe.transform.position = new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0);
        pipe.transform.rotation = transform.rotation;

        currentPipeIndex = (currentPipeIndex + 1) % pipePool.Length;
    }
}