using UnityEngine;
using System.Collections;

public class scenechange : MonoBehaviour {
	
	public AudioSource music;
	
	void OnMouseDown()
	{
		this.GetComponent<SpriteRenderer> ().enabled = false;
		this.GetComponent<BoxCollider2D> ().isTrigger = true;
		music.Stop ();
		Application.LoadLevel(1);
	}
}
