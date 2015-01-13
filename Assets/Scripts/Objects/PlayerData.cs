using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData{
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
	
	public float hpRegen;
	public float mpRegen;
	
	public bool alive;
	
	public Dictionary<string, Skill> skills;
	
	public Dictionary<string, AlteredStatus> alteredStatus;

	public int job;
	public float exp;
	public float expForNextLevel;
	
	public int skillPoints;
	public int statPoints;

	public PlayerData(){
	}

	public void populate(Player player){
		this.bIsPlayer = player.bIsPlayer;

		this.characterName = player.characterName;
		this.level = player.level;
		this.element = player.element;

		this.maxHP = player.maxHP;
		this.maxMP = player.maxMP;
		this.currHP = player.currHP;
		this.currMP = player.currMP;

		this.str = player.str;
		this.agi = player.agi;
		this.vit = player.vit;
		this.itg = player.itg;
		this.dex = player.dex;
		this.luk = player.luk;

		this.atk = player.atk;
		this.matk = player.matk;
		this.def = player.def;
		this.mdef = player.mdef;
		this.hit = player.hit;
		this.flee = player.flee;
		this.critChance = player.critChance;
		this.critDmg = player.critDmg;

		this.hpRegen = player.hpRegen;
		this.mpRegen = player.mpRegen;

		this.alive = player.alive;

		this.skills = player.skills;

		this.alteredStatus = player.alteredStatus;

		this.job = player.job;
		this.exp = player.exp;
		this.expForNextLevel = player.expForNextLevel;

		this.skillPoints = player.skillPoints;
		this.statPoints = player.statPoints;
	}
}

