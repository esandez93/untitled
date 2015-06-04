using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable] 
public class Character : MonoBehaviour{

	public bool attackFinished;
	public static int PERCENT_DAMAGE_VARIATION = 20;
	public static float DIFFICULTY_DAMAGE_VARIATION = 0.25f;

	public Rigidbody2D body;

	public static Character character;
	//public Character clickedObjective;

	int numObjective;
	Character objective;

	public bool bIsPlayer;

	public string characterName;
	public int level;
	public int element;

	public float maxHP;
	public float maxMP;
	public float currHP;
	public float currMP;

	public float str;
	public float agi;
	public float vit;
	public float itg;
	public float dex;
	public float luk;

	public float atk;
	public float matk;
	public float def;
	public float mdef;
	public float hit;
	public float flee;
	public float critChance;
	public float critDmg;

	public float hpRegen = Singleton.Battle.HP_REGEN_RATIO;
	public float mpRegen = Singleton.Battle.MP_REGEN_RATIO;

	public bool defending;

	public bool alive;

	public Dictionary<string, Skill> skills = new Dictionary<string, Skill>();

	public Dictionary<string, AlteredStatus> alteredStatus = new Dictionary<string, AlteredStatus>();

	public virtual void Start(){
		//this.level = 1;
		this.alive = true;
	}
	
	public virtual void Update(){
		if(Input.GetMouseButtonDown(0)){
			detectClick();
		}
	}

	public float getAtk(){
		return this.atk + (this.str * 2);
	}

	public float getMatk(){
		return this.matk + (this.itg * 2);
	}

	public float getMaxHP(){
		return this.maxHP + (this.vit * 5);
	}

	public float getMaxMP(){
		return this.maxMP + (this.itg * 5);
	}

	public float getDef(){
		return this.def + (this.vit / 2);
	}

	public float getMdef(){
		return this.def + (this.itg / 2) + (this.vit / 2);
	}

	public float getHit(){
		return this.hit + this.dex + (this.luk / 2);
	}

	public float getFlee(){
		return this.flee + this.agi + (this.luk / 2);
	}

	public float getCritChance(){
		return this.critChance + this.luk;
	}

	public float getCritDamage(){
		return this.critDmg + (this.str / 20) + (this.dex / 15);
	}

	public float getCurrHP(){
		return this.currHP;
	}

	public float getCurrMP(){
		return this.currMP;
	}

	public bool addSkill(string id){
		return addSkill(id, Singleton.Instance.allSkills[id]);
	}

	public bool addSkill(Skill skill){
		return addSkill(skill.id, skill);
	}

	public bool addSkill(string id, Skill skill){
		bool success;
		if(!this.skills.ContainsKey(id)){
			success = skill.levelUp(); // for set it to lv 1
			this.skills.Add(id, skill);
		}
		else{
			success = this.skills[id].levelUp();
		}

		return success;
	}

	public float reduceStat(string stat, float quantity){
		switch (stat){
			case StatName.MAXHP:
				this.maxHP -= quantity;
				break;
			case StatName.MAXMP:
				this.maxMP -= quantity;
				break;
			case StatName.CURRHP:
				this.receiveDamage(quantity);
				break;
			case StatName.CURRMP:
				this.reduceMana(quantity);
				break;

			case StatName.STR:
				this.str -= quantity;
				break;
			case StatName.AGI:
				this.agi -= quantity;
				break;
			case StatName.VIT:
				this.vit -= quantity;
				break;
			case StatName.INT:
				this.itg -= quantity;
				break;
			case StatName.DEX:
				this.dex -= quantity;
				break;
			case StatName.LUK:
				this.luk -= quantity;
				break;

			case StatName.ATK:
				this.atk -= quantity;
				break;
			case StatName.MATK:
				this.matk -= quantity;
				break;
			case StatName.DEF:
				this.def -= quantity;
				break;
			case StatName.MDEF:
				this.mdef -= quantity;
				break;
			case StatName.HIT:
				this.hit -= quantity;
				break;
			case StatName.FLEE:
				this.flee -= quantity;
				break;
			case StatName.CRITCHANCE:
				this.critChance -= quantity;
				break;
			case StatName.CRITDAMAGE:
				this.critDmg -= quantity;
				break;

			case StatName.HP_REGEN:
				this.hpRegen -= quantity;
				break;
			case StatName.MP_REGEN:
				this.mpRegen -= quantity;
				break;

			default:
				break;
		}

		return quantity;
	}

