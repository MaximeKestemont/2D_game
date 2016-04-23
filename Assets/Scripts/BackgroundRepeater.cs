using UnityEngine;
using System.Collections;

public class BackgroundRepeater : MonoBehaviour {

	private Transform cameraTransform;
	private float spriteWidth;

	// Use this for initialization
	void Start () {
		// Store camera and sprite width
		cameraTransform = Camera.main.transform;
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();// as SpriteRenderer;
		spriteWidth = spriteRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
		// If the background is more than 1 spriteWidth away of the camera
		if ( (transform.position.x + spriteWidth) < cameraTransform.position.x) {
			Vector3 newPos = transform.position;
		  	newPos.x += 2.0f * spriteWidth; 
		  	transform.position = newPos;
		}
	}
}
