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

	void OnGUI() {
		if (GUI.Button (new Rect (730, 30, 150, 30), "Level Up")) { // DEBUG
			Mage mage = findPlayer("Mage").GetComponent<Mage>();
			mage.levelUp();
		}

		if (GUI.Button (new Rect (930, 30, 150, 30), "Save Game")) { // DEBUG
			SaveManager.Instance.save();
		}
	}

	void Awake() {
		if(instance == null){
			DontDestroyOnLoad(gameObject);
			instance = this;
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
		return findPlayer(name).GetComponent<Player>().getData();
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
			if(arePlayersOnLevel()){
				foreach(PlayerData data in playersData){
					findPlayer(data.characterName).GetComponent<Player>().populate(data);
					findPlayer(data.characterName).GetComponent<Player>().addSkill("skill_name_fireball");
					findPlayer(data.characterName).GetComponent<Player>().addSkill("skill_name_fireball");
					findPlayer(data.characterName).GetComponent<Player>().addSkill("skill_name_fireball");
					findPlayer(data.characterName).GetComponent<Player>().addSkill("skill_name_fireball"); // DEBUG
				}

				if(!positionIsDefault()){
					findPlayer("Mage").transform.position = Singleton.Instance.playerPositionInMap;
				}
			}
		}
	}

	private bool arePlayersOnLevel(){
		return playersData != null && playersData.Count > 0;
	}

	private bool positionIsDefault(){ // (0, 0)
		return Singleton.Instance.playerPositionInMap.x == 0 && Singleton.Instance.playerPositionInMap.y == 0;
	}

	/*public enum MapType{
		PLATFORM,
		BATTLE
	}*/
}
