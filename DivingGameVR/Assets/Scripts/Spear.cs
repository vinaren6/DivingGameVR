using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public Material deadFishMat;
    Vector3 startPos;
    Quaternion startRot;
    Rigidbody rb;
    Transform parent;

    private void Awake()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        rb = GetComponent<Rigidbody>();

        parent = transform.parent;

        DeactivateSpear();
    }


    void FixedUpdate()
    {
        if (rb.velocity != Vector3.zero)
            rb.rotation = Quaternion.LookRotation(rb.velocity);
    }

    public void ShootSpear(float vel)
    {
        rb.AddForce(transform.forward * vel, ForceMode.Impulse);
    }
    public void ActivateSpear()
    {
        rb.isKinematic = false;
        transform.SetParent(null);
    }

    public void DeactivateSpear()
    {
        rb.isKinematic = true;
        transform.position = startPos;
        transform.rotation = startRot;
        rb.velocity = Vector3.zero;
        transform.SetParent(parent);
    }

    public void HitFish(GameObject fishTarget)
    {
        Boid boid = fishTarget.transform.parent.GetComponent<Boid>();

        if (boid != null)
        {
            Boid[] boids = BoidManager.Instance.GetBoids();

            var boidList = new List<Boid>(boids);

            boidList.Remove(boid);
            BoidManager.Instance.SetBoids(boidList.ToArray());

            Destroy(boid);
            boid.gameObject.AddComponent<Rigidbody>();
            fishTarget.GetComponent<Renderer>().material = boid.deadMaterial;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            HitFish(collision.gameObject);
        }
    }

}
