using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class MonsterInfo{
	public string name;
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

	public string type;
	
	public string[] drops;
	public Dictionary<string, int> dropQuantity;
	public Dictionary<string, int> dropRates;
	
	public float expGiven;

	public MonsterInfo(){

	}

	public MonsterInfo(string type, string name, int level, int element, int hp, int mp, int str, int agi, int vit, int itg, int dex, int luk, 
	                   string drops, string dropQuantity, string dropRates, float expGiven){
		
		this.type = type;
		this.name = name;
		this.level = level;
		this.element = element;
		
		this.maxHP = hp;
		this.maxMP = mp;
		
		this.str = str;
		this.agi = agi;
		this.vit = vit;
		this.itg = itg;
		this.dex = dex;
		this.luk = luk;

		this.dropQuantity = new Dictionary<string, int>();
		this.dropRates = new Dictionary<string, int>();

		this.expGiven = expGiven;

		//Debug.Log ("Drops: " + drops);
		this.drops = drops.Split(';');
		//Debug.Log ("this.Drops: " + this.drops.Length);
		string[] dropsAux = dropRates.Split(';');
		string[] dropsAux2 = dropQuantity.Split(';');

		if(this.hasDrops()){
			for(int i = 0; i < this.drops.Length; i++){
				//Debug.Log (i + " - " + this.drops[i] + " " + dropsAux[i] + "% x" + dropsAux2[i]);
				this.dropRates.Add(this.drops[i], Convert.ToInt32(dropsAux[i]));
				this.dropQuantity.Add(this.drops[i], Convert.ToInt32(dropsAux2[i]));

			}
		}
	}
	
	public bool hasDrops(){
		if(this.drops.Length > 0){//this.drops.Length == this.dropRates.Count && this.drops.Length > 0 && this.dropRates.Count > 0){
			return true;
		}
		else{
			return false;
		}
	}
}

