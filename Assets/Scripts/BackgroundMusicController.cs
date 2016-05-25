using UnityEngine;
using System.Collections;

public class BackgroundMusicController : MonoBehaviour {
	//Game variables
	public GameObject boss;
	public GameObject enemyPrefab;
	private bool inStage = true;
	private float time;

	//An iterator will be used for multiple arrays - each array controlling a different variable on the spawned object
	private int arrayCount;							//iterator for the array
	//time for next enemy spawn
	private float[] nextTime;

	private int nextWave = 1;

	//x location of next enemy spawn 	4.5 is slightly offscreen
	private float[] xSpawn;
	//y location of next enemy spawn	5.6 is slightly offscreen
	private float[] ySpawn;
	//Type of movement - see EnemyMovement script
	private int[] movement;
	//projectile speed
	private float[] projectileSpeed;
	//frequency of enemy pattern
	private float[] freq;
	//type of bullet pattern
	private int[] style;
	//timing of next spawn - set to 999 to allow for a long buffer period before actual values are set
	public float[] spawnTime = {999};
	//delay before firing
	private float[] delay;
	//sets enemy health
	private int[] health;
	
	//Music variables
	public AudioSource stage1;
	public AudioSource boss1;
	public float songTime;			//current progression in song

	//Gui variables
	private bool guiOn;
	private int guiNum;
	
	void Start () {
		guiOn = false;
		guiNum = 0;
		arrayCount = 0;
		for (int x = 0; x<spawnTime.Length; x++) {
			spawnTime[x] = spawnTime[x]+25.8f;
		}

		stage1.Play ();
		StartCoroutine(MusicDelay (1));
		/*
		StartCoroutine (WaveDelay (25.8f));		//how long before the first wave
		//StartCoroutine (WaveDelay (0));
		StartCoroutine(MusicDelay (188.0f)); 	//how long the song is
		StartCoroutine(GUIDelay (4));			//how long before tutorial text*/

		//For testing purposes
		/*GameObject newObject = Instantiate (enemyPrefab, new Vector2 (0, 6), Quaternion.identity) as GameObject;
		EnemyMovement enemy = newObject.GetComponent<EnemyMovement> ();
		enemy.move (7, 4, .5f, 3, .1f, 99);*/
	}

	void Update () {
		songTime = stage1.time;
		while (inStage && songTime >= spawnTime[arrayCount]) {
			GameObject newObject = Instantiate (enemyPrefab, new Vector2 (xSpawn [arrayCount], ySpawn [arrayCount]), Quaternion.identity) as GameObject;
			EnemyMovement enemy = newObject.GetComponent<EnemyMovement> ();
			enemy.move (movement[arrayCount], projectileSpeed[arrayCount], freq[arrayCount], style[arrayCount], delay[arrayCount], health[arrayCount]);
			arrayCount++;
		}
	}

	//Left burst, 3 right rains x3 with increasing difficulty
	void wave1 (){
		xSpawn = new float[] {-4.5f,4.5f,4.5f,4.5f,-4.5f,4.5f,4.5f,4.5f,-4.5f,4.5f,4.5f,4.5f};
		ySpawn = new float[]{4,4,4,4,4,4,4,4,4,4,4,4};
		movement = new int[] {0,2,2,2,0,2,2,2,0,2,2,2};
		projectileSpeed = new float[] {3,4,4,4,3,4,4,4,3,4,4,4};
		freq = new float[] {1,.5f,.5f,.5f,1,.4f,.4f,.4f,1,.3f,.3f,.3f};
		style = new int[] {4,3,3,3,5,3,3,3,6,3,3,3};
		spawnTime = new float[] {1,4,5,6,9,12,13,14,17,20,21,22,999};
		delay = new float[] {.5f,.1f,.2f,.25f,.5f,.1f,.2f,.25f,.5f,.1f,.2f,.25f};
		health = new int[] {10,4,4,4,10,4,4,4,10,4,4,4};
		time = songTime;
		for (int x = 0; x<spawnTime.Length; x++) {
			spawnTime[x] = spawnTime[x]+time;
		}
		StartCoroutine (WaveDelay (25)); //22 second wave + 3 second delay

	}

	//circle pulses + lockdown waves
	void wave2(){
		arrayCount = 0;

		xSpawn = new float[] {-3,3,-2,2,2,-2,3,-3,1,-1,-4,4,-3.5f,3.5f,-3,3,-2.5f,2.5f,-2,2,-1.5f,1.5f,-1,1,-4,4,0};
		ySpawn = new float[]{5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f};
		movement = new int[] {4,4,5,5,4,4,4,4,4,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,4,4,4};
		projectileSpeed = new float[] {2,2,3,3,4,4,4,4,4,4,8,8,8,8,8,8,8,8,8,8,8,8,8,8,4,4,5};
		freq = new float[] {2,2,1,1,.6f,.6f,.6f,.6f,.6f,.6f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.4f};
		style = new int[] {6,6,4,4,0,1,2,2,1,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,5};
		spawnTime = new float[]{1,1,14,14,25,30,35,40,45,45,55,55,55.5f,55.5f,56,56,56.5f,56.5f,57,57,57.5f,57.5f,58,58,60,60,60,999};
		delay = new float[] {2,3,2,3,1.6f,1.6f,1.6f,1.6f,1.6f,1.6f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,1.6f,1.6f,4};
		health = new int[] {15,15,20,20,20,20,12,12,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,20,20,80};

		time = songTime;
		for (int x = 0; x<spawnTime.Length; x++) {
			spawnTime[x] = spawnTime[x]+time;
		}
		StartCoroutine (WaveDelay (80));	//60 second wave + 20 second delay
	}

