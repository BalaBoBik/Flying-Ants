using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum TargetState
{
    Calling,
    Working,
    Inactive,
}
public class Target : MonoBehaviour
{
    // Start is called before the first frame update

    // State
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public int boidsCount;

    private Collider[] boids;


    //Settings
    public int boidRequired;
    public float targetRadius;
    //Cached
    Transform cachedTransform;
    LayerMask boidMask;
    MeshRenderer mesh;
    void Start()
    {
        mesh = this.GetComponent<MeshRenderer>();
        cachedTransform = transform;
        position = cachedTransform.position;
        boidMask = LayerMask.GetMask("Boids");
    }
    Target(int boidRequired, float targetRadius)
    {
        this.boidRequired = boidRequired;
        this.targetRadius = targetRadius;
    }

    // Update is called once per frame
    void Update()
    {
        
        boids = Physics.OverlapSphere(position, targetRadius, boidMask);
        boidsCount = boids.Length;
        
        if (boidRequired < boidsCount)
        {
            mesh.material.color = Color.red;
        }
    }
}
