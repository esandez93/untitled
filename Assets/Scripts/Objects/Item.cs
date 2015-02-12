using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] 
public class Item : MonoBehaviour{

	public string id;

	public string type;
	public string name;
	public string description;
	public int buyValue;
	public int sellValue;
	public bool sellable;
	public string statAffected; // SYNTAX: stat number -> str 2 
	public float quantityAffected;

	public int quantity;

	public Item (){
		name = "";
		description = "";
	}

	public Item (string type, string name, string description, int buyValue, int sellValue, bool sellable, string statAffected, float quantityAffected){
		this.id = name;

		this.type = type;
		//this.name = name;
		this.description = description;
		this.buyValue = buyValue;
		this.sellValue = sellValue;
		this.sellable = sellable;
		this.statAffected = statAffected;
		this.quantityAffected = quantityAffected;
	}
	
	public Item dropping(){
		return this;
	}

	public bool isUsable(){
		return type.Equals(Properties.USABLE);
	}

	public class Properties{
		public const string USABLE = "Usable";
	}
}

