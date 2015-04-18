using UnityEngine;
using System.Collections; 
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Gamestate : MonoBehaviour {	
	public static Gamestate instance;
	public Knight knight;
	public Rogue rogue;
	public Mage mage;
	public MapInfo map;
	public List<Monster> monstersInBattle = new List<Monster>();

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

	void OnGUI() {
		/*if (GUI.Button (new Rect (730, 30, 150, 30), "Level Up")) { // DEBUG
			Mage mage = findPlayer("Mage").GetComponent<Mage>();
			mage.levelUp();
		}

		if (GUI.Button (new Rect (930, 30, 150, 30), "Save Game")) { // DEBUG
			SaveManager.Instance.save();
		}*/

		if (GUI.Button (new Rect (930, 30, 150, 30), "Craft")) { // DEBUG
			Singleton.Instance.inventory.addItem("item_name_potion", 2);
			CraftManager.Instance.craft("item_name_potion", "item_name_potion");
			Singleton.Instance.inventory.addItem("item_name_grenade", 2);
		}
	}

	void Awake() {
		if(instance == null){
			DontDestroyOnLoad(gameObject);
			instance = this;
			findPlayer("Mage").GetComponent<PlayerBehaviour>().enabled = false;
		}
		else if (instance != this){
			Destroy(gameObject);
		}
	}

	void Start() {
		this.initialize();
	}

	void Update() {

	}

	public void initialize(){
		OptionsManager.Instance.initialize();
		PauseMenuManager.Instance.hideCanvas();
		enemiesToDisable = new List<string>();
		openedChests = new List<string>();
		//checkMap = false;
		//Singleton.Instance.Instance.initialize();

		//mage = GameObject.FindWithTag("Mage").GetComponent<Mage>();
		//map = Singleton.Instance.allMaps["Forest"];
		//players = new List<Player>();
		
		//players.Add(mage);
		
		//BattleData.map = map;
		//BattleData.players = players;
	}

	private void loadDebugData(){
		
		//knight = GameObject.FindWithTag("Knight").GetComponent<Knight>();
		//rogue = GameObject.FindWithTag("Rogue").GetComponent<Rogue>();
		mage = GameObject.FindWithTag("Mage").GetComponent<Mage>();
		map = Singleton.Instance.allMaps["Forest"];
		players = new List<Player>();
		
		//Singleton.Instance.inventory.addItem("Potion", 20);
		//mage.addAlteredStatus();
		
		//players.Add(knight);
		//players.Add(rogue);
		//mage.addSkill("Fireball", Singleton.Instance.allSkills["Fireball"]);	
		
		mage.levelUp();
		mage.levelUp();
		
		//mage.addSkill("Impale");
		//mage.addSkill("Bulwark");
		//mage.addSkill("Reaper");
		
		mage.addSkill("Fireball");
		mage.skillUp("Fireball");
		mage.skillUp("Fireball");
		
		players.Add(mage);
		
		//addMonsterToMap(map.getMonster().name, map);
		//map.addMonster();
		
		BattleData.map = map;
		BattleData.players = players;
	}

	public void addMonsterToMap(string monsterName, MapInfo map){
		MonsterInfo monster = Singleton.Instance.allMonsters[monsterName];/*GameObject.FindWithTag("Monster1").GetComponent<Monster>();
		monster.initializeMonster(Singleton.Instance.allMonsters["Wolf"]);*/
		map.addMonster(monster);
	}

	// Testing
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
		//this.knight.populate(data);
		//addPlayer(this.knight);

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
		if(players == null){
			players = new List<Player>();
		}

		if(!playerExist(player.name)){
			players.Add(player);
			BattleData.addPlayer(player);
		}
		else{
			Debug.Log ("Player Exists");
		}
	}

	private void addPlayerData(PlayerData data){
		if(playersData == null){
			playersData = new List<PlayerData>();
		}

		if(!playerDataExist(data.characterName)){
			playersData.Add(data);
		}
		else{
			/*foreach(PlayerData d in this.playersData){ // Si da error, usar index
				if(d.characterName.Equals(name)){
					d = data;
				}
			}*/
			for(int i = 0; i < this.playersData.Count; i++){
				if(playersData[i].characterName.Equals(data.characterName)){					
					playersData[i] = data;
				}
			}
		}
	}

	private bool playerExist(string name){
		foreach(Player p in this.players){
			if(p.name.Equals(name)){
				return true;
			}
		}
		
		return false;		
	}

	private bool playerDataExist(string name){//PlayerData data){
		foreach(PlayerData d in this.playersData){
			if(d.characterName.Equals(name)){
				return true;
			}
		}
		
		return false;		
	}

	public PlayerData getPlayerData(string name){
		PlayerData playerData = null;

		foreach(PlayerData p in playersData){
			if(p.characterName.Equals(name)){
				playerData = p;
			}
		}

		return playerData;
		//return findPlayer(name).GetComponent<Player>().getData();
	}

	private static GameObject findPlayer(string tag){
		return GameObject.FindGameObjectWithTag(tag);
	}

	public Player getPlayer(string name){
		Player player = null;

		switch(name){
			case "Mage":
				player = mage;
				break;
			case "Knight":
				player = knight;
				break;	
			case "Rogue":
				player = rogue;
				break;				
		}

		return player;
	}

	public void changeFromMapToBattle(string battleMapName){
		//AutoSaveMap();
		//Application.LoadLevel(battleMapName);
	}

	public void disableEnemy(GameObject enemy){
		disable = true;
		/*if(enemiesToDisable == null){
			enemiesToDisable = new List<string>();
		}*/

		enemiesToDisable.Add(enemy.name);
	}

	// Sets the instance to null when the application quits 
	public void OnApplicationQuit() { 
		instance = null; 
		//SaveManager.Instance.autoSave();
	}

	void OnLevelWasLoaded(int level) {
		pausable = false;
		currentLevel = level;

		if (isPlatformLevel()){
			setPlatformLevelConf();
		}
		else if(isBattleLevel()){
			setBattleLevelConf();
		}
	}

	private void setPlatformLevelConf(){
		if(disable){
			foreach(string enemy in enemiesToDisable){
				GameObject.FindGameObjectWithTag(enemy).SetActive(false);
			}
		}		

		pausable = true;
		destroyBattleManager();

		foreach(string chest in openedChests){
			GameObject.Find(chest).GetComponent<Chest>().setOpened();
		}

		setChests();		

		if(arePlayersOnLevel()){
			GameObject player;
			foreach(PlayerData data in playersData){
				player = findPlayer(data.characterName);

				player.GetComponent<Player>().populate(data);
				player.GetComponent<Player>().addSkill("skill_name_fireball");
				player.GetComponent<Player>().addSkill("skill_name_fireball");
				player.GetComponent<Player>().addSkill("skill_name_fireball");
				player.GetComponent<Player>().addSkill("skill_name_fireball"); // DEBUG
				player.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animations/"+data.characterName+"/Platform");
			}

			if(!positionIsDefault()){					
				findPlayer("Mage").transform.position = Singleton.Instance.playerPositionInMap;
				GameObject.FindWithTag("MainCamera").transform.position = new Vector3(Singleton.Instance.playerPositionInMap.x, Singleton.Instance.playerPositionInMap.y, -1);
			}
		}
	}

	private List<Chest> getChests(){
		GameObject[] chestsGos = GameObject.FindGameObjectsWithTag("Chest");
		List<Chest> chests = new List<Chest>();

		foreach(GameObject chest in chestsGos){
			chests.Add(chest.GetComponent<Chest>());
		}

		return chests;
	}

	private void setChests(){
		List<Chest> chests = getChests();

		foreach(Chest chest in chests){
			chest.setContent(Singleton.Instance.getChestContent(chest.getId()));
		}
	}

	private void setBattleLevelConf(){
		BattleManager bm = BattleManager.Instance;

		if(arePlayersOnLevel()){
			GameObject player;
			foreach(PlayerData data in playersData){
				player = findPlayer(data.characterName);
				player.GetComponent<PlayerBehaviour>().enabled = true;
				player.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animations/"+data.characterName+"/Battle");
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

		if(battleManager != null){
			GameObject.Destroy(battleManager, 0.0f);
		}
	}

	private bool arePlayersOnLevel(){
		return playersData != null && playersData.Count > 0;
	}

	private bool positionIsDefault(){ // (0, 0)
		return Singleton.Instance.playerPositionInMap.x == 0 && Singleton.Instance.playerPositionInMap.y == 0;
	}
}
