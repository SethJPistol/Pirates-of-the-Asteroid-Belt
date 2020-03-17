using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
	public Camera Camera;
	private Vector3[] CameraFrustumCorners; //0 = bottom left, 1 = top left, 2 = top right, 3 = bottom right
	private float SceneWidth;
	private float SceneHeight;

	public static ScreenWrap Instance;

	private void Awake()
	{
		Instance = this;

		CameraFrustumCorners = new Vector3[4];
		Camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), 24, Camera.MonoOrStereoscopicEye.Mono, CameraFrustumCorners);
		for (int i = 0; i < 4; ++i)
			CameraFrustumCorners[i] *= 0.91f;   //Offset the corners inwards a little
		SceneWidth = (CameraFrustumCorners[2].x);
		SceneHeight = (CameraFrustumCorners[2].y);
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
			else if (Position.x > (SceneWidth)) //Off the right side
			{
				Object.transform.position += new Vector3(-SceneWidth * 2.0f, 0.0f, 0.0f);
			}

			if (Position.z < -(SceneHeight))  //Off the bottom
			{
				Object.transform.position += new Vector3(0.0f, 0.0f, SceneHeight * 2.0f);
			}
			else if (Position.z > (SceneHeight))    //Off the top
			{
				Object.transform.position += new Vector3(0.0f, 0.0f, -SceneHeight * 2.0f);
			}
		}
	}
}
