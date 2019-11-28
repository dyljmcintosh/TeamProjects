using UnityEngine;
using System.Collections;

public class Attacker : MonoBehaviour 
{
	//distance between this and the waypoint
	public float Distance;
	public float Speed;
	public float MaxHealth;
	private float currentHealth;
	private float speedModifier; 	//for adjusting the speed of an enemy
	public int Bounty;
	private int DoTCounter = 0;

	public GameObject Slowparticle;
	public GameObject BountyParticle;
	public GameObject Bar;

	public Transform bountyposition;
	public Vector3 PositionOffset;
	
	//get Spawner info
	private Spawner spawn;
	//Which waypoint are we heading to?
	public Waypoint CurrentWaypoint;

	public bool Air = false;
	bool isHitbyProjectile = false;

	public void Awake()
	{
		transform.position += PositionOffset;
	}

	public void Start()
	{
		currentHealth = MaxHealth;
		speedModifier = 1.0f;
		spawn = GameObject.Find ("SpawnController").GetComponent<Spawner> ();
	}
	
	public void Update()
	{
		MovementController ();
	}

	//RandomWaypoint was coded my Brock McCall
	public void RandomWaypoint ()
	{
		int i = Random.Range (1, 10);
		//set the next waypoint if its an odd number
		if (i % 2 !=0)
		{
			CurrentWaypoint = CurrentWaypoint.Waypoint1;
		}
		
		//set the next waypoint if its an even number
		else
		{
			CurrentWaypoint = CurrentWaypoint.Waypoint2;
		}
	}

	private void MovementController()
	{
		//How close are we to the target?
		float distance = Vector3.Distance (transform.position, 
		                                   CurrentWaypoint.transform.position);
		
		//check if within range
		if(distance < Distance)
		{
			if(CurrentWaypoint.tag == "Random")
				RandomWaypoint ();
			else
			//set the next waypoint
			CurrentWaypoint = CurrentWaypoint.NextWaypoint;
		}
		
		transform.Translate (Vector3.forward * Speed * speedModifier * Time.deltaTime);
		FaceTarget ();
	}
	
	//strength of rotation
	private float rotStrength = 8.5f;
	private void FaceTarget()
	{
		Quaternion targetRotation = Quaternion.LookRotation( CurrentWaypoint.transform.position - transform.position);
		float str = Mathf.Min (rotStrength * Time.deltaTime, 1);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, str);
		Quaternion rot = gameObject.transform.rotation;
		gameObject.transform.rotation = rot;
	}
	
	public virtual void OnHit(float damage)
	{
		currentHealth -= damage;
		float CalculateHealth = currentHealth / MaxHealth;
		
		SetHealth (CalculateHealth);
		
		//check if now dead
		if(currentHealth <= 0)
		{
			if (BountyParticle != null)
				Instantiate(BountyParticle, bountyposition.position, bountyposition.rotation);

			if (!isHitbyProjectile)
			{
				isHitbyProjectile = true;
				spawn.DecreaseEnemyCount();
				transform.position = new Vector3(1000f,1000f);
				PlayerData.Instance.Money += Bounty;
				PlayerData.Instance.Score += Bounty;
				StartCoroutine (DEATH ());
			}


		}
	}
	
	private IEnumerator DEATH()
	{
		yield return new WaitForSeconds (0.1f);
		Destroy (gameObject);
	}
	
	public void Slow()
	{
		//Make sure not already slowed
		if(speedModifier >= 1.0f)
			StartCoroutine (SlowRoutine());

		Slowparticle.gameObject.SetActive (true);
	}
	
	private IEnumerator SlowRoutine()
	{
		//halve speed
		speedModifier = 0.5f;
		yield return new WaitForSeconds (5.0f);
		//reset speed modifier to normal
		speedModifier = 1.0f;
		Slowparticle.gameObject.SetActive (false);
	}

	public void DoT()
	{
		if (DoTCounter == 0)
			StartCoroutine (DoTRoutine ());
	}
	
	private IEnumerator DoTRoutine()
	{
		DoTCounter = 20;
		while (DoTCounter > 0)
		{
			OnHit(27.5f);
			DoTCounter -= 1;
			yield return new WaitForSeconds(0.25f);
		}
	}
	
	public void SetHealth (float UnitHealth)
	{
		//UnitHealth value 0 - 1
		Bar.transform.localScale = new Vector3 (Mathf.Clamp (UnitHealth, 0f, 1f), Bar.transform.localScale.y, Bar.transform.localScale.z);
	}


}