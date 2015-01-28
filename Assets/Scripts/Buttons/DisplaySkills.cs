using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DisplaySkills : MonoBehaviour {
	public static int MAX_SKILLS_UI = 4;
	public static string DEFAULT_SKILL_TEXT = "Skill";
	public static int ALLOCATE_HEIGHT = 15;

	public List<GameObject> skillButtons;
	public bool finished = false;

	BattleManager bm;

	Player player;
	List<Skill> skills;

	Vector2 parentPosition;

	int numInactives = 0;

	void Start () {
		bm = GameObject.Find("BattleCanvas").GetComponent<BattleManager>();
		this.gameObject.SetActive(false);

		skillButtons = new List<GameObject>();

		parentPosition = this.transform.position;
	}

	void Update () {
		if(bm != null && bm.isPlayerTurn()){
			player = bm.getCurrentPlayer();
			if(player != null){
				skills = player.getSkills();

				populateButtons();
				allocatePanel();
			}
		}
	}

	private void populateButtons(){
		if(skills != null){
			if(!finished){
				int i = 0;
				if(skills.Count > 0){
					foreach(Skill skill in skills){
						if(i <= MAX_SKILLS_UI){
							GameObject instance = createInstance();
							string text = skill.name + " Lv. " + skill.currLevel;
							setTextToButton(instance, text);
							
							if(sameAsLast(i) || text.Equals(DEFAULT_SKILL_TEXT)){
								skillButtons[i].SetActive(false);
								numInactives++;
							}
							i++;
						}
					}
				}
				else{
					GameObject instance = createInstance();
					LanguageManager.Instance.getMenuText(Skill.EMPTY);
					instance.GetComponent<getSkillButtonText>().enabled = false;
				}

				finished = true;
			}
		}
	}
	
	private GameObject createInstance(){
		GameObject instance = (GameObject)Instantiate(Resources.Load("Prefabs/Buttons/SkillButton"));
		instance.transform.SetParent(this.transform);
		skillButtons.Add(instance);

		return instance;
	}
	
	private void setTextToButton(GameObject instance, string txt){
		Button button = instance.GetComponent<Button>();
		Text text = button.GetComponentInChildren<Text>();
		text.text = txt;
	}

	private bool sameAsLast(int position){
		if(position != 0){
			string last = skillButtons[position-1].GetComponentInChildren<Text>().text;
			string actual = skillButtons[position].GetComponentInChildren<Text>().text;

			if(last.Equals(actual)){
				return true;
			}
		}

		return false;
	}

	private void allocatePanel(){
		numInactives = MAX_SKILLS_UI - skillButtons.Count;

		this.transform.position = new Vector2(this.transform.position.x, parentPosition.y + (ALLOCATE_HEIGHT*numInactives));
		
		numInactives = 0;
	}
}
