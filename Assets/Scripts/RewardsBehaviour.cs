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

	//PLAYER 1
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
	private Text player3LevelUp;

	private bool filled;
	private bool clickContinue = false;

	private List<Text> dropNames;
	private List<Text> dropQuantities;

	void Start () {
		initialize();
	}

	void Update() {
		if(!filled && clickContinue){
			fillPlayer1();

			if(playersNumber > 1){
				fillPlayer2();

				if(playersNumber > 2){
					fillPlayer3();
				}
			}			

			filled = true;
		}		
	}

	public void initialize(){
		battleResults = Singleton.Instance.lastBattleResults;

		playersNumber = battleResults.getNumberOfPlayers();

		Transform transform = GameObject.FindGameObjectWithTag("RewardPanel").transform;

		//PLAYER 1
		player1Frame = transform.FindChild("Player1Frame").gameObject;
		player1Portrait = transform.FindChild("Player1Frame").FindChild("Portrait").GetComponent<Image>();		
		player1CurrentLevel = transform.FindChild("Player1Frame").FindChild("CurrentLevel").GetComponent<Text>();
		player1Name = transform.FindChild("Player1Frame").FindChild("PlayerName").GetComponent<Text>();
		player1Exp = transform.FindChild("Player1Frame").FindChild("ExpBar").GetComponent<Image>();
		player1CurrentExp = transform.FindChild("Player1Frame").FindChild("CurrentExp").GetComponent<Text>();
		player1GivenExp = transform.FindChild("Player1Frame").FindChild("GivenExp").GetComponent<Text>();
		player1LevelUp = transform.FindChild("Player1Frame").FindChild("LevelUpText").GetComponent<Text>();

		initializePlayer1();

		player2Frame = transform.FindChild("Player2Frame").gameObject;
		player3Frame = transform.FindChild("Player3Frame").gameObject;

		if(playersNumber > 1){

			//PLAYER 2
			player2Portrait = transform.FindChild("Player2Frame").FindChild("Portrait").GetComponent<Image>();		
			player2CurrentLevel = transform.FindChild("Player1Frame").FindChild("CurrentLevel").GetComponent<Text>();
			player2Name = transform.FindChild("Player1Frame").FindChild("PlayerName").GetComponent<Text>();
			player2Exp = transform.FindChild("Player2Frame").FindChild("ExpBar").GetComponent<Image>();
			player2CurrentExp = transform.FindChild("Player2Frame").FindChild("CurrentExp").GetComponent<Text>();
			player2GivenExp = transform.FindChild("Player2Frame").FindChild("GivenExp").GetComponent<Text>();
			player2LevelUp = transform.FindChild("Player2Frame").FindChild("LevelUpText").GetComponent<Text>();

			initializePlayer2();

			if(playersNumber > 2){

				//PLAYER 3				
				player3Portrait = transform.FindChild("Player3Frame").FindChild("Portrait").GetComponent<Image>();		
				player3CurrentLevel = transform.FindChild("Player1Frame").FindChild("CurrentLevel").GetComponent<Text>();
				player3Name = transform.FindChild("Player1Frame").FindChild("PlayerName").GetComponent<Text>();
				player3Exp = transform.FindChild("Player3Frame").FindChild("ExpBar").GetComponent<Image>();
				player3CurrentExp = transform.FindChild("Player3Frame").FindChild("CurrentExp").GetComponent<Text>();
				player3GivenExp = transform.FindChild("Player3Frame").FindChild("GivenExp").GetComponent<Text>();
				player3LevelUp = transform.FindChild("Player3Frame").FindChild("LevelUpText").GetComponent<Text>();

				initializePlayer3();
			}
			else{
				player3Frame.SetActive(false);
			}
		}
		else{
			player2Frame.SetActive(false);
			player3Frame.SetActive(false);
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

	private void initializePlayer1(){ 
		player1 = battleResults.getPlayer(1);

		player1Name.text = player1.characterName;

		player1Portrait.sprite = Resources.Load <Sprite> ("Portraits/" + player1Name.text + "Portrait");
		player1CurrentLevel.text = player1.level.ToString();
		player1Exp.fillAmount = (player1.exp / player1.expForNextLevel);
		player1CurrentExp.text = "(" + player1.exp + "/" + player1.expForNextLevel + ")";
		player1GivenExp.text = "+" + battleResults.getExp().ToString();
		player1LevelUp.text = "";
	}

	private void initializePlayer2(){				
		player2 = battleResults.getPlayer(2);

		player2Name.text = player2.characterName;

		player2Portrait.sprite = Resources.Load <Sprite> ("Portraits/" + player2Name.text + "Portrait");
		player2CurrentLevel.text = player2.level.ToString();
		player2Exp.fillAmount = (player2.exp / player2.expForNextLevel);
		player2CurrentExp.text = "(" + player2.exp + "/" + player2.expForNextLevel + ")";
		player2GivenExp.text = "+" + battleResults.getExp().ToString();
		player2LevelUp.text = "";
	}

	private void initializePlayer3(){		
		player3 = battleResults.getPlayer(3);

		player3Name.text = player3.characterName;

		player3Portrait.sprite = Resources.Load <Sprite> ("Portraits/" + player3Name.text + "Portrait");
		player3CurrentLevel.text = player3.level.ToString();
		player3Exp.fillAmount = (player3.exp / player3.expForNextLevel);
		player3CurrentExp.text = "(" + player3.exp + "/" + player3.expForNextLevel + ")";
		player3GivenExp.text = battleResults.getExp().ToString();
		player3LevelUp.text = "";
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

	private void fillPlayer1(){
		//Debug.Log(player1.ToString());
		bool leveledUp = player1.giveExp(battleResults.getExp());
		Gamestate.instance.setPlayer(player1);

		if(leveledUp){
			player1LevelUp.text = "Level Up!";
		}

		player1CurrentLevel.text = player1.level.ToString();
		player1Exp.fillAmount = (player1.exp / player1.expForNextLevel);
		player1CurrentExp.text = "(" + player1.exp + "/" + player1.expForNextLevel + ")";
	}

	private void fillPlayer2(){
		bool leveledUp = player2.giveExp(battleResults.getExp());
		//Gamestate.instance.setPlayer(player2);

		if(leveledUp){
			player2LevelUp.text = "Level Up!";
		}

		player2CurrentLevel.text = player2.level.ToString();
		player2Exp.fillAmount = (player2.exp / player2.expForNextLevel);
		player2CurrentExp.text = "(" + player2.exp + "/" + player2.expForNextLevel + ")";
	}

	private void fillPlayer3(){
		bool leveledUp = player3.giveExp(battleResults.getExp());
		//Gamestate.instance.setPlayer(player3);

		if(leveledUp){
			player3LevelUp.text = "Level Up!";
		}

		player3CurrentLevel.text = player3.level.ToString();
		player3Exp.fillAmount = (player3.exp / player3.expForNextLevel);
		player3CurrentExp.text = "(" + player3.exp + "/" + player3.expForNextLevel + ")";
	}

	public void clickedContinue(){
		clickContinue = true;		
	}

	public void goPlatform(){
		if(clickContinue){
			Application.LoadLevel("Forest");
		}
	}
}