	public float reducePercentStat(string stat, float quantity){
		float difference = 0;
		
		switch (stat){
		case StatName.MAXHP:
			difference = maxHP * (quantity/100);
			this.maxHP -= difference;			 
			break;
		case StatName.MAXMP:
			difference = maxMP * (quantity/100);
			this.maxMP -= difference;
			break;
		case StatName.CURRHP:
			difference = currMP * (quantity/100);
			this.receiveDamage(difference);
			break;	
		case StatName.CURRMP:
			difference = currMP * (quantity/100);
			this.reduceMana(difference);
			break;
			
		case StatName.STR:
			difference = str * (quantity/100);
			this.str -= difference;
			break;
		case StatName.AGI:
			difference = agi * (quantity/100);
			this.agi -= difference;
			break;
		case StatName.VIT:
			difference = vit * (quantity/100);
			this.vit -= difference;
			break;
		case StatName.INT:
			difference = itg * (quantity/100);
			this.itg -= difference;
			break;
		case StatName.DEX:
			difference = dex * (quantity/100);
			this.dex -= difference;
			break;
		case StatName.LUK:
			difference = luk * (quantity/100);
			this.luk -= difference;
			break;
			
		case StatName.ATK:
			difference = atk * (quantity/100);
			this.atk -= difference;
			break;
		case StatName.MATK:
			difference = matk * (quantity/100);
			this.matk -= difference;
			break;
		case StatName.DEF:
			difference = def * (quantity/100);
			this.def -= difference;
			break;
		case StatName.MDEF:
			difference = mdef * (quantity/100);
			this.mdef -= difference;
			break;
		case StatName.HIT:
			difference = hit * (quantity/100);
			this.hit -= difference;
			break;
		case StatName.FLEE:
			difference = flee * (quantity/100);
			this.flee -= difference;
			break;
		case StatName.CRITCHANCE:
			difference = critChance * (quantity/100);
			this.critChance -= difference;
			break;
		case StatName.CRITDAMAGE:
			difference = critDmg * (quantity/100);
			this.critDmg -= difference;
			break;			
			
		case StatName.HP_REGEN:
			difference = hpRegen * (quantity/100);
			this.hpRegen -= difference;
			break;
		case StatName.MP_REGEN:
			difference = mpRegen * (quantity/100);
			this.mpRegen -= difference;
			break;
		default:
			break;
		}
		
		return difference;
	}

	public float increaseStat(string stat, float quantity){
		switch (stat){
		case StatName.MAXHP:
			this.maxHP += quantity;
			break;
		case StatName.MAXMP:
			this.maxMP += quantity;
			break;
		case StatName.CURRHP:
			this.restoreHP(quantity);
			break;
		case StatName.CURRMP:
			this.restoreMP(quantity);
			break;
		case StatName.STR:
			this.str += quantity;
			break;
		case StatName.AGI:
			this.agi += quantity;
			break;
		case StatName.VIT:
			this.vit += quantity;
			break;
		case StatName.INT:
			this.itg += quantity;
			break;
		case StatName.DEX:
			this.dex += quantity;
			break;
		case StatName.LUK:
			this.luk += quantity;
			break;
			
		case StatName.ATK:
			this.atk += quantity;
			break;
		case StatName.MATK:
			this.matk += quantity;
			break;
		case StatName.DEF:
			this.def += quantity;
			break;
		case StatName.MDEF:
			this.mdef += quantity;
			break;
		case StatName.HIT:
			this.hit += quantity;
			break;
		case StatName.FLEE:
			this.flee += quantity;
			break;
		case StatName.CRITCHANCE:
			this.critChance += quantity;
			break;
		case StatName.CRITDAMAGE:
			this.critDmg += quantity;
			break;

		case StatName.HP_REGEN:
			this.hpRegen += quantity;
			break;
		case StatName.MP_REGEN:
			this.mpRegen += quantity;
			break;
			
		default:
			break;
		}

		return quantity;
	}
	
