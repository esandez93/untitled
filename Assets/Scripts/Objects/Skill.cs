using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Skill : MonoBehaviour{

	public static string EMPTY = "battle_menu_skills_empty";

	public int idType;

	public string id;
	public string name;
	public int maxLevel;
	public int currLevel;
	public string type;
	public string description;
	public bool usableOutOfCombat;

	public string target;	
	public int mp;
	public float damage;
	public string damageType;
	public int element;

	public float chance;
	public string status;
	public int duration;

	//public List<string> stat;
	//public float[] benefit;
	public Dictionary<string, float> statsBenefits;

	public SkillInfo info;
	//public List<float> benefit;

	public bool initialized = false;

	public Skill (){

	}	
	
	public Skill(string name, int maxLevel, string type, string description, bool usableOutOfCombat, string target, int mp, string stat, string benefit, int duration){
		idType = Type.ACTIVE_HELP;
		
		this.id = name;
		//this.name = LanguageManager.Instance.getMenuText(name);
		this.maxLevel = maxLevel;
		this.type = type;
		this.description = description;
		this.usableOutOfCombat = usableOutOfCombat;
		this.target = target;
		this.mp = mp;
		populateStatsBenefits(stat.Split(new char[] {';'}), benefit.Split(new char[] {';'}));
		//this.stat = stat.Split(new char[] {';'});
		//populateStat(stat.Split(new char[] {';'}));
		//populateBenefit(benefit.Split(new char[] {';'}));
		//populateBenefit(benefit.Split(new char[] {';'}));
		this.duration = duration;
		this.currLevel = 0;

		setInfo();
	}
	
	public Skill(string name, int maxLevel, string type, string description, bool usableOutOfCombat, string target, int mp, float chance, string status, int duration){
		idType = Type.ACTIVE_ADD_STATUS;
		
		this.id = name;
		//this.name = LanguageManager.Instance.getMenuText(name);
		this.maxLevel = maxLevel;
		this.type = type;
		this.description = description;
		this.usableOutOfCombat = usableOutOfCombat;
		this.target = target;
		this.mp = mp;
		this.chance = chance;
		this.status = status;
		this.duration = duration;
		this.currLevel = 0;

		setInfo();
	}
	
	public Skill(string name, int maxLevel, string type, string description, bool usableOutOfCombat, string target, float damage, int element, int mp, string damageType){
		idType = Type.ACTIVE_DAMAGE;
		
		this.id = name;
		//this.name = LanguageManager.Instance.getMenuText(name);
		this.maxLevel = maxLevel;
		this.type = type;
		this.description = description;
		this.usableOutOfCombat = usableOutOfCombat;
		this.target = target;
		this.damage = damage;
		this.element = element;
		this.mp = mp;
		this.damageType = damageType;
		this.currLevel = 0;

		setInfo();
	}
	
	public Skill(string name, int maxLevel, string type, string description, bool usableOutOfCombat, string stat, string benefit){
		idType = Type.PASSIVE_BONUS_STAT;
		
		this.id = name;
		//this.name = LanguageManager.Instance.getMenuText(name);
		this.maxLevel = maxLevel;
		this.type = type;
		this.description = description;
		this.usableOutOfCombat = usableOutOfCombat;
		populateStatsBenefits(stat.Split(new char[] {';'}), benefit.Split(new char[] {';'}));
		//this.stat = stat.Split(new char[] {';'});
		//populateStat(stat.Split(new char[] {';'}));
		//populateBenefit(benefit.Split(new char[] {';'}));
		//populateBenefit(benefit.Split(new char[] {';'}));
		this.currLevel = 0;

		setInfo();
	}

	public Skill(string name, int maxLevel, string type, string description, bool usableOutOfCombat){
		idType = Type.NO_TARGET;

		this.id = name;
		//this.name = LanguageManager.Instance.getMenuText(name);
		this.maxLevel = maxLevel;
		this.type = type;
		this.description = description;
		this.usableOutOfCombat = usableOutOfCombat;
		this.currLevel = 0;

		setInfo();
	}

	public Skill(string name, int maxLevel, string type, string description, bool usableOutOfCombat, string target, float damage, int element, int mp, string damageType, 
	             float chance, string status, int duration){

		idType = Type.ACTIVE_DAMAGE_AND_ADD_STATUS;
		
		this.id = name;
		//this.name = LanguageManager.Instance.getMenuText(name);
		this.maxLevel = maxLevel;
		this.type = type;
		this.description = description;
		this.usableOutOfCombat = usableOutOfCombat;
		this.target = target;
		this.damage = damage;
		this.element = element;
		this.mp = mp;
		this.damageType = damageType;		
		this.chance = chance;
		this.status = status;
		this.duration = duration;
		this.currLevel = 0;

		setInfo();
	}

	public Skill(string name, int maxLevel, string type, string description, bool usableOutOfCombat, string status){
		idType = Type.PASSIVE_ADD_STATUS;
		
		this.id = name;
		//this.name = LanguageManager.Instance.getMenuText(name);
		this.maxLevel = maxLevel;
		this.type = type;
		this.description = description;
		this.usableOutOfCombat = usableOutOfCombat;
		this.status = status;
		this.currLevel = 0;

		setInfo();
	}

	public void updateSkill(){	
		float[] currentLevelInfo = info.getCurrentLevelInfo(currLevel); // currLevel = 3 / info = [80, 80]		

		switch(idType){
		case Type.ACTIVE_DAMAGE_AND_ADD_STATUS:
			updateDamageStatus(currentLevelInfo);
			break;
		case Type.PASSIVE_BONUS_STAT:
			updateBonusStat(currentLevelInfo);
			break;
		case Type.ACTIVE_HELP:
			updateBonusStat(currentLevelInfo);
			break;
		}

		if(name == null){
			getData();
		}
	}

	private void updateDamageStatus(float[] currentLevelInfo){
		int i = 0;
		foreach(string entry in info.evo){
			switch(entry){
			case Character.StatName.MATK:		
				damage = currentLevelInfo[i];
				break;
			case SkillEvo.STATUS:
				chance = 100;//currentLevelInfo[i];
				break;
			}

			i++;
		}
	}

	private void updateBonusStat(float[] currentLevelInfo){
		List<string> keys = new List<string>();//string[] keys = new string[currentLevelInfo.Length];
		foreach(KeyValuePair<string, float> entry in statsBenefits){
			keys.Add(entry.Key);
		}
		statsBenefits.Clear();
		
		int i = 0;
		foreach(string key in keys){
			statsBenefits.Add(key, currentLevelInfo[i]);
			i++;
		}
	}

	private void populateStatsBenefits(string[] stat, string[] benefit){
		statsBenefits = new Dictionary<string, float>();
		int len = stat.Length;

		for(int i = 0; i < len; i++){
			statsBenefits.Add(stat[i], float.Parse(benefit[i]));
		}
	}

	public void setInfo(){
		if(Singleton.Instance.allSkillInfo.ContainsKey(this.id)){
			//Debug.Log (Singleton.Instance.allSkillInfo[this.name].skillName);
			info = Singleton.Instance.allSkillInfo[this.id];
		}
	}

	/*private void populateBenefit(string[] benefit){
		int len = benefit.Length;
		this.benefit = new float[len];
		
		for(int i = 0; i < len; i++){
			this.benefit[i] = Convert.ToInt32(benefit[i]);
		}

		/*this.benefit = new List<float>();

		foreach(string s in benefit){
			this.benefit.Add(Convert.ToInt64(s));

		}
	}

	private void populateStat(string[] stat){
		this.stat = new List<string>();
		
		foreach(string s in stat){
			this.stat.Add(s);
		}
	}*/

	public Dictionary<string, float> getBenefits(){
		if(idType == Type.PASSIVE_BONUS_STAT){
			return statsBenefits;
		}
		else{
			return null;
		}
		/*else if(idType == Type.PASSIVE_ADD_STATUS){
			return status;
		}*/
	}

	public void levelUp(){
		currLevel++;

		updateSkill();
	}

	public bool canLevelUp(){
		if(this.maxLevel > this.currLevel){
			return true;
		}
		else{
			return false;
		}
	}

	public bool isPassive(){
		if(idType == Type.PASSIVE_ADD_STATUS || idType == Type.PASSIVE_BONUS_STAT){
			return true;
		}
		else{
			return false;
		}
	}

	public bool hasBenefits(){
		if(statsBenefits != null){
			return true;
		}
		else{
			return false;
		}
	}

	public void getData(){
		if(!initialized){
			this.name = LanguageManager.Instance.getMenuText(this.id);
			this.description = LanguageManager.Instance.getMenuText(this.description);
			initialized = true;
		}		
	}

	public void translate(){
		this.name = LanguageManager.Instance.getMenuText(this.id);
	}

	public string getStatusName(){
		string[] status = this.status.Split('_');

		return status[status.Length-1];
	}

	public string toString(){
		string res = "";

		res += "Name: " + name + ", chance: " + chance;

		return res;
	}

	public class Type{
		public const int ACTIVE_HELP = 1;
		public const int ACTIVE_ADD_STATUS = 2;
		public const int ACTIVE_DAMAGE = 3;
		public const int PASSIVE_BONUS_STAT = 4;
		public const int NO_TARGET = 5;
		public const int ACTIVE_DAMAGE_AND_ADD_STATUS = 6;
		public const int PASSIVE_ADD_STATUS = 7;
	}

	public class SkillEvo{
		public const string TURNS = "turns";
		public const string STATUS = "status";
		public const string HITS = "hits";
		public const string HEAL = "heal";
		public const string DAMAGE = "damage";
	}
}

