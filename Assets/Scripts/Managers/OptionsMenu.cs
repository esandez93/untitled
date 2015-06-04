using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsMenu : MonoBehaviour {
	
	public bool showMenu;
	private string settings;
	public float environmentLevel;
	public bool fullscreen;
	public string displayWidth;
	public string displayHeight;
	public string language;

	void Start () {
		initialize();
		OptionsManager.Instance.getSettings();
	}

	private void initialize() {
		translateAll();
	}

	private void loadDefault() {
		environmentLevel = 1.0f;
		fullscreen = true;
		displayWidth = "1680";
		displayHeight = "1050";
		language = "EN";

		OptionsManager.Instance.loadDefault();
	}

	private void translateAll() {
		Text[] texts = this.gameObject.GetComponentsInChildren<Text>();

		foreach(Text t in texts) {
			t.text = LanguageManager.Instance.getMenuText(t.text);
		}
	}

	public void saveSettings() {		
		OptionsManager.Instance.getSettingsFromGUI();
	}

	public void cancel() {		
		OptionsManager.Instance.cancel();
	}
	
	/*public void OnGUI(){
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
	}*/
}