	public float increasePercentStat(string stat, float quantity){
		float difference = 0;

		switch (stat){
		case StatName.MAXHP:
			difference = maxHP * (quantity/100);
			this.maxHP += difference;
			break;
		case StatName.MAXMP:
			difference = maxMP * (quantity/100);
			this.maxMP += difference;
			break;
		case StatName.CURRHP:
			difference = currHP * (quantity/100);
			this.restoreHP(difference);
			break;
		case StatName.CURRMP:
			difference = currMP * (quantity/100);
			this.restoreMP(difference);
			break;			
		case StatName.STR:
			difference = str * (quantity/100);
			this.str += difference;
			break;
		case StatName.AGI:
			difference = agi * (quantity/100);
			this.agi += difference;
			break;
		case StatName.VIT:
			difference = vit * (quantity/100);
			this.vit += difference;
			break;
		case StatName.INT:
			difference = itg * (quantity/100);
			this.itg += difference;
			break;
		case StatName.DEX:
			difference = dex * (quantity/100);
			this.dex += difference;
			break;
		case StatName.LUK:
			difference = luk * (quantity/100);
			this.luk += difference;
			break;
			
		case StatName.ATK:
			difference = atk * (quantity/100);
			this.atk += difference;
			break;
		case StatName.MATK:
			difference = matk * (quantity/100);
			this.matk += difference;
			break;
		case StatName.DEF:
			difference = def * (quantity/100);
			this.def += difference;
			break;
		case StatName.MDEF:
			difference = mdef * (quantity/100);
			this.mdef += difference;
			break;
		case StatName.HIT:
			difference = hit * (quantity/100);
			this.hit += difference;
			break;
		case StatName.FLEE:
			difference = flee * (quantity/100);
			this.flee += difference;
			break;
		case StatName.CRITCHANCE:
			difference = critChance * (quantity/100);
			this.critChance += difference;
			break;
		case StatName.CRITDAMAGE:
			difference = critDmg * (quantity/100);
			this.critDmg += difference;
			break;		
			
		case StatName.HP_REGEN:
			difference = hpRegen * (quantity/100);
			this.hpRegen += difference;
			break;
		case StatName.MP_REGEN:
			difference = mpRegen * (quantity/100);
			this.mpRegen += difference;
			break;
		default:
			break;
		}

		return difference;
	}

	// BATTLE METHODS
	
	public bool isCritical(){
		int random = Random.Range(0,100);

		return random >= 0 && random <= this.critChance;
	}

	public void restoreHP(float heal){
		if(this.currHP < this.maxHP){
			this.currHP += heal;
			
			if(this.currHP > this.maxHP)
				this.currHP = this.maxHP;			
		}
	}

	public void restoreMP(float heal){
		if(this.currMP < this.maxMP){
			this.currMP += heal;
			
			if(this.currMP > this.maxMP)
				this.currMP = this.maxMP;			
		}
	}
	
	public void basicAttack(Character enemy){	
		BattleManager.Instance.startCurrentAttack();
		
		if(hits(enemy)){
			float damage = this.atk;
			
			damage = addPercentVariation(damage, PERCENT_DAMAGE_VARIATION);
			
			if(isCritical())
				damage *= this.critDmg;			

			damage -= enemy.def;
			
			if(damage <= 0)
				damage = 1;			
			
			enemy.receiveDamage(damage);
		}
		else{
			Gamestate.instance.showMessage(isPlayer() ? this.characterName : LanguageManager.Instance.getMenuText("enemy_status") + " " + LanguageManager.Instance.getMenuText("failed_attack"));
			BattleManager.Instance.finishCurrentAttack();
		}
	}
	
	public float getElementalModifier(int attackElement, int enemyElement){
		return Singleton.Instance.elementalModifiers[attackElement, enemyElement];
	}
	
	public float addPercentVariation(float damage, float percent){
		return Random.Range(damage-(damage*percent/100), damage+(damage*percent/100));
	}
	
	private bool hits(Character enemy){
		float rand = Random.Range(1,100); // Min 1%, Max 99%;
		float result = enemy.flee - this.hit;
		bool hits = false;
		
		if(result <= 0 && rand < 100)
			hits = true;		
		else if(result > 0){
			float prob = 100 - result;
			
			if(rand <= prob)
				hits = true;			
		}
		
		return hits;
	}

	public void useItemInBattle(string itemId, Character target){
		Singleton.Instance.inventory.useItemInBattle(itemId, target);
	}
	
	public void useSkill(string skillId, Character target){
		if(this.hasSkill(skillId)){
			List<Character> targets = new List<Character>();

			Skill skill = skills[skillId];
			if(skill.target == Target.GROUP){
				GameObject[] gos = GameObject.FindGameObjectsWithTag(target.gameObject.tag);

				foreach(GameObject go in gos)
					targets.Add(go.GetComponent<Character>());				
			}
			else
				targets.Add(target);			

			if(!BattleManager.Instance.attackStarted){
				if(skill.mp <= this.currMP){
					BattleManager.Instance.startCurrentAttack();
					GameObject instance = (GameObject)Instantiate(Resources.Load("Prefabs/"+skill.damageType+"/"+skillId));
					
					this.currMP -= skill.mp;
					
					if(skill.idType == Skill.Type.ACTIVE_DAMAGE_AND_ADD_STATUS)
						useDamageSkill(skill, targets, skill.status, instance);					
				}
			}
		}
		else{
			BattleManager.Instance.skill = false;
			BattleManager.Instance.changePhase(BattleManager.BattlePhases.CHOSEACTION);
		}
	}
	
