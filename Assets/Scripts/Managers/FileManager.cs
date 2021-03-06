using UnityEngine; 
using System;
using System.Collections; 
using System.Collections.Generic; 
using System.IO; 
using System.Text;
using System.Xml; 

public class FileManager : MonoBehaviour { 
	private static FileManager instance;  // Instance of the fileManager 
	public string path;  // Holds the application path 
	public static string XML_PATH = "Gamedata/Xml";
	public static string LANG_PATH = "Gamedata/Lang";
	public static string LOG_PATH = "Gamedata/Logs";
		
	public bool initialized = false;

	public static FileManager Instance { 
		get { 
			if (instance == null) { 
				instance = new GameObject("FileManager").AddComponent<FileManager>();
				instance.initialize();
				DontDestroyOnLoad(instance.gameObject);				
			} 

			return instance; 
		} 
	} 
 
	// called when the application quits 
	public void OnApplicationQuit() { 
		destroyInstance(); 
	} 

	// destroys the file manager instance 
	public void destroyInstance() { 
		instance = null; 
	} 

	// initializes the file manager
	public void initialize() { 
		if(!initialized){
			path = Application.dataPath; 

			// Check for and create the gamedata directory 
			if(!checkDirectory("Gamedata")) { 
				createDirectory("Gamedata"); 
			}

			// Check for and create the saves directory 
			if(!checkDirectory("Gamedata/Saves")) { 
				createDirectory("Gamedata/Saves");
			}

			// Check for and create the languages directory 
			if(!checkDirectory("Gamedata/Lang")) { 
				createDirectory("Gamedata/Lang");
			}

			// Check for and create the logs directory 
			if(!checkDirectory("Gamedata/Logs")) { 
				createDirectory("Gamedata/Logs");
			}

			Debug.Log ("FileManager initialized");

			initialized = true;
		}
	} 

	// checks to see whether the passed directory exists, returning true of false 
	private bool checkDirectory(string directory) { 
		return Directory.Exists(path + "/" + directory);
	}

	// creates a new directory
	private void createDirectory(string directory) { 
		if(!checkDirectory(directory)) {
			print ("Creating directory: " + directory); 		
			Directory.CreateDirectory(path + "/" + directory); 
		} 
		else 
			print("Error: You are trying to create the directory " + directory + " but it already exists!"); 		
	}

	// checks to see whether a file exists 
	public bool checkFile(string filePath) { 
		return File.Exists(path + "/" + filePath);
	}

	public void createFile(string filePath, string name, string extension) { 
		File.Create(path + "/" + filePath + "/" + name + extension);
	}

	public Dictionary<string, string> readChestContents(){
		Dictionary<string, System.Object> chestContents = parseXMLFile(XML_PATH, "chestContents", "chestContent");
		Dictionary<string, string> parsedChestContents = new Dictionary<string, string>();
		
		foreach(KeyValuePair<string, System.Object> entry in chestContents)
			parsedChestContents.Add(entry.Key, (string)entry.Value);		
		
		return parsedChestContents;
	}

	public Dictionary<string, AnyText> readWords(){
		Dictionary<string, System.Object> words = parseXMLFile(LANG_PATH, "Words/words" + LanguageManager.Instance.getLanguage(), "word");
		Dictionary<string, AnyText> parsedWords = new Dictionary<string, AnyText>();
		
		foreach(KeyValuePair<string, System.Object> entry in words)
			parsedWords.Add(entry.Key, (AnyText)entry.Value);				
		
		return parsedWords;
	}

	public Dictionary<string, AnyText> readMenus(){
		Dictionary<string, System.Object> menus = parseXMLFile(LANG_PATH, "Menus/menus" + LanguageManager.Instance.getLanguage(), "menu");
		Dictionary<string, AnyText> parsedMenus = new Dictionary<string, AnyText>();
		
		foreach(KeyValuePair<string, System.Object> entry in menus)
			parsedMenus.Add(entry.Key, (AnyText)entry.Value);				
		
		return parsedMenus;
	}

