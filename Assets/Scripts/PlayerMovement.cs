using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {
	private Rigidbody2D shipRigidbody;
	public GameObject prefab;
	private float speed;
	Vector3 move;
	Animator anim;
	Animator projectileAnim;
	private bool canFire;			//delay for player projectiles
	public bool stopEnemyBullets;	//enemy bullets can't fire for a certain period after death
	public bool noDamage;			//triggered when player dies
	public Image healthbar;
	public AudioClip deathclip;

	
	void Start () {
		canFire = true;
		noDamage = false;
		stopEnemyBullets = false;
		shipRigidbody = GetComponent<Rigidbody2D>();
		speed = .07f;
		anim = GetComponent<Animator>();
	}

	void Update () {
		//Movement
		Vector3 position = this.transform.position;
		if (Input.GetKey (KeyCode.UpArrow) && (position.y + speed)<4.9) {
			position.y+= speed;
		}
		if (Input.GetKey (KeyCode.DownArrow) && (position.y - speed)>-4.4) {
			position.y-= speed;
		}
		if (Input.GetKey (KeyCode.LeftArrow) && (position.x - speed)>-4.2) {
			position.x-= speed;
		}
		if (Input.GetKey (KeyCode.RightArrow) && (position.x + speed)<4.2) {
			position.x+= speed;
		}
		this.transform.position = position;

		//Focused Movement
		if (Input.GetKeyDown(KeyCode.LeftShift)) {
			anim.SetBool("IsFocused", true);
			speed = speed/2;
		}
		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			anim.SetBool("IsFocused", false);
			speed = speed*2;
		}

		//Firing
		if (Input.GetKey (KeyCode.Z) && canFire == true) {
			canFire = false;
			fire ();
		}
	}

	void OnTriggerEnter2D( Collider2D other )
	{
		if (other.CompareTag ("EnemyProjectile") && (other.gameObject.GetComponent<projectileMovement>().hasTriggered == false)) {
			if(!noDamage){
				other.gameObject.GetComponent<projectileMovement>().trigger();
				healthbar.fillAmount -= .03f;
				anim.SetBool("IsHit", true);
				AudioSource.PlayClipAtPoint(deathclip, new Vector3(0,0,-7));
				destroyAllEnemyBullets();
				noDamage = true;
				StartCoroutine (DeathDelay (2));
				if(healthbar.fillAmount <= 0){
					Application.LoadLevel(2);
				}
			}
		}
	}

	//removes all enemy bullets from the screen
	void destroyAllEnemyBullets()
	{
		stopEnemyBullets = true;
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

	//Instantiate object and set delay between fires
	void fire(){
		Vector3 temp = shipRigidbody.position;
		temp.y += .7f;
		GameObject newObject = Instantiate (prefab, temp, Quaternion.identity) as GameObject;
		projectileMovement projectile = newObject.GetComponent<projectileMovement>();
		projectile.setForce(10);
		int angle = 0;
		projectile.setvForce(Quaternion.AngleAxis (angle, Vector3.forward) * Vector3.up);
		projectile.move ();
		StartCoroutine (Delay (.1f));
	}

	//Delay between player-fired bullets
	IEnumerator Delay (float x)
	{
		yield return new WaitForSeconds (x);
		canFire = true;
	}

	//Period of invincibility time after getting hit
	IEnumerator DeathDelay (float x)
	{
		StartCoroutine (StopEnemyBulletsDelay(x-.5f));
		yield return new WaitForSeconds (x);
		noDamage = false;
		anim.SetBool("IsHit", false);
	}

	IEnumerator StopEnemyBulletsDelay (float x)
	{
		yield return new WaitForSeconds (x);
		stopEnemyBullets = false;
	}

	IEnumerator bulletFade(GameObject projectile) 
	{
		yield return new WaitForSeconds(.5f);
		Destroy (projectile);
	}
}
