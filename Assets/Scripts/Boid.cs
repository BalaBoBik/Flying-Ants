using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Boid : MonoBehaviour
{

    // State
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 forward;
    Vector3 velocity;

    private Collider[] boids;



    //To Update

    Vector3 acceleration;
    [HideInInspector]
    public Vector3 avgFlockHeading;
    [HideInInspector]
    public Vector3 avgAvoidanceHeading;
    [HideInInspector]
    public Vector3 centreOfFlockmates;
    [HideInInspector]
    public int numPerceivedFlockmates;


    // Settings

    public Target target;
    public float minSpeed = 2;
    public float maxSpeed = 5;
    public float perceptionRadius = 2.5f;
    public float avoidanceRadius = 1;
    public float maxSteerForce = 3;

    public float alignWeight = 1;
    public float cohesionWeight = 1;
    public float seperateWeight = 1;

    public float targetWeight = 1;

    [Header("Collisions")]
    public float boundsRadius = .27f;
    public float avoidCollisionWeight = 10;
    public float collisionAvoidDst = 5;

    //Cached 
    Transform cachedTransform;
    LayerMask boidMask;
    LayerMask obstacleMask;
    Boid boid;
    int i;
    Vector3 offset;
    float sqrDst;

    private void Awake()
    {
        cachedTransform = transform;
        position = cachedTransform.position;
        forward = cachedTransform.forward;
        boidMask = LayerMask.GetMask("Boids");
        obstacleMask = LayerMask.GetMask("Walls");
        velocity = Vector3.zero;

        target = UpdateTarget();

    }
    private void Start()
    {
        InvokeRepeating("UpdateTarget", 5, 5);
    }
    private void Update()
    {
        acceleration = Vector3.zero;
        avgFlockHeading = Vector3.zero;
        avgAvoidanceHeading = Vector3.zero;
        centreOfFlockmates = Vector3.zero;


        if (target != null)
        {
            Vector3 offsetToTarget = (target.transform.position - position);
            acceleration = SteerTowards(offsetToTarget) * targetWeight;
        }


        boids = Physics.OverlapSphere(position, perceptionRadius, boidMask);

        for (i = 0; i < boids.Length; i++)
        {
            boid = boids[i].GetComponent<Boid>();
            offset = position - boid.position;
            sqrDst = offset.sqrMagnitude;

            avgFlockHeading += boid.forward;        //сомнительно, но окэй
            if (sqrDst < avoidanceRadius)
            {
                avgAvoidanceHeading -= offset / sqrDst;
            }
            centreOfFlockmates += boid.position;
        }


        // расчет вектора ускорения
        if (boids.Length != 0)
        {
            centreOfFlockmates /= boids.Length;

            Vector3 offsetToFlockmatesCentre = (centreOfFlockmates - position);

            var alignmentForce = SteerTowards(avgFlockHeading) * alignWeight;
            var cohesionForce = SteerTowards(offsetToFlockmatesCentre) * cohesionWeight;
            var seperationForce = SteerTowards(avgAvoidanceHeading) * seperateWeight;

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += seperationForce;
        }
        // Если впереди препятствие,То запускается поиск обхода 
        if (IsHeadingForCollision())
        {
            Vector3 collisionAvoidDir = ObstacleRays();
            Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * avoidCollisionWeight;
            acceleration += collisionAvoidForce;
        }

        velocity += acceleration * Time.deltaTime;
        float speed = velocity.magnitude;
        Vector3 dir = velocity / speed;
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        velocity = dir * speed;

        cachedTransform.position += velocity * Time.deltaTime;
        cachedTransform.forward = dir;
        position = cachedTransform.position;
        forward = dir;
        Debug.DrawRay(position, Vector3.ClampMagnitude(acceleration, maxSpeed), Color.blue);
    }
    bool IsHeadingForCollision()
    {
        RaycastHit hit;
        if (Physics.SphereCast(position, boundsRadius, forward, out hit, collisionAvoidDst, obstacleMask))
        {
            return true;
        }
        else { }
        return false;
    }
    Vector3 ObstacleRays()
    {
        Vector3[] rayDirections = BoidHelper.directions;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 dir = cachedTransform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(position, dir);
            if (!Physics.SphereCast(ray, boundsRadius, collisionAvoidDst, obstacleMask))
            {
                return dir;
            }
        }

        return forward;
    }

    Target UpdateTarget()
    {
        if ((target == null)||((target.position - position).sqrMagnitude<target.targetRadius*target.targetRadius)) {
        var targets = FindObjectsOfType<Target>();
        if (targets.Length > 0)
        {
            int index = 0;
            Vector3 OffsetToTarget;
            float maxPreference = 0;
            float preference;
            float lackOfBoids;
            for (int i = 0; i < targets.Length; i++)
            {
                OffsetToTarget = (targets[i].position - position);
                    lackOfBoids = float.MaxValue;
                if (targets[i].boidsCount!=0)
                    lackOfBoids = targets[i].boidRequired / targets[i].boidsCount;
                preference = lackOfBoids/OffsetToTarget.sqrMagnitude; 
                if (maxPreference < preference)
                {
                    maxPreference = preference;
                    index = i;
                }
            }
            return targets[index];
        }}
        return null;
    }

    Vector3 SteerTowards(Vector3 vector)
    {
        Vector3 v = vector.normalized * maxSpeed - velocity;
        return Vector3.ClampMagnitude(v, maxSteerForce);
    }
}


