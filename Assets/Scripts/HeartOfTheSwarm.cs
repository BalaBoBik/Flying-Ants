using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartOfTheSwarm : MonoBehaviour
{
    public Transform boidPrefab;
    public int swarmCount = 10;
    public float spawnRadius = 10;
    public bool Cycle = false;


    void Start()
    {
        Spawn();
        if (Cycle)
        {
            InvokeRepeating("Spawn", 10, 10);
        }
    }
    void Spawn ()
    {
        for (var i = 0; i < swarmCount; i++)
        {
            var boid = Instantiate(boidPrefab, Random.insideUnitSphere * spawnRadius, Quaternion.identity) as Transform;

            boid.parent = transform;
        }
    }
}
