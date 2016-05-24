using UnityEngine;
using System.Collections;

public class starMover : MonoBehaviour {

	public float speed;
	Vector3 position;
	private float rand;			//random number variable

	void Update () {
		transform.Rotate(0, 0, 1);
		position = this.transform.position;
		position.x -= speed;
		if( position.x < -4.6f) {	//just off screen 
			position.x = 4.6f;
			rand = Random.Range (0, 90);
			position.y = (rand/10)-4.5f;
		}
		this.transform.position = position;
	}
}
