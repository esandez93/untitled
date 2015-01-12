using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {

	public int turn = 1;

	public MapInfo currentMap;

	public bool playerTurn = false;
	public bool showPlayerCommandsGUI = false;
	public static bool ended = false;
	public static bool attackObjective = false;
	public static Player playerObjective = null;
	public static int action = -1;

	public static bool basicAttack = false;
	public static bool skill = false;
	public static bool item = false;
	public static string skillName;
	public static string itemName;
	public static bool attackFinished = false;
	public static bool attackStarted = false;
	public static bool damageReceived = false;
	public static bool deathFinished = false;

	public bool showPlayerStatGUI;
	public bool showMonsterStatGUI;

	public static Image playerBackgroundGUI;
	public static Image playerPortraitGUI;
	public static Text playerNameGUI;
	public static Text playerLevelGUI;
	public static Image playerHealthGUI;
	public static Image playerManaGUI;
	public static Image playerExpGUI;
	public static Image playerHealthFrameGUI;
	public static Image playerManaFrameGUI;
	public static Image playerExpFrameGUI;
	public static Text playerHealthTextGUI;
	public static Text playerManaTextGUI;
	public static Text playerExpTextGUI;

	public static Image enemyBackgroundGUI;
	public static Image enemyPortraitGUI;
	public static Text enemyNameGUI;
	public static Text enemyLevelGUI;
	public static Image enemyHealthGUI;
	public static Image enemyManaGUI;
	public static Image enemyHealthFrameGUI;
	public static Image enemyManaFrameGUI;
	public static Text enemyHealthTextGUI;
	public static Text enemyManaTextGUI;

	public static Character currentObjective;

	public static BattleStates currentState;
	public static BattlePhases currentPhase;

	int maxTurns;
	public static Character currentCharacter;

	public static Monster currentMonster;
	public static Player currentPlayer;

	public static int numPlayers = 0;
	public static int numMonsters = 0;
	public static int thisMonster = 0;

	public static List<Monster> monstersInBattle = new List<Monster>();
	public static List<Player> playersInBattle = new List<Player>();
	public static List<Character> turns = new List<Character>();

	public List<Monster> nonStaticMonstersInBattle = new List<Monster>();
	public List<Player> nonStaticPlayersInBattle = new List<Player>();	
	
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

	void OnGUI() {
		if(!ended){
			checkGUI();

			if (GUI.Button (new Rect (530, 30, 150, 30), "Check GameState")) { // DEBUG
				Debug.Log ("State: " + currentState + ", Phase: " + currentPhase);
			}
			if (GUI.Button (new Rect (730, 30, 150, 30), "Add potion")) { // DEBUG
				Singleton.inventory.addItem("Potion", 1);
				DisplayItems.repopulate();
			}
		}
	}

	void Start(){
		currentMap = Gamestate.instance.map;

		playerBackgroundGUI = transform.FindChild("PlayerData").GetComponent<Image>();
		playerPortraitGUI = transform.FindChild("PlayerData").FindChild("PlayerPortrait").GetComponent<Image>();
		playerNameGUI = transform.FindChild("PlayerData").FindChild("PlayerName").GetComponent<Text>();
		playerLevelGUI = transform.FindChild("PlayerData").FindChild("PlayerLevel").GetComponent<Text>();
		playerHealthGUI = transform.FindChild("PlayerData").FindChild("Health").FindChild("HealthBar").GetComponent<Image>();
		playerManaGUI = transform.FindChild("PlayerData").FindChild("Mana").FindChild("ManaBar").GetComponent<Image>();
		playerExpGUI = transform.FindChild("PlayerData").FindChild("Exp").FindChild("ExpBar").GetComponent<Image>();
		playerHealthFrameGUI = transform.FindChild("PlayerData").FindChild("Health").FindChild("HealthBarFrame").GetComponent<Image>();
		playerManaFrameGUI = transform.FindChild("PlayerData").FindChild("Mana").FindChild("ManaBarFrame").GetComponent<Image>();
		playerExpFrameGUI = transform.FindChild("PlayerData").FindChild("Exp").FindChild("ExpBarFrame").GetComponent<Image>();
		playerHealthTextGUI = transform.FindChild("PlayerData").FindChild("Health").FindChild("HealthText").GetComponent<Text>();
		playerManaTextGUI = transform.FindChild("PlayerData").FindChild("Mana").FindChild("ManaText").GetComponent<Text>();
		playerExpTextGUI = transform.FindChild("PlayerData").FindChild("Exp").FindChild("ExpText").GetComponent<Text>();

		enemyBackgroundGUI = transform.FindChild("EnemyData").GetComponent<Image>();
		enemyPortraitGUI = transform.FindChild("EnemyData").FindChild("EnemyPortrait").GetComponent<Image>();
		enemyNameGUI = transform.FindChild("EnemyData").FindChild("EnemyName").GetComponent<Text>();
		enemyLevelGUI = transform.FindChild("EnemyData").FindChild("EnemyLevel").GetComponent<Text>();
		enemyHealthGUI = transform.FindChild("EnemyData").FindChild("Health").FindChild("HealthBar").GetComponent<Image>();
		enemyManaGUI = transform.FindChild("EnemyData").FindChild("Mana").FindChild("ManaBar").GetComponent<Image>();
		enemyHealthFrameGUI = transform.FindChild("EnemyData").FindChild("Health").FindChild("HealthBarFrame").GetComponent<Image>();
		enemyManaFrameGUI = transform.FindChild("EnemyData").FindChild("Mana").FindChild("ManaBarFrame").GetComponent<Image>();
		enemyHealthTextGUI = transform.FindChild("EnemyData").FindChild("Health").FindChild("HealthText").GetComponent<Text>();
		enemyManaTextGUI = transform.FindChild("EnemyData").FindChild("Mana").FindChild("ManaText").GetComponent<Text>();

		changeState(BattleStates.START);
		changePhase(BattlePhases.NONE);

		setMonsters();
		setPlayers();
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
				currentCharacter.startTurn();
			}

			if(currentObjective == null){				
				hideGUIMonsterInfo();
			}
			else{
				setGUIMonsterInfo((Monster)currentObjective);
			}

			break;
		case BattleStates.ENEMYTURN:
			currentMonster = (Monster)currentCharacter;
			setGUIMonsterInfo(currentMonster);
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
			break;
		case BattleStates.WIN:
			Debug.Log ("Players WIN!");
			changePhase(BattlePhases.END);
			giveRewards();
			//CHANGE SCENE TO BATTLE REWARDS
			break;
		}
		
		nonStaticPlayersInBattle = playersInBattle;
		nonStaticMonstersInBattle = monstersInBattle;
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
			if(currentCharacter.isPlayer()){
				hideGUIMonsterInfo();
				playerTurn = true;
				changeState(BattleStates.PLAYERTURN);
				//showPlayerCommandsGUI = true;
			}
			else{
				hideGUIPlayerInfo();
				playerTurn = false;
				showPlayerCommandsGUI = false;
				changeState(BattleStates.ENEMYTURN);
			}

			changePhase(BattlePhases.AFFECT);
			//Debug.Log (currentCharacter.name);
		}
		else{
			checkIfEnded();
			endTurn();
		}
	}

	public static void setGUIPlayerInfo(Player player){
		string job;

		switch(player.job){
		case Player.Job.MAGE:
			job = "Mage";
			break;
		case Player.Job.KNIGHT: 
			job = "Knight";
			break;
		case Player.Job.ROGUE: 
			job = "Rogue";
			break;
		default:
			job = "Default";
			break;
		}

		playerPortraitGUI.sprite = Resources.Load <Sprite> ("Portraits/" + job + "Portrait");
		playerNameGUI.text = player.name;
		playerLevelGUI.text = "Lv " + player.level.ToString();
		playerHealthGUI.fillAmount = (player.currHP / player.maxHP);
		playerManaGUI.fillAmount = (player.currMP / player.maxMP);
		playerExpGUI.fillAmount = (player.exp / player.expForNextLevel);

		playerBackgroundGUI.fillAmount = 1;
		playerHealthFrameGUI.fillAmount = 1;
		playerManaFrameGUI.fillAmount = 1;
		playerExpFrameGUI.fillAmount = 1;
		playerPortraitGUI.fillAmount = 1;

		playerHealthTextGUI.text = "HP";
		playerManaTextGUI.text = "MP";
		playerExpTextGUI.text = "EXP";
	}

	public static void setGUIMonsterInfo(Monster monster){
		enemyPortraitGUI.sprite = Resources.Load <Sprite> ("Portraits/" + monster.name + "Portrait");
		enemyNameGUI.text = monster.name;
		enemyLevelGUI.text = "Lv " + monster.level.ToString();
		enemyHealthGUI.fillAmount = (monster.currHP / monster.maxHP);
		enemyManaGUI.fillAmount = (monster.currMP / monster.maxMP);

		enemyBackgroundGUI.fillAmount = 1;
		enemyHealthFrameGUI.fillAmount = 1;
		enemyManaFrameGUI.fillAmount = 1;
		enemyPortraitGUI.fillAmount = 1;
		
		enemyHealthTextGUI.text = "HP";
		enemyManaTextGUI.text = "MP";
	}

	public void hideGUIPlayerInfo(){
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("PlayerData"); 
		
		foreach(GameObject gameObject in gameObjects){
			foreach(Image image in gameObject.GetComponentsInChildren<Image>()){
				image.fillAmount = 0;
			}
			
			foreach(Text text in gameObject.GetComponentsInChildren<Text>()){
				text.text = null;
			}			
		}
	}

	public static void hideGUIMonsterInfo(){
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("EnemyData"); 

		foreach(GameObject gameObject in gameObjects){
			foreach(Image image in gameObject.GetComponentsInChildren<Image>()){
				image.fillAmount = 0;
			}
			
			foreach(Text text in gameObject.GetComponentsInChildren<Text>()){
				text.text = null;
			}
		}
	}

	public bool isPlayerTurn(){
		if(currentPlayer != null){
			return true;
		}
		else{
			return false;
		}
	}

	public static Player getPlayerInBattle(int index){
		return playersInBattle[index];
	}

	public static Monster getMonsterInBattle(int index){
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

	public static void checkIfEnded(){
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
	private static bool checkPhase(BattlePhases battlePhase){
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

	public static bool changePhase(BattlePhases battlePhase){
		if(currentPhase != battlePhase){
			if(checkPhase(battlePhase)){
				currentPhase = battlePhase;
				
				Debug.Log ("Phase: " + currentPhase);

				return true;
			}
		}

		return false;
	}

	public static void changeState(BattleStates battleState){
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
			go.GetComponent<Animator>().enabled = false; // DEBUG
		}
		else if(go.tag.Equals("Knight")){
			go.GetComponent<Knight>().enabled = true;
		}
		else if(go.tag.Equals("Rogue")){
			go.GetComponent<Rogue>().enabled = true;
		}
	}

	private static bool checkIfCurrentCharacterIsPlayer(){
		return currentCharacter.bIsPlayer;
	}

	private void setMonsters(){
		numMonsters = Random.Range(1, (playersInBattle.Count+1));
		//numMonsters = 3; // DEBUG

		for(int i = 0; i < numMonsters; i++){
			Monster monster = GameObject.FindWithTag("Monster"+(i+1)).GetComponent<Monster>();
			monster.initializeMonster(Gamestate.instance.map.getRandomMonster());
			monstersInBattle.Add(monster);
			
			enableComponents(monster.gameObject);
		}
	}

	private void setPlayers(){
		//numPlayers = playersInBattle.Count;
		playersInBattle = Gamestate.instance.players;

		foreach(Player player in playersInBattle){
			Debug.Log (player);
			enableComponents(player.gameObject);
			player.enablePassives();
		}
	}

	private void giveRewards(){
		foreach(Monster monster in monstersInBattle){
			foreach(Player player in playersInBattle){
				monster.giveExp(player);
			}

			monster.giveDrops();
		}
	}

	public static void finishCurrentAttack(){
		attackFinished = true;
	}

	public static void startCurrentAttack(){
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
		changePhase(BattlePhases.CHOSEOBJECTIVE);
		currentObjective = currentPlayer.decideObjective();
		basicAttack = true;
	}

	public void clickSkill(){
		changePhase(BattlePhases.CHOSEOBJECTIVE);
		currentObjective = currentPlayer.decideObjective();
		skill = true;
	}

	public void setSkillName(string name){
		skillName = name;
		clickSkill ();
	}

	public void clickItem(){
		changePhase(BattlePhases.CHOSEOBJECTIVE);
		currentObjective = currentPlayer.decideObjective();
		item = true;
	}
	
	public void setItemName(string name){
		itemName = name;
		clickItem ();
	}

	public void battleListener(){
		if(!ended){
			if(showPlayerCommandsGUI){ // COMANDS
				if(currentObjective != null && !attackObjective && Input.GetKeyDown(KeyCode.Escape)){
					changePhase(BattlePhases.CHOSEACTION);
					cleanVariables(currentCharacter);
				}

				if(currentObjective != null && !attackObjective){
					setGUIMonsterInfo((Monster)currentObjective);
				}
				else if(currentObjective != null && !attackObjective && Input.GetKeyDown(KeyCode.Return)){
					if(basicAttack && !attackStarted){
						currentPlayer.basicAttack(currentObjective);	
					}
					else if(skill && !attackStarted){
						currentPlayer.useSkill(skillName, currentObjective);
					}
					else if(item && !attackStarted){
						currentPlayer.useItem(itemName, currentObjective);
					}
					setGUIMonsterInfo((Monster)currentObjective);
				}
				else if(currentObjective != null && attackObjective){
					//Debug.Log ("ITEM: " + item + ", ATTACKSTARTED: " + attackStarted + ", ITEMNAME: " + itemName + ", CURRENTOBJECTIVE: " + currentObjective.name);
					if(basicAttack && !attackStarted){
						currentPlayer.basicAttack(currentObjective);	
					}
					else if(skill && !attackStarted){
						currentPlayer.useSkill(skillName, currentObjective);
					}					
					else if(item && !attackStarted){
						currentPlayer.useItem(itemName, currentObjective);
					}
					setGUIMonsterInfo((Monster)currentObjective);
				} 			
				
				if(attackFinished){
					changePhase(BattlePhases.DOACTION);
					endTurn ();
				}
			}
		}
	}

	public static void backToStart(){
		BattleManager.playerObjective = null;
		BattleManager.hideGUIMonsterInfo();
		BattleManager.attackObjective = false;
		BattleManager.attackFinished = false;
		BattleManager.attackStarted = false;
		BattleManager.basicAttack = false;
		BattleManager.damageReceived = false;
		BattleManager.deathFinished = false;
		BattleManager.skill = false;
		BattleManager.item = false;
		BattleManager.skillName = null;
		BattleManager.itemName = null;
		BattleManager.action = -1;

		BattleManager.changePhase(BattlePhases.CHOSEACTION);
	}
}