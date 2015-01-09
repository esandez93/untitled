using UnityEngine;
using System; 
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MapInfo{
	public string mapName;
	public List<MonsterInfo> monsters;
	public Dictionary<string, int> monstersChance;

	public MapInfo (string mapName){
		this.mapName = mapName;
		monsters = new List<MonsterInfo>();
		monstersChance = new Dictionary<string, int>();
	}

	public MapInfo (string mapName, string monsters, string monstersChance){
		this.monsters = new List<MonsterInfo>();
		this.monstersChance = new Dictionary<string, int>();

		this.mapName = mapName;
		this.monstersChance = new Dictionary<string, int>();

		string[] monstersAux = monsters.Split(';');
		string[] monstersAux2 = monstersChance.Split(';');

		for(int i = 0; i < monstersAux.Length; i++){
			addMonster(Singleton.allMonsters[monstersAux[i]]);
			this.monstersChance.Add(monstersAux[i], Convert.ToInt32(monstersAux2[i]));
		}
	}

	public void addMonster(MonsterInfo monster){
		this.monsters.Add(monster);
	}

	// Tiene en cuenta los porcentajes de aparicion de monstruos en este mapa
	public MonsterInfo getMonster(){
		MonsterInfo monster = null;
		int chance = UnityEngine.Random.Range(0, 101);

		foreach(KeyValuePair<string, int> entry in monstersChance){
			if((chance -= entry.Value) < 0){
				monster = Singleton.allMonsters[entry.Key];
				break;
			}
		}
		
		return monster;
	}

	// No tiene en cuenta los porcentajes de aparicion de monstruos, devuelve un monstruo totalmente aleatorio entre los disponibles en el mapa
	public MonsterInfo getRandomMonster(){
		MonsterInfo monster = monsters[UnityEngine.Random.Range(0, monsters.Count)];

		return monster;
	}
}