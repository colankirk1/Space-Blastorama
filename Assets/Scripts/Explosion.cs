using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	public int count;
	
	void Start () {
		count = 0;
	}

	void Update () {
		count++;
		if (count > 10)
			Destroy (gameObject);
	}
}
