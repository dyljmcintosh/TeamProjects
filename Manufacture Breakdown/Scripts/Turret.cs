using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret : MonoBehaviour 
{
	Animator anim;

	//Reference to the cannon object (for aiming)
	public Cannon CannonRef;
	//Current Target
	public Attacker Target;
	//give an offset position for when turret spawning
	public Vector3 PositionOffset;
	
	//How far can the target be
	public float AttackDistance;

	public enum TargetType {Land, Air, LandAir}	
	public TargetType AttackType;
	
	//List of upgrades to pick from
	public UpgradeData[] Upgrades;
	
	public void Awake()
	{
		transform.position += PositionOffset;

	}
	
	public void Update()
	{
		if(Target == null)
		{
			FindTarget();
			CannonRef.Target = Target;
		}
		else
		{
			//check the target is within range
			float distance = Vector3.Distance(transform.position, Target.transform.position);
			if(distance >= AttackDistance)
				Target = null;
		}
	}
	
	//Locate a target on the field
	private void FindTarget()
	{
		//Get all attackers in the map
		GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag ("Attacker");
		
		//Store a list of all targets within range
		List<GameObject> targetList = new List<GameObject> ();
		foreach(GameObject go in potentialTargets)
		{
			if (go.GetComponent<Attacker>() == null)
				continue;
			
			//Check target vaildity
			if (!ValidTarget (go.GetComponent<Attacker>()))
				continue;
			
			//check within range
			if(Vector3.Distance(transform.position, go.transform.position) < AttackDistance)
			{
				//Add to the actual target list
				targetList.Add (go);
			}
		}
		
		//Get the closest target, check we have targets
		if(targetList.Count > 0)
		{
			//distance to first enemy
			float distance = Vector3.Distance(transform.position, targetList[0].transform.position);
			
			float tempDistance = 0;
			foreach(GameObject go in targetList)
			{
				tempDistance = Vector3.Distance(transform.position, go.transform.position);
				if(distance > tempDistance)
				{
					//Target is further away so just loop again
					continue;
				}
				else
				{
					//This target is closer
					distance = tempDistance;
					Target = go.GetComponent<Attacker>();
				}
			}
		}
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
	
	public void OnMouseDown()
	{
		if (GameObject.Find ("Controller").GetComponent<Controller>().tooltipbool) 
		{
			anim = GameObject.FindWithTag ("ToolTip").GetComponent<Animator> ();
			anim.runtimeAnimatorController = GameObject.Find ("Controller").GetComponent<Controller> ().ToolTipAnimOUT;
		}
		GameObject.Find ("Controller").GetComponent<Controller>().ShowUpgrades (this);
		GameObject.FindWithTag ("Menu").GetComponent<MenuAnimation>().PlayAnim();
		GameObject.Find ("Controller").GetComponent<Controller>().upgrading = true;
	}
}