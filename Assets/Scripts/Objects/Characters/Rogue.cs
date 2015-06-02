using UnityEngine;
using System.Collections;

public class Rogue : Player
{
	public override void Start(){
		//DontDestroyOnLoad(gameObject);
		base.Start();
		
		this.name = "Strider";
		this.characterName = "Strider";
		this.job = Player.Job.ROGUE;
		
		//initializePlayer(this.job);
	}
	
	public override void Update(){
		base.Update();
	}
}

