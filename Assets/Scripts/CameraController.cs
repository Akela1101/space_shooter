using UnityEngine;

public class CameraController : MonoBehaviour
{
	void Start ()
	{
		Camera camera = Camera.main;

		// If the screen is taller than 6 / 9, 
		// the viewport should be shrunken to fit the width.

		float screenAspect = (float)Screen.width / Screen.height;
		float gameAspect = 6f / 9;
		if (screenAspect.CompareTo(gameAspect) < 0)
		{
			float d = (1 - screenAspect / gameAspect) / 2;
			camera.rect = new Rect(0, d, 1, 1 - d * 2);
		}
	}
}
