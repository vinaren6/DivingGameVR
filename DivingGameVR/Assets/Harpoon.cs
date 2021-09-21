using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
    private GameObject spearObj;
    private Spear spear;
    public float spearVelocity;
    public bool canShoot;
    // Start is called before the first frame update

    private void Awake()
    {
        spear = GetComponentInChildren<Spear>();
        spearObj = spear.gameObject;
        canShoot = true;
    }
    public void Shoot()
    {
        if(canShoot)
        {

        }
    }
}
