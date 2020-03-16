using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public enum AsteroidType
	{
		Large,
		Small
	}

	public GameObject SmallAsteroidPrefab;

	[HideInInspector]
	public Rigidbody rb;

	public AsteroidType Type;

	[HideInInspector]
	public delegate void WrapHandler(Vector3 Position);	//Delegate type to call when wrapping around the screen
	private WrapHandler Handler = null;					//A handler to hold the function

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	public void SetWrapHandler(WrapHandler WrapFunction)
	{
		Handler = WrapFunction;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Bullet"))
		{
			if (Type == AsteroidType.Large)
			{
				Split();
				Destroy(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}
		else if (collision.gameObject.CompareTag("Player"))
		{
			if (Type == AsteroidType.Large)
			{
				Split();
				Destroy(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}

	private void Split()
	{
		Asteroid Asteroid1 = Instantiate(SmallAsteroidPrefab).GetComponent<Asteroid>();
		Asteroid1.Type = AsteroidType.Small;
		Asteroid1.SetWrapHandler(Handler);
		Asteroid1.transform.position = rb.position;
		Asteroid1.transform.rotation = Quaternion.LookRotation(rb.velocity.normalized);
		Asteroid1.transform.Rotate(0.0f, 90.0f, 0.0f);
		Asteroid1.rb.velocity = (rb.velocity.magnitude * Asteroid1.transform.forward);

		Asteroid Asteroid2 = Instantiate(SmallAsteroidPrefab).GetComponent<Asteroid>();
		Asteroid2.Type = AsteroidType.Small;
		Asteroid2.SetWrapHandler(Handler);
		Asteroid2.transform.position = transform.position;
		Asteroid2.transform.rotation = Quaternion.LookRotation(rb.velocity.normalized);
		Asteroid2.transform.Rotate(0.0f, -90.0f, 0.0f);
		Asteroid2.rb.velocity = (rb.velocity.magnitude * Asteroid2.transform.forward);
	}

	private void OnBecameInvisible()
	{
		if (Handler != null)
			Handler(transform.position);
	}
}
