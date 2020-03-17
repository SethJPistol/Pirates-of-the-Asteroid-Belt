using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting_Power_Up : MonoBehaviour
{
    public float NewShootingDelay = 0.5f;
    private float OriginalShootingDelay;
    public float PowerUpDuration = 3.0f;
    Collider player;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            OriginalShootingDelay = other.gameObject.GetComponent<Pirate_Controller>().shootingDelay;
            
            other.gameObject.GetComponent<Pirate_Controller>().MaxSpeed = NewShootingDelay;
           
            player = other;
            StartCoroutine(PowerUpWaitShoot());
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;


            
        }

    }
    IEnumerator PowerUpWaitShoot()
    {
        yield return new WaitForSeconds(PowerUpDuration);
        player.gameObject.GetComponent<Pirate_Controller>().shootingDelay = OriginalShootingDelay;
        Destroy(gameObject);
    }
}
