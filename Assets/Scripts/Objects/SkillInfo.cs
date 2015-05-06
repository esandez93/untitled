using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SkillInfo{

	public int idType; // 1

	public string id; // skill_name_bulwark
	public string skillName; // Bulwark
	public string branch;

	public float[] level1; // [60, 60]
	public float[] level2; // [70, 70]
	public float[] level3; // [80, 80]
	public float[] level4; // [90, 90]
	public float[] level5; // [100, 100]
	public string[] evo; // [DEF, MDEF]
	
	public int numArguments;
	public int maxLevel;

	public SkillInfo(){
		maxLevel = 0;
	}
	
	public void setLevel(int level, string data){		
		if(data != null && !data.Equals("")){
			switch(level){
				case 1:
					level1 = parseFloatArray(data.Split(new char[]{'/'}));
					break;
				case 2:
					level2 = parseFloatArray(data.Split(new char[]{'/'}));
					break;
				case 3:
					level3 = parseFloatArray(data.Split(new char[]{'/'}));
					break;
				case 4:
					level4 = parseFloatArray(data.Split(new char[]{'/'}));
					break;
				case 5:
					level5 = parseFloatArray(data.Split(new char[]{'/'}));
					break;
			}
			
			maxLevel++;
		}
	}
	
	public void setEvo(string data){
		evo = data.Split(new char[]{'/'});
		numArguments = evo.Length;
	}
	
	public float[] parseFloatArray(string[] data){
		int len = data.Length;
		float[] parsed = new float[len];
	
		for(int i = 0; i < len; i++){
			parsed[i] = float.Parse(data[i]);
		}

		return parsed;
	}
	
	public float[] getCurrentLevelInfo(int currLevel){
		float[] currentLevelInfo = null;
		
		switch(currLevel){
			case 1:
				currentLevelInfo = this.level1;
				break;
			case 2:
				currentLevelInfo = this.level2;
				break;
			case 3:
				currentLevelInfo = this.level3;
				break;
			case 4:
				currentLevelInfo = this.level4;
				break;
			case 5:
				currentLevelInfo = this.level5;
				break;		
		}
		
		return currentLevelInfo;
	}

	public void populate(string[] row){
		this.id = row[0];		
		this.idType = Convert.ToInt32(row[1]);
		
		this.setLevel(1, row[2]);
		this.setLevel(2, row[3]);
		this.setLevel(3, row[4]);
		this.setLevel(4, row[5]);
		this.setLevel(5, row[6]);
		
		this.setEvo(row[7]);

		this.branch = row[8];
	}

	public string getReadableEvo(int currLevel) {
		string result = "";
		float[] info = getCurrentLevelInfo(currLevel+1);

		if(info != null)
			for (int i = 0; i < evo.Length; i++) {
				string e = evo[i];
				string inf = info[i].ToString();
 
				if (e.ToLower().Equals("status"))
					e = LanguageManager.Instance.getMenuText(Singleton.Instance.allSkills[this.id].status);
				else
					e = e.ToUpper();

				if (true)
					inf += "% ";

				result += inf + e + "   ";
			}

		return result;
	}
}