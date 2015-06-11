using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class CraftManager : MonoBehaviour{
	private static CraftManager instance;
	public bool initialized = false;
	private Inventory inventory;

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

	public bool craft(string item1, string item2){

		string sql = "SELECT DISTINCT * FROM CRAFTING WHERE ITEM_1 LIKE '" + item1 + "' OR ITEM_2 LIKE '" + item1
				   + "' OR ITEM_1 LIKE '" + item2 + "' OR ITEM_2 LIKE '" + item2 + "';";

		List<Craft> crafts = DatabaseManager.Instance.getCrafts(sql);

		if(crafts.Count > 0){
			Craft craft = crafts[0];

			if(this.hasNeededItems(craft.item1, craft.item1Quantity, craft.item2, craft.item2Quantity)){
				LanguageManager lm = LanguageManager.Instance;
				string message = "Crafted " + craft.resultQuantity + " " + lm.getMenuText(craft.result) + " using " + 
					craft.item1Quantity + " " + lm.getMenuText(craft.item1) +
					" and " + craft.item2Quantity + " " + lm.getMenuText(craft.item2) + ".";

				inventory.removeItem(craft.item1, craft.item1Quantity);
				inventory.removeItem(craft.item2, craft.item2Quantity);
				inventory.addItem(craft.result, craft.resultQuantity);

				Gamestate.instance.showMessage(message);

				return true;
			}
		}

		return false;
	}

	private bool hasNeededItems(string item1, int item1Quantity, string item2, int item2Quantity){
		if(item1.Equals(item2)){
			return inventory.hasNeededItems(item1, (item1Quantity+item2Quantity));
		}
		else{
			return inventory.hasNeededItems(item1, item1Quantity) && inventory.hasNeededItems(item2, item2Quantity);
		}
	}

	public List<Craft> getRecipes(){
		string sql = "SELECT * FROM CRAFTING;";

		return DatabaseManager.Instance.getCrafts(sql);
	}

	public Craft getRecipe(string name){
		string sql = "SELECT * FROM CRAFTING WHERE result = '" + name + "';";

		return DatabaseManager.Instance.getCrafts(sql)[0];
	}
}