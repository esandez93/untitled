using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class OptionsManager : MonoBehaviour{
	private static OptionsManager instance;
	private string settings;
	public static float environmentLevel = 1.0f; // 
	public static bool fullscreen = true; // Default
	public static string displayWidth = "1680"; // Default
	public static string displayHeight = "1050"; // Default
	private string currWidth = "1680";
	private string currHeight = "1050";
	public string language;// = "EN";
	public string difficulty;

	public bool initialized = false;
	
	public static OptionsManager Instance { 
		get { 
			if (instance == null) { 
				instance = GameObject.FindGameObjectWithTag("OptionsManager").GetComponent<OptionsManager>();//new GameObject("OptionsManager").AddComponent<OptionsManager>();
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

			if(command.Equals("EnvironmentAudio")){
				environmentLevel = float.Parse(value);
				// Debug.Log("Setting ambiance to "+thisLine[1]);
			}
			else if(command.Equals("FullScreen")){
				if(value.Equals("True") || value.Equals("true") || value.Equals("TRUE")){
					Screen.fullScreen = true;
					fullscreen = true;
				}
				else{
					Screen.fullScreen = false;
				}
			}
			else if(command.Equals("DisplayWidth")){
				displayWidth = value;
			}
			else if(command.Equals("DisplayHeight")){
				displayHeight = value;
			}
			else if(command.Equals("Language")){
				language = value;
			}
			else if(command.Equals("Difficulty")){
				difficulty = value;
			}
		}
	}

	public void applySettings(){
		if(compareResolution()){
			Screen.SetResolution(int.Parse(displayWidth), int.Parse(displayHeight), fullscreen);
			
			currWidth = displayWidth;
			currHeight = displayHeight;
		}
		
		if(Screen.fullScreen != fullscreen){
			Screen.fullScreen = fullscreen;
		}

		LanguageManager.setLanguage(language);

		if(Application.loadedLevelName.Equals("MainMenu")){
			LanguageManager.Instance.translateButtons();
		}		
	}

	public bool compareResolution(){
		return (displayWidth.Equals(currWidth) && displayHeight.Equals(currHeight));
	}

	public string getDifficulty(){
		return difficulty;
	}

	// called when the application quits 
	public void OnApplicationQuit() { 
		destroyInstance(); 
	} 
	
	// destroys the file manager instance 
	public void destroyInstance() { 
		instance = null; 
	}
	
	public void saveSettings(){		
		string output = "[Audio]";
		output += "\r\nEnvironmentAudio="+((int)(environmentLevel * 100)).ToString();
		output += "\r\n[Graphics]";		
		output += "\r\nFullScreen="+fullscreen;
		output += "\r\nDisplayWidth="+displayWidth;
		output += "\r\nDisplayHeight="+displayHeight;
		output += "\r\n[Game]";
		output += "\r\nLanguage="+language;

		FileManager.Instance.writeSettings(output);
	}

	public class Difficulty{
		public const string EASY = "Easy";
		public const string NORMAL = "Normal";
		public const string HARD = "Hard";
	}
}