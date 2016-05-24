using UnityEngine;
using System.Collections;

public class BGscroller : MonoBehaviour {

	public float speed;
	Vector3 position;
	private Transform cameraTransform;
	private float spriteWidth;
	
	void Start () {
		cameraTransform = Camera.main.transform;
		SpriteRenderer spriteRenderer =  gameObject.GetComponent<Renderer>() as SpriteRenderer;
		spriteWidth = spriteRenderer.sprite.bounds.size.y;
	}

	void Update () {
		position = this.transform.position;
		position.y -= speed;
		this.transform.position = position;
		if( (transform.position.y + spriteWidth) < cameraTransform.position.y) {
			Vector3 newPos = transform.position;
			newPos.y += 2.0f * spriteWidth; 
			transform.position = newPos;
		}
	}
}
