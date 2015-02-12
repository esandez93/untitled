using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class CraftManager : MonoBehaviour{
	private static CraftManager instance;
	public bool initialized = false;
	Inventory inventory;

	public static CraftManager Instance {
		get {
			if (instance == null) {
				instance = new GameObject("CraftManager").AddComponent<CraftManager>();
				instance.initialize();
				DontDestroyOnLoad(instance.gameObject);
			}

			return instance;
		}
	}

	private void initialize(){
		if(!initialized){
			inventory = Singleton.Instance.inventory;
			Debug.Log("CraftManager initialized.");
			initialized = true;
		}
	}

	public void craft(string item1, string item2){

		string sql = "SELECT DISTINCT * FROM CRAFTING WHERE ITEM_1 LIKE " + item1 + " OR ITEM_2 LIKE " + item1
				   + " OR ITEM_1 LIKE " + item2 + " OR ITEM_2 LIKE " + item2 + ";";

		List<Craft> crafts = DatabaseManager.Instance.getCraft(sql);

		if(crafts.Count > 0){
			Craft craft = crafts.Get(0);

			if(this.haveNeededItems(craft.item1, craft.item1Quantity, craft.item2, craft.item2Quantity)){
				inventory.removeItem(craft.item1, craft.item1Quantity);
				inventory.removeItem(craft.item2, craft.item2Quantity);
				inventory.addItem(craft.result, craft.resultQuantity);
				Debug.Log("Crafted " + craft.resultQuantity + " " + craft.result + " using " + craft.item1Quantity + " " + craft.item1
						   + " and " + craft.item2Quantity + " " + craft.item2 + ".");

				return true;
			}

			return false;
		}
	}

	private bool haveNeededItems(string item1, int item1Quantity, string item2, int item2Quantity){
		if(item1.Equals(item2)){
			return inventory.haveNeededItems(item1, item1Quantity+item2Quantity);
		}
		else{
			return inventory.haveNeededItems(item1, item1Quantity) && inventory.haveNeededItems(item2, item2Quantity);
		}
	}
}