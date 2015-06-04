using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class OptionsManager : MonoBehaviour{
	private static OptionsManager instance;
	private string settings;
	public float ambientLevel = 1.0f;
	public float effectsLevel = 1.0f;
	public bool fullscreen = true; // Default
	public string displayWidth = "1680"; // Default
	public string displayHeight = "1050"; // Default
	private string currWidth = "1680";
	private string currHeight = "1050";
	public string language = "EN";
	public string difficulty = "Normal";

	public bool initialized = false;
	
	public static OptionsManager Instance { 
		get { 
			if (instance == null) { 
				instance = GameObject.FindGameObjectWithTag("OptionsManager").GetComponent<OptionsManager>();
				instance.initialize();
				DontDestroyOnLoad(instance.gameObject);
			} 
			
			return instance; 
		} 
	}

	void Awake(){
		instance = OptionsManager.Instance;
	}

	public void initialize(){	
		if(!initialized){
			getSettings();
			applySettings();
			Debug.Log("OptionsManager initialized.");
			initialized = true;
		}
	}

	public void getSettings(){
		settings = System.IO.File.ReadAllText(@"settings.ini"); // Load the settings.
		string[] settingsLines = Regex.Split(settings,"\r\n");
		
		foreach(string line in settingsLines){
			if(line.StartsWith("[")){
				continue;
			}

			string[] thisLine = Regex.Split(line,"=");

			string command = thisLine[0];
			string value = thisLine[1];

			if(command.Equals("AmbientAudio"))
				ambientLevel = float.Parse(value);			
			else if (command.Equals("EffectsAudio"))
				effectsLevel = float.Parse(value);			
			else if(command.Equals("FullScreen")){
				if(value.ToLower().Equals("true")){
					Screen.fullScreen = true;
					fullscreen = true;
				}
				else
					Screen.fullScreen = false;				
			}
			else if(command.Equals("DisplayWidth"))
				displayWidth = value;			
			else if(command.Equals("DisplayHeight"))
				displayHeight = value;			
			else if(command.Equals("Language"))
				language = value;			
			else if(command.Equals("Difficulty"))
				difficulty = value;			
		}
	}

	public void getSettingsFromGUI() {


		applySettings();
	}

	public void applySettings(){
		if(compareResolution()){
			Screen.SetResolution(int.Parse(displayWidth), int.Parse(displayHeight), fullscreen);
			
			currWidth = displayWidth;
			currHeight = displayHeight;
		}
		
		if(Screen.fullScreen != fullscreen)
			Screen.fullScreen = fullscreen;		

		LanguageManager.setLanguage(language);

		if(Application.loadedLevelName.Equals("MainMenu"))
			LanguageManager.Instance.translateButtons();		
	}

	public void loadDefault() {
	}

	public void cancel() {
	}

	public bool compareResolution(){
		return displayWidth.Equals(currWidth) && displayHeight.Equals(currHeight);
	}

	public string getDifficulty(){
		return difficulty;
	}

	public void OnApplicationQuit() { 
		destroyInstance(); 
	} 
	
	public void destroyInstance() { 
		instance = null; 
	}
	
	public void saveSettings(){		
		string output = "[Audio]";
		output += "\r\nAmbientAudio="+((int)(ambientLevel * 100)).ToString();
		output += "\r\nEffectsAudio="+((int)(effectsLevel * 100)).ToString();
		output += "\r\n[Graphics]";		
		output += "\r\nDisplayWidth="+displayWidth;
		output += "\r\nDisplayHeight="+displayHeight;
		output += "\r\nFullScreen="+fullscreen;
		output += "\r\n[Game]";
		output += "\r\nLanguage="+language;
		output += "\r\nDifficulty="+difficulty;

		FileManager.Instance.writeSettings(output);
	}

	public class Difficulty{
		public const string EASY = "Easy";
		public const string NORMAL = "Normal";
		public const string HARD = "Hard";
	}
}