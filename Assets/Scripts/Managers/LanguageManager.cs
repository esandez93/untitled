using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class LanguageManager : MonoBehaviour{
	private static LanguageManager instance;
	public  static int currentLanguage;// = Languages.ENGLISH;
	public  Dictionary<string, AnyText> dialogs = new Dictionary<string, AnyText>();
	public  Dictionary<string, AnyText> menus = new Dictionary<string, AnyText>();
	public  Dictionary<string, AnyText> words = new Dictionary<string, AnyText>();
	
	public bool initialized = false;

	public static LanguageManager Instance { 
		get {
			if (instance == null) { 
				instance = new GameObject("LanguageManager").AddComponent<LanguageManager>();
				instance.initialize();
				DontDestroyOnLoad(instance.gameObject);
			} 
			
			return instance; 
		}
	}

	public void initialize(){
		if(!initialized){
			dialogs = Singleton.Instance.allDialogues;	
			menus = Singleton.Instance.allMenus;
			words = Singleton.Instance.allWords;
			Singleton.Instance.skillNameId = getSkillNameId();
			translateAlteredStatus();

			Debug.Log("LanguageManager initialized.");
			initialized = true;
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

	public string getLanguage(){
		switch(currentLanguage){
			case Languages.ENGLISH: 
				return "EN";
			case Languages.SPANISH: 
				return "ES";
			default: 
				return "EN";
		}
	}

	public static void setLanguage(string language){
		switch(language){
			case "EN": 
				currentLanguage = Languages.ENGLISH;
				break;
			case "ES": 
				currentLanguage = Languages.SPANISH;
				break;
			default: 
				currentLanguage = Languages.ENGLISH;
				break;
		}
	}

	public string getStatus(string target, string status){
		string result = "";

		if(isEnemy(target)){
			result = formatText(getMenuText("enemy_status"), "@name", target) + " " + getWordTranslation("is");
		}
		else{
			result = target + " " + getWordTranslation("is");
		}

		result += " " + getMenuText("status_"+status) + ".";

		return result;
	}

	public string getStatusAffection(string target, string status){
		string result = "";

		if(isEnemy(target)){
			result = formatText(getMenuText("enemy_status"), "@name", target);
		}
		else{
			result = target;
		}

		result += " " + formatText(getMenuText("status_affected"), getMenuText("status_affected_"+status.ToLower())) + ".";

		return result;
	}

	private string formatText(string text, string substitution){
		string tag = getTag(text);

		return formatText(text, tag, substitution);
	}

	private string formatText(string text, string tag, string substitution){
		return text.Replace(tag, substitution);
	}

	private string formatText(string text, string[] substitutions){
		string[] tags = getTags(text);

		return formatText(text, tags, substitutions);
	}

	private string formatText(string text, string[] tags, string[] substitutions){
		string res = text;

		for(int i = 0; i < tags.Length; i++){
			res = text.Replace(tags[i], substitutions[i]);
		}

		return res;
	}

	public AnyText getDialog(string id){
		return dialogs[id];
	}

	public AnyText getMenu(string id){
		try{
			return menus[id];
		}
		catch(Exception e){
			Debug.Log(e.Message + ": " + id);
		}	

		return null;	
	}

	public AnyText getWord(string id){
		return words[id];
	}

	public string getMenuText(string id){
		return getMenu(id).getText();
	}

	private string getWordTranslation(string id){
		return getWord(id).getText();
	}

	private string getTag(string text){
		string[] words = text.Split(' ');

		foreach(string word in words){
			if(word.StartsWith("@")){
				return word;
			}
		}

		return null;
	}

	private string[] getTags(string text){
		string[] words = text.Split(' ');
		string[] tags;
		List<string> list = new List<string>();

		int i = 0;
		foreach(string word in words){
			if(word.StartsWith("@")){
				i++;
				list.Add(word);
			}
		}

		tags = new string[i];

		for(int z = 0; z < i; z++){
			tags[z] = list[z];
		}

		return tags;
	}

	public void translateButtons(){
		GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");

		string key;
		Text text;
		foreach(GameObject button in buttons){
			text = button.GetComponentInChildren<Text>();
			key = text.text;
			text.text = LanguageManager.Instance.getMenuText(key);
		}
	}

	private bool isEnemy(string target){
		return !target.Equals("Mage") && !target.Equals("Rogue") && !target.Equals("Knight");
	}

	public bool compareLanguage(int language){
		return language != LanguageManager.currentLanguage;
	}

	public Dictionary<string, string> getSkillNameId(){
		try{
			Dictionary<string, string> skillNameId = new Dictionary<string, string>();

			foreach(KeyValuePair<string, Skill> entry in Singleton.Instance.allSkills){
				entry.Value.getData();
				skillNameId.Add(entry.Value.name, entry.Key);
			}

			return skillNameId;
		}
		catch(Exception e){
			Debug.Log(e.Message);
		}

		return null;
	}

	public void translateAlteredStatus(){
		foreach(KeyValuePair<string, AlteredStatus> entry in Singleton.Instance.allAlteredStatus){
			entry.Value.translate();
		}
	}

	public class Languages {
		public const int ENGLISH = 1;
		public const int SPANISH = 2;
	}
}