	public Dictionary<string, AnyText> readDialogues(){
		Dictionary<string, System.Object> dialogues = parseXMLFile(LANG_PATH, "Dialogues/dialogues" + LanguageManager.Instance.getLanguage(), "dialogue");
		Dictionary<string, AnyText> parsedDialogues = new Dictionary<string, AnyText>();
		
		foreach(KeyValuePair<string, System.Object> entry in dialogues)
			parsedDialogues.Add(entry.Key, (AnyText)entry.Value);		
		
		return parsedDialogues;
	}

	public Dictionary<string, Item> readItems(){
		Dictionary<string, System.Object> items = parseXMLFile(XML_PATH, "items", "item");
		Dictionary<string, Item> parsedItems = new Dictionary<string, Item>();

		foreach(KeyValuePair<string, System.Object> entry in items)
			parsedItems.Add(entry.Key, (Item)entry.Value);			

		return parsedItems;
	}

	public Dictionary<string, MonsterInfo> readMonsters(){
		Dictionary<string, System.Object> monsters = parseXMLFile(XML_PATH, "monsters", "monster");
		Dictionary<string, MonsterInfo> parsedMonsters = new Dictionary<string, MonsterInfo>();
		
		foreach(KeyValuePair<string, System.Object> entry in monsters)
			parsedMonsters.Add(entry.Key, (MonsterInfo)entry.Value);	
		
		return parsedMonsters;
	}

	public Dictionary<string, Skill> readSkills(){
		Dictionary<string, System.Object> skills = parseXMLFile(XML_PATH, "skills", "skill");
		Dictionary<string, Skill> parsedSkills = new Dictionary<string, Skill>();
	
		foreach(KeyValuePair<string, System.Object> entry in skills)
			parsedSkills.Add(entry.Key, (Skill)entry.Value);	
		
		return parsedSkills;
	}

	public Dictionary<string, MapInfo> readMaps(){
		Dictionary<string, System.Object> maps = parseXMLFile(XML_PATH, "maps", "map");
		Dictionary<string, MapInfo> parsedMaps = new Dictionary<string, MapInfo>();
		
		foreach(KeyValuePair<string, System.Object> entry in maps)
			parsedMaps.Add(entry.Key, (MapInfo)entry.Value);
		
		return parsedMaps;
	}

	public Dictionary<string, AlteredStatus> readAlteredStatus(){
		Dictionary<string, System.Object> alteredStatus = parseXMLFile(XML_PATH, "alteredStats", "alteredStat");
		Dictionary<string, AlteredStatus> parsedAlteredStatus = new Dictionary<string, AlteredStatus>();
		
		foreach(KeyValuePair<string, System.Object> entry in alteredStatus)
			parsedAlteredStatus.Add(entry.Key, (AlteredStatus)entry.Value);	
		
		return parsedAlteredStatus;
	}

	// reads an XML files contents 
	// returns <name, Object>
	public Dictionary<string, System.Object> parseXMLFile(string directory, string filename, string root) { 

		XmlDocument xmlDoc = new XmlDocument(); 
		xmlDoc.Load(path + "/" + directory + "/" + filename + ".xml");

		XmlNodeList itemList = xmlDoc.GetElementsByTagName(root); 
		Dictionary<string, System.Object> items = new Dictionary<string, System.Object>();

		foreach (XmlNode item in itemList) { 
			XmlNodeList listContent = item.ChildNodes;
			Dictionary<string, string> itemInfo = new Dictionary<string, string>();

			foreach (XmlNode itemChild in listContent) 
				itemInfo.Add(itemChild.Name, itemChild.InnerText);			

			switch(item.Name){
			case XmlTypes.ITEM:
				addItems(items, itemInfo);
				break;
			case XmlTypes.MONSTER:
				addMonsters(items, itemInfo);
				break;
			case XmlTypes.SKILL:
				addSkills(items, itemInfo);
				break;
			case XmlTypes.DIALOG:
				addDialogs(items, itemInfo);
				break;
			case XmlTypes.MENU:
				addMenus(items, itemInfo);
				break;
			case XmlTypes.WORD:
				addWords(items, itemInfo);
				break;
			case XmlTypes.MAP:
				addMaps(items, itemInfo);
				break;
			case XmlTypes.ALTEREDSTATUS:
				addAlteredStatus(items, itemInfo);
				break;
			case XmlTypes.CHESTCONTENT:
				addChestContent(items, itemInfo);
				break;
			}
		} 

		return items;
	}

