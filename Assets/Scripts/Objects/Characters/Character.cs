using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] 
public class Character : MonoBehaviour{

	public bool attackFinished;
	public static int PERCENT_DAMAGE_VARIATION = 20;

	public Rigidbody2D body;

	public static Character character;
	public Character clickedObjective;

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

	public bool alive;

	public Dictionary<string, Skill> skills = new Dictionary<string, Skill>();

	public Dictionary<string, AlteredStatus> alteredStatus = new Dictionary<string, AlteredStatus>();

	public virtual void Start(){
		this.level = 1;
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
		return this.maxHP + (this.vit * 10);
	}

	public float getMaxMP(){
		return this.maxMP + (this.itg * 5);
	}

	public float getDef(){
		return this.def + this.vit;
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
		return this.critDmg + (this.str / 2) + (this.dex / 2);
	}

	public float getCurrHP(){
		return this.currHP;
	}

	public float getCurrMP(){
		return this.currMP;
	}

	public void addSkill(string name){
		addSkill(name, Singleton.allSkills[name]);
	}

	public void addSkill(Skill skill){
		addSkill(skill.name, skill);
	}

	public void addSkill(string name, Skill skill){
		if(!this.skills.ContainsKey(name)){
			skill.levelUp(); // for set it to lv 1
			this.skills.Add(name, skill);
		}
	}

