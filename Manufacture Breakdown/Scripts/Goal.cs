using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour 
{
	
	private Spawner Spawn;
	public GameObject GameOver;
	Controller control;
	
	public void Start()
	{
		Spawn = GameObject.Find ("SpawnController").GetComponent<Spawner>();
		control = GameObject.Find ("Controller").GetComponent<Controller> ();
	}

	public void OnTriggerEnter(Collider col)
	{
		//make sure an enemy enters the area
		if(col.tag == "Attacker")
		{
			//Aaannnddd it's gone!
			Destroy(col.gameObject);
			//UpdateEnemyCount
			Spawn.DecreaseEnemyCount();
			//remove the lives!
			PlayerData.Instance.Lives -= 1;
			if(PlayerData.Instance.Lives <= 0)
			{
				Time.timeScale = 0;
				GameOver.SetActive (true);
				control.txtLoseScore.text = PlayerData.Instance.Score.ToString ();
				control.txtLoseWave.text = control.Currentwave.ToString ();
			}
		}
	}
}