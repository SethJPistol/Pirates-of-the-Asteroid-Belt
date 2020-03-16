using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting_Power_Up : MonoBehaviour
{
    public float NewShootingDelay = 0.5f;
    public float OriginalShootingDelay;
    public float PowerUpDuration = 3.0f;
    void OnCollisionEnter(Collision collision)
    {
        OriginalShootingDelay = collision.gameObject.GetComponent<Pirate_Controller>().shootingDelay;

        collision.gameObject.GetComponent<Pirate_Controller>().shootingDelay = NewShootingDelay;

        StartCoroutine(PowerUpWait());
        collision.gameObject.GetComponent<Pirate_Controller>().shootingDelay = OriginalShootingDelay;
        Destroy(gameObject);
    }
    public IEnumerator PowerUpWait()
    {
        yield return new WaitForSeconds(PowerUpDuration);
    }
}
