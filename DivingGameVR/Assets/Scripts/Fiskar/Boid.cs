using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
public class Boid : MonoBehaviour
{
    public Material deadMaterial;
    BoidSettings boidSettings;

    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 forward;
    Vector3 velocity;

    [HideInInspector]
    public Vector3 averageFlockCource;
    [HideInInspector]
    public Vector3 averageAvoidanceCource;
    [HideInInspector]
    public Vector3 centerPosOfFlock;

    //hur många kompissar
    public int flockNumber;

    Transform savedTransform;
    Transform target;

    public float sizeVariationMax = 1;
    public float sizeVaritaionMin = 1;
    private void Awake()
    {
        savedTransform = gameObject.transform;
        savedTransform.localScale = Vector3.one * Random.Range(sizeVaritaionMin, sizeVariationMax);

        //Animator animator = GetComponent<Animator>();
        //if(animator != null)
        //{
        //    float randomStart = Random.Range(0, animator.GetCurrentAnimatorStateInfo(0).length);
        //    var clip = animator.GetCurrentAnimatorClipInfo(0);
        //    animator.Play(clip[0].clip, 0, randomStart);
        //}
    }

    public void SetUpFish(BoidSettings settings, Transform target)
    {
        this.target = target;
        this.boidSettings = settings;
        position = savedTransform.position;
        forward = savedTransform.forward;

        float startSpeed = Random.Range(settings.minSpeed, settings.maxSpeed);
        velocity = transform.forward * startSpeed;
    }

    public void UpdateBoid()
    {
        Vector3 acc = Vector3.zero;

        if(target != null)
        {
            Vector3 targetOffset = target.position - position;
            acc = SteerTorwards(targetOffset) * boidSettings.targetWeight;
        }

        //Ställer in så att den gämnar ut stats med flocken
        if(flockNumber != 0)
        {
            centerPosOfFlock = centerPosOfFlock / flockNumber;

            Vector3 offSetToFlockMates = centerPosOfFlock - position;

            //kraft i flockriktning
            Vector3 alignmentForce = SteerTorwards(averageFlockCource) * boidSettings.alignWeight;

            //hur starkt man sitter ihop
            Vector3 cohesionForce = SteerTorwards(offSetToFlockMates) * boidSettings.cohesionWeight;

            //kraft att splittra
            Vector3 seperationForce = SteerTorwards(averageAvoidanceCource) * boidSettings.seperateWeight;

            acc += alignmentForce;
            acc += cohesionForce;
            acc += seperationForce;

        }

        if(IsObstacleInTheWay())
        {
            Vector3 collisionAvoidDir = ObstacleRays();
            Vector3 collisionAvoidForce = SteerTorwards(collisionAvoidDir) * boidSettings.avoidCollisionWeight;
            acc += collisionAvoidForce;
        }

        velocity += acc * Time.deltaTime;
        float speed = velocity.magnitude;
        Vector3 dir = velocity / speed;
        speed = Mathf.Clamp(speed, boidSettings.minSpeed, boidSettings.maxSpeed);
        velocity = dir * speed;

        savedTransform.position += velocity * Time.deltaTime;
        //KAN GÅ ÅT SKOGEN
        savedTransform.rotation = Quaternion.Slerp(savedTransform.rotation,
                    Quaternion.LookRotation(dir),
                    boidSettings.rotationSpeed * Time.deltaTime);

        position = savedTransform.position;
        forward = dir;
    }

    bool IsObstacleInTheWay()
    {
        RaycastHit hit;
        if(Physics.SphereCast(position, boidSettings.boundsRadius, forward, out hit, boidSettings.collisionAvoidDist, boidSettings.obstacleMask))
        {
            return true;
        }

        return false;
    }

    Vector3 ObstacleRays()
    {
        //hämtar riktningar
        Vector3[] rayDirections = BoidHelper.directions;
        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 dir = savedTransform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(position, dir);

            //kollar om det finns en väg att åka
            if(!Physics.SphereCast(ray, boidSettings.boundsRadius, boidSettings.collisionAvoidDist, boidSettings.obstacleMask))
            {
                return dir;
            }
        }

        return forward;
    }

    Vector3 SteerTorwards(Vector3 vector)
    {
        Vector3 v = vector.normalized * boidSettings.maxSpeed - velocity;
        return Vector3.ClampMagnitude(v, boidSettings.rotationSpeed);
    }

    public Collider GetCollider()
    {
        Collider collider = GetComponent<Collider>();
        
        if (collider == null)
            collider = GetComponentInChildren<Collider>();

        return collider;
    }

}
