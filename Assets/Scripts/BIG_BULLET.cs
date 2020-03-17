using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BIG_BULLET : MonoBehaviour
{
    public GameObject NewBullet;
    private GameObject OriginalBullet;
    public float PowerUpDuration = 3.0f;
    Collider player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            OriginalBullet = other.gameObject.GetComponent<Pirate_Controller>().GetComponentInChildren<Cannon>().bullet;
            other.gameObject.GetComponent<Pirate_Controller>().GetComponentInChildren<Cannon>().bullet = NewBullet;

            StartCoroutine(PowerUpWait());

            Destroy(gameObject);
        }
    }
    IEnumerator PowerUpWait()
    {
        yield return new WaitForSeconds(PowerUpDuration);
        player.gameObject.GetComponent<Pirate_Controller>().GetComponentInChildren<Cannon>().bullet = OriginalBullet;
    }
}
