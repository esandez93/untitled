using UnityEngine;
using System.Collections;

public class Knight : Player
{
	public override void Start(){
		//DontDestroyOnLoad(gameObject);
		base.Start();
		
		this.name = "Gilgamesh";
		this.characterName = "Gilgamesh";
		this.job = Player.Job.KNIGHT;
		
		//initializePlayer(this.job);
	}
	
	public override void Update(){
		base.Update();
	}
}

