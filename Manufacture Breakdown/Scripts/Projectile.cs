using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	//Percentage change of inflicting FrostBurn
	public int FrostBurnChance = 0;
	public float Speed;
	public float DestroyTime;
	public int Damage;
	
	//Does this add a slow effect
	public bool CanSlow = false;
	
	//Create an impact/explosion on hit
	public GameObject ImpactPrefab;
	
	public IEnumerator Start()
	{
		//wait DestroyTime seconds and destroy the projectile
		yield return new WaitForSeconds (DestroyTime);
		
		Destroy (gameObject);
	}
	
	public void Update()
	{
		transform.Translate (Vector3.forward * Speed * Time.deltaTime);
	}
	
	public void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Attacker")
		{
			Attacker attacker = col.gameObject.GetComponent<Attacker> ();

			if(ImpactPrefab != null)
				Instantiate(ImpactPrefab, transform.position, transform.rotation);
			
			attacker.OnHit(Damage);

			//Check if can inflict slow
			if(CanSlow)
				attacker.Slow();
			Destroy (gameObject);
			
			//Can we inflict FrostBurn?
			if(FrostBurnChance > 0)
			{
				int chance = Random.Range (0,100);
				if (chance <= FrostBurnChance)
				{
					//inflict Frostburn
					attacker.DoT();
				}
			}
			
			Destroy(gameObject);
		}
	}

}
