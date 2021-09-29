using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject kaboom;
    public void Explode()
    {
        GameObject particleFire = Instantiate(kaboom, transform.position, Quaternion.identity);
        Destroy(particleFire, 7f);
        SoundManager.PlaySound(SoundManager.Sound.Kaboom);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Explode();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            Explode();
    }
}