	//edge homing
	void wave3(){
		arrayCount = 0;

		xSpawn = new float[]{4,4,4,4,4,4,-4,-4,-4,-4,-4,-4,4,4,4,4,4,4,-4,-4,-4,-4,-4,-4};
		ySpawn = new float[]{5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f};
		movement = new int[] {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3};
		projectileSpeed = new float[] {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4};
		freq = new float[] {.5f,.5f,.5f,.5f,.5f,.5f,.5f,.5f,.5f,.5f,.5f,.5f,.5f,.5f,.5f,.5f,.5f,.5f,.5f,.5f,.5f,.5f,.5f,.5f};
		style = new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
		spawnTime = new float[] {1,1.4f,1.8f,2.2f,2.6f,3,3.4f,3.8f,4.2f,4.6f,5,5.4f,5.8f,6.2f,6.6f,7,7.4f,7.8f,8.2f,8.6f,9,9.4f,9.8f,10.2f,999};
		delay = new float[] {.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f};
		health = new int[] {15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15};

		time = songTime;
		for (int x = 0; x<spawnTime.Length; x++) {
			spawnTime[x] = spawnTime[x]+time;
		}
		StartCoroutine (WaveDelay (16));	//10 second wave, 6 second delay
	}

	void wave4(){
		arrayCount = 0;

		xSpawn = new float[] {-3,-2,-1,0,1,2,3};
		ySpawn = new float[] {5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f};
		movement = new int[] {4,4,5,5,5,4,4};
		projectileSpeed = new float[] {4,4,4,4,4,4,4};
		freq = new float[] {.7f,.7f,.7f,.7f,.7f,.7f,.7f};
		style = new int[] {0,0,0,0,0,0,0};
		spawnTime = new float[] {1,1,1,1,1,1,1,999};
		delay = new float[] {2,2,2,2,2,2,2};
		health = new int[] {10,10,10,10,10,10,10};

		time = songTime;
		for (int x = 0; x<spawnTime.Length; x++) {
			spawnTime[x] = spawnTime[x]+time;
		}
		StartCoroutine (WaveDelay (20));	//1 second wave + 19 second delay
	}

	void wave5(){
		arrayCount = 0;

		xSpawn = new float[] {-4,4,-3.5f,3.5f,-3,3,-2.5f,2.5f,-2,2,-1.5f,1.5f,-1,1,-4.5f,-4.5f,4.5f,4.5f,-4.5f,-4.5f,4.5f,4.5f,-4.5f,-4.5f,4.5f,4.5f};
		ySpawn = new float[] {5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5.6f,5,5,4,4,4,4,5,5,0,0,0,0};
		movement = new int[] {3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,1,2,6,0,1,2,6,0,1,2,6};
		projectileSpeed = new float[] {8,8,8,8,8,8,8,8,8,8,8,8,8,8,6,6,6,6,6,6,6,6,6,6,6,6};
		freq = new float[] {.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f,.2f};
		style = new int[] {3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,1,1,1,1,1,1,1,1,1};
		spawnTime = new float[] {1,1,1.5f,1.5f,2,2,2.5f,2.5f,3,3,3.5f,3.5f,4,4,5,5,5,5,6,6,6,6,7,7,7,7,999};
		delay = new float[] {.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f,.1f};
		health = new int[] {15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15};

		time = songTime;
		for (int x = 0; x<spawnTime.Length; x++) {
			spawnTime[x] = spawnTime[x]+time;
		}
	}
		
	void OnGUI()
	{
		if(guiOn)
		{
			//Start up Tutorial messages
			GUI.color = new Color(1,1,1);
			if(guiNum == 1)
				GUI.Box(new Rect(Screen.width/2-100,Screen.height/2,200,25), "Use the Arrow Keys to Move");
			if(guiNum == 2)
				GUI.Box(new Rect(Screen.width/2-60,Screen.height/2,120,25), "Hold Z to attack");
			if(guiNum == 3)
				GUI.Box(new Rect(Screen.width/2-80,Screen.height/2,160,25), "Hold Shift to slow down");
			if(guiNum == 4)
				GUI.Box(new Rect(Screen.width/2-40,Screen.height/2,80,25), "Good Luck");
		}
	}

	//switches the soundtrack from stage to boss after a set period
	IEnumerator MusicDelay (float x)
	{
		yield return new WaitForSeconds (x);
		stage1.Stop ();
		inStage = false;
		boss1.Play ();
		boss.SetActive (true);
	}

	IEnumerator WaveDelay (float x)
	{
		yield return new WaitForSeconds (x);
		switch (nextWave) {
		case 1:
			wave1 ();
			nextWave++;
			break;
		case 2:
			wave2();
			nextWave++;
			break;
		case 3:
			wave3();
			nextWave++;
			break;
		case 4:
			wave4();
			nextWave++;
			break;
		case 5:
			wave5();
			break;
		}
	}

	//switches the GUI display
	private IEnumerator GUIDelay(int seconds)
	{
		yield return new WaitForSeconds (seconds);
		if (guiNum < 5)
		{
			if (guiOn) {
				guiOn = false;
				StartCoroutine(GUIDelay(2));
			} 
			else {
				guiOn = true;
				guiNum++;
				StartCoroutine(GUIDelay(5));
			}
		}
			
	}
}
