using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LanguageManager : MonoBehaviour
{
	private static LanguageManager instance;  // Instance of the fileManager 
	public  int currentLanguage = 1;
	public  Dictionary<string, AnyText> dialogs = new Dictionary<string, AnyText>();
	public  Dictionary<string, AnyText> menus = new Dictionary<string, AnyText>();
	
	// constructor, creates an instance of fileManager if one does not exist 
	public static LanguageManager Instance { 
		get { 
			if (instance == null) { 
				instance = new GameObject("LanguageManager").AddComponent<LanguageManager>();
			} 
			
			return instance; 
		} 
	}

	public void initialize(){

	}

	// called when the application quits 
	public void OnApplicationQuit() { 
		destroyInstance(); 
	} 
	
	// destroys the file manager instance 
	public void destroyInstance() { 
		instance = null; 
	}

	public string getLanguage(){
		switch(currentLanguage){
			case 1: return "EN";
			case 2: return "ES";
			default: return "EN";
		}
	}

	public class Languages {
		public static int ENGLISH = 1;
		public static int SPANISH = 2;
	}
}

