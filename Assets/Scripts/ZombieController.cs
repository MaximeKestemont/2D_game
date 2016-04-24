using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieController : MonoBehaviour {


	public float moveSpeed;
	public float turnSpeed;

	public AudioClip enemyContactSound;
	public AudioClip catContactSound;

	private Vector3 moveDirection;

	[SerializeField]
	private PolygonCollider2D[] colliders;
	private int currentColliderIndex = 0;

	private List<Transform> congaLine = new List<Transform>();

	private bool isInvincible = false;
	private float timeSpentInvincible;

	private int lives = 3;

	// Use this for initialization
	void Start () {
		moveDirection = Vector3.right;
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 currentPosition = transform.position;
		
		if( Input.GetButton("Fire1") ) {
		  
		  Vector3 moveToward = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		  
		  moveDirection = moveToward - currentPosition;
		  moveDirection.z = 0; 
		  moveDirection.Normalize();
		}

		Vector3 target = moveDirection * moveSpeed + currentPosition;
		transform.position = Vector3.Lerp( currentPosition, target, Time.deltaTime );

		// Angle between x-axis and move direction
		float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

		// Turn toward the target
		transform.rotation = Quaternion.Slerp( 
			transform.rotation, 
            Quaternion.Euler( 0, 0, targetAngle ), 
            turnSpeed * Time.deltaTime 
            );

		EnforceBounds();

		if (isInvincible) {
			timeSpentInvincible += Time.deltaTime;
		 
		  	if (timeSpentInvincible < 3f) {
		    	float remainder = timeSpentInvincible % .3f;
		    	GetComponent<Renderer>().enabled = remainder > .15f; 			// make the zombie blink every 0.15s
		  	} else {
		    	GetComponent<Renderer>().enabled = true;
		    	isInvincible = false;
		  	}
		}
	}


	public void SetColliderForSprite( int spriteNum ) {
		colliders[currentColliderIndex].enabled = false;
	  	currentColliderIndex = spriteNum;
	  	colliders[currentColliderIndex].enabled = true;
	}


	private void EnforceBounds() {
	  	Vector3 newPosition = transform.position; 
	  	Camera mainCamera = Camera.main;
	  	Vector3 cameraPosition = mainCamera.transform.position;
	 	
	  	float xDist = mainCamera.aspect * mainCamera.orthographicSize; 
	  	float xMax = cameraPosition.x + xDist;
	  	float xMin = cameraPosition.x - xDist;
	 	
	  	// If the new position exceeds the horizontal bounds, inverse the direction
	  	if ( newPosition.x < xMin || newPosition.x > xMax ) {
	  	  	newPosition.x = Mathf.Clamp( newPosition.x, xMin, xMax );
	  	  	moveDirection.x = -moveDirection.x;
	  	}

		// Vertical bounds	  	
	  	float yMax = mainCamera.orthographicSize;
 
	 	if (newPosition.y < -yMax || newPosition.y > yMax) {
  			newPosition.y = Mathf.Clamp( newPosition.y, -yMax, yMax );
  			moveDirection.y = -moveDirection.y;
		}
	 
	  	// Update the position
	  	transform.position = newPosition;
	}



	void OnTriggerEnter2D( Collider2D other ) {
		Debug.Log ("Hit " + other.gameObject);
	  	if ( other.CompareTag( "cat" ) ) {
	  		GetComponent<AudioSource>().PlayOneShot(catContactSound);
	  		Transform followTarget = congaLine.Count == 0 ? transform : congaLine[congaLine.Count-1];
			other.transform.parent.GetComponent<CatController>().JoinConga( followTarget, moveSpeed, turnSpeed );
	  		congaLine.Add( other.transform );
	  		
	  		// win condition 
	  		if (congaLine.Count >= 5) {
  				Debug.Log("You won!");
  				Application.LoadLevel("WinScene");
			}
		
		} else if( !isInvincible && other.CompareTag( "enemy" ) ) {
			GetComponent<AudioSource>().PlayOneShot(enemyContactSound);

			// lose condition
			if (--lives <= 0) {
  				Debug.Log("You lost!");
  				Application.LoadLevel("LoseScene");
			}

  			isInvincible = true;
  			timeSpentInvincible = 0;
			
			// Remove the last 2 cats in the line
			for( int i = 0; i < 2 && congaLine.Count > 0; i++ ) {
			  	int lastIdx = congaLine.Count-1;
			  	Transform cat = congaLine[ lastIdx ];
			  	congaLine.RemoveAt(lastIdx);
			  	cat.parent.GetComponent<CatController>().ExitConga();
			}
		}
	}


}
