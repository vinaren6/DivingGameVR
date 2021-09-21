using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    private Rigidbody rb;

    public Material deadFishMat;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void ActivateSpear()
    {
        rb.isKinematic = false;
    }

    public void DeactivateSpear()
    {
        rb.isKinematic = true;
    }

    public void Shoot(float force)
    {
        //sätt rätt
        rb.AddForce(transform.up * force, ForceMode.VelocityChange);
        //rb.AddForce(transform.forward * (-force/3f), ForceMode.VelocityChange);
    }

    public void HitFish(GameObject fish)
    {
        Boid boid = fish.transform.parent.GetComponent<Boid>();

        if (boid != null)
        {
            Boid[] boids = BoidManager.Instance.GetBoids();

            var boidList = new List<Boid>(boids);

            boidList.Remove(boid);
            BoidManager.Instance.SetBoids(boidList.ToArray());

            Destroy(boid);
            boid.gameObject.AddComponent<Rigidbody>();
            fish.GetComponent<Renderer>().material = deadFishMat;
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
