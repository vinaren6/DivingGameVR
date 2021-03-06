using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Harpoon : MonoBehaviour
{
    private Spear spear;
    public float spearVelocity;
    bool canShoot;
    // Start is called before the first frame update


    LineRenderer line;

    public Transform lineStartPos;
    public Transform endLinePos;
    public int lineCount;
    public float height;

    Transform parent;


    //reset
    private bool whatTheFuck = false;
    Vector3 startPos;
    Quaternion startRot;
    private void Awake()
    {
        SaveStartPos();
        SoundManager.Initialize();

        spear = GetComponentInChildren<Spear>();
        canShoot = true;
        //inputActions = new MouseClick();
        //inputActions.Player.Mouse.performed += Shoot;
        //inputActions.Player.Mouse.Enable();


        parent = transform.parent;
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
    }

    private void SaveStartPos()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    public void ResetPosition()
    {
        spear.DeactivateSpear();
    }

    private void Update()
    {
        SoundManager.PlaySound(SoundManager.Sound.PlayerMove);
        if (canShoot == false)
        {
            DrawRope();
        }
    }

    public void Shoot()
    {
        //if ()
        {
            if (canShoot)
            {
                SoundManager.PlaySound(SoundManager.Sound.ShootSound, lineStartPos.position);
                line.positionCount = 2;
                spear.ActivateSpear();
                spear.ShootSpear(spearVelocity);
                canShoot = false;
            }
            else
            {
                line.positionCount = 0;
                spear.DeactivateSpear();
                canShoot = true;
            }
        }
    }

    public void DrawRope()
    {
        line.SetPosition(0, lineStartPos.position);
        line.SetPosition(1, endLinePos.position);

        //for (int i = 0; i < lineCount; i++)
        //{
        //    line.SetPosition(i, Arc(lineStartPos.position, endLinePos.position, height, i, transform.up));
        //}
    }

    //UR FUNKTION
    //Vector3 Arc(Vector3 start, Vector3 end, float height, float t, Vector3 outDirection)
    //{
    //    float parabolicT = t * 2 - 1;
    //    Vector3 travelDirection = end - start;
    //    Vector3 levelDirection = end - new Vector3(start.x, end.y, start.z);
    //    Vector3 right = Vector3.Cross(travelDirection, levelDirection);
    //    Vector3 up = outDirection;
    //    Vector3 result = start + t * travelDirection;
    //    result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
    //    return result;
    //}
}
