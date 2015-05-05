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

	void Start() {
		/*pauseCanvas = GameObject.FindGameObjectWithTag("PauseCanvas");
		pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
		pauseMenu.SetActive(false);*/
	}

	void Update() {
		/*if(pauseCanvas == null){
			pauseCanvas = GameObject.FindGameObjectWithTag("PauseCanvas");
		}*/
		if(instance.paused && Input.GetButtonDown("Quit Menu")){
			instance.paused = togglePause();
		}
		if(Input.GetButtonDown("Pause") && Gamestate.instance.isPausable()){
			instance.paused = togglePause();
		}
	}

	public void initialize() {
		if(!initialized) {
			Debug.Log("PauseManager initialized");
			initialized = true;
		}
	}

	void OnGUI() {
		if(instance.paused){
			//GUILayout.Label("Game is paused!");
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
		/*pauseCanvas.SetActive(true);
		pauseMenu.SetActive(true);

		if(pauseBackground == null){
			pauseBackground = GameObject.FindGameObjectWithTag("PauseBackground").GetComponent<Image>();
			pauseBackground.rectTransform.sizeDelta = new Vector2( Screen.width, Screen.height);
		}

		pauseBackground.CrossFadeAlpha(0.3f, 0.01f, false);
		pauseBackground.enabled = true;*/

		//Singleton.Instance.playerPositionInMap = GameObject.FindWithTag("Mage").transform.position;
		//Application.LoadLevel("PauseMenu");
		PauseMenuManager.Instance.showCanvas();
	}

	private void unpause(){
		/*pauseCanvas.SetActive(false);
		pauseMenu.SetActive(false);
		pauseBackground.enabled = false;*/		

		PauseMenuManager.Instance.hideCanvas();
	}

	public bool isPaused(){
		return instance.paused;
	}
}