using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed_Power_up : MonoBehaviour
{
    public float NewMaxSpeed = 50;
    public float NewMoveSpeed = 20;
    public float PowerUpDuration = 3;

    //original speeds;
    [HideInInspector]public float OriginalMoveSpeed;
    [HideInInspector]public float OriginalMaxSpeed;
    Collider player;
    AudioSource mysource;
    public AudioClip SpeedBoost;

    private void Start()
    {
        mysource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            OriginalMaxSpeed = other.gameObject.GetComponent<Pirate_Controller>().MaxSpeed;
            OriginalMoveSpeed = other.gameObject.GetComponent<Pirate_Controller>().MoveSpeed;

            other.gameObject.GetComponent<Pirate_Controller>().MaxSpeed = NewMaxSpeed;
            other.gameObject.GetComponent<Pirate_Controller>().MoveSpeed = NewMoveSpeed;
            player = other;
            StartCoroutine(PowerUpWait());
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            mysource.PlayOneShot(SpeedBoost, 0.5f);


        }
        
    }
    IEnumerator PowerUpWait()
    {
        yield return new WaitForSeconds(PowerUpDuration);
        player.gameObject.GetComponent<Pirate_Controller>().MaxSpeed = OriginalMaxSpeed;
        player.gameObject.GetComponent<Pirate_Controller>().MoveSpeed = OriginalMoveSpeed;
        Destroy(gameObject);

    }

}
