using UnityEngine;
using System.Collections;

public class Mage : Player
{
	public override void Start(){
		//DontDestroyOnLoad(gameObject);
		base.Start();

		this.name = "Ki";
		this.characterName = "Ki";
		this.job = Player.Job.MAGE;

		//initializePlayer(this.job);
	}
	
	public override void Update(){
		base.Update();
	}
}

