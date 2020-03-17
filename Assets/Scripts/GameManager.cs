using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public Camera Camera;

	public GameObject Player1;
	public GameObject Player2;

	public Collider Player1SafeZone;
	public Collider Player2SafeZone;

	public GameObject AsteroidPrefab;

	public int StartingAsteroidAmount = 3;
	public int MaxAsteroidAmount = 5;
	public float AsteroidSpawnTime = 5.0f;
	private float AsteroidSpawnTimer;

	private Vector3[] CameraFrustumCorners; //0 = bottom left, 1 = top left, 2 = top right, 3 = bottom right
	private float SceneWidth;
	private float SceneHeight;

	void Start()
    {
		AsteroidSpawnTimer = AsteroidSpawnTime;

		CameraFrustumCorners = new Vector3[4];
		Camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), 11, Camera.MonoOrStereoscopicEye.Mono, CameraFrustumCorners);
		for (int i = 0; i < 4; ++i)
			CameraFrustumCorners[i] *= 0.95f;	//Offset the corners inwards a little
		SceneWidth = (CameraFrustumCorners[2].x);
		SceneHeight = (CameraFrustumCorners[2].y);

		//Spawn a few asteroids around on the screen
		for (int i = 0; i < StartingAsteroidAmount; ++i)
		{
			Vector3 Spawnpoint = new Vector3(Random.Range(-SceneWidth, SceneWidth), 0.0f, Random.Range(-SceneHeight, SceneHeight));
			bool Spawning = true;
			while (Spawning)
			{
				if (Player1SafeZone.bounds.Contains(Spawnpoint) || Player2SafeZone.bounds.Contains(Spawnpoint))    //If too close to a player,
					Spawnpoint = new Vector3(Random.Range(-SceneWidth, SceneWidth), 0.0f, Random.Range(-SceneHeight, SceneHeight)); //Randomise again
				else
					Spawning = false;
			}
			SpawnAsteroid(Spawnpoint);
		}
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

		if (AsteroidSpawnTimer > 0.0f)
			AsteroidSpawnTimer -= Time.deltaTime;
		else
		{
			GameObject[] Asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
			Debug.Log(Asteroids.Length);
			if (Asteroids.Length < MaxAsteroidAmount)
				SpawnAsteroid();

			AsteroidSpawnTimer = AsteroidSpawnTime;	//Reset the timer each time, even if no asteroid was spawned
		}
	}

	private void SpawnAsteroid()
	{
		Vector3 Spawnpoint;
		if (Random.value > 0.5f)
			Spawnpoint = new Vector3(SceneWidth, 0.0f, Random.Range(-SceneHeight, SceneHeight));
		else
			Spawnpoint = new Vector3(-SceneWidth, 0.0f, Random.Range(-SceneHeight, SceneHeight));
		bool Spawning = true;
		while (Spawning)
		{
			if (Player1SafeZone.bounds.Contains(Spawnpoint) || Player2SafeZone.bounds.Contains(Spawnpoint))    //If too close to a player,
			{
				Spawnpoint = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
				Spawnpoint.x *= SceneWidth;
				Spawnpoint.z *= SceneHeight;
			}
			else
				Spawning = false;
		}
		SpawnAsteroid(Spawnpoint);
	}

	private void SpawnAsteroid(Vector3 StartPosition)
	{
		Asteroid NewAsteroid = Instantiate(AsteroidPrefab).GetComponent<Asteroid>();
		NewAsteroid.Type = Asteroid.AsteroidType.Large;
		NewAsteroid.transform.position = StartPosition;
		Vector3 Direction = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
		NewAsteroid.transform.rotation = Quaternion.LookRotation(Direction.normalized);
		NewAsteroid.rb.velocity = NewAsteroid.MaxSpeed * NewAsteroid.transform.forward.normalized;
		NewAsteroid.SetWrapHandler(Wrap);
	}

	public void Wrap(GameObject Object)
	{
		//Check that the object is still active
		if (Object.activeInHierarchy)
		{
			Vector3 Position = Object.transform.position;

			if (Position.x < -(SceneWidth))  //Off the left side
			{
				Object.transform.position += new Vector3(SceneWidth * 2.0f, 0.0f, 0.0f);
			}
			else if (Position.x > (SceneWidth))	//Off the right side
			{
				Object.transform.position += new Vector3(-SceneWidth * 2.0f, 0.0f, 0.0f);
			}

			if (Position.z < -(SceneHeight))  //Off the bottom
			{
				Object.transform.position += new Vector3(0.0f, 0.0f, SceneHeight * 2.0f);
			}
			else if (Position.z > (SceneHeight))	//Off the top
			{
				Object.transform.position += new Vector3(0.0f, 0.0f, -SceneHeight * 2.0f);
			}
		}
	}
}
