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

            other.gameObject.GetComponent<Pirate_Controller>().jetoriginal1.GetComponent<ParticleSystem>().Stop();
            other.gameObject.GetComponent<Pirate_Controller>().jetoriginal2.GetComponent<ParticleSystem>().Stop();
            other.gameObject.GetComponent<Pirate_Controller>().newjetparent.SetActive(true);
            other.gameObject.GetComponent<Pirate_Controller>().jetnew1.GetComponent<ParticleSystem>().Play();
            other.gameObject.GetComponent<Pirate_Controller>().jetnew2.GetComponent<ParticleSystem>().Play();
            StartCoroutine(PowerUpWait());
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            mysource.PlayOneShot(SpeedBoost, 0.5f);


        }
        
    }
    IEnumerator PowerUpWait()
    {
        yield return new WaitForSeconds(PowerUpDuration);
        player.gameObject.GetComponent<Pirate_Controller>().MaxSpeed = OriginalMaxSpeed;
        player.gameObject.GetComponent<Pirate_Controller>().MoveSpeed = OriginalMoveSpeed;
        player.gameObject.GetComponent<Pirate_Controller>().jetoriginal1.GetComponent<ParticleSystem>().Play();
        player.gameObject.GetComponent<Pirate_Controller>().jetoriginal2.GetComponent<ParticleSystem>().Play();
        player.gameObject.GetComponent<Pirate_Controller>().jetnew1.GetComponent<ParticleSystem>().Stop();
        player.gameObject.GetComponent<Pirate_Controller>().jetnew2.GetComponent<ParticleSystem>().Stop();
        player.gameObject.GetComponent<Pirate_Controller>().newjetparent.SetActive(false);
        
        Destroy(gameObject);

    }

}
