using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] 
public class Inventory {

	public static string EMPTY = "battle_menu_items_empty";
	public static int MAX_ITEMS = 99;
	public Dictionary<string, Item> objects;
	public float money;
	private int totalItems = 0;

	public Inventory (){
		objects = new Dictionary<string, Item>();
	}

	public bool isItemInInventory(string itemName){
		return this.objects.ContainsKey(itemName);
	}

	public bool hasItem(string itemName, int itemQuantity){
		return isItemInInventory(itemName) && this.objects[itemName].quantity >= itemQuantity;
	}

	public bool hasNeededItems(string itemName, int quantity){
		if (isItemInInventory(itemName)){
			if(this.objects[itemName].quantity >= quantity){
				return true;
			}
		}

		return false;
	}
	
	public void addItem(string itemName, int newQuantity){
		if (isItemInInventory(itemName)){
			if((this.objects[itemName].quantity + newQuantity) <= MAX_ITEMS){
				this.objects[itemName].quantity += newQuantity;
				//Debug.Log ("Added " + quantity + " Potions.");
			}
			else{
				this.objects[itemName].quantity = MAX_ITEMS;
			}
		}
		else{
			Item item = getItem(itemName);
			item.quantity = newQuantity;
			this.objects.Add(itemName, item);
			totalItems++;
		}		
	}
	
	public bool removeItem(string itemName, int quantity){
		//int inventorySize = this.objects.Count;		
		if (isItemInInventory(itemName)){
			Item item = this.objects[itemName];
			if(item.quantity >= quantity){
				item.quantity -= quantity;

				if(item.quantity == 0){
					this.objects.Remove(itemName);
					Debug.Log (itemName + " REMOVED FROM INVENTORY");
					totalItems--;
				}
			}
			else{
				this.objects.Remove(itemName);
				Debug.Log (itemName + " REMOVED FROM INVENTORY");
				totalItems--;
			}

			if(Gamestate.instance.isBattleLevel()){
				Singleton.Instance.cleanItems();			
			}

			return true;
		}
		else{
			Debug.Log("Item " + itemName + " doesn't exist in inventory.");
			return false;
		}
	}

	public void useItemInBattle(string itemName, Character target){
		if(this.isItemInInventory(itemName)){
			Item item = objects[itemName];
			if(item.type.Equals(Item.Properties.USABLE)){
				List<Character> targets = new List<Character>();
				if(item.target.Equals(Character.Target.GROUP)){
					List<GameObject> gos = new List<GameObject>();
					
					if(target.gameObject.tag.ToLower().Contains("monster")) {
						for(int i = 1; i <= 3; i++)
							gos.Add(GameObject.FindGameObjectWithTag("Monster"+i));
					}
					else{
						foreach (Player p in BattleManager.Instance.playersInBattle)
							gos.Add(GameObject.FindGameObjectWithTag(p.characterName));
					}
					
					foreach(GameObject go in gos) {
						targets.Add(go.GetComponent<Character>());	
					}
				}
				else
					targets.Add(target);				

				foreach(Character character in targets)
					character.receiveItemUsage(item);				

				this.removeItem(itemName, 1);
				BattleManager.Instance.changePhase(BattleManager.BattlePhases.DOACTION);
				BattleManager.Instance.finishCurrentAttack();
			}
		}
		else
			BattleManager.Instance.backToStart();		
	}

	public void useItem(string itemName, Character target){
		if(this.isItemInInventory(itemName)){
			Item item = objects[itemName];
			if(item.type.Equals(Item.Properties.USABLE)){	
				if(this.removeItem(itemName, 1)){
					Debug.Log("ITEM: "+ item.itemName + ", TARGET: " + target.characterName);
					target.receiveItemUsage(item);	
				}	
				else{
					Debug.Log("THE FAILS");
				}
			}
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

	public int getTotalItems(){
		return totalItems;
	}
}

