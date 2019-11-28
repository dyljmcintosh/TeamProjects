using UnityEngine;
using System.Collections;

public class NextScene : MonoBehaviour 
{
	public GameObject Panel;
	public string LoadedScene;

	bool Play;
	public static bool continuesBool = false;

	public void PressPlay()
	{
		if (!Play)
		{
			Panel.gameObject.SetActive (true);
			Play = true;
			continuesBool = false;
			PlayerData.Instance.Money = 300;
			PlayerData.Instance.Lives = 10 ;
		}
		else
		{
			Panel.gameObject.SetActive (false);
			Play = false;
		}
		
	}

	public void Restart()
	{
		PlayerData.Instance.Money = 300;
		PlayerData.Instance.Lives = 10 ;
		Application.LoadLevel (LoadedScene);
	}


	public void Continues()
	{
		continuesBool = true;
		Application.LoadLevel (1);
	}

	public void ToLevelOne()
	{
		Application.LoadLevel (1);
	}

	public void ToLevelTwo()
	{
		Application.LoadLevel (2);
	}

	public void ToLevelThree()
	{
		Application.LoadLevel (3);
	}

	public void ToMainMenu()
	{
		Application.LoadLevel (0);
	}

	public void ToQuitGame()
	{
		Application.Quit();
	}
}
