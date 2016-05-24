using UnityEngine;
using System.Collections;

//used on back button in Credits scene
public class backScript : MonoBehaviour {
	void OnMouseDown()
	{
		Application.LoadLevel(0);
	}
}
