using UnityEngine;
using System.Collections;

public class MyUnitySingleton : MonoBehaviour {

	private static MyUnitySingleton instance = null;

	public static MyUnitySingleton Instance {
		get { return instance; }
	}

	void Awake() {
		if (instance != null) {
			instance.GetComponent<SpriteRenderer> ().enabled = true;
			instance.GetComponent<BoxCollider2D> ().isTrigger = false;
			if(instance.GetComponent<AudioSource>().isPlaying == false)
				instance.GetComponent<AudioSource>().Play();
		}
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
}
