using UnityEngine;
using System.Collections;

public class whitescript : MonoBehaviour {

	Animator anim;
	
	void Start () {
		anim = GetComponent<Animator>();
	}

	public void set(){
		anim.SetBool("hasWon", true);
	}
}
