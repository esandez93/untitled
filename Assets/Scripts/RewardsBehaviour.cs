using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RewardsBehaviour : MonoBehaviour {

	public BattleResults battleResults;

	public int playersNumber;

	public string player1Name;
	public string player2Name;
	public string player3Name;

	//PLAYER 1
	private GameObject player1Frame;
	private Image player1Portrait;
	private Image player1Exp;
	private Text player1CurrentExp;
	private Text player1GivenExp;
	private Text player1LevelUp;

	//PLAYER 2
	private GameObject player2Frame;
	private Image player2Portrait;
	private Image player2Exp;
	private Text player2CurrentExp;
	private Text player2GivenExp;
	private Text player2LevelUp;

	//PLAYER 3
	private GameObject player3Frame;
	private Image player3Portrait;
	private Image player3Exp;
	private Text player3CurrentExp;
	private Text player3GivenExp;
	private Text player3LevelUp;

	void Start () {
		initialize();
	}

	public void initialize(){
		battleResults = Singleton.Instance.lastBattleResults;

		playersNumber = battleResults.getNumberOfPlayers();

		Transform transform = GameObject.FindGameObjectWithTag("RewardPanel").transform;

		//PLAYER 1
		player1Frame = transform.FindChild("Player1Frame").gameObject;
		player1Portrait = transform.FindChild("Player1Frame").FindChild("Portrait").GetComponent<Image>();
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
			player2Exp = transform.FindChild("Player2Frame").FindChild("ExpBar").GetComponent<Image>();
			player2CurrentExp = transform.FindChild("Player2Frame").FindChild("CurrentExp").GetComponent<Text>();
			player2GivenExp = transform.FindChild("Player2Frame").FindChild("GivenExp").GetComponent<Text>();
			player2LevelUp = transform.FindChild("Player2Frame").FindChild("LevelUpText").GetComponent<Text>();

			initializePlayer2();

			if(playersNumber > 2){

				//PLAYER 3				
				player3Portrait = transform.FindChild("Player3Frame").FindChild("Portrait").GetComponent<Image>();
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
		
	}

	private void initializePlayer1(){ 
		Gamestate.instance.findPlayer("player1Name"); // MIRAR Y CONTINUAR POR AQUÍ !!!!!!!

		player1Name = battleResults.players[0];
		PlayerData data = Gamestate.instance.getPlayerData(player1Name);

		player1Portrait.sprite = Resources.Load <Sprite> ("Portraits/" + player1Name + "Portrait");
		player1Exp.fillAmount = (data.exp / data.expForNextLevel);
		player1CurrentExp.text = "(" + data.exp + "/" + data.expForNextLevel + ")";
		player1GivenExp.text = battleResults.gainedExp[data.characterName].ToString();
		player1LevelUp.text = "";
	}

	private void initializePlayer2(){	
		player2Name = battleResults.players[1];	
		PlayerData data = Gamestate.instance.getPlayerData(player2Name);

		player2Portrait.sprite = Resources.Load <Sprite> ("Portraits/" + player2Name + "Portrait");
		player2Exp.fillAmount = (data.exp / data.expForNextLevel);
		player2CurrentExp.text = "(" + data.exp + "/" + data.expForNextLevel + ")";
		player2GivenExp.text = battleResults.gainedExp[data.characterName].ToString();
		player2LevelUp.text = "";
	}

	private void initializePlayer3(){
		player3Name = battleResults.players[2];
		PlayerData data = Gamestate.instance.getPlayerData(player3Name);

		player3Portrait.sprite = Resources.Load <Sprite> ("Portraits/" + player3Name + "Portrait");
		player3Exp.fillAmount = (data.exp / data.expForNextLevel);
		player3CurrentExp.text = "(" + data.exp + "/" + data.expForNextLevel + ")";
		player3GivenExp.text = battleResults.gainedExp[data.characterName].ToString();
		player3LevelUp.text = "";
	}

	private void fillPlayer1(){
		Player player

		//player1Portrait.sprite = Resources.Load <Sprite> ("Portraits/" + data.characterName + "Portrait");
		player1Exp.fillAmount = (data.exp / data.expForNextLevel);
		player1CurrentExp.text = "(" + data.exp + "/" + data.expForNextLevel + ")";
		player1GivenExp.text = battleResults.gainedExp[data.characterName].ToString();
		player1LevelUp.text = "";
	}
	
	void Update () {
		
	}
}
