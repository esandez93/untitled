using System;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour{
	private bool paused = false;
	private Image pauseBackground;
	private GameObject pauseCanvas;
	private GameObject pauseMenu;

	void Start(){
		pauseCanvas = GameObject.FindGameObjectWithTag("PauseCanvas");
		pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
		pauseMenu.SetActive(false);
	}

	void Update(){
		if(pauseCanvas == null){
			pauseCanvas = GameObject.FindGameObjectWithTag("PauseCanvas");
		}

		if(Input.GetButtonDown("Pause") && Gamestate.instance.isPausable()){
			paused = togglePause();
		}
	}

	void OnGUI(){
		if(paused){
			GUILayout.Label("Game is paused!");
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
		pauseCanvas.SetActive(true);
		pauseMenu.SetActive(true);

		if(pauseBackground == null){
			pauseBackground = GameObject.FindGameObjectWithTag("PauseBackground").GetComponent<Image>();
			pauseBackground.rectTransform.sizeDelta = new Vector2( Screen.width, Screen.height);
		}

		pauseBackground.CrossFadeAlpha(0.3f, 0.01f, false);
		pauseBackground.enabled = true;
	}

	private void unpause(){
		pauseCanvas.SetActive(false);
		pauseMenu.SetActive(false);
		pauseBackground.enabled = false;
	}

	public bool isPaused(){
		return paused;
	}
}