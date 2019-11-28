using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pulse : MonoBehaviour 
{
	public GameObject PulsePrefab;
	public Transform PulsePos;
	public float Distance;
	public List<GameObject> Targets;
	public bool Inrange = false;

	GameObject part = null;

	public void OnTriggerEnter(Collider col)
	{
		Debug.Log (col.name);
		if (col.tag == "Attacker") 
		{
			Targets.Add (col.gameObject);
			if (Targets.Count > 0) 
			{
				Debug.Log ("inrange");
				if (part == null)
					part = Instantiate (PulsePrefab, PulsePos.position, PulsePos.rotation) as GameObject;
			} 
		} else if (col.tag == "Towers")
			Targets.Remove (col.gameObject);
	}

	public void OnTriggerExit(Collider col)
	{
		if (col.tag == "Attacker") 
		{
			Targets.Remove(col.gameObject);
			if (Targets.Count == 0) 
			{
				Debug.Log ("inrange");
				Destroy(part);
			}
		}
	}

	//Attacking the correct unit type
	public enum TargetType {Land, Air, LandAir}
	
	public TargetType AttackType;

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
}