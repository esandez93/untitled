using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RewardsBehaviour : MonoBehaviour {

	public BattleResults battleResults;

	public int playersNumber;

	/*public string player1Name;
	public string player2Name;
	public string player3Name;*/

	/*
		Hacer struct con las variables y crear un array de 3. Así se puede acceder sin repetir código.
	*/

	private struct DataBundle {
		public Player player;
		public GameObject playerFrame;
		public Image playerPortrait;
		public Text playerName;
		public Text playerCurrentLevel;
		public Image playerExp;
		public Text playerCurrentExp;
		public Text playerGivenExp;
		public Text playerLevelUp;
	}

	private DataBundle[] playersBundle;

	/*//PLAYER 1
	private Player player1;
	private GameObject player1Frame;
	private Image player1Portrait;
	private Text player1Name;
	private Text player1CurrentLevel;
	private Image player1Exp;
	private Text player1CurrentExp;
	private Text player1GivenExp;
	private Text player1LevelUp;

	//PLAYER 2
	private Player player2;
	private GameObject player2Frame;
	private Image player2Portrait;
	private Text player2Name;
	private Text player2CurrentLevel;
	private Image player2Exp;
	private Text player2CurrentExp;
	private Text player2GivenExp;
	private Text player2LevelUp;

	//PLAYER 3
	private Player player3;
	private GameObject player3Frame;
	private Image player3Portrait;
	private Text player3Name;
	private Text player3CurrentLevel;
	private Image player3Exp;
	private Text player3CurrentExp;
	private Text player3GivenExp;
	private Text player3LevelUp;*/

	private bool filled;
	private bool clickContinue = false;

	private List<Text> dropNames;
	private List<Text> dropQuantities;

	void Start () {
		initialize();
	}

	void Update() {
		if(!filled && clickContinue){
			for (int i = 0; i < playersNumber; i++)
				fillPlayer(i);

			filled = true;
		}		
	}

	public void initialize(){
		playersBundle = new DataBundle[3];

		battleResults = Singleton.Instance.lastBattleResults;

		playersNumber = battleResults.getNumberOfPlayers();

		Transform transform = GameObject.FindGameObjectWithTag("RewardPanel").transform;

		for (int i = 0; i < 3; i++) {
			playersBundle[i].playerFrame = transform.FindChild("Player"+(i+1)+"Frame").gameObject;
			playersBundle[i].playerPortrait = transform.FindChild("Player"+(i+1)+"Frame").FindChild("Portrait").GetComponent<Image>();
			playersBundle[i].playerName = transform.FindChild("Player"+(i+1)+"Frame").FindChild("PlayerName").GetComponent<Text>();
			playersBundle[i].playerCurrentLevel = transform.FindChild("Player"+(i+1)+"Frame").FindChild("CurrentLevel").GetComponent<Text>();
			playersBundle[i].playerExp = transform.FindChild("Player"+(i+1)+"Frame").FindChild("ExpBar").GetComponent<Image>();
			playersBundle[i].playerCurrentExp = transform.FindChild("Player"+(i+1)+"Frame").FindChild("CurrentExp").GetComponent<Text>();
			playersBundle[i].playerGivenExp = transform.FindChild("Player"+(i+1)+"Frame").FindChild("GivenExp").GetComponent<Text>();
			playersBundle[i].playerLevelUp = transform.FindChild("Player"+(i+1)+"Frame").FindChild("LevelUpText").GetComponent<Text>();

			if (i < playersNumber)
				initializePlayer(i);
			else
				playersBundle[i].playerFrame.SetActive(false);
		}

		//DROPS
		dropNames = new List<Text>();
		dropQuantities = new List<Text>();

		for(int i = 1; i < 8; i++){
			dropNames.Add(transform.FindChild("DropFrame").FindChild("Item"+i).FindChild("ItemName").GetComponent<Text>());
			dropQuantities.Add(transform.FindChild("DropFrame").FindChild("Item"+i).FindChild("ItemQuantity").GetComponent<Text>());
		}

		initializeDrops();

		filled = false;		
	}

	private void initializePlayer(int numPlayer) {
		playersBundle[numPlayer].player = battleResults.getPlayer(numPlayer+1);

		playersBundle[numPlayer].playerName.text = playersBundle[numPlayer].player.characterName;

		playersBundle[numPlayer].playerPortrait.sprite = Resources.Load <Sprite> ("Portraits/" + playersBundle[numPlayer].playerName.text + "Portrait");
		playersBundle[numPlayer].playerCurrentLevel.text = playersBundle[numPlayer].player.level.ToString();
		playersBundle[numPlayer].playerExp.fillAmount = (playersBundle[numPlayer].player.exp / playersBundle[numPlayer].player.expForNextLevel);
		playersBundle[numPlayer].playerCurrentExp.text = "(" + playersBundle[numPlayer].player.exp + "/" + playersBundle[numPlayer].player.expForNextLevel + ")";
		playersBundle[numPlayer].playerGivenExp.text = "+" + battleResults.getExp().ToString();
		playersBundle[numPlayer].playerLevelUp.text = "";
	}
	
	private void initializeDrops(){
		for(int i = 0; i < 7; i++){
			dropNames[i].text = "";
			dropQuantities[i].text = "";
		}

		Dictionary<string,int> drops = battleResults.getDrops();
		int currentDrop = 0;

		foreach(KeyValuePair<string, int> entry in drops){
			dropNames[currentDrop].text = LanguageManager.Instance.getMenuText(entry.Key);
			dropQuantities[currentDrop].text = entry.Value.ToString();

			Singleton.Instance.inventory.addItem(entry.Key, entry.Value);

			currentDrop++;
		}
	}

	private void fillPlayer(int numPlayer) {
		bool leveledUp = playersBundle[numPlayer].player.giveExp(battleResults.getExp());
		Gamestate.instance.setPlayer(playersBundle[numPlayer].player);

		if(leveledUp)
			playersBundle[numPlayer].playerLevelUp.text = "Level Up!";		

		playersBundle[numPlayer].playerCurrentLevel.text = playersBundle[numPlayer].player.level.ToString();
		playersBundle[numPlayer].playerExp.fillAmount = (playersBundle[numPlayer].player.exp / playersBundle[numPlayer].player.expForNextLevel);
		playersBundle[numPlayer].playerCurrentExp.text = "(" + playersBundle[numPlayer].player.exp + "/" + playersBundle[numPlayer].player.expForNextLevel + ")";
	}
		
	public void clickedContinue(){
		clickContinue = true;		
	}

	public void goPlatform(){
		if(clickContinue)
			Application.LoadLevel("Forest");		
	}
}
