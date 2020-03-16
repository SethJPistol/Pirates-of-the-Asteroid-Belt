using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public Camera Camera;

	public GameObject AsteroidPrefab;

	private Vector3[] CameraFrustumCorners; //0 = bottom left, 1 = top left, 2 = top right, 3 = bottom right
	private float SceneWidth;
	private float SceneHeight;

	void Start()
    {
		Asteroid StartingAsteroid = Instantiate(AsteroidPrefab).GetComponent<Asteroid>();
		StartingAsteroid.rb.velocity = (new Vector3(1.5f, 0, 1.5f));
		StartingAsteroid.SetWrapHandler(Wrap);

		CameraFrustumCorners = new Vector3[4];
		Camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), 11, Camera.MonoOrStereoscopicEye.Mono, CameraFrustumCorners);
		for (int i = 0; i < 4; ++i)
			CameraFrustumCorners[i] *= 0.95f;	//Offset the corners inwards a little
		SceneWidth = (CameraFrustumCorners[2].x * 2.0f);
		SceneHeight = (CameraFrustumCorners[2].y * 2.0f);

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

	public void Wrap(GameObject Object)
	{
		//Check that the object is still active
		if (Object.activeInHierarchy)
		{
			Vector3 Position = Object.transform.position;

			if (Position.x < -(SceneWidth / 2.0f))  //Off the left side
			{
				Object.transform.position += new Vector3((SceneWidth), 0.0f, 0.0f);
			}
			else if (Position.x > (SceneWidth / 2.0f))	//Off the right side
			{
				Object.transform.position += new Vector3(-(SceneWidth), 0.0f, 0.0f);
			}

			if (Position.z < -(SceneHeight / 2.0f))  //Off the bottom
			{
				Object.transform.position += new Vector3(0.0f, 0.0f, (SceneHeight));
			}
			else if (Position.z > (SceneHeight / 2.0f))	//Off the top
			{
				Object.transform.position += new Vector3(0.0f, 0.0f, -(SceneHeight));
			}
		}
	}
}
