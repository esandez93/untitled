using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour{
	private bool paused = false;
	private Image pauseBackground;
	private GameObject pauseCanvas;
	private GameObject pauseMenu;
	private bool initialized = false;

	private static PauseManager instance = null;
	public static PauseManager Instance{
		get{
			if (instance == null){
				instance = new GameObject("PauseManager").AddComponent<PauseManager>();
				instance.initialize();
				DontDestroyOnLoad(instance.gameObject);
			}
				
			return instance;
		}
	}

	void Update() {
		if(Input.GetButtonDown("Pause") && Gamestate.instance.isPausable())
			instance.paused = togglePause();		
	}

	public void initialize() {
		if(!initialized) {
			Debug.Log("PauseManager initialized");
			initialized = true;
		}
	}

	public bool togglePause(){
		if(Time.timeScale == 0f){
			Time.timeScale = 1f;
			unpause();
			return false;
		}
		else{
			Time.timeScale = 0f;
			pause();
			return true;
		}
	}

	private void pause(){
		PauseMenuManager.Instance.showCanvas();
	}

	private void unpause(){
		PauseMenuManager.Instance.hideCanvas();
	}

	public bool isPaused(){
		return instance.paused;
	}
}