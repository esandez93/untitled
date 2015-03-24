using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Monster : Character{
	public string type;

	public string[] drops;
	public Dictionary<string, int> dropRates;
	public Dictionary<string, int> dropQuantity;

	public float expGiven;

	public override void Start(){
		base.Start();

		bIsPlayer = false;
		body = GetComponent<Rigidbody2D>();
	}

	public override void Update(){
		base.Update();
	}

	public void initializeMonster(MonsterInfo monsterInfo){
		this.type = monsterInfo.type;
		this.name = monsterInfo.name;
		this.level = monsterInfo.level;
		this.element = monsterInfo.element;
		
		this.maxHP = monsterInfo.maxHP;
		this.maxMP = monsterInfo.maxMP;
		
		this.str = monsterInfo.str;
		this.agi = monsterInfo.agi;
		this.vit = monsterInfo.vit;
		this.itg = monsterInfo.itg;
		this.dex = monsterInfo.dex;
		this.luk = monsterInfo.luk;
		
		this.expGiven = monsterInfo.expGiven;
		this.drops = monsterInfo.drops;
		this.dropRates = monsterInfo.dropRates;
		this.dropQuantity = monsterInfo.dropQuantity;

		this.currHP = this.maxHP;
		this.currMP = this.maxMP;

		this.atk = getAtk();
		this.matk = getMatk();
		this.def = getDef();
		this.mdef = getMdef();
		this.hit = getHit();
		this.flee = getFlee();
		this.critChance = getCritChance();
		this.critDmg = getCritDamage();
		this.alive = true;

		this.alteredStatus = new Dictionary<string, AlteredStatus>();		
	}

	public float giveExp(){		
		//player.getExp(this.expGiven);
		//Debug.Log(player.name + " got " + expGiven + " exp from " + this.name + ".");

		return this.expGiven;
	}

	public bool hasDrops(){
		return this.drops.Length == this.dropRates.Count && this.drops.Length > 0 && this.dropRates.Count > 0;
	}

	public Dictionary<string, int> giveDrops(){
		Dictionary<string, int> droppedItems = new Dictionary<string, int>();

		//bool res = this.hasDrops();

		if(this.hasDrops()){
			float rand;
			foreach(KeyValuePair<string, int> entry in dropRates){
				rand = Random.Range(0, 101);

				if(rand <= entry.Value){ // Success
					droppedItems.Add(entry.Key, dropQuantity[entry.Key]);
					//Singleton.Instance.inventory.addItem(entry.Key, dropQuantity[entry.Key]);
					Debug.Log(this.name + " dropped " + dropQuantity[entry.Key] + " " + entry.Key + ".");
				}
			}

			/*foreach(string item in droppedItems){
				Singleton.Instance.inventory.addItem(item, dropQuantity[item]);
			}*/
		}

		return droppedItems;
		//return res;
	}
	
	public void doAction(int action, Player objective){
		BattleManager.Instance.damageReceived = false;

		switch(action){
			case Character.Actions.BASIC_ATTACK:
				GetComponent<MonsterBehaviour>().basicAttack(objective);
				break;
		}
	}

	public string toString(){
		return this.name + " Lv. " + this.level;
	}
}