	private void addChestContent(Dictionary<string, System.Object> items, Dictionary<string, string> itemInfo){
		items.Add(itemInfo["id"], itemInfo["content"]);
	}

	private void addItems(Dictionary<string, System.Object> items, Dictionary<string, string> itemInfo){
		Item newItem = null;

		switch(Convert.ToInt32(itemInfo["idType"])){
	/*1*/	case Item.Type.HEAL:
				newItem = new Item(itemInfo["type"], itemInfo["name"], itemInfo["description"], Convert.ToInt32(itemInfo["buyValue"]),
		                        Convert.ToInt32(itemInfo["sellValue"]), Convert.ToBoolean(itemInfo["sellable"]), float.Parse(itemInfo["heal"]), itemInfo["target"]);
				break;
	/*2*/	case Item.Type.DAMAGE:
				newItem = new Item(itemInfo["type"], itemInfo["name"], itemInfo["description"], Convert.ToInt32(itemInfo["buyValue"]),
		                        Convert.ToInt32(itemInfo["sellValue"]), Convert.ToBoolean(itemInfo["sellable"]), float.Parse(itemInfo["damage"]), 
								itemInfo["target"], Convert.ToInt32(itemInfo["element"]));
				break;
	/*5*/	case Item.Type.CRAFT:
				newItem = new Item(itemInfo["type"], itemInfo["name"], itemInfo["description"], Convert.ToInt32(itemInfo["buyValue"]),
		                        Convert.ToInt32(itemInfo["sellValue"]), Convert.ToBoolean(itemInfo["sellable"]));
				break;
		}		
		
		items.Add(itemInfo["name"], newItem);
	}

	private void addMonsters(Dictionary<string, System.Object> items, Dictionary<string, string> itemInfo){
		MonsterInfo newMonster = new MonsterInfo(itemInfo["type"], itemInfo["name"], Convert.ToInt32(itemInfo["level"]), Convert.ToInt32(itemInfo["element"]),
				                                 Convert.ToInt32(itemInfo["hp"]), Convert.ToInt32(itemInfo["mp"]), Convert.ToInt32(itemInfo["str"]), 
				                                 Convert.ToInt32(itemInfo["agi"]), Convert.ToInt32(itemInfo["vit"]), Convert.ToInt32(itemInfo["int"]), 
				                                 Convert.ToInt32(itemInfo["dex"]), Convert.ToInt32(itemInfo["luk"]), itemInfo["drop"],  
		                                         itemInfo["dropQuantity"], itemInfo["dropRates"], float.Parse(itemInfo["expGiven"]));
		
		items.Add(itemInfo["name"], newMonster);
	}

