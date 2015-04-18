using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Item : MonoBehaviour{

	public string id;

	public int idType;

	public string type;
	public string name;
	public string itemName;
	public string description;
	public int buyValue;
	public int sellValue;
	public bool sellable;

	public string statAffected; // SYNTAX: stat number -> str 2
	public float quantityAffected;

	public float heal;
	public float damage;

	public string target;
	public int element;

	public float chance;
	public string status;
	public int duration;

	public int quantity;

	public Item (){
		name = "";
		description = "";
	}

	public Item (string type, string name, string description, int buyValue, int sellValue, bool sellable, float heal, string target){
		this.id = name;

		this.idType = Type.HEAL;

		this.type = type;
		//this.name = name;
		this.description = description;
		this.buyValue = buyValue;
		this.sellValue = sellValue;
		this.sellable = sellable;
		this.heal = heal;
		this.target = target;
	}

	public Item (string type, string name, string description, int buyValue, int sellValue, bool sellable, float damage, string target, int element){
		this.id = name;

		this.idType = Type.DAMAGE;

		this.type = type;
		//this.name = name;
		this.description = description;
		this.buyValue = buyValue;
		this.sellValue = sellValue;
		this.sellable = sellable;
		this.damage = damage;
		this.target = target;
		this.element = element;
	}

	public Item (string type, string name, string description, int buyValue, int sellValue, bool sellable, string statAffected, float quantityAffected, string target, int element, float chance, string status, int duration){
		this.id = name;

		this.idType = Type.DAMAGE_AND_STATUS;

		this.type = type;
		//this.name = name;
		this.description = description;
		this.buyValue = buyValue;
		this.sellValue = sellValue;
		this.sellable = sellable;
		this.statAffected = statAffected;
		this.quantityAffected = quantityAffected;
		this.target = target;
		this.element = element;

		this.chance = chance;
		this.status = status;
		this.duration = duration;
	}

	public Item (string type, string name, string description, int buyValue, int sellValue, bool sellable, string target, float chance, string status, int duration){
		this.id = name;

		this.idType = Type.STATUS;

		this.type = type;
		//this.name = name;
		this.description = description;
		this.buyValue = buyValue;
		this.sellValue = sellValue;
		this.sellable = sellable;
		this.target = target;

		this.chance = chance;
		this.status = status;
		this.duration = duration;
	}

	public Item (string type, string name, string description, int buyValue, int sellValue, bool sellable){
		this.id = name;

		this.idType = Type.CRAFT;

		this.type = type;
		//this.name = name;
		this.description = description;
		this.buyValue = buyValue;
		this.sellValue = sellValue;
		this.sellable = sellable;
	}

	public void populate(string id){
		this.id = id;

		Item item = Singleton.Instance.inventory.getItem(id);

		this.type = item.type;
		this.name = LanguageManager.Instance.getMenuText(id);
		this.description = item.description;

		this.quantity = quantity;
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

	public class Type{
		public const int HEAL = 1;
		public const int DAMAGE = 2;
		public const int DAMAGE_AND_STATUS = 3;
		public const int STATUS = 4;
		public const int CRAFT = 5;
	}
}

