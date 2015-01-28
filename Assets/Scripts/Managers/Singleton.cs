using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Singleton : MonoBehaviour {

	public Vector2 playerPositionInMap;

	public static string statsFileName = "Atributos por lv.csv";
	public static string elementalModifiersFileName = "Modificadores elemento.csv";
	public static string skillInfosFileName = "Skills por lv.csv";
	public static string exceptionSkillsName = "exceptionSkills.csv";
	public float[,] statsPerLv;
	public float[,] elementalModifiers;
	public Dictionary<int, float> expNeeded;

	public Dictionary<string, Item> allItems;
	public Dictionary<string, MonsterInfo> allMonsters;
	public Dictionary<string, Skill> allSkills;
	public Dictionary<string, AnyText> allMenus;
	public Dictionary<string, AnyText> allDialogues;
	public Dictionary<string, AnyText> allWords;
	public Dictionary<string, MapInfo> allMaps;
	public Dictionary<string, AlteredStatus> allAlteredStatus;
	public Dictionary<string, SkillInfo> allSkillInfo;

	public Dictionary<string, string> skillNameId;

	public Inventory inventory = new Inventory();

	public List<string> exceptionSkills;

	public bool initialized = false;

	private static Singleton instance = null;
	public static Singleton Instance{
		get{
			if (instance == null){
				instance = new GameObject("Singleton").AddComponent<Singleton>();
				instance.initialize();
				DontDestroyOnLoad(instance.gameObject);
			}

				
			return instance;
		}
	}

	public void getStatsFromFile(){
		statsPerLv = FileManager.Instance.readNumberCsv(FileManager.Instance.path, statsFileName);
	}

	public void getElementalModifiersFromFile(){
		elementalModifiers = FileManager.Instance.readNumberCsv(FileManager.Instance.path, elementalModifiersFileName);
	}
	
	public void getSkillInfoFromFile(){
		List<string[]> skillInfos = FileManager.Instance.readStringCsv(FileManager.Instance.path, skillInfosFileName);

		allSkillInfo = new Dictionary<string, SkillInfo>();
		foreach(string[] row in skillInfos){
			populateSkillInfo(row);
		}	
	}
	
	public void getExceptionSkills(){
		List<string[]> exceptions = FileManager.Instance.readStringCsv(FileManager.Instance.path, exceptionSkillsName);

		exceptionSkills = new List<string>();
		foreach(string[] row in exceptions){
			populateExceptionSkills(row);
		}
	}
	
	public void populateSkillInfo(string[] row){
		SkillInfo skillInfo = new SkillInfo();
		
		skillInfo.populate(row);

		allSkillInfo.Add(skillInfo.id, skillInfo);
	}

	public void populateExceptionSkills(string[] row){
		foreach(string exception in row){
			exceptionSkills.Add(exception);
		}
	}

	public void getExpNeeded(){
		float exp = 0; 

		for(int i = 1; i < 100; i++){
			exp = (float)System.Math.Pow(i, 3);

			Singleton.Instance.expNeeded.Add(i, exp);
		}
	}

	public void initialize(){		
		if(!initialized){
			Singleton.Instance.expNeeded = new Dictionary<int, float>();

			Singleton.Instance.getExpNeeded();

			Singleton.Instance.getStatsFromFile();
			Singleton.Instance.getElementalModifiersFromFile();
			Singleton.Instance.getSkillInfoFromFile();
			Singleton.Instance.getExceptionSkills();

			allItems = FileManager.Instance.readItems();
			allMonsters = FileManager.Instance.readMonsters();
			allSkills = FileManager.Instance.readSkills();
			allMenus = FileManager.Instance.readMenus();
			allDialogues = FileManager.Instance.readDialogues();
			allWords = FileManager.Instance.readWords();
			allMaps = FileManager.Instance.readMaps();
			allAlteredStatus = FileManager.Instance.readAlteredStatus();		

			Debug.Log ("Singleton initialized");

			initialized = true;
		}
	}

	public void cleanItems(){
		Destroy (GameObject.Find("ItemButton(Clone)"));
		//DisplayItems.repopulate();
	}

	public class Element{
		public static int FIRE = 0;
		public static int PLANT = 1;
		public static int WATER = 2;
		public static int ELECTRIC = 3;
		public static int WIND = 4;
		public static int GROUND = 5;
		public static int LIGHT = 6;
		public static int DARK = 7;
		public static int NEUTRAL = 8;
	}

	public class Battle{
		public static float MP_REGEN_RATIO = 0.05f; // +5%
		public static float HP_REGEN_RATIO = 0f; // +0%
	}

	public class Layers{
		public static int PLAYER_NO_COLLISION = 31;
		public static int PLAYER = 9;
		public static int BOTTOM_PLATFORM = 13;
	}
}
