using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            Boid boid = collision.gameObject.transform.parent.GetComponent<Boid>();

            if (boid != null)
            {
                Boid[] boids = BoidManager.Instance.GetBoids();

                var boidList = new List<Boid>(boids);

                boidList.Remove(boid);
                BoidManager.Instance.SetBoids(boidList.ToArray());

                Destroy(boid);
                boid.gameObject.AddComponent<Rigidbody>();
            }
        }
    }


}
