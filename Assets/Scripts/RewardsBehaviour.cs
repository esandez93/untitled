using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RewardsBehaviour : MonoBehaviour {

	public BattleResults battleResults;

	public int playersNumber;

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

	private bool filled;
	private bool clickContinue = false;

	private AudioSource ost;

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

		loadBackgroundMusic();

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
	
	public void loadBackgroundMusic() {
		ost = this.gameObject.GetComponent<AudioSource>();

		string songName = "Victory Fanfare";

		AudioClip audio = Resources.Load<AudioClip>("Sounds/OST/" + songName);
		if (audio != null){
			ost.minDistance = 10f;
			ost.maxDistance = 10f;
			ost.loop = true;
			ost.clip = audio;
			ost.Play();
		}
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
