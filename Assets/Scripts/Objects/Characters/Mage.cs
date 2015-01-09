using UnityEngine;
using System.Collections;

public class Mage : Player
{
	public override void Start(){
		base.Start();
		
		initializePlayer();
	}
	
	public override void Update(){
		base.Update();
	}
	
	public void initializePlayer(){	
		this.name = "Mage";
		this.job = Player.Job.MAGE;
		
		this.level = 1;
		this.exp = 0;
		
		element = Singleton.Element.NEUTRAL;
		this.str = Singleton.statsPerLv[Character.Stat.STR, this.job];
		this.agi = Singleton.statsPerLv[Character.Stat.AGI, this.job];
		this.dex = Singleton.statsPerLv[Character.Stat.DEX, this.job];
		this.itg = Singleton.statsPerLv[Character.Stat.INT, this.job];
		this.vit = Singleton.statsPerLv[Character.Stat.VIT, this.job];
		this.luk = Singleton.statsPerLv[Character.Stat.LUK, this.job];
		
		this.maxHP += Singleton.statsPerLv[Character.Stat.HP, this.job] + getMaxHP();
		this.currHP += this.maxHP;//Singleton.statsPerLv[Character.Stat.HP, this.job] + getMaxHP();
		this.maxMP += Singleton.statsPerLv[Character.Stat.MP, this.job] + getMaxMP();
		this.currMP += this.maxMP;//Singleton.statsPerLv[Character.Stat.MP, this.job] + getMaxMP();
		
		this.atk += Singleton.statsPerLv[Character.Stat.ATK, this.job] + getAtk();
		this.matk += Singleton.statsPerLv[Character.Stat.MATK, this.job] + getMatk();
		
		this.def = getDef();
		this.mdef = getMdef();
		this.hit = getHit();
		this.flee = getFlee();
		this.critChance = getCritChance();
		this.critDmg = getCritDamage();
	}
}