	public void useDamageSkill(Skill skill, List<Character> targets, GameObject skillPrefab){
		useDamageSkill(skill, targets, null, skillPrefab);
	}
	
	public void useDamageSkill(Skill skill, List<Character> targets, string status, GameObject skillPrefab){
		bool physic = true;
		float modifier;
		float damage = 0f;

		if(skill.damageType == "Physic") 
			physic = true;		
		else if(skill.damageType == "Magic")
			physic = false;
		
		foreach(Character target in targets){
			modifier = getElementalModifier(skill.element, target.element);

			if(physic){
				damage = this.atk * (skill.damage/100);
				damage -= target.def;
				Debug.Log("Skill: " + skill.name + ", Modifier: " + modifier + ", ATK: " + this.atk + " * " + skill.damage + ", MonsDEF: " + target.def + ", DMG: " + (damage*modifier))	;
			}
			else {
				damage = this.matk * (skill.damage/100);
				damage -= target.mdef;
				Debug.Log("Skill: " + skill.name + ", Modifier: " + modifier + ", MATK: " + this.matk + " * " + skill.damage + "%, MonsMDEF: " + target.mdef + ", DMG: " + (damage*modifier));
			}

			damage *= modifier;

			skillPrefab.SetActive(true);
			skillPrefab.GetComponent<MagicMovement>().useMagic(this, target, damage, modifier, status, skill);
		}

		//Character skillTarget = GameObject.FindGameObjectWithTag("Monster2").GetComponent<Character>();

		// RETOCAR PARA QUE VAYA AL DEL MEDIO. USAR NUMERO DE TAG
	}
	
	public void doElementalDamage(float damage, float modifier, string status, float chance, bool isItem){
		string message = "";
		switch((int)(modifier*100)) {
			case 0:
				message = LanguageManager.Instance.getMenuText("attack_no_effect");
				break;
			case 50: 
				message = LanguageManager.Instance.getMenuText("attack_no_effective");
				break;
			case 100: 
				break;
			case 200: 
				message = LanguageManager.Instance.getMenuText("attack_effective");
				break;
		}
		if (message != "")
			Gamestate.instance.showMessage(message);
		
		if(status != null && damage < this.currHP)
			trySetAlteredStatus(status, chance);		
		
		this.receiveDamage(isItem, damage);
	}
	
	private void trySetAlteredStatus(string status, float chance){
		
		int rand = Random.Range(0,101);
		
		if(rand <= chance){ // Success
			if(!this.alteredStatus.ContainsKey(status))
				this.addAlteredStatus(Singleton.Instance.allAlteredStatus[status]);			
			else
				this.alteredStatus[status].resetDuration();			
			
			Gamestate.instance.showMessage(LanguageManager.Instance.getStatus(LanguageManager.Instance.getMenuText(this.name), getStatusName(status)));
		}
	}

	public string getStatusName(string status){
		string[] allStatus = status.Split('_');

		return allStatus[allStatus.Length-1];
	}
	
	private void setStatusMessage(string status){
		string message = LanguageManager.Instance.getStatus(this.name, status.ToLower());
		
		Debug.Log (message);
	}
	
	public void receivePercentDamage(float damage){
		receiveDamage(false, this.maxHP * (damage/100), true);
	}
	
	public void receiveDamage(bool isItem, float damage){
		receiveDamage(isItem, damage, false);
	}
	
	public void receiveDamage(float damage){
		receiveDamage(false, damage, false);
	}

