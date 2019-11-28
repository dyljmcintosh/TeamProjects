using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Controller : MonoBehaviour 
{
	public Text txtLives;
	public Text txtMoney;
	public Text txtWaveComplete;	//GUI related to completing a wave
	public Text txtMoneyAwarded;
	public Text txtMoneyGained;
	public Text txtWave;
	public Text txtWinWave;
	public Text txtWinScore;
	public Text txtLoseScore;
	public Text txtLoseWave;
	public Text[] TxtTooltip;

	public int Currentwave =1;
	//int wave;

	public string sceneName;

	public Button btnNextWave;
	public Button[] btnTowers;

	public GameObject Restart;
	public GameObject nextLevel;
	public GameObject settingsPause;
	public GameObject panel;
	public GameObject constructParticle;
	public GameObject CongratzPanel;
	GameObject menuAnim;

	//Array of towers
	public TowerData[] towerList;
	public SpawnList spawn;
	//All of the game states
	public enum ControllerState {None, Purchase, Upgrade}
	
	private ControllerState currentState = ControllerState.None;

	//current tile selected
	private Tile currentTile = null;
	private Turret currentTurret = null;
	private Turret Tower = null;
	//public Turret turret;

	public Sprite PlaySprite;
	public Sprite PauseSprite;
	public Sprite MuteSprite;
	public Sprite UnMuteSprite;

	bool continues;
	bool paused;
	bool muted;
	public bool upgrading;
	public bool tooltipbool = false;
	bool levelThree;


	Animator anim;
	public RuntimeAnimatorController ToolTipAnimIN;
	public RuntimeAnimatorController ToolTipAnimOUT;


	void Awake () 
	{

		GameObject LevelThree = GameObject.Find ("Dumpster");
		if(LevelThree == true)
		{
			levelThree = true;
		}
		GameObject LevelOne = GameObject.FindWithTag ("XRay");
		if(LevelOne == true)
		{
			PlayerData.Instance.Lives = 10;
		}
		Time.timeScale = 1;
		upgrading = false;
		Currentwave = GameObject.Find ("SpawnController").GetComponent<Spawner>().currentWave;
		spawn = GameObject.Find ("SpawnController").GetComponent<Spawner>().spawnData;
		Currentwave +=1;

		PlayerData.Instance.Money = 300;
		anim = GameObject.FindWithTag ("ToolTip").GetComponent<Animator>();
		ResetTowerList ();
	}

	public void Update()
	{
		
		//constantly update the text to show how many lives are left
		txtLives.text = PlayerData.Instance.Lives.ToString();
		txtMoney.text = PlayerData.Instance.Money.ToString();
		txtWave.text = Currentwave.ToString ();
	}

	//Activate build menu, show tower buttons and store the tile
	public void BuildMenu(Tile tile)
	{
		upgrading = false;
		currentState = ControllerState.Purchase;
		currentTile = tile;
		//activate our buttons
		foreach(TowerData td in towerList)
		{
			td.btnPurchase.enabled = true;
			td.btnPurchase.gameObject.SetActive(true);
			td.btnPurchase.GetComponent<Image>().sprite   = td.Images;

		}
	}
	
	
	
	public void OnPurchase (int index)
	{
	
		if (currentState == ControllerState.Purchase)
		{
			if (currentTile == null)
				return;
			if (!currentTile.CanBuild ())
				return;
			
			//Can we afford it?
			if(PlayerData.Instance.Money >= towerList[index].Cost)
			{
				Debug.Log (PlayerData.Instance.Money);
				Debug.Log (towerList[index].Cost);
				PlayerData.Instance.Money -= towerList[index].Cost;
				//Build the tower on the tile
				currentTile.Build(towerList[index].TowerPrefab);
				currentTile = null;
			}

		}else if (currentState == ControllerState.Upgrade)
		{
			if(currentTurret == null)
				return;
			if (PlayerData.Instance.Money >= Tower.Upgrades[index].Cost)
			{
				PlayerData.Instance.Money -= Tower.Upgrades[index].Cost;
				StartCoroutine (Turretupgrade(index, Tower));
				currentTurret.gameObject.SetActive (false);
				upgrading = false;

			}
		}
		for (int i = 0; i < btnTowers.Length; ++i) {
			if (i < towerList.Length) {
				btnTowers [i].enabled = false;
			}
		}
		anim.runtimeAnimatorController = ToolTipAnimOUT;
		menuAnim = GameObject.FindWithTag ("Menu");
		menuAnim.GetComponent<MenuAnimation>().buttonBool = true;
		menuAnim.GetComponent<MenuAnimation>().PlayAnim();
	}

	public void ShowToolTip(int index, Turret turret)
	{

				if(!upgrading){
					TxtTooltip[0].GetComponent<Text>().text = towerList[index].Cost.ToString();
					TxtTooltip[1].GetComponent<Text>().text = towerList[index].Damage.ToString();
					TxtTooltip[2].GetComponent<Text>().text = towerList[index].Desc.ToString();
					Debug.Log ("Tool Tip");
				
				}
			
				if(upgrading)
				{
					//Debug.Log (currentTurret.Upgrades[index].ToString());
			TxtTooltip[0].GetComponent<Text>().text = turret.Upgrades[index].Cost.ToString();
			TxtTooltip[1].GetComponent<Text>().text = turret.Upgrades[index].Damage.ToString();
			TxtTooltip[2].GetComponent<Text>().text = turret.Upgrades[index].Desc.ToString();
							
				}
			


	}

	public void PurchaseTower(int index)
	{


		Tower = currentTurret;
		int Index = index;
		tooltipbool = true;
		anim.runtimeAnimatorController = ToolTipAnimIN;
		ShowToolTip(Index, Tower);
		GameObject.Find ("Construct").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("Construct").GetComponent<Button>().onClick.AddListener (() => OnPurchase(index));
	}
	
	public IEnumerator Turretupgrade(int index, Turret buildTile)
	{
		GameObject tempTower = Tower.Upgrades [index].NewTowerPreFab;
		GameObject construction = (GameObject)Instantiate (constructParticle, buildTile.transform.position, currentTurret.Upgrades[index].SpawnPosition.rotation);
		yield return new WaitForSeconds (3);
		Debug.Log ("Spawn");
		Destroy (construction);
		Instantiate(tempTower, buildTile.transform.position, currentTurret.Upgrades[index].SpawnPosition.rotation);
			
	}
	
	private bool victoryWait;
	public bool VictoryWait
	{
		get {return victoryWait;}
	}
	
	public void CompleteWave(int bounty)
	{



		PlayerData.Instance.Score += bounty;
		//Debug.Log ("CURRENTWAVE = " + wave);
		Debug.Log ("WAVE = " + spawn.Wave.Length);

		if (Currentwave >= spawn.Wave.Length) {
		
			if(NextScene.continuesBool){
				Restart.SetActive (false);
				nextLevel.SetActive (true);
			} else{
				Restart.SetActive (true);
				nextLevel.SetActive (false);
			}
			txtWinScore.text = PlayerData.Instance.Score.ToString ();
			txtWinWave.text = Currentwave.ToString ();

			CongratzPanel.gameObject.SetActive(true);
			Debug.Log ("congratz");
			Time.timeScale = 0;
		} else {
			panel.gameObject.SetActive (true);
			PlayerData.Instance.Money += bounty;
			victoryWait = true;
			txtMoneyGained.text = bounty.ToString ();
		}
	}

	public void Wavecheck ()
	{
		Application.LoadLevel (sceneName);
	}
	
	public void NextWave()
	{
		victoryWait = false;
		panel.gameObject.SetActive (false);
		Currentwave = GameObject.Find ("SpawnController").GetComponent<Spawner>().currentWave;
		Currentwave +=1;
	}
	
	public void ShowUpgrades(Turret turret)
	{
		currentTurret = turret;
		currentState = ControllerState.Upgrade;
		foreach(TowerData td in towerList)
		{
			td.btnPurchase.enabled = true;
			td.btnPurchase.gameObject.SetActive(true);
			td.btnPurchase.GetComponent<Image>().sprite   = td.Images;

		}
		for (int i = 0; i < btnTowers.Length; ++i)
		{
			if ( i < turret.Upgrades.Length)
			{
				btnTowers [i].enabled = true;
				btnTowers[i].GetComponent<Image>().sprite = turret.Upgrades[i].UpgradeImage;
			}
			else
			{
				btnTowers[i].gameObject.SetActive (false);
			}
		}
	}
	
	public void ResetTowerList()
	{

		for (int i = 0; i < btnTowers.Length; ++i)
		{
			if ( i < towerList.Length)
			{
				btnTowers [i].enabled = true;
				btnTowers[i].GetComponent<Image>().sprite  = towerList[i].Images;
			}
			else
			{
				btnTowers[i].gameObject.SetActive (false);
			}
		}
	}

	public void MenuPause()
	{
		if (!paused) 
		{
			GameObject.Find ("Settings Btn").GetComponent<Button>().enabled = false;
			foreach(TowerData td in towerList)
			{
				td.btnPurchase.enabled = false;
			
				
			}
			anim.runtimeAnimatorController = ToolTipAnimOUT;
			settingsPause.SetActive (true);
			GameObject.FindWithTag ("PauseButton").GetComponent<Image> ().sprite = PlaySprite;

			StartCoroutine (pause ());

		}else 
		{
			foreach(TowerData td in towerList)
			{
				td.btnPurchase.enabled = true;
				
				
			}
			settingsPause.SetActive (false);
			Time.timeScale = 1;
			GameObject.FindWithTag ("PauseButton").GetComponent<Image> ().sprite = PauseSprite;
			paused = false;
		}
	}
	
	public void Pause()
	{

		if (!paused) 
		{
			GameObject.FindWithTag ("PauseButton").GetComponent<Button>().enabled = false;
			foreach(TowerData td in towerList)
			{
				td.btnPurchase.enabled = false;
				
				
			}
			anim.runtimeAnimatorController = ToolTipAnimOUT;
			GameObject.FindWithTag ("PauseButton").GetComponent<Image> ().sprite = PlaySprite;

			StartCoroutine (pause ());

		} else 
		{
			foreach(TowerData td in towerList)
			{
				td.btnPurchase.enabled = true;
				
				
			}
			settingsPause.SetActive (false);
			Time.timeScale = 1;
			GameObject.FindWithTag ("PauseButton").GetComponent<Image> ().sprite = PauseSprite;
			paused = false;
		}
	}
	public IEnumerator pause()
	{
		yield return new WaitForSeconds (0.45f);
		Time.timeScale = 0;
		paused = true;
		GameObject.FindWithTag ("PauseButton").GetComponent<Button>().enabled = true;
		GameObject.Find ("Settings Btn").GetComponent<Button>().enabled = true;

	}
	public void Mute()
	{
		
		if (!muted) 
		{
			GameObject.FindWithTag ("MuteButton").GetComponent<Image> ().sprite = UnMuteSprite;
			muted = true;
		} else
		{
			GameObject.FindWithTag ("MuteButton").GetComponent<Image> ().sprite = MuteSprite;
			muted = false;
		}
	}
}

[System.Serializable]
public class TowerData
{
	public string Name;
	public string Damage;
	public string Desc;
	public Sprite Images;
	public int Cost;
	public Button btnPurchase;
	public GameObject TowerPrefab;
}
