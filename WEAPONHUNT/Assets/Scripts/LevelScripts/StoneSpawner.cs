using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner : MonoBehaviour {

    public float spawnDelay = 0.3f;

    public GameObject stone;

    float nextTimetoSpawn = 0f;

     void Update()
    {
        if(nextTimetoSpawn <= Time.time)
        {
            SpawnStone();
            nextTimetoSpawn = Time.time + spawnDelay;
        }
        
    }

     void SpawnStone()
    {
        Instantiate(stone);
    }
}
