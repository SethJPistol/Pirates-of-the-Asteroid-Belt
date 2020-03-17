using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_hit : MonoBehaviour
{
	//AudioSource mysource;
	//public AudioClip astExplode;
	
	private void OnBecameInvisible()    //When this object leaves the camera's view,
	{
		ScreenWrap.Instance.Wrap(gameObject);
	}

	private void OnCollisionEnter(Collision collision)
    {
        
		//if(collision.gameObject.tag == "Asteroid")
		//{
		//	GameObject shotBullet = (GameObject)Instantiate(bullet, bulletSpawnPoint.transform.position, transform.rotation);
		//	mysource.PlayOneShot(astExplode, 1.0f);
		//}
		Destroy(gameObject);
	}

}
