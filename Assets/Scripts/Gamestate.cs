using UnityEngine;
using UnityEngine.UI;
using System.Collections; 
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class Gamestate : MonoBehaviour {	
	public static Gamestate instance;
	public Knight knight;
	public Rogue rogue;
	public Mage mage;
	public MapInfo map;
	public List<Monster> monstersInBattle = new List<Monster>();

	private GameObject messageCanvas;
	private Text messageText;

	public bool bossBattle = false;

	public Dictionary<string, Item> allItems;
	public Dictionary<string, Monster> allMonsters;
	public Dictionary<string, Skill> allSkills;

	public List<Player> players;
	public List<PlayerData> playersData;

	private bool pausable;

	private bool disable = false;
	private List<string> enemiesToDisable;
	public List<string> openedChests;

	private int currentLevel = -1;

	/*void OnGUI() {
		if (GUI.Button (new Rect (530, 30, 150, 30), "Give Potion")) { // DEBUG
			Singleton.Instance.inventory.addItem("item_name_potion", 1);
		}
		if (GUI.Button (new Rect (530, 30, 150, 30), "Level Up")) { // DEBUG
			Mage mage = findPlayer("Mage").GetComponent<Mage>();
			mage.addSkill("skill_name_fireball");
		}

		if (GUI.Button (new Rect (730, 30, 150, 30), "Save Game")) { // DEBUG
			SaveManager.Instance.save();
		}

		if (GUI.Button (new Rect (930, 30, 150, 30), "Craft")) { // DEBUG
			Singleton.Instance.inventory.addItem("item_name_potion", 2);
			CraftManager.Instance.craft("item_name_potion", "item_name_potion");
			Singleton.Instance.inventory.addItem("item_name_grenade", 2);
			Singleton.Instance.inventory.addItem("item_name_gunpowder", 2);
		}
	}*/

	void Awake() {
		if(instance == null){
			DontDestroyOnLoad(gameObject);
			instance = this;
			findPlayer("Strider").GetComponent<PlayerBehaviour>().enabled = false;
		}
		else if (instance != this)
			Destroy(gameObject);		
	}

	void Start() {
		this.initialize();
	}

	public void initialize(){
		//OptionsManager.Instance.initialize();
		PauseMenuManager.Instance.hideCanvas();

		messageCanvas = GameObject.Find("Gamestate/MessageCanvas");
		messageText = GameObject.Find("Gamestate/MessageCanvas/Image/Text").GetComponent<Text>();

		messageCanvas.SetActive(false);

		enemiesToDisable = new List<string>();
		openedChests = new List<string>();

		Debug.Log("Gamestate initialized.");
	}

	public void addMonsterToMap(string monsterName, MapInfo map){
		MonsterInfo monster = Singleton.Instance.allMonsters[monsterName];
		map.addMonster(monster);
	}

	public Monster generateMonster(string monsterName){
		return allMonsters[monsterName];
	}

	public void setPlayer(Player player){
		addPlayerData(player.getData());
	}

	public void setKnight(Knight knight, PlayerData data){
		this.knight = knight;
		setKnight(data);
	}

	public void setKnight(PlayerData data){
		addPlayerData(data);
	}

	public void setRogue(Rogue rogue, PlayerData data){
		this.rogue = rogue;
		setRogue(data);
	}

	public void setRogue(PlayerData data){
		addPlayerData(data);
	}

	public void setMage(Mage mage, PlayerData data){
		this.mage = mage;
		setMage(data);
	}

	public void setMage(PlayerData data){
		addPlayerData(data);
	}

	public void setMap(MapInfo map){
		this.map = map;
		BattleData.map = map;
	}

	private void addPlayer(Player player){
		if(players == null)
			players = new List<Player>();		

		if(!playerExist(player.characterName)){
			players.Add(player);
			BattleData.addPlayer(player);
		}
		else
			Debug.Log ("Player Exists");		
	}

	public void addPlayerData(PlayerData data){
		if(playersData == null)
			playersData = new List<PlayerData>();
		
		if(players == null)
			players = new List<Player>();		

		Player p = new Player();		
		p.populate(data);

		if(!playerDataExist(data.characterName))
			playersData.Add(data);		
		else{
			for(int i = 0; i < this.playersData.Count; i++){
				if(playersData[i].characterName.Equals(data.characterName))				
					playersData[i] = data;				
			}
		}
	}

	private bool playerExist(string name){
		foreach(Player p in this.players){
			if(p.characterName.Equals(name))
				return true;			
		}
		
		return false;		
	}

	private bool playerDataExist(string name){
		foreach(PlayerData d in this.playersData){
			if(d.characterName.Equals(name))
				return true;			
		}
		
		return false;		
	}

	public PlayerData getPlayerData(string name){
		PlayerData playerData = null;

		foreach(PlayerData p in playersData){
			if(p.characterName.Equals(name))
				playerData = p;			
		}

		return playerData;
	}

	public static GameObject findPlayer(string tag){
		foreach(GameObject go in GameObject.FindGameObjectsWithTag(tag)) {
			if (go.layer == 11)
				return go;
		}

		return null;
	}

	public Player getPlayer(string name){
		Player player = null;

		switch(name){
			case "Ki":
				player = mage;
				break;
			case "Gilgamesh":
				player = knight;
				break;	
			case "Strider":
				player = rogue;
				break;				
		}

		return player;
	}

	public void disableEnemy(GameObject enemy){
		disable = true;

		enemiesToDisable.Add(enemy.name);
	}

	public void showMessage(string message) {
		StartCoroutine(showMessage(message, 2));
	}

	IEnumerator showMessage(string message, int time) {
		messageCanvas.SetActive(true);
		messageText.text = message;

		yield return new WaitForSeconds(time);

		messageText.text = "";
		messageCanvas.SetActive(false);
	}

	// Sets the instance to null when the application quits 
	public void OnApplicationQuit() { 
		instance = null; 
	}

	void OnLevelWasLoaded(int level) {
		pausable = false;
		currentLevel = level;

		if (isPlatformLevel())
			setPlatformLevelConf();		
		else if(isBattleLevel())
			setBattleLevelConf();		
	}

	private void setPlatformLevelConf(){
		if(disable){
			foreach(string enemy in enemiesToDisable)
				GameObject.FindGameObjectWithTag(enemy).SetActive(false);			
		}

		bossBattle = false;
		pausable = true;
		destroyBattleManager();

		foreach(string chest in openedChests)
			GameObject.Find(chest).GetComponent<Chest>().setOpened();		

		setChests();		

		if(arePlayersOnLevel()){
			GameObject player;
			foreach(PlayerData data in playersData){
				player = findPlayer(data.characterName);
				if(player != null && data != null) {
					try{
						if(data.characterName.Equals("Ki")) {
							data.skills.Add("skill_name_fireball", Singleton.Instance.allSkills["skill_name_fireball"]);
							data.skills["skill_name_fireball"].levelUp();
							data.skills["skill_name_fireball"].levelUp();
							data.skillPoints = 5;
							//data.agi += 250;
						}
						Singleton.Instance.inventory.addItem("item_name_potion", 1);
						Singleton.Instance.inventory.addItem("item_name_grenade", 1);
					}
					catch(Exception e) {
					}

					player.AddComponent<Player>();
					player.GetComponent<Player>().populate(data);
					player.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animations/"+data.characterName+"/Platform");
				}
			}

			if(!positionIsDefault()){
				Debug.Log("POSITION NOT DEFAULT");	
				findPlayer("Strider").transform.position = Singleton.Instance.playerPositionInMap;
				GameObject.FindWithTag("MainCamera").transform.position = new Vector3(Singleton.Instance.playerPositionInMap.x, Singleton.Instance.playerPositionInMap.y, -1);
			}
		}
	}

	private List<Chest> getChests(){
		GameObject[] chestsGos = GameObject.FindGameObjectsWithTag("Chest");
		List<Chest> chests = new List<Chest>();

		foreach(GameObject chest in chestsGos)
			chests.Add(chest.GetComponent<Chest>());		

		return chests;
	}

	private void setChests(){
		List<Chest> chests = getChests();

		foreach(Chest chest in chests)
			chest.setContent(Singleton.Instance.getChestContent(chest.getId()));		
	}

	private void setBattleLevelConf(){
		BattleManager bm = BattleManager.Instance;

		if(arePlayersOnLevel()){
			GameObject player;
			foreach(PlayerData data in playersData){
				player = findPlayer(data.characterName);
				if(player != null) {					
					player.GetComponent<PlayerBehaviour>().enabled = true;
					player.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animations/"+data.characterName+"/Battle");
				}
			}
		}
	}

	public bool isPlatformLevel(){		
		return currentLevel == 1;
	}

	public bool isBattleLevel(){
		return currentLevel == 2;
	}

	public bool isPausable(){
		return pausable;
	}

	private void destroyBattleManager(){
		GameObject battleManager = GameObject.Find("BattleManager");

		if(battleManager != null)
			GameObject.Destroy(battleManager, 0.0f);		
	}

	private bool arePlayersOnLevel(){
		return playersData != null && playersData.Count > 0;
	}

	private bool positionIsDefault(){ // (0, 0)
		return Singleton.Instance.playerPositionInMap.x == 0 && Singleton.Instance.playerPositionInMap.y == 0;
	}

	public float[] getPlayerPosition(){
		Vector3 playerPosition = findPlayer("Strider").transform.position;
		float[] position = new float[3];

		position[0] = playerPosition.x;
		position[1] = playerPosition.y;
		position[2] = playerPosition.z;

		return position;
	}

	public void setPlayerPosition(float[] position){
		Vector3 playerPosition = new Vector3(position[0], position[1], position[2]);

		findPlayer("Strider").transform.position = Singleton.Instance.playerPositionInMap;
	}
}
