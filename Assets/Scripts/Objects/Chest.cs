using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chest : MonoBehaviour{
	public string id;
	public string content;

	public Chest(){

	}

	public Chest(string id, string content){
		this.id = id;
		this.content = content;
	}
}