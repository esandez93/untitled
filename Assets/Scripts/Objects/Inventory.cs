using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] 
public class Inventory{

	public static string EMPTY = "battle_menu_items_empty";
	public static int MAX_ITEMS = 99;
	public Dictionary<string, Item> objects;
	public float money;

	public Inventory (){
		objects = new Dictionary<string, Item>();
	}

	public bool isItemInInventory(string itemName){
		return this.objects.ContainsKey(itemName);
	}
	
	public void addItem(string itemName, int newQuantity){
		if (isItemInInventory(itemName)){
			if((this.objects[itemName].quantity + newQuantity) <= MAX_ITEMS){
				this.objects[itemName].quantity += newQuantity;
				//MonoBehaviour.print ("Added " + quantity + " Potions.");
			}
			else{
				this.objects[itemName].quantity = MAX_ITEMS;
			}
		}
		else{
			Item item = getItem(itemName);
			item.quantity = newQuantity;
			this.objects.Add(itemName, item);
		}		
	}
	
	public void removeItem(string itemName, int quantity){
		//int inventorySize = this.objects.Count;		
		if (isItemInInventory(itemName)){
			Item item = this.objects[itemName];
			if(item.quantity >= quantity){
				Debug.Log ("PRE: " + item.quantity);
				item.quantity -= quantity;
				Debug.Log ("POST: " + item.quantity);

				if(item.quantity == 0){
					this.objects.Remove(itemName);
					Debug.Log (itemName + " REMOVED FROM INVENTORY");
				}
			}
			else{
				this.objects.Remove(itemName);
				Debug.Log (itemName + " REMOVED FROM INVENTORY");
			}

			Singleton.Instance.cleanItems();
		}
		else{
			MonoBehaviour.print("Item " + itemName + " doesn't exist in inventory.");
		}
	}
	
	/*public int count(){
		int i = 0;
		foreach(KeyValuePair<string, int> entry in this.objects){
			i += entry.Value;
		}
		
		return i;
	}*/

	public void useItem(string itemName, Character target){
		if(this.isItemInInventory(itemName)){
			Item item = objects[itemName];

			if(item.type.Equals(Item.Properties.USABLE)){
				target.increaseStat(item.statAffected, item.quantityAffected);
				this.removeItem(itemName, 1);
				BattleManager.finishCurrentAttack();
			}
		}
		else{
			BattleManager.backToStart();
		}
	}

	public List<Item> getItems(){
		List<Item> items = new List<Item>();

		foreach(KeyValuePair<string,Item> entry in objects){
			items.Add(entry.Value);
		}

		return items;
	}

	public List<Item> getUsableItems(){
		List<Item> items = new List<Item>();
		
		foreach(KeyValuePair<string,Item> entry in objects){
			if(entry.Value.isUsable()){
				items.Add(entry.Value);
			}
		}
		
		return items;
	}

	public Item getItem(string name){
		return Singleton.Instance.allItems[name];
	}

	public void addMoney(float quantity){
		money += quantity;
	}

	// If the money is enough to spend it, return true
	public bool spendMoney(float quantity){
		if(money >= quantity){
			money -= quantity;

			return true;
		}
		else{
			return false;
		}
	}
}

