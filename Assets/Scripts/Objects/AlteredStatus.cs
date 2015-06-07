using UnityEngine;
using System.Collections;

public class AlteredStatus {

	public int idType;

	public string id;
	public string name;

	public int damagePerTurn;
	public int duration;
	public int maxDuration;

	public string statAffected;
	public float quantityStatAffected;
	public float preStatDifference;

	public Character comesFrom;

	public int element;

	public bool initialized = false;

	public AlteredStatus (){

	}

	// Reduce stat
	public AlteredStatus (string name, int duration, string statAffected, float quantityStatAffected){
		idType = Type.REDUCE_STAT;
		
		this.id = name;
		this.duration = duration;
		this.maxDuration = duration;
		this.statAffected = statAffected;
		this.quantityStatAffected = quantityStatAffected;
	}

	// Reduce stat and damage per turn
	public AlteredStatus (string name, int duration, int damagePerTurn, string statAffected, float quantityStatAffected){
		idType = Type.REDUCE_STAT_AND_DOT;

		this.id = name;
		this.duration = duration;
		this.maxDuration = duration;
		this.damagePerTurn = damagePerTurn;
		this.statAffected = statAffected;
		this.quantityStatAffected = quantityStatAffected;
	}

	// Keeps a reference of who caused the altered status
	public AlteredStatus (string name, int duration, Character comesFrom){
		idType = Type.VINCULATING;

		this.id = name;
		this.duration = duration;
		this.maxDuration = duration;
		this.maxDuration = duration;
		this.comesFrom = comesFrom;
	}

	// Protect from or add element to attack
	public AlteredStatus (string name, int duration, int element){ 
		idType = Type.PROTECT_OR_ADD_ELEMENT;

		this.id = name;
		this.duration = duration;
		this.maxDuration = duration;
		this.element = element;
	}
	
	// Misc
	public AlteredStatus (string name, int duration){
		idType = Type.MISC;
		
		this.id = name;
		this.duration = duration;
		this.maxDuration = duration;
	}

	public void affect(Character character){
		switch(this.idType){
			case Type.REDUCE_STAT:
				if(isFirstTurn()){
					affectReduceStat(character);
				}
				break;
			case Type.REDUCE_STAT_AND_DOT:
				if(isFirstTurn()){
					affectReduceStat(character);
				}
				affectDamagePerTurn(character);
				break;
			case Type.VINCULATING:
				//affectVinculating(character);
				break;
			case Type.PROTECT_OR_ADD_ELEMENT:
				//affectProtectOrAddElement(character);
				break;
			case Type.MISC:
				//affectMisc(character);
				break;
		}
		
		Gamestate.instance.showMessage(LanguageManager.Instance.getStatusAffection(character.isPlayer() ? character.characterName : LanguageManager.Instance.getMenuText(character.characterName), this.getStatusName()));

		this.duration--;

		if(this.duration <= 0)
			cure(character);		
	}

	private void affectReduceStat(Character character){
		preStatDifference = character.reducePercentStat(this.statAffected, this.quantityStatAffected);
	}

	private void affectDamagePerTurn(Character character){		
		character.receivePercentDamage(this.damagePerTurn);
	}

	private void restoreStat(Character character, float stat){
		switch(this.statAffected){
			case Character.StatName.DEF:
				character.def += stat;
				break;
			case Character.StatName.ATK:
				character.atk += stat;
				break;
			case Character.StatName.VIT:
				character.vit += stat;
				break;
			/*case Character.StatName.DEF:
				character.def = stat;
				break;
			case Character.StatName.DEF:
				character.def = stat;
				break;
			case Character.StatName.DEF:
				character.def = stat;
				break;*/
		}
	}

	public void cure(Character character){
		restoreStat(character, this.preStatDifference);
	}

	public bool isFirstTurn(){
		return duration == maxDuration;
	}

	public void resetDuration(){
		duration = maxDuration;
	}

	public void translate(){
		this.name = LanguageManager.Instance.getMenuText(this.id);
	}

	public string getStatusName(){
		string[] status = this.id.Split('_');

		return status[status.Length-1];
	}

	public class Name{
		public const string POISON = "Poison";
		public const string BURN = "Burn";
		public const string BLEED = "Bleed";
		public const string BLIND = "Blind";
		public const string PROVOKE = "Provoke";
		public const string CONFUSION = "Confusion";
		public const string INVISIBLE = "Invisible";
		public const string PROTECTED = "Protected";
		public const string IMMORTAL = "Immortal";
		public const string SHIELDED = "Shielded";
		public const string ENDOW = "Endow";
	}
	
	public class Type{
		public const int REDUCE_STAT = 1;
		public const int REDUCE_STAT_AND_DOT = 2;
		public const int VINCULATING = 3;
		public const int PROTECT_OR_ADD_ELEMENT = 4;
		public const int MISC = 5;
	}
}
