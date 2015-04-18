using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;

public class DatabaseManager : MonoBehaviour {
	public static string DATABASE_PATH = "/Gamedata/database.db3";

	private static DatabaseManager instance;
	private bool initialized = false;
	private SQLiteConnection db;

	public static DatabaseManager Instance {
		get {
			if (instance == null) {
				instance = new GameObject("DatabaseManager").AddComponent<DatabaseManager>();
				instance.initialize();
				DontDestroyOnLoad(instance.gameObject);
			}

			return instance;
		}
	}

	private void initialize(){
		if(!initialized){
			getDatabaseConnection(DATABASE_PATH);
			initializeDatabase(db);
			Debug.Log("DatabaseManager initialized.");
			initialized = true;
		}
	}

	private void getDatabaseConnection(string path){
		db = new SQLiteConnection(Application.dataPath + path);
	}

	private void initializeDatabase(SQLiteConnection db){
		db.CreateTable<Craft> ();
	   if (db.Table<Craft>().Count() == 0) {
	        // Only insert the data if it doesn't already exist
	        Craft newCraft = new Craft ();
	        newCraft.item1 = "item_name_potion";
	        newCraft.item1Quantity = 1;
	        newCraft.item2 = "item_name_potion";
	        newCraft.item2Quantity = 1;
	        newCraft.result = "item_name_high_potion";
	        newCraft.resultQuantity = 1;
	        db.Insert (newCraft);

	        newCraft.item1 = "item_name_high_potion";
	        newCraft.item1Quantity = 1;
	        newCraft.item2 = "item_name_high_potion";
	        newCraft.item2Quantity = 1;
	        newCraft.result = "item_name_ultra_potion";
	        newCraft.resultQuantity = 1;
	        db.Insert (newCraft);

	        newCraft.item1 = "item_name_container";
	        newCraft.item1Quantity = 1;
	        newCraft.item2 = "item_name_gunpowder";
	        newCraft.item2Quantity = 1;
	        newCraft.result = "item_name_grenade";
	        newCraft.resultQuantity = 1;
	        db.Insert (newCraft);
	    }
	}

	public List<Craft> getCrafts(string sql){
		return db.Query<Craft>(sql);
	}
}