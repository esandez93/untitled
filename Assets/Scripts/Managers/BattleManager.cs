using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {

	public int turn = 1;
	public bool bossBattle = false;

	public MapInfo currentMap;

	public bool playerTurn = false;
	public bool showPlayerCommandsGUI = false;
	public bool ended = false;
	public bool attackObjective = false;
	public Player playerObjective = null;
	public Character enemyObjective = null;
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

	GameObject playerInterface;

	GameObject[] playerDataBundle;
	Transform playerData;
	Transform playerHealth;
	Transform playerMana;

	GameObject[] enemyDataBundle;
	Transform enemyData;
	Transform enemyHealth;
	Transform enemyMana;

	public AudioSource ost;

	public SpriteRenderer background;

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
		if (GUI.Button (new Rect (930, 30, 150, 30), "End Turn")) { // DEBUG
			endTurn();
		}
		/*if(!instance.ended)
			checkGUI();	*/	
	}

	void Start(){
		instance.currentMap = gamestate.map;
		
		Transform transform = GameObject.FindGameObjectWithTag("BattleCanvas").transform;

		instance.playerInterface = transform.FindChild("PlayerInterface").gameObject;
		
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

		loadBackgroundMusic();
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
				if (!instance.playerInterface.activeInHierarchy)
					instance.playerInterface.SetActive(true);

				//cleanVariables();
				instance.currentPlayer = (Player) instance.currentCharacter;
				setGUIPlayerInfo(instance.currentPlayer);

				if(instance.currentPhase == BattlePhases.AFFECT) {
					instance.currentPlayer.startTurn();	
					hideGUIEnemyInfo();
				}
				else{
					if(instance.currentObjective == null)		
						hideGUIEnemyInfo();			
					else{
						if(instance.currentObjective.isPlayer())
							setGUIPlayerInfo((Player) instance.currentObjective);				
						else
							setGUIEnemyInfo((Monster) instance.currentObjective);
					}
				}
				
				break;
			case BattleStates.ENEMYTURN:
				//cleanVariables();				
				if (instance.playerInterface.activeInHierarchy)
					instance.playerInterface.SetActive(false);

				instance.currentMonster = (Monster) instance.currentCharacter;			
				setGUIEnemyInfo(instance.currentMonster);

				if(instance.currentPhase == BattlePhases.AFFECT){
					instance.currentMonster.startTurn();
					hideGUIPlayerInfo();				
				}
				else {
					if(instance.action == -1){				
						changePhase(BattlePhases.CHOSEACTION);		
						instance.action = instance.currentMonster.decideAction();
					}
					else {
						if(instance.playerObjective == null){
							hideGUIPlayerInfo();			
							changePhase(BattlePhases.CHOSEOBJECTIVE);
							instance.playerObjective = (Player) instance.currentMonster.decideObjective();
						}
						else{
							setGUIPlayerInfo(instance.playerObjective);

							instance.currentMonster.doAction(instance.action, instance.playerObjective);
				
							if(instance.attackFinished && instance.currentPhase == BattlePhases.DOACTION){
								//changePhase(BattlePhases.DOACTION);
								//if(canEndTurn())
									endTurn();
							}
						}
					}
				}				
				
				break;
			case BattleStates.LOSE:
				Debug.Log ("Monsters WIN!");
				changePhase(BattlePhases.END);
				//CHANGE SCENE TO LOAD LAST SAVEGAME
				if(SaveManager.Instance.load())
					Application.LoadLevel(gamestate.map.mapName);				
				break;
			case BattleStates.WIN:
				Debug.Log ("Players WIN!");
				changePhase(BattlePhases.END);
				giveRewards();
				Application.LoadLevel("forestWinBattle");
				deleteInstance();
				break;
		}
	}

	public void loadBackgroundMusic() {
		instance.ost = this.gameObject.AddComponent<AudioSource>();

		string songName = isBossBattle() ? "Otherworld" : "Normal Battle";

		AudioClip audio = Resources.Load<AudioClip>("Sounds/OST/" + songName);
		if (audio != null){
			Debug.Log("PLAYING " + audio.name);
			instance.ost.minDistance = 10f;
			instance.ost.maxDistance = 10f;
			instance.ost.loop = true;
			instance.ost.clip = audio;
			instance.ost.Play();
		}
		else
			Debug.Log("FAILED OST ");
	}

	public bool isBossBattle() {
		return gamestate.bossBattle;
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

		foreach(Player player in instance.playersInBattle)
			if(player.isAlive())
				listAux.Add(player);		

		foreach(Monster monster in instance.monstersInBattle)
			if(monster.isAlive())
				listAux.Add(monster);

		float agi;
		for(int i = 0; i < totalCharacters; i++){
			Character characterAux = null;
			agi = 0;

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
		checkIfEnded();

		if(instance.turn < instance.maxTurns)
			instance.turn++;
		else
			instance.turn = 1;		

		cleanVariables(instance.currentCharacter);

		checkIfPlayerTurn();
	}

	public void checkIfPlayerTurn(){
		instance.currentCharacter = instance.turns[instance.turn - 1];

		if(!instance.ended){
			if(instance.currentCharacter.isAlive()){
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

				changePhase(BattlePhases.AFFECT);
			}
			else {
				if (currentPhase != BattlePhases.AFFECT)
					endTurn();	
			}
		}
	}

	public void setGUIPlayerInfo(Character player){
		instance.playerPortraitGUI.sprite = Resources.Load <Sprite> ("Portraits/" + player.characterName + "Portrait");
		instance.playerNameGUI.text = player.characterName;
		instance.playerLevelGUI.text = "Lv " + player.level.ToString();
		instance.playerHealthGUI.fillAmount = (player.currHP / player.maxHP);
		instance.playerManaGUI.fillAmount = (player.currMP / player.maxMP);

		instance.playerBackgroundGUI.fillAmount = 1;
		instance.playerHealthFrameGUI.fillAmount = 1;
		instance.playerManaFrameGUI.fillAmount = 1;
		instance.playerPortraitGUI.fillAmount = 1;

		instance.playerHealthTextGUI.text = "HP";
		instance.playerManaTextGUI.text = "MP";
	}

	public void setGUIEnemyInfo(Character enemy){
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
		GameObject[] asdf = instance.playerDataBundle = GameObject.FindGameObjectsWithTag("PlayerData"); 		
		
		foreach(GameObject gameObject in asdf){
			foreach(Image image in gameObject.GetComponentsInChildren<Image>())
				image.fillAmount = 0;			
			
			foreach(Text text in gameObject.GetComponentsInChildren<Text>())
				text.text = "";						
		}
	}

	public void hideGUIEnemyInfo(){
		GameObject[] asdf = GameObject.FindGameObjectsWithTag("EnemyData"); 

		foreach(GameObject gameObject in asdf){
			foreach(Image image in gameObject.GetComponentsInChildren<Image>())
				image.fillAmount = 0;			
			
			foreach(Text text in gameObject.GetComponentsInChildren<Text>())
				text.text = "";			
		}
	}

	public bool isPlayerTurn(){
		return instance.currentPlayer != null && instance.currentPlayer.isAlive();
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
		return
			battlePhase == BattlePhases.NONE ||
			(instance.currentPhase == BattlePhases.NONE && battlePhase == BattlePhases.AFFECT) ||
			(instance.currentPhase == BattlePhases.AFFECT && battlePhase == BattlePhases.CHOSEACTION) ||
			(instance.currentPhase == BattlePhases.CHOSEACTION && battlePhase == BattlePhases.CHOSEOBJECTIVE) ||
			(instance.currentPhase == BattlePhases.CHOSEOBJECTIVE && battlePhase == BattlePhases.CHOSEACTION && instance.currentCharacter.isPlayer()) ||
			(instance.currentPhase == BattlePhases.CHOSEOBJECTIVE && battlePhase == BattlePhases.DOACTION) ||
			(instance.currentPhase == BattlePhases.DOACTION && battlePhase == BattlePhases.AFFECT);
	}

	public bool changePhase(BattlePhases battlePhase){
		//if(instance.currentPhase != battlePhase){
			instance.currentPhase = battlePhase;	
			return true;
		//}

		//return false;
	}

	public void changeState(BattleStates battleState){
		instance.currentState = battleState;
	}

	public void cleanVariables() {
		cleanVariables(null);
	}

	public void cleanVariables(Character character){
		instance.playerTurn = false;
		instance.currentCharacter = null;
		instance.currentPlayer = null;
		instance.currentMonster = null;
		instance.playerObjective = null;
		instance.enemyObjective = null;
		instance.currentObjective = null;
		instance.attackObjective = false;
		instance.attackFinished = false;
		instance.attackStarted = false;
		instance.damageReceived = false;
		instance.deathFinished = false;
		instance.basicAttack = false;
		instance.skill = false;
		instance.item = false;
		instance.defend = false;
		instance.skillName = null;
		instance.itemName = null;
		instance.action = -1;

		if (character != null) {
			character.cleanVariables();
		}

		//if (!character.isAlive())
			//killCharacter(character);		
	}

	/*private void killCharacter (Character character) {
		instance.turns.Remove(character);

		if (character.isPlayer())
			instance.playersInBattle.Remove((Player)character);
		else
			instance.monstersInBattle.Remove((Monster)character);

		instance.maxTurns--;
	}*/

	private void enableComponents(GameObject go){
		go.GetComponent<SpriteRenderer>().enabled = true;
		go.GetComponent<Animator>().enabled = true;
		go.GetComponent<BoxCollider2D>().enabled = true;

		if(go.tag.Contains("Monster")){
			go.GetComponent<MonsterBehaviour>().enabled = true;
			go.GetComponent<Monster>().enabled = true;
		}
		else if(go.tag.Equals("Ki")){
			go.GetComponent<Mage>().enabled = true;
			go.GetComponent<Animator>().enabled = true;
		}
		else if(go.tag.Equals("Gilgamesh")){
			go.GetComponent<Knight>().enabled = true;
			go.GetComponent<Animator>().enabled = true;
		}
		else if(go.tag.Equals("Strider")){
			go.GetComponent<Rogue>().enabled = true;
			go.GetComponent<Animator>().enabled = true;
		}
	}

	/*private bool checkIfCurrentCharacterIsPlayer(){
		return instance.currentCharacter.bIsPlayer;
	}*/

	private void setMonsters(){
		if (isBossBattle())
			instance.numMonsters = 3;
		else {
			//instance.numMonsters = Random.Range(1, (instance.playersInBattle.Count+1));
			instance.numMonsters = 1; // DEBUG
		}
		
		Monster monster;
		for(int i = 1; i <= instance.numMonsters; i++){
			if(isBossBattle() && i == 2){
				monster = GameObject.FindWithTag("Monster2").GetComponent<Monster>();
				monster.initializeMonster(instance.gamestate.map.getBossMonster());	
			}
			else{
				monster = GameObject.FindWithTag("Monster"+(i)).GetComponent<Monster>();
				monster.initializeMonster(instance.gamestate.map.getRandomMonster());				
			}

			enableComponents(monster.gameObject);
			instance.monstersInBattle.Add(monster);
		}
	}

	private void setPlayers(){
		Player player;

		foreach(PlayerData data in instance.gamestate.playersData){
			player = buildPlayer(data);
			if(player != null) {
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

		foreach(Player player in instance.playersInBattle) {
			instance.battleResults.addPlayer(player);
			//Debug.Log("ADDING " + player.characterName);
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
		instance.skill = false;
		instance.item = false;
		instance.defend = false;
	}

	public void clickSkill(){
		instance.changePhase(BattlePhases.CHOSEOBJECTIVE);
		instance.currentObjective = instance.currentPlayer.decideObjective();
		instance.basicAttack = false;
		instance.skill = true;
		instance.item = false;
		instance.defend = false;
	}

	public void setSkillName(string name){
		instance.skillName = name;
		instance.clickSkill();
	}

	public void clickItem(){
		instance.changePhase(BattlePhases.CHOSEOBJECTIVE);
		instance.currentObjective = instance.currentPlayer.decideObjective();
		instance.basicAttack = false;
		instance.skill = false;
		instance.item = true;
		instance.defend = false;
	}
	
	public void setItemName(string name){
		instance.itemName = name;
		instance.clickItem();
	}

	public void clickDefend(){
		changePhase(BattlePhases.DOACTION);
		instance.attackFinished = true;

		instance.currentObjective = null;
		instance.basicAttack = false;
		instance.skill = false;
		instance.item = false;
		instance.defend = true;
	}

	public void clickRun(){		
		int random = Random.Range(0,100);
		if (random >= 0 && random <= 50) { // 50% success
			instance.gamestate.showMessage(LanguageManager.Instance.getMenuText("run_success"));
			run();
			Debug.Log("Scape success");
		}
		else {
			instance.gamestate.showMessage(LanguageManager.Instance.getMenuText("run_failed"));
			instance.attackFinished = true;
			Debug.Log("Scape failed");
		}

		changePhase(BattlePhases.DOACTION);

		instance.currentObjective = null;
	}

	private void run() {		
		
		foreach(Player p in instance.playersInBattle) {
			if (p.isAlive())
				p.run();
		}
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
				
				if(instance.attackFinished && instance.currentPhase == BattlePhases.DOACTION){
					//changePhase(BattlePhases.DOACTION);
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
		instance.enemyObjective = null;
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
		instance.enemyObjective = null;
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