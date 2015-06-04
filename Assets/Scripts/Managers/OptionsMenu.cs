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
		setBegin();
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

		foreach(Text t in texts)
			t.text = LanguageManager.Instance.getMenuText(t.text);		
	}

	private void setBegin() {
		OptionsManager.Instance.setSettingsToGUI();
	}

	public void saveSettings() {		
		OptionsManager.Instance.getSettingsFromGUI();
	}

	public void cancel() {		
		OptionsManager.Instance.cancel();
	}
}