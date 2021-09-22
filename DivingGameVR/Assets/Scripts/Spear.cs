using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public Material deadFishMat;
    Rigidbody rb;
    Transform parent;
    Transform anchor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        parent = transform.parent.transform.parent;
        
        anchor = transform.parent;

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
        transform.position = anchor.position;
        transform.rotation = anchor.rotation;
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
