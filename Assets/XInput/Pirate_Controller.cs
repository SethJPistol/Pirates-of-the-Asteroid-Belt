using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
//using XInputDotNetPure;
public class Pirate_Controller : MonoBehaviour
{
    // what player it is, will allow all players to be interacted with
    [SerializeField] XboxController Controller = XboxController.All;
    //speed can be changed in editor and by other scripts if there are powerups
    public float MoveSpeed =  500;

    //spawning points for respawning
    private Vector3 SpawnPos;
    private Quaternion SpawnRot;

    //debug checking controllers connected
    private bool CheckNumControlers;
    //rigidbody physics code
    private Rigidbody rb;
    //bullet stuff
    
    public GameObject cannonRight;
    public GameObject cannonLeft;

    //delay between shots
    public float shootingDelay = 2.0f;
    [HideInInspector] public bool canShootAgain = true;

    // Start is called before the first frame update
    void Start()
    {
        // debug num of contollers connected
        if (!CheckNumControlers)
        {
            CheckNumControlers = true;

            int queriedNumberOfCtrlrs = XCI.GetNumPluggedCtrlrs();
            if (queriedNumberOfCtrlrs == 1)
            {
                Debug.Log("Only " + queriedNumberOfCtrlrs + " Xbox controller plugged in.");
            }
            else if (queriedNumberOfCtrlrs == 0)
            {
                Debug.Log("No Xbox controllers plugged in!");
            }
            else
            {
                Debug.Log(queriedNumberOfCtrlrs + " Xbox controllers plugged in.");
            }

            XCI.DEBUG_LogControllerNames();
        }

        SpawnPos = transform.position;
        SpawnRot = transform.rotation;

        rb = gameObject.GetComponent<Rigidbody>();
        cannonRight.GetComponent<Cannon>();
        cannonLeft.GetComponent<Cannon>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // get move input
        Vector3 moveInput = new Vector3(XCI.GetAxisRaw(XboxAxis.LeftStickX, Controller), 0.0f, XCI.GetAxisRaw(XboxAxis.LeftStickY, Controller));
        rb.AddForce  (moveInput.normalized * MoveSpeed);
        
        // if input detected look in direction player is moving
        if (moveInput != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveInput);
        }
        //rb.velocity = (moveInput * MoveSpeed);

        if (XCI.GetAxis(XboxAxis.RightTrigger, Controller) > 0.5)
        {
            if(canShootAgain)
            {
                cannonRight.GetComponent<Cannon>().ShootBullet(10.0f);
                StartCoroutine(shootdelay());
            }
        }
        if (XCI.GetAxis(XboxAxis.LeftTrigger, Controller) > 0.5)
        {
            if (canShootAgain)
            {
                cannonLeft.GetComponent<Cannon>().ShootBullet(10.0f);
                StartCoroutine(shootdelay());
            }
        }
    }

    public IEnumerator shootdelay()
    {
        canShootAgain = false;
        yield return new WaitForSeconds(shootingDelay);
        canShootAgain = true;
    }


}
