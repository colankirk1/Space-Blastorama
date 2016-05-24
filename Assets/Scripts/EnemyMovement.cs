using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	private int rand;					//random number variable
	public float projectileSpeed;		//speed of projectile
	private float initialy;				//y location of enemy on creation - needed for movementType 0
	private float movement = .05f;		//fixed movement speed	
	public int movementType;			//see move() for movement styles
	public float count;					//count for calculations
	public bool positive = true;
	public GameObject prefab;
	public GameObject prefab2;
	public GameObject deathprefab;
	private Vector3 position;
	public int health;					//amount of hits the enemy can take
	public bool canMove = false;		//initialized to fault to allow time to set parameters
	public AudioClip deathclip;
	private GameObject tempPlayer;
	Animator projectileAnim;
	
	void Start () {
		initialy = transform.position.y - Mathf.Cos (count / 35);	//correction to stop jumping on movementType 0
		tempPlayer = GameObject.Find("Player");
	}

	void Update () {
		if (canMove) {
			if(movementType == 0){		//curved movement, elongated "U" shape left to right
				position = this.transform.position;
				position.x += movement;
				position.y = Mathf.Cos (count / 35) + initialy;
				count++;
				this.transform.position = position;
			}
			else if(movementType == 1){		//horizontal movement right
				position = this.transform.position;
				position.x += movement;
				this.transform.position = position;
			}
			else if(movementType == 2){		//horizontal movement left
				position = this.transform.position;
				position.x -= movement;
				this.transform.position = position;
			}
			else if(movementType == 3){		//downward verticle movement
				position = this.transform.position;
				position.y -= movement;
				this.transform.position = position;
			}
			else if (movementType == 4){	//downward movement, stop after a set period, continue after another set period
				if (count < 90 || count > 800){
					position = this.transform.position;
					position.y -= movement/2;
					this.transform.position = position;
				}
				count++;
			}
			else if (movementType == 5){	//downward movement, stop after a set period, continue after another set period. slightly longer
				if (count < 120 || count > 830){
					position = this.transform.position;
					position.y -= movement/2;
					this.transform.position = position;
				}
				count++;
			}
			else if(movementType == 6){		//curved movement, elongated "U" shape right to left
				position = this.transform.position;
				position.x -= movement;
				position.y = Mathf.Cos (count / 35) + initialy;
				count++;
				this.transform.position = position;
			}
			else if(movementType == 7){		//stops partially down - testing purposes
				if (count < 120){
					position = this.transform.position;
					position.y -= movement/2;
					this.transform.position = position;
				}
				count++;
			}
		}
	}

	void OnTriggerEnter2D( Collider2D other )
	{
		if (other.CompareTag ("PlayerProjectile") && (other.gameObject.GetComponent<projectileMovement>().hasTriggered == false)) {
			other.gameObject.GetComponent<projectileMovement>().trigger();
			Destroy(other.gameObject);
			health--;
			if(health == 0){
				dies ();
			}
		}
	}

	//called to set parameters and start movement
	public void move(int theMovementType, float theProjectileSpeed, float frequency, int style, float delay, int theHealth){
		movementType = theMovementType;
		projectileSpeed = theProjectileSpeed;
		health = theHealth;
		if (style == 0) {
			InvokeRepeating("shootHoming", delay, frequency);
		} else if (style == 1) {
			InvokeRepeating("shootDualHoming", delay, frequency);
		}else if (style == 2) {
			InvokeRepeating("shootTriHoming", delay, frequency);
		}else if (style == 3) {
			InvokeRepeating ("shootRain", delay, frequency);
		}else if (style == 4) {
			InvokeRepeating("shootCircle", delay, frequency);
		}else if (style == 5) {
			InvokeRepeating("shootCircle2", delay, frequency);
		}else if (style == 6) {
			InvokeRepeating("shootCircle3", delay, frequency);
		}
		canMove = true;
	}

	void dies(){
		AudioSource.PlayClipAtPoint(deathclip, new Vector3(0,0,-7));
		Instantiate (deathprefab, position, Quaternion.Euler (new Vector3 (0, 0, 0)));
		Destroy (gameObject);
	}

	void OnBecameInvisible()
	{
		Destroy (gameObject);
	}

	//shoots a bullet aimed directly at the player
	public void shootHoming(){
		GameObject newObject = Instantiate (prefab, position, Quaternion.Euler (new Vector3 (0, 0, 0))) as GameObject;
		//Vector3 dir = (GameObject.Find("Player").transform.position - position).normalized;
		Vector3 dir = (tempPlayer.transform.position - position).normalized;
		newObject.GetComponent<Rigidbody2D> ().velocity = dir * projectileSpeed;
		if (tempPlayer.GetComponent<PlayerMovement> ().stopEnemyBullets == true) {
			projectileAnim = newObject.GetComponent<Animator>();
			projectileAnim.SetBool("fade", true);
			StartCoroutine(bulletFade(newObject));
		}
	}

	//shoots 2 bullets - aimed at player, but both missing by a margin on each side
	public void shootDualHoming(){
		//Vector3 temp = GameObject.Find("Player").transform.position;
		Vector3 temp = tempPlayer.transform.position;
		GameObject newObject = Instantiate (prefab, position, Quaternion.Euler (new Vector3 (0, 0, 0))) as GameObject;
		Vector3 dir = (temp - position).normalized;
		if(Mathf.Abs(temp.y - position.y) > 2)
			dir.x += .2f;
		else {
			dir.y += .2f;
		}
		newObject.GetComponent<Rigidbody2D> ().velocity = dir * projectileSpeed;
		if (tempPlayer.GetComponent<PlayerMovement> ().stopEnemyBullets == true) {
			projectileAnim = newObject.GetComponent<Animator>();
			projectileAnim.SetBool("fade", true);
			StartCoroutine(bulletFade(newObject));
		}
		newObject = Instantiate (prefab, position, Quaternion.Euler (new Vector3 (0, 0, 0))) as GameObject;
		dir = (temp - position).normalized;
		if(Mathf.Abs(temp.y - position.y) > 2)
			dir.x -= .2f;
		else {
			dir.y -= .2f;
		}
		newObject.GetComponent<Rigidbody2D> ().velocity = dir * projectileSpeed;
		if (tempPlayer.GetComponent<PlayerMovement> ().stopEnemyBullets == true) {
			projectileAnim = newObject.GetComponent<Animator>();
			projectileAnim.SetBool("fade", true);
			StartCoroutine(bulletFade(newObject));
		}
	}

	//shoots 3 bullets - one aimed directly at the player, and the other two missing by a margin on each side
	public void shootTriHoming(){
		//Vector3 temp = GameObject.Find("Player").transform.position;
		Vector3 temp = tempPlayer.transform.position;
		GameObject newObject = Instantiate (prefab, position, Quaternion.Euler (new Vector3 (0, 0, 0))) as GameObject;
		Vector3 dir = (temp - position).normalized;
		newObject.GetComponent<Rigidbody2D> ().velocity = dir * projectileSpeed;
		if (tempPlayer.GetComponent<PlayerMovement> ().stopEnemyBullets == true) {
			projectileAnim = newObject.GetComponent<Animator>();
			projectileAnim.SetBool("fade", true);
			StartCoroutine(bulletFade(newObject));
		}
		newObject = Instantiate (prefab, position, Quaternion.Euler (new Vector3 (0, 0, 0))) as GameObject;
		dir = (temp - position).normalized;
		if(Mathf.Abs(temp.y - position.y) > 2)
			dir.x += .3f;
		else {
			dir.y += .3f;
		}
		newObject.GetComponent<Rigidbody2D> ().velocity = dir * projectileSpeed;
		if (tempPlayer.GetComponent<PlayerMovement> ().stopEnemyBullets == true) {
			projectileAnim = newObject.GetComponent<Animator>();
			projectileAnim.SetBool("fade", true);
			StartCoroutine(bulletFade(newObject));
		}
		newObject = Instantiate (prefab, position, Quaternion.Euler (new Vector3 (0, 0, 0))) as GameObject;
		dir = (temp - position).normalized;
		if(Mathf.Abs(temp.y - position.y) > 2)
			dir.x -= .3f;
		else {
			dir.y -= .3f;
		}
		newObject.GetComponent<Rigidbody2D> ().velocity = dir * projectileSpeed;
		if (tempPlayer.GetComponent<PlayerMovement> ().stopEnemyBullets == true) {
			projectileAnim = newObject.GetComponent<Animator>();
			projectileAnim.SetBool("fade", true);
			StartCoroutine(bulletFade(newObject));
		}
	}

	//creates a burst of bullets outward from center
	public void shootCircle(){
		rand = Random.Range (0, 30);
		for (int y = rand; y<360; y+=30) {
			GameObject newObject = Instantiate (prefab, position, Quaternion.identity) as GameObject;
			projectileMovement projectile = newObject.GetComponent<projectileMovement>();
			projectile.setForce(projectileSpeed);
			projectile.setvForce (Quaternion.AngleAxis (y, Vector3.forward) * Vector3.up);
			projectile.move ();
			if (tempPlayer.GetComponent<PlayerMovement> ().stopEnemyBullets == true) {
				projectileAnim = newObject.GetComponent<Animator>();
				projectileAnim.SetBool("fade", true);
				StartCoroutine(bulletFade(newObject));
			}
		}
	}

	//More dense
	public void shootCircle2(){
		rand = Random.Range (0, 20);
		for (int y = rand; y<360; y+=20) {
			GameObject newObject = Instantiate (prefab, position, Quaternion.identity) as GameObject;
			projectileMovement projectile = newObject.GetComponent<projectileMovement>();
			projectile.setForce(projectileSpeed);
			projectile.setvForce (Quaternion.AngleAxis (y, Vector3.forward) * Vector3.up);
			projectile.move ();
			if (tempPlayer.GetComponent<PlayerMovement> ().stopEnemyBullets == true) {
				projectileAnim = newObject.GetComponent<Animator>();
				projectileAnim.SetBool("fade", true);
				StartCoroutine(bulletFade(newObject));
			}
		}
	}

	//significantly more dense
	public void shootCircle3(){
		rand = Random.Range (0, 10);
		for (int y = rand; y<360; y+=10) {
			GameObject newObject = Instantiate (prefab, position, Quaternion.identity) as GameObject;
			projectileMovement projectile = newObject.GetComponent<projectileMovement>();
			projectile.setForce(projectileSpeed);
			projectile.setvForce (Quaternion.AngleAxis (y, Vector3.forward) * Vector3.up);
			projectile.move ();
			if (tempPlayer.GetComponent<PlayerMovement> ().stopEnemyBullets == true) {
				projectileAnim = newObject.GetComponent<Animator>();
				projectileAnim.SetBool("fade", true);
				StartCoroutine(bulletFade(newObject));
			}
		}
	}

	//bullets fall straight down
	public void shootRain(){
		GameObject newObject = Instantiate (prefab, position, Quaternion.identity) as GameObject;
		projectileMovement projectile = newObject.GetComponent<projectileMovement>();
		projectile.setForce(projectileSpeed);
		projectile.setvForce (Quaternion.AngleAxis (180, Vector3.forward) * Vector3.up);
		projectile.move ();
		if (tempPlayer.GetComponent<PlayerMovement> ().stopEnemyBullets == true) {
			projectileAnim = newObject.GetComponent<Animator>();
			projectileAnim.SetBool("fade", true);
			StartCoroutine(bulletFade(newObject));
		}
	}

	IEnumerator bulletFade(GameObject projectile) 
	{
		yield return new WaitForSeconds(.5f);
		Destroy (projectile);
	}
}
