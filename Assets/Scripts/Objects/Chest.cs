using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Chest : MonoBehaviour{
	private bool opened = false;

	public string id;
	private string content;

	public Chest(){

	}

	public Chest(string id, string content){
		this.id = id;
		this.content = content;
	}

	public string getId(){
		return this.id;
	}

	public void setContent(string content){
		this.content = content;
	}

	public string getContent(){
		return this.content;
	}

	public bool isOpened(){
		return opened;
	}

	public void open(){
		string[] items = getContent().Split(';');
		Dictionary<string, int> contents = new Dictionary<string, int>();

		string[] currThings;
		foreach (string item in items) {
			currThings = item.Split(',');
			
			if (currThings != null && currThings.Length == 2)
				contents.Add(currThings[0], Convert.ToInt32(currThings[1]));
		}

		string message = LanguageManager.Instance.getMenuText("open_chest_1");// Debug.Log("You found x" + getContent() + " in the chest!");

		int i = 0;
		foreach (KeyValuePair<string, int> item in contents) {
			if (i != 0)
				message += ", ";

			message += " x" + item.Value + " " + item.Key;
			Singleton.Instance.inventory.addItem(item.Key, item.Value);
		}

		Gamestate.instance.showMessage(message);
		
		this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load <Sprite>("Sprites/Chest/chest_opened");
		Gamestate.instance.openedChests.Add(this.gameObject.name);

		opened = true;
	}

	public void setOpened(){
		this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load <Sprite>("Sprites/Chest/chest_opened");
		opened = true;
	}
}