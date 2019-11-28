public class PlayerData
{
	//Create a singleton (can only have one instance)
	private static PlayerData Self;
	
	public  int Money { get; set; }
	public  int Lives { get; set; }
	public  int Score { get; set; }

	private PlayerData()
	{
		Money = 300;
		Lives = 10;
	}

	public static PlayerData Instance
	{	
		get {
			if (Self == null)
				Self = new PlayerData();
			
			return Self;
		}
	}
}
