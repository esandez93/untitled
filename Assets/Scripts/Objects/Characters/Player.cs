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

		expForNextLevel = Singleton.expNeeded[this.level];
	}
	
	public override void Update(){
		base.Update();
	}
	
	public void levelUp(){
		level++;
		this.str += Singleton.statsPerLv[Character.Stat.STR, this.job];
		this.agi += Singleton.statsPerLv[Character.Stat.AGI, this.job];
		this.dex += Singleton.statsPerLv[Character.Stat.DEX, this.job];
		this.itg += Singleton.statsPerLv[Character.Stat.INT, this.job];
		this.vit += Singleton.statsPerLv[Character.Stat.VIT, this.job];
		this.luk += Singleton.statsPerLv[Character.Stat.LUK, this.job];
		
		this.maxHP += Singleton.statsPerLv[Character.Stat.HP, this.job];
		this.currHP += Singleton.statsPerLv[Character.Stat.HP, this.job];
		this.maxMP += Singleton.statsPerLv[Character.Stat.MP, this.job];
		this.currMP += Singleton.statsPerLv[Character.Stat.MP, this.job];
		
		this.atk += Singleton.statsPerLv[Character.Stat.ATK, this.job];
		this.matk += Singleton.statsPerLv[Character.Stat.MATK, this.job];

		this.skillPoints += SKILL_POINTS_PER_LEVEL;
		this.statPoints += STAT_POINTS_PER_LEVEL;

		expForNextLevel = Singleton.expNeeded[this.level];
	}
	
	public void getExp(float exp){
		this.exp += exp;

		while(this.exp >= Singleton.expNeeded[this.level]){
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
		if(Singleton.inventory.isItemInInventory(itemName)){
			Singleton.inventory.useItem(itemName, target);

			BattleManager.currentPhase = BattleManager.BattlePhases.DOACTION;
		}
	}*/

	public bool hasSkill(string name){
		if(this.skills.ContainsKey(name)){
			return true;
		}
		else{
			return false;
		}
	}	

	public void enablePassives(){
		Skill skill;
		Dictionary<string, float> benefits;
		foreach(KeyValuePair<string, Skill> entry in skills){ //for each skill in the player
			skill = entry.Value;

			if(skill.idType == Skill.Type.PASSIVE_BONUS_STAT){
				benefits = skill.getBenefits();

				foreach(KeyValuePair<string, float> benefit in benefits){
					if(Singleton.exceptionSkills.Contains(skill.name)){ // Don't use percent, just constant numbers
						increaseStat(benefit.Key, benefit.Value);
					}
					else{
						increasePercentStat(benefit.Key, benefit.Value);
					}
				}
			}
			else if(skill.idType == Skill.Type.PASSIVE_ADD_STATUS){
				addAlteredStatus(Singleton.allAlteredStatus[skill.status]);
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
	
	public class Job{		
		public const int MAGE = 0;
		public const int ROGUE = 1;
		public const int KNIGHT = 2;
	}
}