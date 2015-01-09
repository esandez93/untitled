using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Singleton : MonoBehaviour {

	public static Vector2 playerPositionInMap;

	public static string statsFileName = "Atributos por lv.csv";
	public static string elementalModifiersFileName = "Modificadores elemento.csv";
	public static string skillInfosFileName = "Skills por lv.csv";
	public static string exceptionSkillsName = "exceptionSkills.csv";
	public static float[,] statsPerLv;
	public static float[,] elementalModifiers;
	public static Dictionary<int, float> expNeeded;

	public static Dictionary<string, Item> allItems;
	public static Dictionary<string, MonsterInfo> allMonsters;
	public static Dictionary<string, Skill> allSkills;
	public static Dictionary<string, AnyText> allMenus;
	public static Dictionary<string, AnyText> allDialogues;
	public static Dictionary<string, MapInfo> allMaps;
	public static Dictionary<string, AlteredStatus> allAlteredStatus;
	public static Dictionary<string, SkillInfo> allSkillInfo;

	public static Inventory inventory = new Inventory();

	public static List<string> exceptionSkills;

	private static Singleton instance = null;
	public static Singleton Instance{
		get{
			if (instance == null){
				instance = new GameObject("Singleton").AddComponent<Singleton>();
				instance.initialize();
			}
				
			return instance;
		}
	}

	public static void getStatsFromFile(){
		statsPerLv = FileManager.Instance.readNumberCsv(FileManager.Instance.path, statsFileName);
	}

	public static void getElementalModifiersFromFile(){
		elementalModifiers = FileManager.Instance.readNumberCsv(FileManager.Instance.path, elementalModifiersFileName);
	}
	
	public static void getSkillInfoFromFile(){
		List<string[]> skillInfos = FileManager.Instance.readStringCsv(FileManager.Instance.path, skillInfosFileName);

		allSkillInfo = new Dictionary<string, SkillInfo>();
		foreach(string[] row in skillInfos){
			populateSkillInfo(row);
		}	
	}
	
	public static void getExceptionSkills(){
		List<string[]> exceptions = FileManager.Instance.readStringCsv(FileManager.Instance.path, exceptionSkillsName);

		exceptionSkills = new List<string>();
		foreach(string[] row in exceptions){
			populateExceptionSkills(row);
		}
	}
	
	public static void populateSkillInfo(string[] row){
		SkillInfo skillInfo = new SkillInfo();
		
		skillInfo.populate(row);

		allSkillInfo.Add(skillInfo.skillName, skillInfo);
	}

	public static void populateExceptionSkills(string[] row){
		foreach(string exception in row){
			exceptionSkills.Add(exception);
		}
	}

	public static void getExpNeeded(){
		float exp = 0; 

		for(int i = 1; i < 100; i++){
			exp = (float)System.Math.Pow(i, 3);

			Singleton.expNeeded.Add(i, exp);
		}
	}

	public void initialize(){
		//Singleton.inventory = new Inventory();	
		Singleton.expNeeded = new Dictionary<int, float>();

		FileManager.Instance.initialize();

		Singleton.getExpNeeded();

		Singleton.getStatsFromFile();
		Singleton.getElementalModifiersFromFile();
		Singleton.getSkillInfoFromFile();
		Singleton.getExceptionSkills();

		allItems = FileManager.Instance.readItems();
		allMonsters = FileManager.Instance.readMonsters();
		allSkills = FileManager.Instance.readSkills();
		allMenus = FileManager.Instance.readMenus();
		allDialogues = FileManager.Instance.readDialogues();
		allMaps = FileManager.Instance.readMaps();
		allAlteredStatus = FileManager.Instance.readAlteredStatus();
	}

	public void cleanItems(){
		Destroy (GameObject.Find("ItemButton(Clone)"));
		DisplayItems.repopulate();
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
