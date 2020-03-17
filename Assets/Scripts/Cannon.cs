using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject bulletSpawnPoint;
    private Rigidbody rigb;
    public GameObject bullet;

	// Start is called before the first frame update
	void Start()
    {
        rigb = gameObject.GetComponent<Rigidbody>();
    }
    public void ShootBullet(float bulletSpeed)
    {
        //canShootAgain = false;
        GameObject shotBullet = (GameObject)Instantiate(bullet, bulletSpawnPoint.transform.position, transform.rotation);
        shotBullet.GetComponent<Rigidbody>().velocity = rigb.velocity + (bulletSpawnPoint.transform.forward * bulletSpeed);
        // yield return new WaitForSeconds(shootingDelay);
        //canShootAgain = true;
    }
}