	private void addSkills(Dictionary<string, System.Object> items, Dictionary<string, string> itemInfo){
		Skill newSkill = null;

		switch(Convert.ToInt32(itemInfo["idType"])){
		case Skill.Type.ACTIVE_HELP:
			newSkill = new Skill(itemInfo["name"], itemInfo["branch"], Convert.ToInt32(itemInfo["maxLevel"]), itemInfo["type"], itemInfo["description"], 
			                     Convert.ToBoolean(itemInfo["usableOutOfCombat"]), itemInfo["target"], Convert.ToInt32(itemInfo["mp"]), 
			                     itemInfo["stat"], itemInfo["benefit"], Convert.ToInt32(itemInfo["duration"]));
			break;
		case Skill.Type.ACTIVE_ADD_STATUS:
			newSkill = new Skill(itemInfo["name"], itemInfo["branch"], Convert.ToInt32(itemInfo["maxLevel"]), itemInfo["type"], itemInfo["description"], 
			                     Convert.ToBoolean(itemInfo["usableOutOfCombat"]), itemInfo["target"], Convert.ToInt32(itemInfo["mp"]), 
			                     float.Parse(itemInfo["chance"]), itemInfo["status"], Convert.ToInt32(itemInfo["duration"]));
			break;
		case Skill.Type.ACTIVE_DAMAGE:
			newSkill = new Skill(itemInfo["name"], itemInfo["branch"], Convert.ToInt32(itemInfo["maxLevel"]), itemInfo["type"], itemInfo["description"], 
			                     Convert.ToBoolean(itemInfo["usableOutOfCombat"]), itemInfo["target"], float.Parse(itemInfo["damage"]), 
			                     Convert.ToInt32(itemInfo["element"]), Convert.ToInt32(itemInfo["mp"]), itemInfo["damageType"]);

			break;
		case Skill.Type.PASSIVE_BONUS_STAT:
			newSkill = new Skill(itemInfo["name"], itemInfo["branch"], Convert.ToInt32(itemInfo["maxLevel"]), itemInfo["type"], itemInfo["description"], 
			                     Convert.ToBoolean(itemInfo["usableOutOfCombat"]), itemInfo["stat"], itemInfo["benefit"]);
			break;
		case Skill.Type.NO_TARGET:
			newSkill = new Skill(itemInfo["name"], itemInfo["branch"], Convert.ToInt32(itemInfo["maxLevel"]), itemInfo["type"], itemInfo["description"], 
			                     Convert.ToBoolean(itemInfo["usableOutOfCombat"]));
			break;
		case Skill.Type.ACTIVE_DAMAGE_AND_ADD_STATUS:
			newSkill = new Skill(itemInfo["name"], itemInfo["branch"], Convert.ToInt32(itemInfo["maxLevel"]), itemInfo["type"], itemInfo["description"], 
			                    Convert.ToBoolean(itemInfo["usableOutOfCombat"]), itemInfo["target"], float.Parse(itemInfo["damage"]), 
			                    Convert.ToInt32(itemInfo["element"]), Convert.ToInt32(itemInfo["mp"]), itemInfo["damageType"], 
			                    float.Parse(itemInfo["chance"]), itemInfo["status"], Convert.ToInt32(itemInfo["duration"]));
			break;
		default:
			newSkill = new Skill(itemInfo["name"], itemInfo["branch"], Convert.ToInt32(itemInfo["maxLevel"]), itemInfo["type"], itemInfo["description"], 
			                     Convert.ToBoolean(itemInfo["usableOutOfCombat"]));
			break;
		}
		
		items.Add(itemInfo["name"], newSkill);
	}

	private void addDialogs(Dictionary<string, System.Object> items, Dictionary<string, string> itemInfo){
		AnyText newDialog = new AnyText(itemInfo["dialogId"], itemInfo["text"], itemInfo["speaker"]);
		
		items.Add(itemInfo["dialogId"], newDialog);
	}

	private void addMenus(Dictionary<string, System.Object> items, Dictionary<string, string> itemInfo){
		AnyText newMenu = new AnyText(itemInfo["menuId"], itemInfo["text"]);
		
		items.Add(itemInfo["menuId"], newMenu);
	}

	private void addWords(Dictionary<string, System.Object> items, Dictionary<string, string> itemInfo){
		AnyText newWord = new AnyText(itemInfo["wordId"], itemInfo["text"]);
		
		items.Add(itemInfo["wordId"], newWord);
	}
	
