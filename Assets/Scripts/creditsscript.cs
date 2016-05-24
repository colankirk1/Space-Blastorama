using UnityEngine;
using System.Collections;

public class creditsscript : MonoBehaviour {

	GameObject playbutton;

	void OnMouseDown()
	{
		playbutton = GameObject.Find ("Button - Play (contains music)");
		playbutton.GetComponent<SpriteRenderer> ().enabled = false;
		playbutton.GetComponent<BoxCollider2D> ().isTrigger = true;
		Application.LoadLevel(2);
	}
}
