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
			gamestate = Gamestate.instance;
			Debug.Log("BattleManager initialized.");
			initialized = true;
		}
	}

	void OnGUI() {
		if(!ended){
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

		currentMap = gamestate.map;
		
		Transform transform = GameObject.FindGameObjectWithTag("BattleCanvas").transform;
		
		Transform playerDataTransform = transform.FindChild("PlayerData");
		playerBackgroundGUI = playerDataTransform.GetComponent<Image>();
		playerPortraitGUI = playerDataTransform.FindChild("PlayerPortrait").GetComponent<Image>();
		playerNameGUI = playerDataTransform.FindChild("PlayerName").GetComponent<Text>();
		playerLevelGUI = playerDataTransform.FindChild("PlayerLevel").GetComponent<Text>();
		playerHealthGUI = playerDataTransform.FindChild("Health").FindChild("HealthBar").GetComponent<Image>();
		playerManaGUI = playerDataTransform.FindChild("Mana").FindChild("ManaBar").GetComponent<Image>();
		playerExpGUI = playerDataTransform.FindChild("Exp").FindChild("ExpBar").GetComponent<Image>();
		playerHealthFrameGUI = playerDataTransform.FindChild("Health").FindChild("HealthBarFrame").GetComponent<Image>();
		playerManaFrameGUI = playerDataTransform.FindChild("Mana").FindChild("ManaBarFrame").GetComponent<Image>();
		playerExpFrameGUI = playerDataTransform.FindChild("Exp").FindChild("ExpBarFrame").GetComponent<Image>();
		playerHealthTextGUI = playerDataTransform.FindChild("Health").FindChild("HealthText").GetComponent<Text>();
		playerManaTextGUI = playerDataTransform.FindChild("Mana").FindChild("ManaText").GetComponent<Text>();
		playerExpTextGUI = playerDataTransform.FindChild("Exp").FindChild("ExpText").GetComponent<Text>();
		
		Transform enemyDataTransform = transform.FindChild("EnemyData");
		enemyBackgroundGUI = enemyDataTransform.GetComponent<Image>();
		enemyPortraitGUI = enemyDataTransform.FindChild("EnemyPortrait").GetComponent<Image>();
		enemyNameGUI = enemyDataTransform.FindChild("EnemyName").GetComponent<Text>();
		enemyLevelGUI = enemyDataTransform.FindChild("EnemyLevel").GetComponent<Text>();
		enemyHealthGUI = enemyDataTransform.FindChild("Health").FindChild("HealthBar").GetComponent<Image>();
		enemyManaGUI = enemyDataTransform.FindChild("Mana").FindChild("ManaBar").GetComponent<Image>();
		enemyHealthFrameGUI = enemyDataTransform.FindChild("Health").FindChild("HealthBarFrame").GetComponent<Image>();
		enemyManaFrameGUI = enemyDataTransform.FindChild("Mana").FindChild("ManaBarFrame").GetComponent<Image>();
		enemyHealthTextGUI = enemyDataTransform.FindChild("Health").FindChild("HealthText").GetComponent<Text>();
		enemyManaTextGUI = enemyDataTransform.FindChild("Mana").FindChild("ManaText").GetComponent<Text>();

		background = GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>();

		changeState(BattleStates.START);
		changePhase(BattlePhases.NONE);

		initializeVariables();

		setBackground();
		setMonsters();
		setPlayers();
		LanguageManager.Instance.translateButtons();
	}

	void Update(){
		battleListener();

		switch(currentState){
		case BattleStates.START:
			generateBattle();
			break;
		case BattleStates.PLAYERTURN:
			currentPlayer = (Player) currentCharacter;
			setGUIPlayerInfo(currentPlayer);

			if(currentPhase == BattlePhases.AFFECT){
				currentPlayer.startTurn();
			}

			if(currentObjective == null){				
				hideGUIEnemyInfo();
			}
			else{
				if(currentObjective.isPlayer()){
					setGUIPlayerInfo((Player)currentObjective);
				}
				else{
					setGUIEnemyInfo((Monster)currentObjective);
				}				
			}

			break;
		case BattleStates.ENEMYTURN:
			currentMonster = (Monster)currentCharacter;
			setGUIEnemyInfo(currentMonster);
			if(currentPhase == BattlePhases.AFFECT){
				currentMonster.startTurn();
				hideGUIPlayerInfo();
			}
			else if(action == -1){				
				changePhase(BattlePhases.CHOSEACTION);		
				action = currentMonster.decideAction();
			}
			else if(playerObjective == null){
				changePhase(BattlePhases.CHOSEOBJECTIVE);
				playerObjective = (Player)currentMonster.decideObjective();
			}
			else{
				setGUIPlayerInfo(playerObjective);

				currentMonster.doAction(action, playerObjective);

				//currentMonster.GetComponent<MonsterBehaviour>().basicAttack(playerObjective);
				
				if(attackFinished){
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
		numPlayers = playersInBattle.Count;

		giveTurns();

		checkIfPlayerTurn();
	}

	public Monster getMonster(){
		if(thisMonster < monstersInBattle.Count){
			return monstersInBattle[thisMonster];
		}
		else{
			return null;
		}
	}

	//Por el momento, solo se tendra en cuenta la AGI a la hora de repartir turnos
	public void giveTurns(){
		int totalCharacters = playersInBattle.Count + monstersInBattle.Count;

		List<Character> listAux = new List<Character>();

		foreach(Player player in playersInBattle){
			listAux.Add(player);
		}

		foreach(Monster monster in monstersInBattle){
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

			turns.Add(characterAux);
			listAux.Remove(characterAux);
		}

		maxTurns = turns.Count;
	}

	public void endTurn(){
		if(turn < maxTurns){
			turn++;
		}
		else{
			turn = 1;
		}

		cleanVariables(currentCharacter);

		checkIfPlayerTurn();
	}

	public void checkIfPlayerTurn(){
		currentCharacter = turns[turn-1];

		if(currentCharacter.isAlive()){
			if(!ended){
				if(currentCharacter.isPlayer()){
					hideGUIEnemyInfo();
					playerTurn = true;
					currentMonster = null;
					currentPlayer = (Player) currentCharacter;
					changeState(BattleStates.PLAYERTURN);
					//showPlayerCommandsGUI = true;
				}
				else{
					hideGUIPlayerInfo();
					playerTurn = false;
					showPlayerCommandsGUI = false;
					currentMonster = (Monster) currentCharacter;
					currentPlayer = null;
					changeState(BattleStates.ENEMYTURN);

				}
			}

			changePhase(BattlePhases.AFFECT);
			//Debug.Log (currentCharacter.name);
		}
		else{
			checkIfEnded();
			endTurn();
		}
	}

	public void setGUIPlayerInfo(Character player){
		playerPortraitGUI.sprite = Resources.Load <Sprite> ("Portraits/" + player.name + "Portrait");
		playerNameGUI.text = player.name;
		playerLevelGUI.text = "Lv " + player.level.ToString();
		playerHealthGUI.fillAmount = (player.currHP / player.maxHP);
		playerManaGUI.fillAmount = (player.currMP / player.maxMP);
		//playerExpGUI.fillAmount = (player.exp / player.expForNextLevel);

		playerBackgroundGUI.fillAmount = 1;
		playerHealthFrameGUI.fillAmount = 1;
		playerManaFrameGUI.fillAmount = 1;
		//playerExpFrameGUI.fillAmount = 1;
		playerPortraitGUI.fillAmount = 1;

		playerHealthTextGUI.text = "HP";
		playerManaTextGUI.text = "MP";
		//playerExpTextGUI.text = "EXP";
	}

	public void setGUIEnemyInfo(Character enemy){//Monster monster){
		enemyPortraitGUI.sprite = Resources.Load <Sprite> ("Portraits/" + enemy.name + "Portrait");
		enemyNameGUI.text = LanguageManager.Instance.getMenuText(enemy.name);
		enemyLevelGUI.text = "Lv " + enemy.level.ToString();
		enemyHealthGUI.fillAmount = (enemy.currHP / enemy.maxHP);
		enemyManaGUI.fillAmount = (enemy.currMP / enemy.maxMP);

		enemyBackgroundGUI.fillAmount = 1;
		enemyHealthFrameGUI.fillAmount = 1;
		enemyManaFrameGUI.fillAmount = 1;
		enemyPortraitGUI.fillAmount = 1;
		
		enemyHealthTextGUI.text = "HP";
		enemyManaTextGUI.text = "MP";
	}

	public void hideGUIPlayerInfo(){
		if(playerDataBundle == null){
			playerDataBundle = GameObject.FindGameObjectsWithTag("PlayerData"); 
		}
		
		foreach(GameObject gameObject in playerDataBundle){
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

	/*public void hideGUIObjectiveInfo(){
		if(){

		}
	}*/

	public bool isPlayerTurn(){
		if(currentPlayer != null){
			return true;
		}
		else{
			return false;
		}
	}

	public Player getPlayerInBattle(int index){
		return playersInBattle[index];
	}

	public Monster getMonsterInBattle(int index){
		return monstersInBattle[index];
	}

	public void checkGUI(){
		// PLAYER TURN AND (CHOSING OBJECTIVE OR DOING DAMAGE) ||  ENEMY TURN AND NOT DOING ACTION
		if(playerTurn && (currentPhase == BattlePhases.CHOSEOBJECTIVE || currentPhase == BattlePhases.DOACTION) || (!playerTurn)){
			showMonsterStatGUI = true;
		}
		else{
			showMonsterStatGUI = false;
		}

		// ENEMY TURN AND (CHOSING OBJECTIVE OR DOING DAMAGE)  ||  PLAYER TURN AND NOT DOING ACTION
		if(!playerTurn && (currentPhase == BattlePhases.CHOSEOBJECTIVE || currentPhase == BattlePhases.DOACTION) || (playerTurn)){
			showPlayerStatGUI = true;
		}
		else{
			showPlayerStatGUI = false;
		}

		// PLAYER TURN AND CHOSING ACTION
		if(playerTurn && currentPhase == BattlePhases.CHOSEACTION){
			showPlayerCommandsGUI = true;
		}
		/*else{
			showPlayerCommandsGUI = false;
		}*/
	}

	public void checkIfEnded(){
		int playersAlive = 0;
		int monstersAlive = 0;

		foreach(Player player in playersInBattle){
			if(player.alive){
				playersAlive++;
			}
		}
		foreach(Monster monster in monstersInBattle){
			if(monster.alive){
				monstersAlive++;
			}
		}

		if(monstersAlive == 0){
			changeState(BattleStates.WIN);
			ended = true;
		}
		else if(playersAlive == 0){
			changeState(BattleStates.LOSE);
			ended = true;
		}
	}

	// Check if the current phase can be changed
	private bool checkPhase(BattlePhases battlePhase){
		if(battlePhase == BattlePhases.NONE){
			return true;
		}
		else if(currentPhase == BattlePhases.NONE && battlePhase == BattlePhases.AFFECT){
			return true;
		}
		else if(currentPhase == BattlePhases.AFFECT && battlePhase == BattlePhases.CHOSEACTION){
			return true;
		}
		else if(currentPhase == BattlePhases.CHOSEACTION && battlePhase == BattlePhases.CHOSEOBJECTIVE){
			return true;
		}
		else if(currentPhase == BattlePhases.CHOSEOBJECTIVE && battlePhase == BattlePhases.CHOSEACTION && checkIfCurrentCharacterIsPlayer()){
			return true;
		}
		else if(currentPhase == BattlePhases.CHOSEOBJECTIVE && battlePhase == BattlePhases.DOACTION){
			return true;
		}
		else if(currentPhase == BattlePhases.DOACTION && battlePhase == BattlePhases.AFFECT){
			return true;
		}
		else{
			return false;
		}
	}

	public bool changePhase(BattlePhases battlePhase){
		if(currentPhase != battlePhase){
			//if(checkPhase(battlePhase)){
				currentPhase = battlePhase;
				
				Debug.Log ("Phase: " + currentPhase);

				return true;
			//}
		}

		return false;
	}

	public void changeState(BattleStates battleState){
		currentState = battleState;

		Debug.Log ("State: " + currentState);
	}

	public void cleanVariables(Character character){
		playerObjective = null;
		currentObjective = null;
		attackObjective = false;
		attackFinished = false;
		attackStarted = false;
		basicAttack = false;
		damageReceived = false;
		deathFinished = false;
		skill = false;
		item = false;
		skillName = null;
		itemName = null;
		action = -1;

		character.cleanVariables();
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
		return currentCharacter.bIsPlayer;
	}

	private void setMonsters(){
		numMonsters = Random.Range(1, (playersInBattle.Count+1));
		//numMonsters = 1; // DEBUG

		for(int i = 0; i < numMonsters; i++){
			Monster monster = GameObject.FindWithTag("Monster"+(i+1)).GetComponent<Monster>();
			monster.initializeMonster(gamestate.map.getRandomMonster());
			monstersInBattle.Add(monster);

			enableComponents(monster.gameObject);
		}
	}

	private void setPlayers(){
		//numPlayers = playersInBattle.Count;
		//playersInBattle = BattleData.players;
		//gamestate.players;
		Player player;

		foreach(PlayerData data in gamestate.playersData){
			player = buildPlayer(data);
			if(player != null) {
				Debug.Log("SUCCESS " + data.characterName);
				player.populate(data);
				playersInBattle.Add(player);
				enableComponents(player.gameObject);
				player.enablePassives();
			}
			else {
				Debug.Log("FAILED " + data.characterName);
			}
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
		battleResults = new BattleResults();

		foreach(Player player in playersInBattle){
			battleResults.addPlayer(player);
		}

		foreach(Monster monster in monstersInBattle){
			battleResults.addExp(monster.giveExp());
			battleResults.addDrops(monster.giveDrops());						
		}

		Singleton.Instance.lastBattleResults = battleResults;
	}

	public void finishCurrentAttack(){
		attackFinished = true;
	}

	public void startCurrentAttack(){
		attackStarted = true;
	}

	public Player getCurrentPlayer(){
		if(isPlayerTurn()){
			return currentPlayer;
		}
		else{
			return null;
		}
	}

	public bool battleFinished(){
		return ended;
	}

	public void clickAttack(){
		instance.changePhase(BattlePhases.CHOSEOBJECTIVE);
		currentObjective = instance.currentPlayer.decideObjective();
		instance.basicAttack = true;
	}

	public void clickSkill(){
		instance.changePhase(BattlePhases.CHOSEOBJECTIVE);
		instance.currentObjective = instance.currentPlayer.decideObjective();
		instance.skill = true;
	}

	public void setSkillName(string name){
		instance.skillName = name;
		instance.clickSkill ();
	}

	public void clickItem(){
		instance.changePhase(BattlePhases.CHOSEOBJECTIVE);
		instance.currentObjective = instance.currentPlayer.decideObjective();
		instance.item = true;
	}
	
	public void setItemName(string name){
		instance.itemName = name;
		instance.clickItem ();
	}

	public void clickDefend(){
		//changePhase(BattlePhases.DOACTION);
		instance.currentObjective = null;
		instance.defend = true;
	}

	public void battleListener(){
		if(!ended){
			if(isPlayerTurn()){ // COMANDS
				if(defend){
					currentPlayer.defend();
					attackFinished = true;
				}

				if(currentObjective != null && !attackObjective && Input.GetButtonDown("Cancel")){
					changePhase(BattlePhases.CHOSEACTION);
					cleanVariables(currentCharacter);
				}

				if(currentObjective != null && !attackObjective){
					if(currentObjective.isPlayer()){
						setGUIPlayerInfo((Player)currentObjective);
					}
					else{
						setGUIEnemyInfo((Monster)currentObjective);
					}
				}
				else if(currentObjective != null && !attackObjective && Input.GetButtonDown("Submit")){
					if(basicAttack && !attackStarted){
						//currentPlayer.basicAttack(currentObjective);	
						currentPlayer.doBasicAttack(currentObjective);	
					}
					else if(skill && !attackStarted){
						currentPlayer.useSkill(skillName, currentObjective);
					}
					else if(item && !attackStarted){
						currentPlayer.useItemInBattle(itemName, currentObjective);
					}

					if(currentObjective.isPlayer()){
						setGUIPlayerInfo((Player)currentObjective);
					}
					else{
						setGUIEnemyInfo((Monster)currentObjective);
					}
				}
				else if(currentObjective != null && attackObjective){
					if(basicAttack && !attackStarted){
						//currentPlayer.basicAttack(currentObjective);	
						currentPlayer.doBasicAttack(currentObjective);	
					}
					else if(skill && !attackStarted){
						currentPlayer.useSkill(skillName, currentObjective);
					}					
					else if(item && !attackStarted){
						currentPlayer.useItemInBattle(itemName, currentObjective);
					}

					if(currentObjective.isPlayer()){
						setGUIPlayerInfo((Player)currentObjective);
					}
					else{
						setGUIEnemyInfo((Monster)currentObjective);
					}
				}		
				
				if(attackFinished){
					changePhase(BattlePhases.DOACTION);
					endTurn ();
				}
			}
		}
	}

	private void setBackground(){
		background.sprite = Resources.Load <Sprite> ("Backgrounds/Battle/" + currentMap.mapName);
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

		playersInBattle = new List<Player>();
		monstersInBattle = new List<Monster>();
		turns = new List<Character>();
	}

	private void deleteInstance(){
		Object.Destroy(this.gameObject, 0.0f);		
	}
}