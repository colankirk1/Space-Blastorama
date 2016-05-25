using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Boss1Behavior : MonoBehaviour {
	
	private Rigidbody2D bossRigidbody;
	public GameObject prefab;
	public GameObject prefab2;
	public GameObject prefab3;
	private GameObject tempPlayer;

	private bool isInPattern1;	
	private bool isInPattern2;
	private bool isInPattern3;
	private bool isInPattern4;
	private int patternMode; 	//used by each wave for calculations
	private int rand;			//random number variable
	private float count;		//used by each wave for additional calculations
	public Image healthbar;	
	public Image playerHealthbar;
	public Text playerHealthtext;

	private bool canTakeDamage;	
	public AudioClip shock;
	public AudioClip death;
	public GameObject white;
	Animator projectileAnim;
	
	void Start () {
		bossRigidbody = GetComponent<Rigidbody2D>();
		isInPattern1 = false;//true;
		isInPattern2 = true;//false;
		isInPattern3 = false;
		isInPattern4 = false;
		patternMode = 0;
		rand = 0;
		count = 0;
		healthbar.fillAmount = 1;
		canTakeDamage = true;
		tempPlayer = GameObject.Find("Player");

		InvokeRepeating ("pattern2", .5f, .1f);
		//InvokeRepeating("pattern1", .5f, .1f);
	}

	void OnTriggerEnter2D( Collider2D other )
	{
		if (other.CompareTag ("PlayerProjectile") && (other.gameObject.GetComponent<projectileMovement>().hasTriggered == false)) {
			other.gameObject.GetComponent<projectileMovement>().trigger();
			Destroy(other.gameObject);
			//Varying damage amounts
			if(canTakeDamage){
				if(isInPattern2)
					healthbar.fillAmount -= .015f;
				else if(isInPattern3)
					healthbar.fillAmount -= .005f;
				else if(isInPattern4)
					healthbar.fillAmount -= .008f;
				else
					healthbar.fillAmount -= .01f;
				if(healthbar.fillAmount <= 0){
					endWave();
				}
			}
		}
	}

	//Circle bursts of slightly increased angles
	//0.1 seconds between invokes
	private void pattern1(){
		if (patternMode < 5) {
			if (patternMode == 0)
				rand = Random.Range (0, 14);
			rand += 2;
			if(rand >= 15)
				rand -=15;
			for (int x = rand; x<360; x+=15) {
				GameObject newObject = Instantiate (prefab, bossRigidbody.position, Quaternion.identity) as GameObject;
				projectileMovement projectile = newObject.GetComponent<projectileMovement> ();
				projectile.setForce (4);
				projectile.setvForce (Quaternion.AngleAxis (x, Vector3.forward) * Vector3.up);
				projectile.move ();
				if (tempPlayer.GetComponent<PlayerMovement> ().stopEnemyBullets == true) {
					projectileAnim = newObject.GetComponent<Animator>();
					projectileAnim.SetBool("fade", true);
					StartCoroutine(bulletFade(newObject));
				}
			}
		}
		patternMode++;
		if (patternMode == 15)
			patternMode = 0;
	}

	//Zigzag waves of circle bursts
	//0.1 seconds between invokes
	private void pattern2(){
		if (count < 40) {
			if (patternMode == 0)
				rand += 3;
			else {
				rand -= 3;
			}
			if (rand >= 24) {
				patternMode = 1;
			} else if (rand <= 0) {
				patternMode = 0;
			}
			if (rand >= 15) {
				GameObject newObject = Instantiate (prefab, bossRigidbody.position, Quaternion.identity) as GameObject;
				projectileMovement projectile = newObject.GetComponent<projectileMovement> ();
				projectile.setForce (4);
				projectile.setvForce (Quaternion.AngleAxis (rand - 25, Vector3.forward) * Vector3.up);
				projectile.move ();
				if (tempPlayer.GetComponent<PlayerMovement> ().stopEnemyBullets == true) {
					projectileAnim = newObject.GetComponent<Animator>();
					projectileAnim.SetBool("fade", true);
					StartCoroutine(bulletFade(newObject));
				}

			}
			for (int x = rand; x<360; x+=15) {
				GameObject newObject = Instantiate (prefab, bossRigidbody.position, Quaternion.identity) as GameObject;
				projectileMovement projectile = newObject.GetComponent<projectileMovement> ();
				projectile.setForce (4);
				projectile.setvForce (Quaternion.AngleAxis (x - 10, Vector3.forward) * Vector3.up);
				projectile.move ();
				if (tempPlayer.GetComponent<PlayerMovement> ().stopEnemyBullets == true) {
					projectileAnim = newObject.GetComponent<Animator>();
					projectileAnim.SetBool("fade", true);
					StartCoroutine(bulletFade(newObject));
				}
			}
		}
		count++;
		if (count > 50)
			count = 0;
	}

	//Encloses player to center with waves. Circle burst after every cycle
	//Count must be initiazized to 4, .3 seconds between invokes
	private void pattern3(){
		Vector3 temp = bossRigidbody.position;
		temp.y += .8f;
		if (count == .5f) {
			count = 4;
			rand = Random.Range (0, 6);
			//circle burst
			for (int x = rand; x<360; x+=6) {
				GameObject newObject = Instantiate (prefab, bossRigidbody.position, Quaternion.identity) as GameObject;
				projectileMovement projectile = newObject.GetComponent<projectileMovement> ();
				projectile.setForce (3);
				projectile.setvForce (Quaternion.AngleAxis (x, Vector3.forward) * Vector3.up);
				projectile.move ();
				if (tempPlayer.GetComponent<PlayerMovement> ().stopEnemyBullets == true) {
					projectileAnim = newObject.GetComponent<Animator>();
					projectileAnim.SetBool("fade", true);
					StartCoroutine(bulletFade(newObject));
				}
			}
		}
		//waves - x = 6 are the 6 bullets in each wave that move at varying speeds
		for (int x = 6; x>0; x--) {
			temp.x +=count;
			GameObject newObject = Instantiate (prefab2, temp, Quaternion.identity) as GameObject;
			projectileMovement projectile = newObject.GetComponent<projectileMovement> ();
			projectile.setForce (x);
			projectile.setvForce (Quaternion.AngleAxis (180, Vector3.forward) * Vector3.up);
			projectile.move ();
			if (tempPlayer.GetComponent<PlayerMovement> ().stopEnemyBullets == true) {
				projectileAnim = newObject.GetComponent<Animator>();
				projectileAnim.SetBool("fade", true);
				StartCoroutine(bulletFade(newObject));
			}
			temp.x -=count*2;
			newObject = Instantiate (prefab2, temp, Quaternion.identity) as GameObject;
			projectile = newObject.GetComponent<projectileMovement> ();
			projectile.setForce (x);
			projectile.setvForce (Quaternion.AngleAxis (180, Vector3.forward) * Vector3.up);
			projectile.move ();
			if (tempPlayer.GetComponent<PlayerMovement> ().stopEnemyBullets == true) {
				projectileAnim = newObject.GetComponent<Animator>();
				projectileAnim.SetBool("fade", true);
				StartCoroutine(bulletFade(newObject));
			}
			temp.x+=count;
		}
		count -= .5f;
	}

	// rotating tri circle bursts of varying speed
	//.5 seconds between invokes
	private void pattern4(){
		Vector3 temp = bossRigidbody.position;
		rand = Random.Range (0, 44);
		for (int x = rand; x<360; x+=45) {
			if(count == 0){
				temp.y += .8f;
				p4spawn(x, temp);
				temp.y -= 1.6f;
				temp.x -= .8f;
				p4spawn(x, temp);
				temp.x += 1.6f;
				p4spawn(x, temp);
				temp.x-=.8f;
				temp.y+=.8f;
			}
			else{
				temp.x += .4f;
				p4spawn(x, temp);
				temp.x -= .8f;
				p4spawn(x, temp);
				temp.y -= .8f;
				temp.x += .4f;
				p4spawn(x, temp);
				temp.y+=.8f;
			}
		}
		if (count == 1) {
			count = 0;
			patternMode++;
			if (patternMode == 3)
				patternMode = 0;
		} else {
			count = 1;
		}
	}

	//helper function for pattern4
	private void p4spawn(int x, Vector3 temp){
		if (patternMode == 0) {
			GameObject newObject = Instantiate (prefab, temp, Quaternion.identity) as GameObject;
			projectileMovement projectile = newObject.GetComponent<projectileMovement> ();
			projectile.setForce (3);
			projectile.setvForce (Quaternion.AngleAxis (x, Vector3.forward) * Vector3.up);
			projectile.move ();
			if (tempPlayer.GetComponent<PlayerMovement> ().stopEnemyBullets == true) {
				projectileAnim = newObject.GetComponent<Animator>();
				projectileAnim.SetBool("fade", true);
				StartCoroutine(bulletFade(newObject));
			}
		} else if (patternMode == 1) {
			GameObject newObject = Instantiate (prefab2, temp, Quaternion.identity) as GameObject;
			projectileMovement projectile = newObject.GetComponent<projectileMovement> ();
			projectile.setForce (2.5f);
			projectile.setvForce (Quaternion.AngleAxis (x, Vector3.forward) * Vector3.up);
			projectile.move ();
			if (tempPlayer.GetComponent<PlayerMovement> ().stopEnemyBullets == true) {
				projectileAnim = newObject.GetComponent<Animator>();
				projectileAnim.SetBool("fade", true);
				StartCoroutine(bulletFade(newObject));
			}
		}
		else if (patternMode == 2) {
			GameObject newObject = Instantiate (prefab3, temp, Quaternion.identity) as GameObject;
			projectileMovement projectile = newObject.GetComponent<projectileMovement> ();
			projectile.setForce (2);
			projectile.setvForce (Quaternion.AngleAxis (x, Vector3.forward) * Vector3.up);
			projectile.move ();
			if (tempPlayer.GetComponent<PlayerMovement> ().stopEnemyBullets == true) {
				projectileAnim = newObject.GetComponent<Animator>();
				projectileAnim.SetBool("fade", true);
				StartCoroutine(bulletFade(newObject));
			}
		}
		patternMode++;
		if (patternMode == 3)
			patternMode = 0;
	}

	//Stops current pattern, resets stats to prepare for next wave
	private void endWave(){
		AudioSource.PlayClipAtPoint(shock, new Vector3(0,0,-9));
		canTakeDamage = false;
		CancelInvoke();
		destroyAllEnemyBullets();
		rand = 0;
		count = 0;
		patternMode = 0;
		StartCoroutine (bossDelay (2.0f));

	}

	//removes all enemy bullets from the screen
	void destroyAllEnemyBullets()
	{
		GameObject[] gameObjects;
		gameObjects = GameObject.FindGameObjectsWithTag ("EnemyProjectile");
		
		for(var i = 0 ; i < gameObjects.Length ; i ++)
		{
			projectileAnim = gameObjects[i].GetComponent<Animator>();
			projectileAnim.SetBool("fade", true);
			StartCoroutine(bulletFade(gameObjects[i]));
			//Destroy(gameObjects[i]);
		}
	}



	//Sets a cooldown period between boss attacks then invokes the next wave
	IEnumerator bossDelay (float x)
	{
		yield return new WaitForSeconds (x);
		if (! isInPattern2) {
			canTakeDamage = true;
			healthbar.fillAmount = 100;
			if (isInPattern1) {
				isInPattern1 = false;
				isInPattern3 = true;
				count = 4;
				InvokeRepeating ("pattern3", .5f, .3f);
			} else if (isInPattern3) {
				isInPattern3 = false;
				isInPattern4 = true;
				InvokeRepeating ("pattern4", .5f, .5f);
			} else if (isInPattern4) {
				isInPattern4 = false;
				isInPattern2 = true;
				InvokeRepeating ("pattern2", .5f, .1f);
			}
		} else {
			AudioSource.PlayClipAtPoint(death, new Vector3(0,0,-8));
			white.SetActive (true);
			//white.GetComponent<whitescript>().set();
			playerHealthbar.CrossFadeAlpha(0, 2, false);
			playerHealthtext.CrossFadeAlpha(0, 2, false);
			StartCoroutine (endDelay (3.0f));
		}
	}

	//Delay before changing to Credits scene
	IEnumerator endDelay (float x){
		yield return new WaitForSeconds (x);
		Application.LoadLevel(2);
	}

	//Delete bullets after their fade animation
	IEnumerator bulletFade(GameObject projectile) 
	{
		yield return new WaitForSeconds(.5f);
		Destroy (projectile);
	}
}