	public void receiveDamage(bool isItem, float damage, bool trueDamage){
		bool goingToDie = false;
		if(!BattleManager.Instance.damageReceived){
			showBattleData();
			
			if(this.isDefending() && !trueDamage)
				damage /= 2;			

			damage = applyDifficultyDamage(damage, OptionsManager.Instance.getDifficulty());

			this.currHP -= damage;
			
			if(this.currHP <= 0){
				this.currHP = 0;
				goingToDie = true;
			}
			
			showBattleData();
			
			if(!isItem)
				BattleManager.Instance.damageReceived = true;
		}
		
		if(!this.isPlayer()){
			gameObject.GetComponent<MonsterBehaviour>().damageStarted();
			gameObject.GetComponent<MonsterBehaviour>().receiveDamage();
		}
		else{
			gameObject.GetComponent<PlayerBehaviour>().damageStarted();
			gameObject.GetComponent<PlayerBehaviour>().receiveDamage();
		}
		
		if(goingToDie)
			this.die();

		/*Debug.Log("alive: " + alive);
		Debug.Log("BattleManager.Instance.attackFinished: " + BattleManager.Instance.attackFinished);
		Debug.Log("alive && BattleManager.Instance.attackFinished: " + (alive && BattleManager.Instance.attackFinished));
		Debug.Log("!alive && BattleManager.Instance.deathFinished: " + (!alive && BattleManager.Instance.deathFinished));*/

		if(!isItem && ((alive && BattleManager.Instance.attackFinished) || (!alive && BattleManager.Instance.deathFinished) || (BattleManager.Instance.skill) || (BattleManager.Instance.defend))) {
			BattleManager.Instance.finishCurrentAttack();			
			BattleManager.Instance.changePhase(BattleManager.BattlePhases.DOACTION);
		}
	}

	private float applyDifficultyDamage(float damage, string difficulty){
		float newDamage = 0;

		if(difficulty.Equals(OptionsManager.Difficulty.EASY)){
			if(this.isPlayer())
				newDamage = damage - (damage * DIFFICULTY_DAMAGE_VARIATION); // total - 25%
			else
				newDamage = damage + (damage * DIFFICULTY_DAMAGE_VARIATION); // total + 25%
		}
		else if(difficulty.Equals(OptionsManager.Difficulty.HARD)){
			if(this.isPlayer())
				newDamage = damage + (damage * DIFFICULTY_DAMAGE_VARIATION); // total + 25%			
			else
				newDamage = damage - (damage * DIFFICULTY_DAMAGE_VARIATION); // total - 25%			
		}			
		else if(difficulty.Equals(OptionsManager.Difficulty.NORMAL))
			newDamage = damage;		
		else{
			Debug.Log("Using not defined difficulty. DEFAULT: Normal");	
			newDamage = damage;
		}

		return newDamage;
	}

	public void reduceMana(float damage){
		if(!BattleManager.Instance.damageReceived){
			showBattleData();

			this.currMP -= damage;
			
			if(this.currMP <= 0)
				this.currMP = 0;			
			
			showBattleData();
			
			BattleManager.Instance.damageReceived = true;
		}
	}

	public void receiveItemUsage(Item item){
		switch(item.idType){
			case Item.Type.HEAL:
				this.restoreHP(item.heal);
				break;
			case Item.Type.DAMAGE:
				float modifier = getElementalModifier(item.element, this.element);
				this.doElementalDamage(item.damage, modifier, item.status, item.chance, true);
				break;
		}
	}

	public void defend(){
		defending = true;
	}

	public bool isDefending(){
		return defending;
	}
	
	private void showBattleData(){
		if(isPlayer())
			BattleManager.Instance.setGUIPlayerInfo((Player)this);		
		else
			BattleManager.Instance.setGUIEnemyInfo((Monster)this);		
	}
	
	public void die(){
		if (isAlive()){
			Debug.Log(this.characterName + " is dying!");
			alive = false;

			if (this.isPlayer()){
				gameObject.GetComponent<PlayerBehaviour>().deathStarted();
				gameObject.GetComponent<PlayerBehaviour>().die();
			}
			else{
				gameObject.GetComponent<MonsterBehaviour>().deathStarted();
				gameObject.GetComponent<MonsterBehaviour>().die();
			}
		}
		else
			Debug.Log(this.characterName + " is already dead!");
	}
	
	public bool isAlive(){
		return this.alive;
	}

	public bool hasSkill(string skillId){
		return this.skills.ContainsKey(skillId);
	}

	public bool isProvoked(){
		return this.alteredStatus.ContainsKey(AlteredStatus.Name.PROVOKE);
	}
	
	public void addAlteredStatus(AlteredStatus status){
		this.alteredStatus.Add(status.id, status);
	}

	public void removeAlteredStatus(string statusName){
		if(hasAlteredStatus(statusName))
			this.alteredStatus[statusName].cure(this);		
	}

	public bool hasAlteredStatus(string name){
		return this.alteredStatus.ContainsKey(name);
	}

