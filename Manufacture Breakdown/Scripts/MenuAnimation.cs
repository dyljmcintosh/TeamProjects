using UnityEngine;
using System.Collections;

public class MenuAnimation : MonoBehaviour {

	Animator anim;
	public RuntimeAnimatorController MenuIN;
	public RuntimeAnimatorController MenuOUT;
	public bool buttonBool = false;

	void Start()
	{
		anim = GetComponent<Animator>();
	}

	public void PlayAnim()
	{
		anim.GetComponent<RuntimeAnimatorController>();

		if (buttonBool) 
		{
			anim.runtimeAnimatorController = MenuOUT;
			buttonBool = false;
		}else 
			anim.runtimeAnimatorController = MenuIN;
	}

}
