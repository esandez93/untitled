using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {

	public int turn = 1;

	public MapInfo currentMap;

	public bool playerTurn = false;
	public bool showPlayerCommandsGUI = false;
	public bool ended = false;
	public bool attackObjective = false;
	public Player playerObjective = null;
	public int action = -1;

	public bool basicAttack = false;
	public bool skill = false;
	public bool item = false;
	public bool defend = false;
	public string skillName;
	public string itemName;
	public bool attackFinished = false;
	public bool attackStarted = false;
	public bool damageReceived = false;
	public bool deathFinished = false;

	public bool showPlayerStatGUI;
	public bool showMonsterStatGUI;

	public Image playerBackgroundGUI;
	public Image playerPortraitGUI;
	public Text playerNameGUI;
	public Text playerLevelGUI;
	public Image playerHealthGUI;
	public Image playerManaGUI;
	public Image playerExpGUI;
	public Image playerHealthFrameGUI;
	public Image playerManaFrameGUI;
	public Image playerExpFrameGUI;
	public Text playerHealthTextGUI;
	public Text playerManaTextGUI;
	public Text playerExpTextGUI;

	public Image enemyBackgroundGUI;
	public Image enemyPortraitGUI;
	public Text enemyNameGUI;
	public Text enemyLevelGUI;
	public Image enemyHealthGUI;
	public Image enemyManaGUI;
	public Image enemyHealthFrameGUI;
	public Image enemyManaFrameGUI;
	public Text enemyHealthTextGUI;
	public Text enemyManaTextGUI;

	GameObject[] playerDataBundle;
	Transform playerData;
	Transform playerHealth;
	Transform playerMana;

	GameObject[] enemyDataBundle;
	Transform enemyData;
	Transform enemyHealth;
	Transform enemyMana;

	public SpriteRenderer background;

	public Character currentObjective;

	public BattleStates currentState;
	public BattlePhases currentPhase;

	public int maxTurns;
	public Character currentCharacter;

	public Monster currentMonster;
	public Player currentPlayer;

	public int numPlayers = 0;
	public int numMonsters = 0;
	public int thisMonster = 0;

	public List<Monster> monstersInBattle = new List<Monster>();
	public List<Player> playersInBattle = new List<Player>();
	public List<Character> turns = new List<Character>();

	public BattleResults battleResults;
	
	public enum BattleStates{
		START,
		PLAYERTURN,
		ENEMYTURN,
		LOSE,
		WIN
	}
	
	public enum BattlePhases{
		NONE,
		AFFECT,
		CHOSEACTION,
		CHOSEOBJECTIVE,
		DOACTION,
		END
	}

	private bool initialized;
	private Gamestate gamestate;

	private static BattleManager instance = null;
	public static BattleManager Instance{
		get{
			if (instance == null){				
				//instance = (BattleManager)FindObjectOfType(typeof(BattleManager));
				instance = new GameObject("BattleManager").AddComponent<BattleManager>();
				instance.initialize();
				DontDestroyOnLoad(instance.gameObject);
			}
			return instance;
		}
	}

	public void initialize(){
		if(!initialized){
			instance.gamestate = Gamestate.instance;
			Debug.Log("BattleManager initialized.");
			instance.initialized = true;
		}
	}

	void OnGUI() {
		if(!instance.ended){
			checkGUI();

			/*if (GUI.Button (new Rect (530, 30, 150, 30), "Check GameState")) { // DEBUG
				Debug.Log ("State: " + currentState + ", Phase: " + currentPhase);
			}
			if (GUI.Button (new Rect (730, 30, 150, 30), "Add potion")) { // DEBUG
				Singleton.Instance.inventory.addItem("Potion", 1);
				//DisplayItems.repopulate();
			}*/
		}
	}

	void Start(){
		//GameObject.Find("BattleCanvas").GetComponent<AudioSource>().Play();
		AudioSource audio = Resources.Load<AudioSource>("Sounds/Otherworld.mp3");
		if (audio != null)
			audio.Play();

		instance.currentMap = gamestate.map;
		
		Transform transform = GameObject.FindGameObjectWithTag("BattleCanvas").transform;
		
		Transform playerDataTransform = transform.FindChild("PlayerData");
		instance.playerBackgroundGUI = playerDataTransform.GetComponent<Image>();
		instance.playerPortraitGUI = playerDataTransform.FindChild("PlayerPortrait").GetComponent<Image>();
		instance.playerNameGUI = playerDataTransform.FindChild("PlayerName").GetComponent<Text>();
		instance.playerLevelGUI = playerDataTransform.FindChild("PlayerLevel").GetComponent<Text>();
		instance.playerHealthGUI = playerDataTransform.FindChild("Health").FindChild("HealthBar").GetComponent<Image>();
		instance.playerManaGUI = playerDataTransform.FindChild("Mana").FindChild("ManaBar").GetComponent<Image>();
		instance.playerExpGUI = playerDataTransform.FindChild("Exp").FindChild("ExpBar").GetComponent<Image>();
		instance.playerHealthFrameGUI = playerDataTransform.FindChild("Health").FindChild("HealthBarFrame").GetComponent<Image>();
		instance.playerManaFrameGUI = playerDataTransform.FindChild("Mana").FindChild("ManaBarFrame").GetComponent<Image>();
		instance.playerExpFrameGUI = playerDataTransform.FindChild("Exp").FindChild("ExpBarFrame").GetComponent<Image>();
		instance.playerHealthTextGUI = playerDataTransform.FindChild("Health").FindChild("HealthText").GetComponent<Text>();
		instance.playerManaTextGUI = playerDataTransform.FindChild("Mana").FindChild("ManaText").GetComponent<Text>();
		instance.playerExpTextGUI = playerDataTransform.FindChild("Exp").FindChild("ExpText").GetComponent<Text>();
		
		Transform enemyDataTransform = transform.FindChild("EnemyData");
		instance.enemyBackgroundGUI = enemyDataTransform.GetComponent<Image>();
		instance.enemyPortraitGUI = enemyDataTransform.FindChild("EnemyPortrait").GetComponent<Image>();
		instance.enemyNameGUI = enemyDataTransform.FindChild("EnemyName").GetComponent<Text>();
		instance.enemyLevelGUI = enemyDataTransform.FindChild("EnemyLevel").GetComponent<Text>();
		instance.enemyHealthGUI = enemyDataTransform.FindChild("Health").FindChild("HealthBar").GetComponent<Image>();
		instance.enemyManaGUI = enemyDataTransform.FindChild("Mana").FindChild("ManaBar").GetComponent<Image>();
		instance.enemyHealthFrameGUI = enemyDataTransform.FindChild("Health").FindChild("HealthBarFrame").GetComponent<Image>();
		instance.enemyManaFrameGUI = enemyDataTransform.FindChild("Mana").FindChild("ManaBarFrame").GetComponent<Image>();
		instance.enemyHealthTextGUI = enemyDataTransform.FindChild("Health").FindChild("HealthText").GetComponent<Text>();
		instance.enemyManaTextGUI = enemyDataTransform.FindChild("Mana").FindChild("ManaText").GetComponent<Text>();

		instance.background = GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>();

		changeState(BattleStates.START);
		changePhase(BattlePhases.NONE);

		initializeVariables();

		setBackground();
		setPlayers();		
		setMonsters();
		LanguageManager.Instance.translateButtons();
	}

	void Update(){
		battleListener();

		switch(currentState){
		case BattleStates.START:
			generateBattle();
			break;
		case BattleStates.PLAYERTURN:
			instance.currentPlayer = (Player) instance.currentCharacter;
			setGUIPlayerInfo(instance.currentPlayer);

			if(instance.currentPhase == BattlePhases.AFFECT){
				instance.currentPlayer.startTurn();
				//Debug.Log("BEGIN TURN " + instance.currentPlayer.characterName);
			}

			if(instance.currentObjective == null){				
				hideGUIEnemyInfo();
			}
			else{
				if(instance.currentObjective.isPlayer()){
					setGUIPlayerInfo((Player) instance.currentObjective);
				}
				else{
					setGUIEnemyInfo((Monster) instance.currentObjective);
				}				
			}

			break;
		case BattleStates.ENEMYTURN:
			instance.currentMonster = (Monster) instance.currentCharacter;
			
			setGUIEnemyInfo(instance.currentMonster);
			if(instance.currentPhase == BattlePhases.AFFECT){
				instance.currentMonster.startTurn();
				hideGUIPlayerInfo();				
			}
			else if(instance.action == -1){				
				changePhase(BattlePhases.CHOSEACTION);		
				instance.action = instance.currentMonster.decideAction();
			}
			else if(instance.playerObjective == null){
				changePhase(BattlePhases.CHOSEOBJECTIVE);
				instance.playerObjective = (Player) instance.currentMonster.decideObjective();
			}
			else{
				setGUIPlayerInfo(instance.playerObjective);

				instance.currentMonster.doAction(instance.action, instance.playerObjective);
				
				if(instance.attackFinished){
					changePhase(BattlePhases.DOACTION);
					endTurn();
				}
			}
			break;
		case BattleStates.LOSE:
			Debug.Log ("Monsters WIN!");
			changePhase(BattlePhases.END);
			//CHANGE SCENE TO LOAD LAST SAVEGAME
			if(SaveManager.Instance.load()){
				Application.LoadLevel(gamestate.map.mapName);
			}			
			break;
		case BattleStates.WIN:
			Debug.Log ("Players WIN!");
			changePhase(BattlePhases.END);
			giveRewards();
			//SaveManager.Instance.saveBattleStatus();
			//CHANGE SCENE TO BATTLE REWARDS
			Application.LoadLevel("forestWinBattle");
			deleteInstance();
			break;
		}
	}

	public void generateBattle(){
		instance.numPlayers = instance.playersInBattle.Count;

		giveTurns();

		checkIfPlayerTurn();
	}

	public Monster getMonster(){
		if(instance.thisMonster < instance.monstersInBattle.Count)
			return instance.monstersInBattle[instance.thisMonster];		
		else
			return null;
	}

	//Por el momento, solo se tendra en cuenta la AGI a la hora de repartir turnos
	public void giveTurns(){
		int totalCharacters = instance.playersInBattle.Count + instance.monstersInBattle.Count;

		List<Character> listAux = new List<Character>();

		foreach(Player player in instance.playersInBattle){
			listAux.Add(player);
		}

		foreach(Monster monster in instance.monstersInBattle){
			listAux.Add(monster);
		}

		for(int i = 0; i < totalCharacters; i++){
			Character characterAux = null;
			float agi = 0;

			foreach(Character character in listAux){				
				if(character.agi > agi){
					agi = character.agi;
					characterAux = character;
				}
			}

			instance.turns.Add(characterAux);
			listAux.Remove(characterAux);
		}

		instance.maxTurns = instance.turns.Count;
	}

	public void endTurn(){
		if(instance.turn < instance.maxTurns)
			instance.turn++;
		else
			instance.turn = 1;		

		Debug.Log("END TURN " + instance.currentCharacter.characterName);

		cleanVariables(instance.currentCharacter);

		checkIfPlayerTurn();
	}

	public void checkIfPlayerTurn(){
		Debug.Log("TURNS COUNT = " + instance.turns.Count);
		Debug.Log("TURN = " + instance.turn);
		Debug.Log("MAX TURNS = " + instance.maxTurns);
		instance.currentCharacter = instance.turns[instance.turn - 1];

		if(instance.currentCharacter.isAlive()){
			if(!instance.ended){
				if(instance.currentCharacter.isPlayer()){
					hideGUIEnemyInfo();
					instance.playerTurn = true;
					instance.currentMonster = null;
					instance.currentPlayer = (Player) instance.currentCharacter;
					changeState(BattleStates.PLAYERTURN);
				}
				else{
					hideGUIPlayerInfo();
					instance.playerTurn = false;
					instance.showPlayerCommandsGUI = false;
					instance.currentMonster = (Monster) instance.currentCharacter;
					instance.currentPlayer = null;
					changeState(BattleStates.ENEMYTURN);
				}
			}

			changePhase(BattlePhases.AFFECT);
		}
		else{
			checkIfEnded();
			endTurn();
		}
	}

	public void setGUIPlayerInfo(Character player){
		instance.playerPortraitGUI.sprite = Resources.Load <Sprite> ("Portraits/" + player.name + "Portrait");
		instance.playerNameGUI.text = player.name;
		instance.playerLevelGUI.text = "Lv " + player.level.ToString();
		instance.playerHealthGUI.fillAmount = (player.currHP / player.maxHP);
		instance.playerManaGUI.fillAmount = (player.currMP / player.maxMP);
		//playerExpGUI.fillAmount = (player.exp / player.expForNextLevel);

		instance.playerBackgroundGUI.fillAmount = 1;
		instance.playerHealthFrameGUI.fillAmount = 1;
		instance.playerManaFrameGUI.fillAmount = 1;
		//playerExpFrameGUI.fillAmount = 1;
		instance.playerPortraitGUI.fillAmount = 1;

		instance.playerHealthTextGUI.text = "HP";
		instance.playerManaTextGUI.text = "MP";
		//playerExpTextGUI.text = "EXP";
	}

	public void setGUIEnemyInfo(Character enemy){//Monster monster){
		instance.enemyPortraitGUI.sprite = Resources.Load <Sprite> ("Portraits/" + enemy.name + "Portrait");
		instance.enemyNameGUI.text = LanguageManager.Instance.getMenuText(enemy.name);
		instance.enemyLevelGUI.text = "Lv " + enemy.level.ToString();
		instance.enemyHealthGUI.fillAmount = (enemy.currHP / enemy.maxHP);
		instance.enemyManaGUI.fillAmount = (enemy.currMP / enemy.maxMP);

		instance.enemyBackgroundGUI.fillAmount = 1;
		instance.enemyHealthFrameGUI.fillAmount = 1;
		instance.enemyManaFrameGUI.fillAmount = 1;
		instance.enemyPortraitGUI.fillAmount = 1;
		
		instance.enemyHealthTextGUI.text = "HP";
		instance.enemyManaTextGUI.text = "MP";
	}

	public void hideGUIPlayerInfo(){
		if(instance.playerDataBundle == null){
			instance.playerDataBundle = GameObject.FindGameObjectsWithTag("PlayerData"); 
		}
		
		foreach(GameObject gameObject in instance.playerDataBundle){
			foreach(Image image in gameObject.GetComponentsInChildren<Image>()){
				image.fillAmount = 0;
			}
			
			foreach(Text text in gameObject.GetComponentsInChildren<Text>()){
				text.text = "";
			}			
		}
	}

	public void hideGUIEnemyInfo(){
		/*if(enemyDataBundle == null){
			enemyDataBundle = GameObject.FindGameObjectsWithTag("EnemyData"); 
		}	*/

		GameObject[] asdf = GameObject.FindGameObjectsWithTag("EnemyData"); 

		foreach(GameObject gameObject in asdf){//enemyDataBundle){
			foreach(Image image in gameObject.GetComponentsInChildren<Image>()){
				image.fillAmount = 0;
			}
			
			foreach(Text text in gameObject.GetComponentsInChildren<Text>()){
				text.text = "";
			}
		}
	}

	public bool isPlayerTurn(){
		return instance.currentPlayer != null;
	}

	public Player getPlayerInBattle(int index){
		return instance.playersInBattle[index];
	}

	public Monster getMonsterInBattle(int index){
		return instance.monstersInBattle[index];
	}

	public void checkGUI(){
		// PLAYER TURN AND (CHOSING OBJECTIVE OR DOING DAMAGE) ||  ENEMY TURN AND NOT DOING ACTION
		if(instance.playerTurn && (instance.currentPhase == BattlePhases.CHOSEOBJECTIVE || instance.currentPhase == BattlePhases.DOACTION) || (!instance.playerTurn))
			instance.showMonsterStatGUI = true;		
		else
			instance.showMonsterStatGUI = false;
		

		// ENEMY TURN AND (CHOSING OBJECTIVE OR DOING DAMAGE)  ||  PLAYER TURN AND NOT DOING ACTION
		if(!instance.playerTurn && (instance.currentPhase == BattlePhases.CHOSEOBJECTIVE || instance.currentPhase == BattlePhases.DOACTION) || (instance.playerTurn))
			instance.showPlayerStatGUI = true;
		else
			instance.showPlayerStatGUI = false;

		// PLAYER TURN AND CHOSING ACTION
		if(instance.playerTurn && instance.currentPhase == BattlePhases.CHOSEACTION)
			instance.showPlayerCommandsGUI = true;
		
		/*else{
			showPlayerCommandsGUI = false;
		}*/
	}

	public bool checkIfEnded(){
		int playersAlive = 0;
		int monstersAlive = 0;

		foreach(Player player in instance.playersInBattle){
			if(player.isAlive())
				playersAlive++;			
		}
		foreach(Monster monster in instance.monstersInBattle){
			if(monster.isAlive())
				monstersAlive++;			
		}

		if(monstersAlive == 0){
			instance.ended = true;
			changeState(BattleStates.WIN);			
		}
		else if(playersAlive == 0){
			instance.ended = true;
			changeState(BattleStates.LOSE);
		}

		return instance.ended;
	}

	// Check if the current phase can be changed
	private bool checkPhase(BattlePhases battlePhase){
		if(battlePhase == BattlePhases.NONE)
			return true;		
		else if(instance.currentPhase == BattlePhases.NONE && battlePhase == BattlePhases.AFFECT)
			return true;
		else if(instance.currentPhase == BattlePhases.AFFECT && battlePhase == BattlePhases.CHOSEACTION)
			return true;
		else if(instance.currentPhase == BattlePhases.CHOSEACTION && battlePhase == BattlePhases.CHOSEOBJECTIVE)
			return true;
		else if(instance.currentPhase == BattlePhases.CHOSEOBJECTIVE && battlePhase == BattlePhases.CHOSEACTION && checkIfCurrentCharacterIsPlayer())
			return true;
		else if(instance.currentPhase == BattlePhases.CHOSEOBJECTIVE && battlePhase == BattlePhases.DOACTION)
			return true;
		else if(instance.currentPhase == BattlePhases.DOACTION && battlePhase == BattlePhases.AFFECT)
			return true;
		else
			return false;
	}

	public bool changePhase(BattlePhases battlePhase){
		if(instance.currentPhase != battlePhase){
			instance.currentPhase = battlePhase;				
			//Debug.Log ("Phase: " + currentPhase);
			return true;
		}

		return false;
	}

	public void changeState(BattleStates battleState){
		instance.currentState = battleState;
		//Debug.Log ("State: " + currentState);
	}

	public void cleanVariables(Character character){
		instance.playerObjective = null;
		instance.currentObjective = null;
		instance.attackObjective = false;
		instance.attackFinished = false;
		instance.attackStarted = false;
		instance.basicAttack = false;
		instance.damageReceived = false;
		instance.deathFinished = false;
		instance.skill = false;
		instance.item = false;
		instance.skillName = null;
		instance.itemName = null;
		instance.action = -1;

		character.cleanVariables();

		//if (!character.isAlive())
			//killCharacter(character);		
	}

	private void killCharacter (Character character) {
		instance.turns.Remove(character);

		if (character.isPlayer())
			instance.playersInBattle.Remove((Player)character);
		else
			instance.monstersInBattle.Remove((Monster)character);

		instance.maxTurns--;
	}

	private void enableComponents(GameObject go){
		go.GetComponent<SpriteRenderer>().enabled = true;
		go.GetComponent<Animator>().enabled = true;
		go.GetComponent<BoxCollider2D>().enabled = true;

		if(go.tag.Equals("Monster1") || go.tag.Equals("Monster2") || go.tag.Equals("Monster3")){
			go.GetComponent<MonsterBehaviour>().enabled = true;
			go.GetComponent<Monster>().enabled = true;
		}
		else if(go.tag.Equals("Mage")){
			go.GetComponent<Mage>().enabled = true;
			go.GetComponent<Animator>().enabled = true;
		}
		else if(go.tag.Equals("Knight")){
			go.GetComponent<Knight>().enabled = true;
			go.GetComponent<Animator>().enabled = true;
		}
		else if(go.tag.Equals("Rogue")){
			go.GetComponent<Rogue>().enabled = true;
			go.GetComponent<Animator>().enabled = true;
		}
	}

	private bool checkIfCurrentCharacterIsPlayer(){
		return instance.currentCharacter.bIsPlayer;
	}

	private void setMonsters(){
		//Debug.Log("P IN B: " + instance.playersInBattle.Count);
		//instance.numMonsters = Random.Range(1, (instance.playersInBattle.Count+1));
		numMonsters = 3; // DEBUG

		for(int i = 0; i < instance.numMonsters; i++){
			Monster monster = GameObject.FindWithTag("Monster"+(i+1)).GetComponent<Monster>();
			monster.initializeMonster(instance.gamestate.map.getRandomMonster());
			monstersInBattle.Add(monster);

			enableComponents(monster.gameObject);
		}
	}

	private void setPlayers(){
		//numPlayers = playersInBattle.Count;
		//playersInBattle = BattleData.players;
		//gamestate.players;
		Player player;

		foreach(PlayerData data in instance.gamestate.playersData){
			player = buildPlayer(data);
			if(player != null) {
				//Debug.Log("SUCCESS " + data.characterName);
				player.populate(data);
				instance.playersInBattle.Add(player);
				enableComponents(player.gameObject);
				player.enablePassives();
			}
			else 
				Debug.Log("FAILED " + data.characterName);			
		}
	}

	private Player buildPlayer(PlayerData data){
		GameObject player = GameObject.FindGameObjectWithTag(data.characterName);

		if (player != null)
			return player.GetComponent<Player>();
		else
			return null; 		
	}

	private void giveRewards(){
		instance.battleResults = new BattleResults();

		foreach(Player player in instance.playersInBattle){
			instance.battleResults.addPlayer(player);
		}

		foreach(Monster monster in instance.monstersInBattle){
			instance.battleResults.addExp(monster.giveExp());
			instance.battleResults.addDrops(monster.giveDrops());						
		}

		Singleton.Instance.lastBattleResults = instance.battleResults;
	}

	public void finishCurrentAttack(){
		instance.attackFinished = true;
	}

	public void startCurrentAttack(){
		instance.attackStarted = true;
	}

	public Player getCurrentPlayer(){
		if(isPlayerTurn())
			return instance.currentPlayer;
		else
			return null;		
	}

	public bool battleFinished(){
		return instance.ended;
	}

	public void clickAttack(){
		instance.changePhase(BattlePhases.CHOSEOBJECTIVE);
		instance.currentObjective = instance.currentPlayer.decideObjective();
		instance.basicAttack = true;
	}

	public void clickSkill(){
		instance.changePhase(BattlePhases.CHOSEOBJECTIVE);
		instance.currentObjective = instance.currentPlayer.decideObjective();
		instance.skill = true;
	}

	public void setSkillName(string name){
		instance.skillName = name;
		instance.clickSkill();
	}

	public void clickItem(){
		instance.changePhase(BattlePhases.CHOSEOBJECTIVE);
		instance.currentObjective = instance.currentPlayer.decideObjective();
		instance.item = true;
	}
	
	public void setItemName(string name){
		instance.itemName = name;
		instance.clickItem();
	}

	public void clickDefend(){
		//changePhase(BattlePhases.DOACTION);
		instance.currentObjective = null;
		instance.defend = true;
	}

	public void battleListener(){
		if(!instance.ended){
			if(isPlayerTurn()){ // COMANDS
				if(instance.defend){
					instance.currentPlayer.defend();
					instance.attackFinished = true;
				}

				if(instance.currentObjective != null && !instance.attackObjective && Input.GetButtonDown("Cancel")){
					changePhase(BattlePhases.CHOSEACTION);
					instance.cleanVariables(instance.currentCharacter);
				}

				if(instance.currentObjective != null && !instance.attackObjective){
					if(instance.currentObjective.isPlayer())
						setGUIPlayerInfo((Player) instance.currentObjective);					
					else
						setGUIEnemyInfo((Monster) instance.currentObjective);					
				}
				else if(instance.currentObjective != null && !instance.attackObjective && Input.GetButtonDown("Submit")){
					if(instance.basicAttack && !instance.attackStarted)
						instance.currentPlayer.doBasicAttack(instance.currentObjective);						
					else if(instance.skill && !instance.attackStarted)
						instance.currentPlayer.useSkill(instance.skillName, instance.currentObjective);					
					else if(instance.item && !instance.attackStarted)
						instance.currentPlayer.useItemInBattle(instance.itemName, instance.currentObjective);

					if(instance.currentObjective.isPlayer())
						setGUIPlayerInfo((Player) instance.currentObjective);					
					else
						setGUIEnemyInfo((Monster) instance.currentObjective);
					
				}
				else if(currentObjective != null && attackObjective){
					if(instance.basicAttack && !instance.attackStarted)
						instance.currentPlayer.doBasicAttack(instance.currentObjective);						
					else if(instance.skill && !instance.attackStarted)
						instance.currentPlayer.useSkill(instance.skillName, instance.currentObjective);					
					else if(instance.item && !instance.attackStarted)
						instance.currentPlayer.useItemInBattle(instance.itemName, instance.currentObjective);

					if(instance.currentObjective.isPlayer())
						setGUIPlayerInfo((Player) instance.currentObjective);					
					else
						setGUIEnemyInfo((Monster) instance.currentObjective);			
				}		
				
				if(instance.attackFinished){
					changePhase(BattlePhases.DOACTION);
					endTurn ();
				}
			}
		}
	}

	private void setBackground(){
		instance.background.sprite = Resources.Load <Sprite> ("Backgrounds/Battle/" + instance.currentMap.mapName);
	}

	public void backToStart(){
		instance.playerObjective = null;
		instance.hideGUIEnemyInfo();
		instance.attackObjective = false;
		instance.attackFinished = false;
		instance.attackStarted = false;
		instance.basicAttack = false;
		instance.damageReceived = false;
		instance.deathFinished = false;
		instance.skill = false;
		instance.item = false;
		instance.skillName = null;
		instance.itemName = null;
		instance.action = -1;

		instance.changePhase(BattlePhases.CHOSEACTION);
	}

	private void initializeVariables(){
		instance.playerObjective = null;
		instance.hideGUIEnemyInfo();
		instance.attackObjective = false;
		instance.attackFinished = false;
		instance.attackStarted = false;
		instance.basicAttack = false;
		instance.damageReceived = false;
		instance.deathFinished = false;
		instance.skill = false;
		instance.item = false;
		instance.skillName = null;
		instance.itemName = null;
		instance.action = -1;

		instance.playersInBattle = new List<Player>();
		instance.monstersInBattle = new List<Monster>();
		instance.turns = new List<Character>();
	}

	private void deleteInstance(){
		Object.Destroy(this.gameObject, 0.0f);		
	}
}