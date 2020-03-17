using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_hit : MonoBehaviour
{
	private void OnBecameInvisible()    //When this object leaves the camera's view,
	{
		ScreenWrap.Instance.Wrap(gameObject);
	}

	private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