	public bool hasSomeAlteredStatus(){
		return alteredStatus.Count > 0;
	}

	public void affectAlteredStatus(){
		foreach(KeyValuePair<string, AlteredStatus> status in this.alteredStatus)
			status.Value.affect(this);		
	}

	public void startTurn(){
		if(this.isAlive()){
			this.restoreMP(this.maxMP * mpRegen);
			this.restoreHP(this.maxHP * hpRegen);		

			if(this.hasSomeAlteredStatus())
				this.affectAlteredStatus();			
		}

		BattleManager.Instance.changePhase(BattleManager.BattlePhases.CHOSEACTION);
	}

	public bool isPlayer(){
		return bIsPlayer;
	}
	
	public int decideAction(){
		return Actions.BASIC_ATTACK;
	}
	
	public Character decideObjective(){
		if(this.isPlayer() && BattleManager.Instance.currentPhase == BattleManager.BattlePhases.CHOSEOBJECTIVE)
			objective = this;		
		else if(!this.isPlayer() && BattleManager.Instance.currentPhase == BattleManager.BattlePhases.CHOSEOBJECTIVE){
			numObjective = Random.Range(0, BattleManager.Instance.numPlayers);
			objective = BattleManager.Instance.getPlayerInBattle(numObjective);

			if (objective == null || !objective.isAlive())
				return null;
		}

		return objective;
	}

	private void detectClick(){
		if(Gamestate.instance.isBattleLevel()){
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			if(collider2D.OverlapPoint(mousePosition)){				
				if (BattleManager.Instance.currentPhase == BattleManager.BattlePhases.CHOSEOBJECTIVE){						
					if(BattleManager.Instance.enemyObjective != null && BattleManager.Instance.enemyObjective.gameObject.Equals(this.gameObject))// && !BattleManager.Instance.enemyObjective.isPlayer())
						BattleManager.Instance.attackObjective = true;					
					else{
						BattleManager.Instance.enemyObjective = this;						
						BattleManager.Instance.currentObjective = BattleManager.Instance.enemyObjective;
					}
				}			
			}
		}
	}

	public void cleanVariables(){
		//clickedObjective = null;
		objective = null;
		numObjective = -1;
	}

	public void showInfo(){
		if(isPlayer())
			BattleManager.Instance.setGUIPlayerInfo((Player)this);		
		else
			BattleManager.Instance.setGUIEnemyInfo((Monster)this);		
	}

	public void finishCurrentAttack(){
		attackFinished = true;
	}

	public List<Skill> getSkillsByBranch(string branch) {
		return this.skills.Where(x => x.Value.branch.Equals(branch)).Select(y => y.Value).ToList<Skill>();
	}

	public string getBranch(string skillName) {
		return this.skills.Where(x => x.Key.Equals(skillName)).Select(y => y.Value.branch).FirstOrDefault().ToString();
	}

	public class Stat{		
		public static int STR = 0;
		public static int AGI = 1;
		public static int DEX = 2;
		public static int INT = 3;
		public static int VIT = 4;
		public static int LUK = 5;
		public static int HP = 6;
		public static int MP = 7;
		public static int ATK = 8;
		public static int MATK = 9;
		public static int DEF = 10;
		public static int MDEF = 11;
		public static int HIT = 12;
		public static int FLEE = 13;
		public static int CRITCHANCE = 14;
		public static int CRITDAMAGE = 15;
	}	
	
	public class StatName{		
		public const string STR = "str";
		public const string AGI = "agi";
		public const string DEX = "dex";
		public const string INT = "itg";
		public const string VIT = "vit";
		public const string LUK = "luk";
		public const string MAXHP = "maxHP";
		public const string MAXMP = "maxMP";
		public const string CURRHP = "currHP";
		public const string CURRMP = "currMP";
		public const string ATK = "atk";
		public const string MATK = "matk";
		public const string DEF = "def";
		public const string MDEF = "mdef";
		public const string HIT = "hit";
		public const string FLEE = "flee";
		public const string CRITCHANCE = "critChance";
		public const string CRITDAMAGE = "critDamage";
		public const string HP_REGEN = "hpRegen";
		public const string MP_REGEN = "mpRegen";
	}

	public class Actions{
		public const int BASIC_ATTACK = 1;
		public const int SKILL = 2;
		public const int USE_OBJECT = 3;
		public const int RUN = 4;
		public const int DEFEND = 5;
	}

	public class Target{
		public const string SINGLE = "Single";
		public const string GROUP = "Group";
	}
}

