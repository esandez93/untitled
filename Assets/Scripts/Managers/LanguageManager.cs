using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LanguageManager : MonoBehaviour{
	private static LanguageManager instance;  // Instance of the fileManager 
	public  int currentLanguage;// = Languages.ENGLISH;
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
				return "ES";
		}
	}

	public void setLanguage(string language){
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

	private AnyText getDialog(string id){
		return dialogs[id];
	}

	private AnyText getMenu(string id){
		return menus[id];
	}

	private AnyText getWord(string id){
		return words[id];
	}

	private string getMenuText(string id){
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

	private bool isEnemy(string target){
		if (!target.Equals("Mage") && !target.Equals("Rogue") && !target.Equals("Knight")){
			return true;
		}

		return false;
	}

	public class Languages {
		public const int ENGLISH = 1;
		public const int SPANISH = 2;
	}
}