	/*private void checkIfStatIsAffected(string stat){
		if(this.){

		}
	}
*/
	public float reduceStat(string stat, float quantity){
		switch (stat){
			case StatName.MAXHP:
				this.maxHP -= quantity;
				break;
			case StatName.MAXMP:
				this.maxMP -= quantity;
				break;
			case StatName.CURRMP:
				this.currMP -= quantity;
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
		case StatName.CURRMP:
			difference = currMP * (quantity/100);
			this.currMP -= currMP *difference;
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
		case StatName.CURRMP:
			this.currMP += quantity;
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
		case StatName.CURRMP:
			difference = currMP * (quantity/100);
			this.currMP += difference;
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
		
		if(random >= 0 && random <= this.critChance){
			return true;
		}
		else{
			return false;
		}
	}

	public void restoreHP(float heal){
		if(this.currHP < this.maxHP){
			this.currHP += heal;
			
			if(this.currHP > this.maxHP){
				this.currHP = this.maxHP;
			}
		}
	}

	public void restoreMP(float heal){
		if(this.currMP < this.maxMP){
			this.currMP += heal;
			
			if(this.currMP > this.maxMP){
				this.currMP = this.maxMP;
			}
		}
	}
	
	public void basicAttack(Character enemy){	
		BattleManager.startCurrentAttack();
		
		if(hits(enemy)){
			bool isCrit = isCritical();
			float damage = this.atk;
			
			damage = addPercentVariation(damage, PERCENT_DAMAGE_VARIATION);
			
			if(isCrit){
				damage *= this.critDmg;
			}

			damage -= enemy.def;
			
			if(damage <= 0){
				damage = 1;
			}
			
			//MonoBehaviour.print("isCrit: " + isCrit + ", ATK: " + this.atk + ", MonsDEF: " + enemy.def + ", DMG: " + damage);
			
			enemy.receiveDamage(damage);
		}
		else{
			Debug.Log ("El ataque de " + this.name + " contra " + enemy.name + " ha fallado!");
			BattleManager.finishCurrentAttack();
		}
	}
	
	public float getElementalModifier(Skill skill, Character enemy){
		int skillElement = skill.element;
		int enemyElement = enemy.element;
		
		return Singleton.elementalModifiers[skillElement, enemyElement];
	}
	
	public float addPercentVariation(float damage, float percent){
		return Random.Range(damage-(damage*percent/100), damage+(damage*percent/100));
	}
	
	private bool hits(Character enemy){
		float rand = Random.Range(1,100); // Min 1%, Max 99%;
		float result = enemy.flee - this.hit;
		bool hits = false;
		
		if(result <= 0 && rand < 100){
			hits = true;
		}
		else if(result > 0){
			float prob = 100 - result;
			
			if(rand <= prob){
				hits = true;
			}
		}
		
		return hits;
	}

	public void useItem(string itemName, Character target){
		Singleton.inventory.useItem(itemName, target);
	}
	
	public void useSkill(string skillName, Character target){
		if(skills.ContainsKey(skillName)){
			Skill skill = skills[skillName];
			
			if(!BattleManager.attackStarted){
				if(skill.mp <= this.currMP){
					BattleManager.startCurrentAttack();
					GameObject instance = (GameObject)Instantiate(Resources.Load("Prefabs/"+skill.damageType+"/"+skillName));
					
					this.currMP -= skill.mp;
					
					if(skill.idType == Skill.Type.ACTIVE_DAMAGE_AND_ADD_STATUS){
						useDamageSkill(skill, target, skill.status, instance);
					}
				}
			}
		}
		else{
			BattleManager.skill = false;
			BattleManager.changePhase(BattleManager.BattlePhases.CHOSEACTION);
		}
	}
	
	public void useDamageSkill(Skill skill, Character target, GameObject skillPrefab){
		useDamageSkill(skill, target, null, skillPrefab);
	}
	
	public void useDamageSkill(Skill skill, Character target, string status, GameObject skillPrefab){
		bool physic = true;
		float modifier = getElementalModifier(skill, target);
		float damage = 0f;
		
		if(skill.damageType == "Physic"){
			physic = true;
		}
		else if(skill.damageType== "Magic"){
			physic = false;
		}
		
		if(physic){
			damage = this.atk * (skill.damage/100);
			damage -= target.def;
			MonoBehaviour.print("Skill: " + skill.name + ", Modifier: " + modifier + ", ATK: " + this.atk + " * " + skill.damage + ", MonsDEF: " + target.def + ", DMG: " + (damage*modifier))	;
		}
		else{
			damage = this.matk * (skill.damage/100);
			damage -= target.mdef;
			MonoBehaviour.print("Skill: " + skill.name + ", Modifier: " + modifier + ", MATK: " + this.matk + " * " + skill.damage + "%, MonsMDEF: " + target.mdef + ", DMG: " + (damage*modifier));
		}
		
		damage *= modifier;
		
		skillPrefab.SetActive(true);
		skillPrefab.GetComponent<MagicMovement>().useMagic(this, target, damage, modifier, status, skill);
	}
	
	public void doElementalDamage(float damage, float modifier, string status, Skill skill){
		switch((int)(modifier*100)){
		case 0: MonoBehaviour.print("No afecta");
			break;
		case 50: MonoBehaviour.print("No es muy efectivo");
			break;
		case 100: MonoBehaviour.print("Efecto normal");
			break;
		case 200: MonoBehaviour.print("Es muy efectivo");
			break;
		}
		
		if(status != null){
			trySetAlteredStatus(skill);
		}
		
		this.receiveDamage(damage);
	}
	
	private void trySetAlteredStatus(Skill skill){
		
		int rand = Random.Range(0,101);
		
		if(rand <= skill.chance){ // Success
			if(!this.alteredStatus.ContainsKey(skill.status)){
				this.addAlteredStatus(Singleton.allAlteredStatus[skill.status]);
			}
			else{
				this.alteredStatus[skill.status].resetDuration();
			}
			
			setStatusMessage(skill.status);
		}
	}
	
	private void setStatusMessage(string status){
		string message = "El enemigo esta ";
		
		switch(status){
		case AlteredStatus.Name.POISON:
			message += "envenenado";
			break;
		case AlteredStatus.Name.BURN:
			message += "quemado";
			break;
		}
		
		message += ".";
		
		Debug.Log (message);
	}
	
	public void receivePercentDamage(float damage){
		receiveDamage(this.maxHP * (damage/100));
	}
	
	public void receiveDamage(float damage){
		if(!BattleManager.damageReceived){
			showBattleData();
			
			this.currHP -= damage;
			
			if(this.currHP <= 0){
				this.currHP = 0;
				this.die();
			}
			
			showBattleData();
			
			BattleManager.damageReceived = true;
		}
		
		if(!this.isPlayer()){
			gameObject.GetComponent<MonsterBehaviour>().damageStarted();
			gameObject.GetComponent<MonsterBehaviour>().receiveDamage();
		}		
		
		if(!alive){
			this.die();
		}
		
		if((alive && BattleManager.attackFinished)|| (!alive && BattleManager.deathFinished)){
			BattleManager.finishCurrentAttack();
		}
	}
	
	private void showBattleData(){
		if(isPlayer()){
			BattleManager.setGUIPlayerInfo((Player)this);
		}
		else{
			BattleManager.setGUIMonsterInfo((Monster)this);
		}
	}
	
	public void die(){		
		alive = false;
		if(BattleManager.deathFinished){
			MonoBehaviour.print(this.name + " is dead!");
			BattleManager.checkIfEnded();
		}
		else{
			if(this.isPlayer()){
				
			}
			else{
				gameObject.GetComponent<MonsterBehaviour>().deathStarted();
				gameObject.GetComponent<MonsterBehaviour>().die();
			}
		}
	}
	
	public bool isAlive(){
		return alive;
	}

	public bool isProvoked(){
		if(this.alteredStatus.ContainsKey(AlteredStatus.Name.PROVOKE)){
			return true;
		}
		else{
			return false;
		}
	}
	
	public void addAlteredStatus(AlteredStatus status){
		this.alteredStatus.Add(status.name, status);
	}

	public void removeAlteredStatus(string statusName){
		if(hasAlteredStatus(statusName)){
			this.alteredStatus[statusName].cure(this);
		}
	}

	public bool hasAlteredStatus(string name){
		if(this.alteredStatus.ContainsKey(name)){
			return true;
		}
		else{
			return false;
		}
	}

	public bool hasSomeAlteredStatus(){
		if(alteredStatus.Count > 0){
			return true;
		}
		else{
			return false;
		}
	}

	public void affectAlteredStatus(){
		foreach(KeyValuePair<string, AlteredStatus> status in this.alteredStatus){
			status.Value.affect(this);
		}
	}

	public void startTurn(){
		if(this.isAlive()){
			this.restoreMP(this.maxMP * mpRegen);
			this.restoreHP(this.maxHP * hpRegen);		

			if(this.hasSomeAlteredStatus()){
				this.affectAlteredStatus();
			}
		}

		BattleManager.changePhase(BattleManager.BattlePhases.CHOSEACTION);
	}

	public bool isPlayer(){
		return bIsPlayer;
	}
	
	public int decideAction(){
		return Actions.BASIC_ATTACK;
	}
	
	public Character decideObjective(){
		if(this.isPlayer() && BattleManager.currentPhase == BattleManager.BattlePhases.CHOSEOBJECTIVE){
			objective = clickedObjective;
		}
		else if(!this.isPlayer() && BattleManager.currentPhase == BattleManager.BattlePhases.CHOSEOBJECTIVE){
			numObjective = Random.Range(0, BattleManager.numPlayers);
			objective = BattleManager.getPlayerInBattle(numObjective);
		}

		return objective;
	}

	private void detectClick(){
		if(!bIsPlayer){
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			if(collider2D.OverlapPoint(mousePosition)){				
				if (BattleManager.currentPhase == BattleManager.BattlePhases.CHOSEOBJECTIVE){
					//if(BattleManager.skill){
						//Monster monster = (Monster)this;
						
						if(clickedObjective != null && clickedObjective == this){
							BattleManager.attackObjective = true;
						}
						else{
							clickedObjective = this;						
							BattleManager.currentObjective = clickedObjective;
						}
					//}
				}			
			}
		}
	}

	public void cleanVariables(){
		clickedObjective = null;
		objective = null;
		numObjective = -1;
	}

	public void showInfo(){
		if(isPlayer()){
			BattleManager.setGUIPlayerInfo((Player)this);
		}
		else{
			BattleManager.setGUIMonsterInfo((Monster)this);
		}
	}

	public void finishCurrentAttack(){
		attackFinished = true;
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
}

