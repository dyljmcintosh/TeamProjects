using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
	//The position to spawn an enemy
	public Transform spawnPoint;
	public Transform spawnPoint01;
	
	//Which waypoint to go to
	public Waypoint firstWaypoint;
	public Waypoint firstWaypoint01;
	
	//Attack wave data
	public SpawnList spawnData;
	
	//Which attack wave are we up to?
	public int currentWave;
	//Which unit in the wave are we up to?
	private int currentUnit;	
	//Enemy count
	private int EnemyCount;


	//reference to the controller
	private Controller controller;
	private bool LevelThreeSpawn = false;

	public void Awake()
	{
		GameObject levelThree = GameObject.Find ("Dumpster");
		if(levelThree == true)
		{
			LevelThreeSpawn = true;
		}
		
		currentUnit = 0;
		currentWave = 0;
		EnemyCount = 0;
		
		//retrieve the controller from the scene
		controller = GameObject.Find ("Controller").GetComponent<Controller> ();
	}

	IEnumerator Start()
	{
		yield return new WaitForSeconds (5);
		StartCoroutine (SpawnRoutine ());
		yield break;
	}
	
	//handles the spawning section
	private IEnumerator SpawnRoutine()
	{
		//Loop until every wave is destroyed
		while(currentWave < spawnData.Wave.Length)
		{
			//are we in the victory screen
			while(controller.VictoryWait)
				yield return 0;
			
			currentUnit = 0;
			
			Debug.Log (currentUnit);

			//Loop through each unit to spawn
			while(currentUnit < spawnData.Wave[currentWave].Units.Length)
			{
				GameObject unit = (GameObject)Instantiate(
					spawnData.Wave[currentWave].Units[currentUnit],
					spawnPoint.position, 
					spawnPoint.rotation);
				unit.GetComponent<Attacker>().CurrentWaypoint = firstWaypoint;
				IncreaseEnemyCount();
				
				if( LevelThreeSpawn == true)
				{
					//level three spawn
					GameObject unit1A = (GameObject)Instantiate(
						spawnData.Wave[currentWave].Units[currentUnit],
						spawnPoint01.position, 
						spawnPoint01.rotation);
					unit1A.GetComponent<Attacker>().CurrentWaypoint = firstWaypoint01;
					IncreaseEnemyCount();
					
				}
				currentUnit += 1;
				yield return new WaitForSeconds(spawnData.Wave[currentWave].SpawnDelay);
			
				
				Debug.Log (currentUnit);
				if (currentUnit >= spawnData.Wave[currentWave].Units.Length)
					yield break;
			}		
		}
	}
	
	//increase the current count of enemies on the map
	public void IncreaseEnemyCount()
	{
		EnemyCount += 1;
		Debug.Log (EnemyCount);
	}
	
	//Decrease the current count of enemies on the map
	public void DecreaseEnemyCount()
	{

	    EnemyCount--;
	
		Debug.Log (EnemyCount);

		//if there are no enemies, wave complete
		if (EnemyCount <= 0) 
		{

			controller.CompleteWave (200);
			Debug.Log ("sdghadguasoudgaslkdjbh");
			StartCoroutine (SpawnRoutine ());
			currentWave++;


		}
	}
}

[System.Serializable]
public class SpawnData
{
	public GameObject[] Units;
	public float SpawnDelay;
}

[System.Serializable]
public class SpawnList
{
	public SpawnData[] Wave;
	//How long to wait before the next wave
	public float WaveDelay;
}
