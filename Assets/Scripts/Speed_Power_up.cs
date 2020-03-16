using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed_Power_up : MonoBehaviour
{
    public float NewMaxSpeed = 50;
    public float NewMoveSpeed = 20;
    public float PowerUpDuration = 3;

    //original speeds;
    private float OriginalMoveSpeed;
    private float OriginalMaxSpeed;
   
    private void OnCollisionEnter(Collision collision)
    {
        OriginalMaxSpeed =  collision.gameObject.GetComponent<Pirate_Controller>().MaxSpeed;
        OriginalMoveSpeed = collision.gameObject.GetComponent<Pirate_Controller>().MoveSpeed;

        collision.gameObject.GetComponent<Pirate_Controller>().MaxSpeed = NewMaxSpeed;
        collision.gameObject.GetComponent<Pirate_Controller>().MoveSpeed = NewMoveSpeed;

        StartCoroutine(PowerUpWait());

        collision.gameObject.GetComponent<Pirate_Controller>().MaxSpeed = OriginalMaxSpeed;
        collision.gameObject.GetComponent<Pirate_Controller>().MoveSpeed = OriginalMoveSpeed;

        Destroy(gameObject);
    }

    public IEnumerator PowerUpWait()
    {
        yield return new WaitForSeconds(PowerUpDuration);
    }
}
