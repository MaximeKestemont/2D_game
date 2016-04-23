using UnityEngine;
using System.Collections;

public class CatController : MonoBehaviour {

	private Transform followTarget;
	private float moveSpeed; 
	private float turnSpeed; 
	private bool isZombie;


	void Update () {
	  
	  if(isZombie) {
	    Vector3 currentPosition = transform.position;            
	    Vector3 moveDirection = followTarget.position - currentPosition;
	 
	    float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
	    transform.rotation = Quaternion.Slerp( transform.rotation, 
	                                           Quaternion.Euler(0, 0, targetAngle), 
	                                           turnSpeed * Time.deltaTime );
	 
	    float distanceToTarget = moveDirection.magnitude;
	    if (distanceToTarget > 0) {
	      if ( distanceToTarget > moveSpeed )
	        distanceToTarget = moveSpeed;
	 
	      moveDirection.Normalize();
	      Vector3 target = moveDirection * distanceToTarget + currentPosition;
	      transform.position = Vector3.Lerp(currentPosition, target, moveSpeed * Time.deltaTime);
	    }
	  }
	}

	void mkKillCat(){
		DestroyObject( gameObject );
	}

	void OnBecameInvisible() {
		if ( !isZombie ) Destroy( gameObject ); 
	}

	public void JoinConga( Transform followTarget, float moveSpeed, float turnSpeed ) {

		// Update the cat position so that it matches the zombie one
  		this.followTarget = followTarget;
  		this.moveSpeed = moveSpeed;
  		this.turnSpeed = turnSpeed;
 
  		isZombie = true;

		// Disable the collider so that it is only hit once
  		GetComponent<Collider2D>().enabled = false;

  		// Trigger the animation
  		GetComponent<Animator>().SetBool( "InConga", true );
	}
}
