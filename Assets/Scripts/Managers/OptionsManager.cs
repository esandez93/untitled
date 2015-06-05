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
			//SaveManager.Instance.loadGameFrame = GameObject.Find("Gamestate/loadGameCanvas/Frame");		
			instance.source = GameObject.Find("Gamestate/OptionsCanvas/Frame");
			instance.video = source.transform.FindChild("Video").gameObject;
			instance.game = source.transform.FindChild("Game").gameObject;
			instance.sound = source.transform.FindChild("Sound").gameObject;

			getSettings();
			applySettings();
			Debug.Log("OptionsManager initialized.");
			instance.initialized = true;
		}
	}

	public void getSettings(){
		instance.settings = System.IO.File.ReadAllText(@"settings.ini"); // Load the settings.
		string[] settingsLines = Regex.Split(settings,"\r\n");
		
		foreach(string line in settingsLines){
			if(line.StartsWith("[")){
				continue;
			}

			string[] thisLine = Regex.Split(line,"=");

			string command = thisLine[0].Trim();
			string value = thisLine[1].Trim();

			if(command.Equals("AmbientAudio"))
				instance.ambientLevel = float.Parse(value)/100;			
			else if (command.Equals("EffectsAudio"))
				instance.effectsLevel = float.Parse(value)/100;			
			else if(command.Equals("FullScreen"))
				instance.fullscreen = value.ToLower().Equals("true");			
			else if(command.Equals("DisplayWidth"))
				instance.displayWidth = value;			
			else if(command.Equals("DisplayHeight"))
				instance.displayHeight = value;			
			else if(command.Equals("Language"))
				instance.language = value;			
			else if(command.Equals("Difficulty"))
				instance.difficulty = value;			
		}
	}

	public void showMenu() {
		instance.source.SetActive(true);
		setSettingsToGUI();
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
		instance.displayWidth = string.IsNullOrEmpty(x) ? instance.currWidth : x;

		string y = video.transform.FindChild("Resolution").FindChild("YInput").FindChild("Text").GetComponent<Text>().text;
		instance.displayHeight = string.IsNullOrEmpty(y) ? instance.currHeight : y;

		instance.fullscreen = instance.video.transform.FindChild("Fullscreen").GetComponent<Toggle>().isOn;
	}

	private void getGameSettings() {
		if (instance.game.transform.FindChild("Language").FindChild("SpanishToggle").GetComponent<Toggle>().isOn)
			instance.language = "ES";
		else if (instance.game.transform.FindChild("Language").FindChild("EnglishToggle").GetComponent<Toggle>().isOn)
			instance.language = "EN";
		else
			instance.language = "EN";

		if (instance.game.transform.FindChild("Difficulty").FindChild("EasyToggle").GetComponent<Toggle>().isOn)
			instance.difficulty = Difficulty.EASY;
		else if (instance.game.transform.FindChild("Difficulty").FindChild("NormalToggle").GetComponent<Toggle>().isOn)
			instance.difficulty = Difficulty.NORMAL;
		else if (instance.game.transform.FindChild("Difficulty").FindChild("HardToggle").GetComponent<Toggle>().isOn)
			instance.difficulty = Difficulty.HARD;
		else
			instance.difficulty = Difficulty.NORMAL;
	}

	private void getSoundSettings() {
		instance.ambientLevel = sound.transform.FindChild("Ambient").FindChild("Slider").GetComponent<Slider>().value;
		instance.effectsLevel = sound.transform.FindChild("Effects").FindChild("Slider").GetComponent<Slider>().value;
	}

	public void setSettingsToGUI() {
		if (!instance.source.activeInHierarchy)
			instance.source.SetActive(true);

		setVideoSettings();
		setGameSettings();
		setSoundSettings();
	}

	private void setVideoSettings() {
		instance.video.transform.FindChild("Resolution").FindChild("XInput").FindChild("Text").GetComponent<Text>().text = string.IsNullOrEmpty(instance.displayWidth) ? currWidth : instance.displayWidth;
		instance.video.transform.FindChild("Resolution").FindChild("YInput").FindChild("Text").GetComponent<Text>().text = string.IsNullOrEmpty(instance.displayHeight) ? currHeight : instance.displayHeight;
		
		instance.video.transform.FindChild("Resolution").FindChild("XInput").FindChild("Placeholder").GetComponent<Text>().text = "";
		instance.video.transform.FindChild("Resolution").FindChild("YInput").FindChild("Placeholder").GetComponent<Text>().text = "";

		instance.video.transform.FindChild("Fullscreen").GetComponent<Toggle>().isOn = instance.fullscreen;
	}

	private void setGameSettings() {
		if (instance.language.Equals("ES"))
			instance.game.transform.FindChild("Language").FindChild("SpanishToggle").GetComponent<Toggle>().isOn = true;
		else
			instance.game.transform.FindChild("Language").FindChild("SpanishToggle").GetComponent<Toggle>().isOn = false;

		if (instance.language.Equals("EN"))
			instance.game.transform.FindChild("Language").FindChild("EnglishToggle").GetComponent<Toggle>().isOn = true;
		else
			instance.game.transform.FindChild("Language").FindChild("EnglishToggle").GetComponent<Toggle>().isOn = false;
		

		if (instance.difficulty.Equals(Difficulty.EASY))
			instance.game.transform.FindChild("Difficulty").FindChild("EasyToggle").GetComponent<Toggle>().isOn = true;
		else
			instance.game.transform.FindChild("Difficulty").FindChild("EasyToggle").GetComponent<Toggle>().isOn = false;

		if (instance.difficulty.Equals(Difficulty.NORMAL))
			instance.game.transform.FindChild("Difficulty").FindChild("NormalToggle").GetComponent<Toggle>().isOn = true;
		else 
			instance.game.transform.FindChild("Difficulty").FindChild("NormalToggle").GetComponent<Toggle>().isOn = false;

		if (instance.difficulty.Equals(Difficulty.HARD))
			instance.game.transform.FindChild("Difficulty").FindChild("HardToggle").GetComponent<Toggle>().isOn = true;
		else
			instance.game.transform.FindChild("Difficulty").FindChild("HardToggle").GetComponent<Toggle>().isOn = false;
	}

	private void setSoundSettings() {
		instance.sound.transform.FindChild("Ambient").FindChild("Slider").GetComponent<Slider>().value = instance.ambientLevel;
		instance.sound.transform.FindChild("Effects").FindChild("Slider").GetComponent<Slider>().value = instance.effectsLevel;
	}

	public void applySettings(){
		if(compareResolution()){
			Screen.SetResolution(int.Parse(instance.displayWidth), int.Parse(instance.displayHeight), instance.fullscreen);
			
			instance.currWidth = instance.displayWidth;
			instance.currHeight = instance.displayHeight;
		}
		
		if(Screen.fullScreen != instance.fullscreen)
			Screen.fullScreen = instance.fullscreen;		

		LanguageManager.setLanguage(instance.language);

		if(Application.loadedLevelName.Equals("MainMenu"))
			LanguageManager.Instance.translateButtons();

		saveSettings();
	}

	public void loadDefault() {
		instance.ambientLevel = 1.0f;
		instance.effectsLevel = 1.0f;
		instance.fullscreen = true;
		instance.displayWidth = "1680";
		instance.displayHeight = "1050";
		instance.currWidth = "1680";
		instance.currHeight = "1050";
		instance.language = "EN";
		instance.difficulty = "Normal";

		setSettingsToGUI();
	}

	public void cancel() {
		instance.source.SetActive(false);
	}

	public bool compareResolution(){
		return instance.displayWidth.Equals(instance.currWidth) && instance.displayHeight.Equals(instance.currHeight);
	}

	public string getDifficulty(){
		return instance.difficulty;
	}

	public void OnApplicationQuit() { 
		destroyInstance(); 
	} 
	
	public void destroyInstance() { 
		instance = null; 
	}
	
	public void saveSettings(){		
		string output = "[Audio]";
		output += "\r\nAmbientAudio="+((int)(instance.ambientLevel * 100)).ToString();
		output += "\r\nEffectsAudio="+((int)(instance.effectsLevel * 100)).ToString();
		output += "\r\n[Graphics]";		
		output += "\r\nDisplayWidth="+instance.displayWidth;
		output += "\r\nDisplayHeight="+instance.displayHeight;
		output += "\r\nFullScreen="+instance.fullscreen;
		output += "\r\n[Game]";
		output += "\r\nLanguage="+instance.language;
		output += "\r\nDifficulty="+instance.difficulty;

		FileManager.Instance.writeSettings(output);
	}

	public class Difficulty{
		public const string EASY = "Easy";
		public const string NORMAL = "Normal";
		public const string HARD = "Hard";
	}
}