using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public Camera Camera;

	public GameObject Player1;
	public GameObject Player2;
	private Pirate_Controller Player1Controller;
	private Pirate_Controller Player2Controller;

	public Collider Player1SafeZone;
	public Collider Player2SafeZone;

	public GameObject AsteroidPrefab;

	public int StartingAsteroidAmount = 3;
	public int MaxAsteroidAmount = 5;
	public float AsteroidSpawnTime = 5.0f;
	private float AsteroidSpawnTimer;

	public RectTransform WinScreen;
	public Image Player1LivesDisplay;
	public Image Player2LivesDisplay;

	private bool GameOver = false;

	private Vector3[] CameraFrustumCorners; //0 = bottom left, 1 = top left, 2 = top right, 3 = bottom right
	private float SceneWidth;
	private float SceneHeight;

	void Start()
    {
		AsteroidSpawnTimer = AsteroidSpawnTime;

		Player1Controller = Player1.GetComponent<Pirate_Controller>();
		Player2Controller = Player2.GetComponent<Pirate_Controller>();

		CameraFrustumCorners = new Vector3[4];
		Camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), 24, Camera.MonoOrStereoscopicEye.Mono, CameraFrustumCorners);
		for (int i = 0; i < 4; ++i)
			CameraFrustumCorners[i] *= 0.91f;	//Offset the corners inwards a little
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
		if (!GameOver)
		{
			if (AsteroidSpawnTimer > 0.0f)
				AsteroidSpawnTimer -= Time.deltaTime;
			else
			{
				GameObject[] Asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
				int count = 0;
				foreach (GameObject asteroid in Asteroids)
				{
					if (asteroid.GetComponent<Asteroid>().Type == Asteroid.AsteroidType.Large)
						++count;
				}
				if (count < MaxAsteroidAmount)
					SpawnAsteroid();

				AsteroidSpawnTimer = AsteroidSpawnTime; //Reset the timer each time, even if no asteroid was spawned
			}

			Player1LivesDisplay.fillAmount = Player1Controller.PlayerLives / 3.0f;
			Player2LivesDisplay.fillAmount = Player2Controller.PlayerLives / 3.0f;

			if (Player1Controller.PlayerLives <= 0)
				PlayerWon(2);
			else if (Player2Controller.PlayerLives <= 0)
				PlayerWon(1);
		}
		else
		{
			if (GetButtonDownAny(XboxButton.A))
			{
				Time.timeScale = 1;
				SceneManager.LoadScene(1);
			}
			else if (GetButtonDownAny(XboxButton.B))
			{
				Time.timeScale = 1;
				SceneManager.LoadScene(0);
			}
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
	}

	private void PlayerWon(int Player)
	{
		Time.timeScale = 0;
		GameOver = true;
		WinScreen.gameObject.SetActive(true);

		if (Player == 1)
		{
			WinScreen.GetChild(1).gameObject.SetActive(true);
		}
		else if (Player == 2)
		{
			WinScreen.GetChild(0).gameObject.SetActive(true);
		}
	}

	//Checks if any controller has the specified button pressed
	public bool GetButtonDownAny(XboxButton button)
	{
		if (XCI.GetButtonDown(button, XboxController.First)
			|| XCI.GetButtonDown(button, XboxController.Second)
			|| XCI.GetButtonDown(button, XboxController.Third)
			|| XCI.GetButtonDown(button, XboxController.Fourth))
			return true;
		return false;
	}
}
