using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class OptionsMenu : MonoBehaviour {
	
	public static bool showMenu = true;
	private string settings;
	public static float environmentLevel = 1.0f;
	public static bool fullscreen = true;
	public static string displayWidth = "1680";
	public static string displayHeight = "1050";
	public static string language = "EN";

	void Start () {
		OptionsManager.Instance.getSettings();
	}
	
	public void OnGUI(){
		GUI.depth = 1000;
		
		if(showMenu){
			GUI.Box(new Rect(Screen.width/2-150,Screen.height/2-150,300,300),"");
			GUI.Label(new Rect(Screen.width/2-150,Screen.height/2-150,300,300),"Sound");
			GUI.Label(new Rect(Screen.width/2-145,Screen.height/2-130,300,300),"Environment");
			GUI.Label (new Rect(Screen.width/2+115,Screen.height/2-130,300,300),((int)(environmentLevel * 100)).ToString()+"%");
			environmentLevel = GUI.HorizontalSlider (new Rect (Screen.width/2-80,Screen.height/2-125, 190, 50), environmentLevel, 0.0f, 1.0f);
			
			// Video
			GUI.Label(new Rect(Screen.width/2-150,Screen.height/2-100,300,300),"Graphics");
			fullscreen = GUI.Toggle(new Rect(Screen.width/2-150, Screen.height/2-80, 200, 30), fullscreen, "Fullscreen");
			
			if(GUI.Button(new Rect(Screen.width/2-60,Screen.height/2+110,120,30),"Save and Close")){
				showMenu = false;
				//inGameMenu.showGameMenu = true;
				OptionsManager.Instance.applySettings();
				OptionsManager.Instance.saveSettings();
			}
		}
		
		GUI.depth = 0;
	}
}