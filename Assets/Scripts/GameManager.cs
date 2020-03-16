using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public Camera Camera;

	public GameObject AsteroidPrefab;

	private Vector3[] CameraFrustumCorners;

	void Start()
    {
		Asteroid StartingAsteroid = Instantiate(AsteroidPrefab).GetComponent<Asteroid>();
		StartingAsteroid.rb.velocity = (new Vector3(0.5f, 0, 0.5f));
		StartingAsteroid.SetWrapHandler(Wrap);

		CameraFrustumCorners = new Vector3[4];
		Camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), 11, Camera.MonoOrStereoscopicEye.Mono, CameraFrustumCorners);

		//Find all asteroids currently in scene and set up their wrap
	}

    // Update is called once per frame
    void Update()
    {
		//DEBUG
		for (int i = 0; i < 4; i++)
		{
			var worldSpaceCorner = Camera.transform.TransformVector(CameraFrustumCorners[i]);
			Debug.DrawRay(Camera.transform.position, worldSpaceCorner, Color.blue);
		}


	}

	public void Wrap(Vector3 Position)
	{
		Debug.Log("Wrapped");
		//Check that the object is still active
	}
}
