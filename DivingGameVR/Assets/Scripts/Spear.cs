using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    Rigidbody rb;
    Transform parent;
    Transform anchor;
    Transform spearTip;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        parent = transform.parent.transform.parent;
        
        anchor = transform.parent;

        spearTip = transform.Find("SpearTip");

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
        Boid boid = fishTarget.GetComponent<Boid>();
        if (boid == null)
        {
            boid = fishTarget.transform.parent.GetComponent<Boid>();
        }

        if (boid != null)
        {
            Boid[] boids = BoidManager.Instance.GetBoids();

            var boidList = new List<Boid>(boids);

            boidList.Remove(boid);
            BoidManager.Instance.SetBoids(boidList.ToArray());


            Destroy(boid.GetCollider());
            Destroy(boid);
            boid.gameObject.transform.SetParent(spearTip);
            Renderer renderer = fishTarget.GetComponent<Renderer>();
            if(renderer != null)
                fishTarget.GetComponent<Renderer>().material = boid.deadMaterial;

            Animator animator = fishTarget.GetComponent<Animator>();
            if (animator != null)
                animator.enabled = false;
        }

        ScoreManager.Instance.AddScore();
        ScoreManager.Instance.UpdateText();

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            HitFish(collision.gameObject);
        }
    }

}
