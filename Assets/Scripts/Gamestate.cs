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

	/*public bool checkMap;
	public MapType mapType;*/

	public Dictionary<string, Item> allItems;
	public Dictionary<string, Monster> allMonsters;
	public Dictionary<string, Skill> allSkills;

	public List<Player> players;
	public List<PlayerData> playersData;

	void OnGUI() {
		/*if (GUI.Button (new Rect (930, 30, 150, 30), "Add potion")) { // DEBUG
			Singleton.Instance.inventory.addItem("Potion", 1);
		}*/
		if (GUI.Button (new Rect (730, 30, 150, 30), "Level Up")) { // DEBUG
			Mage mage = findPlayer("Mage").GetComponent<Mage>();
			mage.levelUp();

			/*for(int i = 0; i < 10; i++){
				mage.levelUp();
			}

			mage.addSkill("Fireball");
			mage.skillUp("Fireball");
			mage.skillUp("Fireball");
			mage.skillUp("Fireball");
			mage.skillUp("Fireball");*/
		}

		if (GUI.Button (new Rect (930, 30, 150, 30), "Save Game")) { // DEBUG
			SaveManager.Instance.save();
		}
	}

	void Awake() {
		if(instance == null){
			DontDestroyOnLoad(gameObject);
			instance = this;
			
			//Singleton.Instance.Instance.initialize();
			//SaveManager.Instance.initialize();
		}
		else if (instance != this){
			Destroy(gameObject);
		}
	}

	void Start() {
		this.initialize();

		/*if(SaveManager.Instance.autoLoad()){

		}
		else{
			loadStartGame();
		}*/

		//loadDebugData();
	}

	void Update() {
		/*if(this.map != null && this.map.mapName.Equals("Forest") && !checkMap){
			mage = GameObject.FindGameObjectWithTag("Mage").GetComponent<Mage>();
			checkMap = true;
			mapType = MapType.PLATFORM;
		}*/
	}

	public void initialize(){
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
		this.rogue.populate(data);
		addPlayer(this.rogue);
	}

	public void setMage(Mage mage, PlayerData data){
		this.mage = mage;
		setMage(data);
	}

	/*public void setMage(PlayerData data){
		this.mage.populate(data);
		addPlayer(this.mage);
	}*/

	public void setMage(PlayerData data){
		//this.mage.populate(data);
		//addPlayer(this.mage);
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
			//BattleData.addPlayer(data);
		}
		else{
			Debug.Log ("Player Data Exists");
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
		/*foreach(PlayerData d in this.playersData){
			if(d.characterName.Equals(name)){
				return d;
			}
		}*/
		/*switch(name){
		case "Mage":
			return findPlayer("Mage").GetComponent<Mage>().getData();
		case "Knight":
			return findPlayer("Knight").GetComponent<Knight>().getData();
		case "Rogue":
			return findPlayer("Rogue").GetComponent<Rogue>().getData();
//			break;
		default:
			return null;
		}*/
		return findPlayer(name).GetComponent<Player>().getData();

		//return null;
	}

	public static GameObject findPlayer(string tag){
		return GameObject.FindGameObjectWithTag(tag);
	}

	public void changeFromMapToBattle(string battleMapName){
		//AutoSaveMap();
		//Application.LoadLevel(battleMapName);
	}

	// Sets the instance to null when the application quits 
	public void OnApplicationQuit() { 
		instance = null; 
		//SaveManager.Instance.autoSave();
	}

	void OnLevelWasLoaded(int level) {
		if (level != 0){
			if(playersData != null && playersData.Count > 0){
				foreach(PlayerData data in playersData){
					findPlayer(data.characterName).GetComponent<Player>().populate(data);
				}

				if(Singleton.Instance.playerPositionInMap.x != 0 && Singleton.Instance.playerPositionInMap.y != 0){
					findPlayer("Mage").transform.position = Singleton.Instance.playerPositionInMap;
				}
			}
		}
	}

	/*public enum MapType{
		PLATFORM,
		BATTLE
	}*/
}
