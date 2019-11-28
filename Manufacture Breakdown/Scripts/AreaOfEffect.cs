using UnityEngine;
using System.Collections;

public class AreaOfEffect : MonoBehaviour 
{
	public float DestroyTime;
	public int FrostBurnChance = 0;
	public int Damage;

	public bool CanSlow;

	public enum TargetType {Land, Air, LandAir}
	public TargetType AttackType;
	
	public IEnumerator Start()
	{
		//wait DestroyTime seconds and destroy the projectile
		yield return new WaitForSeconds (DestroyTime);

	}

	public bool ValidTarget (Attacker unit)
	{
		if (AttackType == TargetType.Land)
		{
			//Make sure the target is land based
			if(!unit.Air)
				return true;
		}
		else if (AttackType == TargetType.Air)
		{
			//Make sure the target is aerial
			if(unit.Air)
				return true;
		}
		else
		{
			//Can shoot both
			return true;
		}
		
		return false;
	}
	
	public void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Attacker")
		{
			col.GetComponent<Attacker>().OnHit(Damage);
			
			if (CanSlow)
				col.GetComponent<Attacker> ().Slow ();

			if(FrostBurnChance > 0)
			{
				int chance = Random.Range (0,100);
				if (chance <= FrostBurnChance)
				{
					//inflict poison
					col.GetComponent<Attacker>().DoT();
				}
			}
		}

	}


}
