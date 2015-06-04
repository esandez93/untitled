using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class OptionsManager : MonoBehaviour{
	GameObject source;
	GameObject video;
	GameObject game;
	GameObject sound;

	private static OptionsManager instance;
	private string settings;
	public float ambientLevel = 1.0f;
	public float effectsLevel = 1.0f;
	public bool fullscreen = true;
	public string displayWidth = "1680";
	public string displayHeight = "1050";
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
			source = GameObject.Find("Gamestate/OptionsCanvas/Frame");
			video = source.transform.FindChild("Video").gameObject;
			game = source.transform.FindChild("Game").gameObject;
			sound = source.transform.FindChild("Sound").gameObject;

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

			string command = thisLine[0].Trim();
			string value = thisLine[1].Trim();

			if(command.Equals("AmbientAudio"))
				ambientLevel = float.Parse(value)/100;			
			else if (command.Equals("EffectsAudio"))
				effectsLevel = float.Parse(value)/100;			
			else if(command.Equals("FullScreen"))
				fullscreen = value.ToLower().Equals("true");			
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

	public void showMenu() {
		source.SetActive(true);
	}

	public void getSettingsFromGUI() {
		getVideoSettings();
		getGameSettings();
		getSoundSettings();

		applySettings();

		cancel();
	}

	private void getVideoSettings() {
		string x = video.transform.FindChild("Resolution").FindChild("XInput").FindChild("Text").GetComponent<Text>().text;
		displayWidth = string.IsNullOrEmpty(x) ? currWidth : x;

		string y = video.transform.FindChild("Resolution").FindChild("YInput").FindChild("Text").GetComponent<Text>().text;
		displayHeight = string.IsNullOrEmpty(y) ? currHeight : y;

		fullscreen = video.transform.FindChild("Fullscreen").GetComponent<Toggle>().isOn;
	}

	private void getGameSettings() {
		if (game.transform.FindChild("Language").FindChild("SpanishToggle").GetComponent<Toggle>().isOn)
			language = "ES";
		else if (game.transform.FindChild("Language").FindChild("EnglishToggle").GetComponent<Toggle>().isOn)
			language = "EN";
		else
			language = "EN";

		if (game.transform.FindChild("Difficulty").FindChild("EasyToggle").GetComponent<Toggle>().isOn)
			difficulty = Difficulty.EASY;
		else if (game.transform.FindChild("Difficulty").FindChild("NormalToggle").GetComponent<Toggle>().isOn)
			difficulty = Difficulty.NORMAL;
		else if (game.transform.FindChild("Difficulty").FindChild("HardToggle").GetComponent<Toggle>().isOn)
			difficulty = Difficulty.HARD;
		else
			difficulty = Difficulty.NORMAL;
	}

	private void getSoundSettings() {
		ambientLevel = sound.transform.FindChild("Ambient").FindChild("Slider").GetComponent<Slider>().value;
		effectsLevel = sound.transform.FindChild("Effects").FindChild("Slider").GetComponent<Slider>().value;
	}

	public void setSettingsToGUI() {
		if (!source.activeInHierarchy)
			source.SetActive(true);

		setVideoSettings();
		setGameSettings();
		setSoundSettings();
	}

	private void setVideoSettings() {
		video.transform.FindChild("Resolution").FindChild("XInput").FindChild("Text").GetComponent<Text>().text = displayWidth;
		video.transform.FindChild("Resolution").FindChild("YInput").FindChild("Text").GetComponent<Text>().text = displayHeight;
		
		video.transform.FindChild("Resolution").FindChild("XInput").FindChild("Placeholder").GetComponent<Text>().text = "";
		video.transform.FindChild("Resolution").FindChild("YInput").FindChild("Placeholder").GetComponent<Text>().text = "";

		video.transform.FindChild("Fullscreen").GetComponent<Toggle>().isOn = fullscreen;
	}

	private void setGameSettings() {
		if (language.Equals("ES"))
			game.transform.FindChild("Language").FindChild("SpanishToggle").GetComponent<Toggle>().isOn = true;
		else
			game.transform.FindChild("Language").FindChild("SpanishToggle").GetComponent<Toggle>().isOn = false;

		if (language.Equals("EN"))
			game.transform.FindChild("Language").FindChild("EnglishToggle").GetComponent<Toggle>().isOn = true;
		else
			game.transform.FindChild("Language").FindChild("EnglishToggle").GetComponent<Toggle>().isOn = false;
		

		if (difficulty.Equals(Difficulty.EASY))
			game.transform.FindChild("Difficulty").FindChild("EasyToggle").GetComponent<Toggle>().isOn = true;
		else
			game.transform.FindChild("Difficulty").FindChild("EasyToggle").GetComponent<Toggle>().isOn = false;

		if (difficulty.Equals(Difficulty.NORMAL))
			game.transform.FindChild("Difficulty").FindChild("NormalToggle").GetComponent<Toggle>().isOn = true;
		else 
			game.transform.FindChild("Difficulty").FindChild("NormalToggle").GetComponent<Toggle>().isOn = false;

		if (difficulty.Equals(Difficulty.HARD))
			game.transform.FindChild("Difficulty").FindChild("HardToggle").GetComponent<Toggle>().isOn = true;
		else
			game.transform.FindChild("Difficulty").FindChild("HardToggle").GetComponent<Toggle>().isOn = false;
	}

	private void setSoundSettings() {
		sound.transform.FindChild("Ambient").FindChild("Slider").GetComponent<Slider>().value = ambientLevel;
		sound.transform.FindChild("Effects").FindChild("Slider").GetComponent<Slider>().value = effectsLevel;
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
		ambientLevel = 1.0f;
		effectsLevel = 1.0f;
		fullscreen = true;
		displayWidth = "1680";
		displayHeight = "1050";
		currWidth = "1680";
		currHeight = "1050";
		language = "EN";
		difficulty = "Normal";
	}

	public void cancel() {
		source.SetActive(false);
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