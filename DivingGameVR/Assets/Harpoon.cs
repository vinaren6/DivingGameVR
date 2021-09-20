using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Harpoon : MonoBehaviour
{
    private Spear spear;
    public float spearVelocity;
    public bool canShoot;
    Vector3 spearPos;
    Quaternion spearRot;
    //private MouseClick input;

    private void Awake()
    {
        spear = GetComponentInChildren<Spear>();
        canShoot = true;
        spearPos = spear.transform.position;
        spearRot = spear.transform.rotation;
        //input = new MouseClick();

        //input.Player.Mouse.performed += Shoot;
        //input.Player.Mouse.Enable();
    }

    private void Update()
    {
        if (ControllerManager.Instance.rightTrigger > 0.2f)
            Shoot();
    }

    public void Shoot(/*InputAction.CallbackContext context*/)
    {
        if (canShoot)
        {
            spear.ActivateSpear();
            spear.Shoot(spearVelocity);
            canShoot = false;
        }
        else
        {
            RetriverSpear();
        }
    }
    public void RetriverSpear()
    {
        spear.DeactivateSpear();
        spear.transform.position = spearPos;
        transform.rotation = spearRot;

        canShoot = true;
    }
}
