using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SkillInfo{

	public int idType; // 1

	public string id; // skill_name_bulwark
	public string skillName; // Bulwark

	public float[] level1; // [60, 60]
	public float[] level2; // [70, 70]
	public float[] level3; // [80, 80]
	public float[] level4; // [90, 90]
	public float[] level5; // [100, 100]
	public string[] evo; // [DEF, MDEF]
	
	public int numArguments;
	public int maxLevel;

	public SkillInfo(){
		maxLevel = 0;
	}
	
	public void setLevel(int level, string data){		
		if(data != null && !data.Equals("")){
			switch(level){
				case 1:
					level1 = parseFloatArray(data.Split(new char[]{'/'}));
					break;
				case 2:
					level2 = parseFloatArray(data.Split(new char[]{'/'}));
					break;
				case 3:
					level3 = parseFloatArray(data.Split(new char[]{'/'}));
					break;
				case 4:
					level4 = parseFloatArray(data.Split(new char[]{'/'}));
					break;
				case 5:
					level5 = parseFloatArray(data.Split(new char[]{'/'}));
					break;
			}
			
			maxLevel++;
		}
	}
	
	public void setEvo(string data){
		evo = data.Split(new char[]{'/'});
		numArguments = evo.Length;
	}
	
	public float[] parseFloatArray(string[] data){
		int len = data.Length;
		float[] parsed = new float[len];
	
		for(int i = 0; i < len; i++){
			parsed[i] = float.Parse(data[i]);
		}

		return parsed;
	}
	
	public float[] getCurrentLevelInfo(int currLevel){
		float[] currentLevelInfo = null;
		
		switch(currLevel){
			case 1:
				currentLevelInfo = this.level1;
				break;
			case 2:
				currentLevelInfo = this.level2;
				break;
			case 3:
				currentLevelInfo = this.level3;
				break;
			case 4:
				currentLevelInfo = this.level4;
				break;
			case 5:
				currentLevelInfo = this.level5;
				break;		
		}
		
		return currentLevelInfo;
	}

	public void populate(string[] row){
		this.id = row[0];
		//this.skillName = row[0];		
		this.idType = Convert.ToInt32(row[1]);
		
		this.setLevel(1, row[2]);
		this.setLevel(2, row[3]);
		this.setLevel(3, row[4]);
		this.setLevel(4, row[5]);
		this.setLevel(5, row[6]);
		
		this.setEvo(row[7]);
	}

	/*switch(evo){
			case Character.StatName.ATK:
				break;
			case Character.StatName.MATK:			
				break;
			case Character.StatName.DEF:
				break;
			case Character.StatName.MDEF:
				break;
			case Character.StatName.FLEE:
				break;
			case Character.StatName.HIT:
				break;
			case Character.StatName.CRITCHANCE:
				break;
			case Character.StatName.CRITDAMAGE:
				break;

			case Character.StatName.HP_REGEN:
				break;
			case Character.StatName.MP_REGEN:
				break;

			case SkillEvo.TURNS:
				break;
			case SkillEvo.STATUS:
				break;
			case SkillEvo.HITS:
				break;
			case SkillEvo.HEAL:
				break;
			case SkillEvo.DAMAGE:
				break;
			}*/
	
	/*public float bonusDamage; // +X% MATK, +X% ATK
	public float[] bonusStat; // +X% DEF
	public float bonusApplyChance; // +X% Bleed
	public float bonusDuration; // +X Turns
	public float bonusHit; // +X Hits
	public float bonusHeal; // +X% HP
	public float bonusLevel; // Creation of X level
	public float bonusUseChance; // +X% of block

	public SkillInfo(){

	}

	public SkillInfo(string skillName, float[] bonusStat, float bonusHeal, float bonusDuration){
		idType = Skill.Type.ACTIVE_HELP;

		this.skillName = skillName;
		this.bonusStat = bonusStat;
		this.bonusHeal = bonusHeal;
		this.bonusDuration = bonusDuration;
	}

	public SkillInfo(string skillName, float bonusApplyChance, float bonusDuration){
		idType = Skill.Type.ACTIVE_ADD_STATUS;
		
		this.skillName = skillName;
		this.bonusApplyChance = bonusApplyChance;
		this.bonusDuration = bonusDuration;
	}

	public SkillInfo(string skillName, float bonusDamage, float bonusHit, float bonusHeal){
		idType = Skill.Type.ACTIVE_DAMAGE;
		
		this.skillName = skillName;
		this.bonusDamage = bonusDamage;
		this.bonusHit = bonusHit;
		this.bonusHeal = bonusHeal;
	}

	public SkillInfo(string skillName, float bonusStat){
		idType = Skill.Type.PASSIVE_BONUS_STAT;
		
		this.skillName = skillName;
		this.bonusStat = bonusStat;
	}

	public SkillInfo(string skillName, float bonusLevel){
		idType = Skill.Type.NO_TARGET;
		
		this.skillName = skillName;
		this.bonusLevel = bonusLevel;
	}

	public SkillInfo(string skillName, float bonusDamage, float bonusHit, float bonusHeal, float bonusApplyChance, float bonusDuration){
		idType = Skill.Type.ACTIVE_DAMAGE_AND_ADD_STATUS;
		
		this.skillName = skillName;
		this.bonusDamage = bonusDamage;
		this.bonusHit = bonusHit;
		this.bonusHeal = bonusHeal;
		this.bonusApplyChance = bonusApplyChance;
		this.bonusDuration = bonusDuration;
	}

	public SkillInfo(string skillName, float bonusUseChance, float bonusDuration){
		idType = Skill.Type.PASSIVE_ADD_STATUS;
		
		this.skillName = skillName;
		this.bonusUseChance = bonusUseChance;
		this.bonusDuration = bonusDuration;
	}*/
}