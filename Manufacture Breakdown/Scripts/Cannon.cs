using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cannon : MonoBehaviour 
{
	public Attacker Target;
	public Transform BulletSpawnPoint;
	private Quaternion originalRotation;

	public GameObject BulletPrefab;
	//how long is the delay between shots
	public float RateOfFire;
	private float prevTime;
	private float currentTime;
	private float rotationStrength = 10.0f; //how fast to rotate towards target

	//Threshold for aiming at the target
	private const float AIM_ANGLE = 10;

	//variables to handle reloading
	private bool canShoot = true;

	
	public void Start()
	{
		originalRotation = transform.rotation;
		prevTime = Time.time;//set the previous time (for future use)
	}
	
	public void Update()
	{
		if(Target != null)
		{
			FaceTarget();
			
			if(canShoot)
			{
				if(Vector3.Angle (Target.transform.position - transform.position, transform.forward) < AIM_ANGLE)
				{
					
					Instantiate(BulletPrefab, BulletSpawnPoint.position, BulletSpawnPoint.rotation);
					//begin reload
					canShoot = false;
					currentTime = RateOfFire;
					prevTime = Time.time;
					
				}
			}
			else
			{
				//reloading
				currentTime -= (Time.time - prevTime);
				prevTime = Time.time;
				if(currentTime <= 0.0f)
					canShoot = true;
			}
		}
	}
	
	private void FaceTarget()
	{
		Quaternion targetRotation = Quaternion.LookRotation(Target.transform.position - transform.position);
		float str = Mathf.Min (rotationStrength * Time.deltaTime, 1);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, str);
		Quaternion rot = gameObject.transform.rotation;
		gameObject.transform.rotation = rot;
	}
}