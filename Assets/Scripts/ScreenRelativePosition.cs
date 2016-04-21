using UnityEngine;
using System.Collections;

public class ScreenRelativePosition : MonoBehaviour {

	public enum ScreenEdge {LEFT, RIGHT, TOP, BOTTOM};
	public ScreenEdge screenEdge;
	public float yOffset;
	public float xOffset;

	// Use this for initialization
	void Start () {
		// Only work with one camera
		Vector3 newPosition = transform.position;
		Camera camera = Camera.main;
		 
		// Assume de center of the screen is at (0, 0)
		switch(screenEdge)
		{
		  case ScreenEdge.RIGHT:
		  	// orthographicSize = half the view's height
		    newPosition.x = camera.aspect * camera.orthographicSize + xOffset;
		    newPosition.y = yOffset;
		    break;
		 
		  case ScreenEdge.TOP:
		    newPosition.y = camera.orthographicSize + yOffset;
		    newPosition.x = xOffset;
		    break;

		  case ScreenEdge.LEFT:
		    newPosition.x = -camera.aspect * camera.orthographicSize + xOffset;
		    newPosition.y = yOffset;
		    break;
		    
		  case ScreenEdge.BOTTOM:
		    newPosition.y = -camera.orthographicSize + yOffset;
		    newPosition.x = xOffset;
		    break;
		}
		// 5
		transform.position = newPosition;	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
