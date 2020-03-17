using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BIG_BULLET : MonoBehaviour
{
    public GameObject NewBullet;
    private GameObject OriginalBullet;
    public float PowerUpDuration = 3.0f;
    Collider player;
    AudioSource mysource;
    AudioClip bigBullet;
    private void Start()
    {
        mysource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            OriginalBullet = other.gameObject.GetComponent<Pirate_Controller>().cannonRight.GetComponent<Cannon>().bullet;
            other.gameObject.GetComponent<Pirate_Controller>().cannonRight.GetComponent<Cannon>().bullet = NewBullet;
            other.gameObject.GetComponent<Pirate_Controller>().cannonLeft.GetComponent<Cannon>().bullet = NewBullet;

            StartCoroutine(PowerUpWaitBIG());
            player = other;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;

            mysource.PlayOneShot(bigBullet, 0.5f);
        }
    }
    public IEnumerator PowerUpWaitBIG()
    {
        yield return new WaitForSeconds(PowerUpDuration);
       // player.gameObject.GetComponent<Pirate_Controller>().GetComponentInChildren<Cannon>().bullet = OriginalBullet;
        player.gameObject.GetComponent<Pirate_Controller>().cannonRight.GetComponent<Cannon>().bullet = OriginalBullet;
        player.gameObject.GetComponent<Pirate_Controller>().cannonLeft.GetComponent<Cannon>().bullet = OriginalBullet;
        Destroy(gameObject);
    }
}
