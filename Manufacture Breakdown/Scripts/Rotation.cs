using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour 
{
	public int Speed;

	private void Update ()
	{
		gameObject.transform.Rotate (new Vector3 (0, Speed * Time.deltaTime, 0));
	}
}
