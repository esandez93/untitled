using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		Debug.Log("You find a " + getContent() + " in the chest!");

		Singleton.Instance.inventory.addItem(this.getContent(), 1);
		this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load <Sprite>("Sprites/Chest/chest_opened");
		Gamestate.instance.openedChests.Add(this.gameObject.name);

		opened = true;
	}

	public void setOpened(){
		this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load <Sprite>("Sprites/Chest/chest_opened");
		opened = true;
	}
}