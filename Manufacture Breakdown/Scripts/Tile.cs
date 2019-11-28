using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour 
{
	public Material hoverMaterial;
	private Material originalMaterial;
	
	public bool hasBuilt = false;
	//If true, this can have traps
	public bool Ground = false;

	//Reference to the turret placed here
	private Turret turretRef = null;
	Animator anim;

	public void Awake()
	{
		originalMaterial = GetComponent<Renderer> ().material;

	}
	
	public void OnMouseEnter()
	{
		GetComponent<Renderer> ().material = hoverMaterial;
	}
	
	public void OnMouseExit()
	{
		GetComponent<Renderer> ().material = originalMaterial;
	}

	public void OnMouseDown()
	{
		if (GameObject.Find ("Controller").GetComponent<Controller>().tooltipbool) 
		{
			anim = GameObject.FindWithTag ("ToolTip").GetComponent<Animator> ();
			anim.runtimeAnimatorController = GameObject.Find ("Controller").GetComponent<Controller> ().ToolTipAnimOUT;
		}
		//Open up the build Menu
		GameObject.Find ("Controller").GetComponent<Controller> ().BuildMenu (this);
		GameObject.FindWithTag ("Menu").GetComponent<MenuAnimation> ().PlayAnim ();
		//GameObject.Find("Controller").GetComponent<Controller>().ToolTipoutAnim ();
	
	}
	
	public void Build(GameObject turret)
	{
		GameObject tempTurret = (GameObject)Instantiate (turret, transform.position, Quaternion.identity);
		
		turretRef = tempTurret.GetComponent<Turret> ();
	}
	
	public bool CanBuild()
	{
		return turretRef == null;
	}
}