	private void addMaps(Dictionary<string, System.Object> items, Dictionary<string, string> itemInfo){
		MapInfo newMap = new MapInfo(itemInfo["mapName"], itemInfo["monsters"], itemInfo["monstersChance"]);
		
		items.Add(itemInfo["mapName"], newMap);
	}

	private void addAlteredStatus(Dictionary<string, System.Object> items, Dictionary<string, string> itemInfo){
		AlteredStatus newAlteredStatus = null;
		
		switch(Convert.ToInt32(itemInfo["idType"])){
		case AlteredStatus.Type.REDUCE_STAT:
			newAlteredStatus = new AlteredStatus(itemInfo["name"], Convert.ToInt32(itemInfo["duration"]), itemInfo["statReduced"], 
			                                     Convert.ToInt32(itemInfo["quantityReduced"]));
			break;
		case AlteredStatus.Type.REDUCE_STAT_AND_DOT:
			newAlteredStatus = new AlteredStatus(itemInfo["name"], Convert.ToInt32(itemInfo["duration"]), Convert.ToInt32(itemInfo["damagePerTurn"]), 
			                                     itemInfo["statReduced"], Convert.ToInt32(itemInfo["quantityReduced"]));
			break;
		case AlteredStatus.Type.VINCULATING:
			newAlteredStatus = new AlteredStatus(itemInfo["name"], Convert.ToInt32(itemInfo["duration"]), null);
			
			break;
		case AlteredStatus.Type.PROTECT_OR_ADD_ELEMENT:
			newAlteredStatus = new AlteredStatus(itemInfo["name"], Convert.ToInt32(itemInfo["duration"]), Convert.ToInt32(itemInfo["element"]));
			break;
		case AlteredStatus.Type.MISC:
			newAlteredStatus = new AlteredStatus(itemInfo["name"], Convert.ToInt32(itemInfo["duration"]));
			break;
		}
		
		items.Add(itemInfo["name"], newAlteredStatus);
	}

	public float[,] readNumberCsv(string directory, string filename){
		float[,] content;
		string filePath = directory + "/Gamedata/" + filename;
		StreamReader sr = new StreamReader(filePath);

		List<string[]> lines = new List<string[]>();
		int row = 0;
		int col = 0;
		while (!sr.EndOfStream){
			string[] line = sr.ReadLine().Split(';');
			lines.Add(line);
			row++;
		}

		content = new float[row,lines[1].GetLength(0)];
		row = 0;

		foreach(string[] line in lines){
			col=0;
			foreach(string s in line){
				content[row,col] = float.Parse(s);
				col++;
			}
			row++;
		}

		return content;
	}
	
	public List<string[]> readStringCsv(string directory, string filename){
		string filePath = directory + "/Gamedata/" + filename;
		StreamReader sr = new StreamReader(filePath);

		List<string[]> lines = new List<string[]>();
		while (!sr.EndOfStream){
			string[] line = sr.ReadLine().Split(';');
			lines.Add(line);
		}
		return lines;
	}

	public void writeToLog(string data){
		File.WriteAllText(path+"/"+LOG_PATH+"/log.txt", data);

		Debug.Log ("Log written");
	}

	public void writeSettings(string data){
		System.IO.StreamWriter file = new System.IO.StreamWriter(@"settings.ini");
		file.Write(data);
		file.Close();
	}

	public List<string> getFiles(string path, string search){
		string[] files = Directory.GetFiles(path, search);
		List<string> res = new List<string>();

		foreach(string file in files){
			res.Add(file);
		}

		return res;
	}

	public class XmlTypes{
		public const string ITEM = "item";
		public const string MONSTER = "monster";
		public const string SKILL = "skill";
		public const string DIALOG = "dialog";
		public const string MENU = "menu";
		public const string WORD = "word";
		public const string MAP = "map";
		public const string ALTEREDSTATUS = "alteredStat";
		public const string CHESTCONTENT = "chestContent";
	}
} 