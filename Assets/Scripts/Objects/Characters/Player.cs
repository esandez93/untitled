using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Player : Character{
	public Animator animator;

	public static int SKILL_POINTS_PER_LEVEL = 1;
	public static int STAT_POINTS_PER_LEVEL = 3;

	public int job;
	public float exp;
	public float expForNextLevel;

	public int skillPoints;
	public int statPoints;

	public override void Start(){
		base.Start();

		bIsPlayer = true;

		body = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}
	
	public override void Update(){
		base.Update();
	}

	public void initializePlayer(int job){
		this.job = job;

		this.level = 1;
		this.exp = 0;
		
		this.element = Singleton.Element.NEUTRAL;
		this.str = Singleton.Instance.statsPerLv[Character.Stat.STR, this.job];
		this.agi = Singleton.Instance.statsPerLv[Character.Stat.AGI, this.job];
		this.dex = Singleton.Instance.statsPerLv[Character.Stat.DEX, this.job];
		this.itg = Singleton.Instance.statsPerLv[Character.Stat.INT, this.job];
		this.vit = Singleton.Instance.statsPerLv[Character.Stat.VIT, this.job];
		this.luk = Singleton.Instance.statsPerLv[Character.Stat.LUK, this.job];
		
		this.maxHP += Singleton.Instance.statsPerLv[Character.Stat.HP, this.job] + getMaxHP();
		this.currHP += this.maxHP;//Singleton.Instance.statsPerLv[Character.Stat.HP, this.job] + getMaxHP();
		this.maxMP += Singleton.Instance.statsPerLv[Character.Stat.MP, this.job] + getMaxMP();
		this.currMP += this.maxMP;//Singleton.Instance.statsPerLv[Character.Stat.MP, this.job] + getMaxMP();
		
		this.atk += Singleton.Instance.statsPerLv[Character.Stat.ATK, this.job] + getAtk();
		this.matk += Singleton.Instance.statsPerLv[Character.Stat.MATK, this.job] + getMatk();
		
		this.def = getDef();
		this.mdef = getMdef();
		this.hit = getHit();
		this.flee = getFlee();
		this.critChance = getCritChance();
		this.critDmg = getCritDamage();

		expForNextLevel = Singleton.Instance.expNeeded[this.level];
	}
	
	public void levelUp(){
		Debug.Log(this.characterName + " leveled up!");

		level++;
		this.str += Singleton.Instance.statsPerLv[Character.Stat.STR, this.job];
		this.agi += Singleton.Instance.statsPerLv[Character.Stat.AGI, this.job];
		this.dex += Singleton.Instance.statsPerLv[Character.Stat.DEX, this.job];
		this.itg += Singleton.Instance.statsPerLv[Character.Stat.INT, this.job];
		this.vit += Singleton.Instance.statsPerLv[Character.Stat.VIT, this.job];
		this.luk += Singleton.Instance.statsPerLv[Character.Stat.LUK, this.job];
		
		this.maxHP += Singleton.Instance.statsPerLv[Character.Stat.HP, this.job];
		this.currHP += Singleton.Instance.statsPerLv[Character.Stat.HP, this.job];
		this.maxMP += Singleton.Instance.statsPerLv[Character.Stat.MP, this.job];
		this.currMP += Singleton.Instance.statsPerLv[Character.Stat.MP, this.job];
		
		this.atk += Singleton.Instance.statsPerLv[Character.Stat.ATK, this.job];
		this.matk += Singleton.Instance.statsPerLv[Character.Stat.MATK, this.job];

		this.skillPoints += SKILL_POINTS_PER_LEVEL;
		this.statPoints += STAT_POINTS_PER_LEVEL;

		expForNextLevel = Singleton.Instance.expNeeded[this.level];
	}
	
	public void populate(PlayerData data){
		this.bIsPlayer = data.bIsPlayer;

		this.characterName = data.characterName;
		this.level = data.level;
		this.element = data.element;
		
		this.maxHP = data.maxHP;
		this.maxMP = data.maxMP;
		this.currHP = data.currHP;
		this.currMP = data.currMP;
		
		this.str = data.str;
		this.agi = data.agi;
		this.vit = data.vit;
		this.itg = data.itg;
		this.dex = data.dex;
		this.luk = data.luk;
		
		this.atk = data.atk;
		this.matk = data.matk;
		this.def = data.def;
		this.mdef = data.mdef;
		this.hit = data.hit;
		this.flee = data.flee;
		this.critChance = data.critChance;
		this.critDmg = data.critDmg;
		
		this.hpRegen = data.hpRegen;
		this.mpRegen = data.mpRegen;
		
		this.alive = data.alive;
		
		this.skills = data.skills;
		
		this.alteredStatus = data.alteredStatus;
		
		this.job = data.job;
		this.exp = data.exp;
		this.expForNextLevel = data.expForNextLevel;
		
		this.skillPoints = data.skillPoints;
		this.statPoints = data.statPoints;
	}

	public PlayerData getData(){
		return new PlayerData((Player)this);
	}
	
	public void getExp(float exp){
		this.exp += exp;

		while(this.exp >= Singleton.Instance.expNeeded[this.level]){
			this.levelUp();
		}
	}

	public void statUp(string stat){
		if(hasStatPoints()){
			/*switch(stat){

			}*/
		}
	}

	public void skillUp(string skillName){
		if(hasSkill(skillName)){
			skillUp (skills[skillName]);
		}
	}

	public void skillUp(Skill skill){
		if(hasSkillPoints()){
			if(hasSkill(skill.name)){
				if(skill.canLevelUp()){
					skill.levelUp();
				}
			}
			else{
				this.addSkill(skill);
			}

			skillPoints--;
		}
	}

	public bool hasSkillPoints(){
		if(this.skillPoints > 0){
			return true;
		}
		else{
			return false;
		}
	}

	public bool hasStatPoints(){
		if(this.statPoints > 0){
			return true;
		}
		else{
			return false;
		}
	}
	
	/*public void useItem(string itemName, Character target){
		if(Singleton.Instance.inventory.isItemInInventory(itemName)){
			Singleton.Instance.inventory.useItem(itemName, target);

			BattleManager.currentPhase = BattleManager.BattlePhases.DOACTION;
		}
	}*/	

	public void enablePassives(){
		Skill skill;
		Dictionary<string, float> benefits;
		foreach(KeyValuePair<string, Skill> entry in skills){ //for each skill in the player
			skill = entry.Value;

			if(skill.idType == Skill.Type.PASSIVE_BONUS_STAT){
				benefits = skill.getBenefits();

				foreach(KeyValuePair<string, float> benefit in benefits){
					if(Singleton.Instance.exceptionSkills.Contains(skill.name)){ // Don't use percent, just constant numbers
						increaseStat(benefit.Key, benefit.Value);
					}
					else{
						increasePercentStat(benefit.Key, benefit.Value);
					}
				}
			}
			else if(skill.idType == Skill.Type.PASSIVE_ADD_STATUS){
				addAlteredStatus(Singleton.Instance.allAlteredStatus[skill.status]);
			}
		}
	}

	public List<Skill> getSkills(){
		List<Skill> playerSkills = new List<Skill>();

		foreach(KeyValuePair<string, Skill> entry in skills){
			playerSkills.Add(entry.Value);
		}

		return playerSkills;
	}
	
	public void doBasicAttack(Character objective){
		GetComponent<PlayerBehaviour>().basicAttack(objective);
	}

	public void doSkill(Character objective, string skillName){
		GetComponent<PlayerBehaviour>().skill(objective, skillName);
	}

	/*public void doAction(int action, Player objective){
		BattleManager.Instance.damageReceived = false;

		switch(action){
			case Character.Actions.BASIC_ATTACK:
				GetComponent<PlayerBehaviour>().basicAttack(objective);
				break;
			case Character.Actions.Skill:
				GetComponent<PlayerBehaviour>().skill(objective);
				break;


		}
	}*/

	public class Job{		
		public const int MAGE = 0;
		public const int ROGUE = 1;
		public const int KNIGHT = 2;
	}
}