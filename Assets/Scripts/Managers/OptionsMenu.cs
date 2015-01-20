using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class OptionsMenu : MonoBehaviour {
	
	public static bool showMenu = false;
	private string settings;
	public static float environmentLevel = 1.0f; // 
	public static bool fullscreen = true; // Default
	public static string displayWidth = "1680"; // Default
	public static string displayHeight = "1050"; // Default
	private string currWidth = "1680";
	private string currHeight = "1050";

	void Awake(){

	}

	// Use this for initialization
	void Start () {
		//GameObject.FindGameObjectWithTag("Menu").SetActive(false);

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
		}

		Screen.SetResolution(int.Parse(displayWidth), int.Parse(displayHeight), fullscreen);
	}
	
	// Update is called once per frame
	void Update () {
		if(compareResolution()){
			Screen.SetResolution(int.Parse(displayWidth), int.Parse(displayHeight), fullscreen);

			currWidth = displayWidth;
			currHeight = displayHeight;
		}
		
		if(Screen.fullScreen != fullscreen){
			Screen.fullScreen = fullscreen;
		}
	}
	
	public void OnGUI(){
		GUI.depth = 1000;
		
		if(showMenu){
			//inGameMenu.showGameMenu = false;

			// Audio
			GUI.Box(new Rect(Screen.width/2-150,Screen.height/2-150,300,300),"");
			GUI.Label(new Rect(Screen.width/2-150,Screen.height/2-150,300,300),"Sound");
			GUI.Label(new Rect(Screen.width/2-145,Screen.height/2-130,300,300),"Environment");
			GUI.Label (new Rect(Screen.width/2+115,Screen.height/2-130,300,300),((int)(environmentLevel * 100)).ToString()+"%");
			environmentLevel = GUI.HorizontalSlider (new Rect (Screen.width/2-80,Screen.height/2-125, 190, 50), environmentLevel, 0.0f, 1.0f);
			
			// Video
			GUI.Label(new Rect(Screen.width/2-150,Screen.height/2-100,300,300),"Graphics");
			fullscreen = GUI.Toggle(new Rect(Screen.width/2-150, Screen.height/2-80, 200, 30), fullscreen, "Fullscreen");
			
			if(GUI.Button(new Rect(Screen.width/2-60,Screen.height/2+110,120,30),"Close")){
				showMenu = false;
				//inGameMenu.showGameMenu = true;
				saveSettings();
			}
		}
		
		GUI.depth = 0;
	}
	
	public static void saveSettings(){
		string output = "EnvironmentAudio="+((int)(environmentLevel * 100)).ToString();		
		output = output + "\r\nFullScreen="+fullscreen;
		output = output + "\r\nDisplayWidth="+displayWidth;
		output = output + "\r\nDisplayHeight="+displayHeight;
		
		System.IO.StreamWriter file = new System.IO.StreamWriter(@"settings.ini");
		file.Write(output);
		file.Close();
		// output = output + "\r\n";
	}

	public bool compareResolution(){
		return (displayWidth.Equals(currWidth) && displayHeight.Equals(currHeight));
	}
}