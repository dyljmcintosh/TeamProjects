using UnityEngine;
using System.Collections;

public class Deathbounty : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds (3);
		Destroy (gameObject);
	}

}
