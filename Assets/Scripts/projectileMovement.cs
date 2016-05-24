using UnityEngine;
using System.Collections;

public class projectileMovement : MonoBehaviour {
	
	public float force;
	public bool hasTriggered = false;
	public Vector3 vForce;

	//used to prevent multiple collision triggers
	public void trigger(){
		hasTriggered = true;
	}

	public void setvForce(Vector3 x){
		vForce = x;
	}
	
	public void setForce (float x){
		force = x;
	}

	//Sets the projectile in motion - called after values are set with other methods
	public void move(){
		GetComponent<Rigidbody2D> ().velocity = vForce * force;
	}
	
	void OnBecameInvisible()
	{
		Destroy (gameObject);
	}
}
