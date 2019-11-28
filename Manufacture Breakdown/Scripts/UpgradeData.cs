using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UpgradeData 
{
	public string Name;
	public string Damage;
	public string Desc;
	public Sprite UpgradeImage;
	public int Cost;
	public GameObject NewTowerPreFab;
	public Transform SpawnPosition;
}