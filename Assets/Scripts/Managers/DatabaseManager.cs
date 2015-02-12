using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SQLite;

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
		db = new SQLiteConnection(path);
	}

	private void initializeDatabase(SQLiteConnection db){
		db.CreateTable<Craft> ();
	   if (db.Table<Craft> ().Count() == 0) {
	        // Only insert the data if it doesn't already exist
	        Craft newCraft = new Craft ();
	        newCraft.item1 = "item_name_potion";
	        newCraft.item1Quantity = 1;
	        newCraft.item2 = "item_name_potion";
	        newCraft.item2Quantity = 1;
	        newCraft.result = "item_name_high_potion";
	        newCraft.resultQuantity = 1;
	        db.Insert (newCraft);
	    }
	}

	public List<Craft> getCrafts(string sql){
		return db.Query<Craft>(sql);
	}

	// TABLES
	[Table("CRAFTING")]
	public class Craft {
	    [PrimaryKey, AutoIncrement, Column("ID")]
	    public int Id { get; set; }

	    [Column("ITEM_1")]
	    public string item1 { get; set; }
	    [Column("ITEM_1_QUANTITY")]
	    public int item1Quantity { get; set; }

	    [Column("ITEM_2")]
	    public string item2 { get; set; }
	    [Column("ITEM_2_QUANTITY")]
	    public int item2Quantity { get; set; }

	    [Column("RESULT")]
	    public string result { get; set; }
	    [Column("RESULT_QUANTITY")]
	    public int resultQuantity { get; set; }
	}
}