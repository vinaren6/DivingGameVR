using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseShoot : MonoBehaviour
{
    public GameObject bullet;
    private MouseClick input;

    private void Start()
    {
        input = new MouseClick();

        input.Player.Mouse.performed += Shoot;
        input.Player.Mouse.Enable(); 
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        Debug.Log("I EXIST");
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = Camera.main.farClipPlane * .05f;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);
        GameObject bullet = Instantiate(this.bullet, worldPoint, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 10, ForceMode.Impulse);
    }

}
