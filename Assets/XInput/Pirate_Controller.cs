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
    public float MaxSpeed = 30;
    public float turnspeed = 2;
    //spawning points for respawning
    private Vector3 SpawnPos;
    private Quaternion SpawnRot;
    private Vector3 spawnvel;
    //player lives
    public int PlayerLives = 3;
    public float RespawnImmunity = 2;

    //debug checking controllers connected
    private bool CheckNumControlers;
    //rigidbody physics code
    private Rigidbody rb;
    //bullet stuff
    
    public GameObject cannonRight;
    public GameObject cannonLeft;

    public float bulletSpeed;

    //delay between shots
    public float shootingDelay = 2.0f;
    [HideInInspector] public bool canShootAgain = true;

    // sounds stuff
    AudioSource mysource;
    
    public AudioClip Shot;
    public AudioClip PlayerDeath;

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
        mysource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
		// get move input
		Vector3 moveInput = new Vector3(XCI.GetAxisRaw(XboxAxis.LeftStickX, Controller), 0.0f, XCI.GetAxisRaw(XboxAxis.LeftStickY, Controller));
        rb.AddForce  (moveInput.normalized * MoveSpeed);
        if(rb.velocity.magnitude > MaxSpeed)
        {
            rb.AddForce(moveInput.normalized * -MoveSpeed);
        }
        //looking
        //Vector3 lookinput = new Vector3(XCI.GetAxisRaw(XboxAxis.RightStickX, Controller), 0.0f, XCI.GetAxisRaw(XboxAxis.RightStickY, Controller));

        // if input detected look in direction player is moving
        if (moveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            //gameObject.transform.rotation. = Quaternion.LookRotation(moveInput);
            rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, targetRotation, turnspeed * Time.deltaTime);
        }
        //rb.velocity = (moveInput * MoveSpeed);

        if (XCI.GetAxis(XboxAxis.RightTrigger, Controller) > 0.5)
        {
            if(canShootAgain)
            {
                cannonRight.GetComponent<Cannon>().ShootBullet(bulletSpeed);
                StartCoroutine(shootdelay());
                mysource.PlayOneShot(Shot, 1.0f);
            }
        }
        if (XCI.GetAxis(XboxAxis.LeftTrigger, Controller) > 0.5)
        {
            if (canShootAgain)
            {
                cannonLeft.GetComponent<Cannon>().ShootBullet(bulletSpeed);
                StartCoroutine(shootdelay());
            }
        }
    }

	private void OnBecameInvisible()	//When this object leaves the camera's view,
	{
		ScreenWrap.Instance.Wrap(gameObject);	//Wrap
	}

	public IEnumerator shootdelay()
    {
        canShootAgain = false;
        yield return new WaitForSeconds(shootingDelay);
        canShootAgain = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Asteroid")
        {
            PlayerLives--;
            gameObject.transform.position = SpawnPos;
            gameObject.transform.rotation = SpawnRot;
            rb.velocity = spawnvel;
			StartCoroutine(Immunity());
            mysource.PlayOneShot(PlayerDeath, 1.0f);
            
        }
    }
    public IEnumerator Immunity()
    {
		gameObject.GetComponent<BoxCollider>().enabled = false;
		yield return new WaitForSeconds(RespawnImmunity);
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

}
