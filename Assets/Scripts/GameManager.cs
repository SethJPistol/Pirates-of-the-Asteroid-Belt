using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public Camera Camera;

	public GameObject AsteroidPrefab;

    void Start()
    {
		Asteroid StartingAsteroid = Instantiate(AsteroidPrefab).GetComponent<Asteroid>();
		StartingAsteroid.rb.velocity = (new Vector3(0.5f, 0, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
		Vector3[] FrustumCorners = new Vector3[4];
		Camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), 11, Camera.MonoOrStereoscopicEye.Mono, FrustumCorners);
		for (int i = 0; i < 4; i++)
		{
			var worldSpaceCorner = Camera.transform.TransformVector(FrustumCorners[i]);
			Debug.DrawRay(Camera.transform.position, worldSpaceCorner, Color.blue);
		}
	}
}
