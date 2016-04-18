using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour {


	public float moveSpeed;

	private Vector3 moveDirection;

	// Use this for initialization
	void Start () {
	
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
	}
}
