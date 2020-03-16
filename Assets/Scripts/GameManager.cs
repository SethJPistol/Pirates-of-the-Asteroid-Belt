using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject AsteroidPrefab;

    void Start()
    {
		Asteroid StartingAsteroid = Instantiate(AsteroidPrefab).GetComponent<Asteroid>();
		StartingAsteroid.rb.velocity = (new Vector3(0.5f, 0, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
